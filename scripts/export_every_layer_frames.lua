-- Export every layer as individual frame images.
-- Usage (PowerShell):
--   Aseprite.exe -b "file.aseprite" --script-param out='export/{layer}/{frame}.png' ^
--     --script "scripts/export_every_layer_frames.lua"
-- Optional params: from, to, scale, includeHidden, layers

local spr = app.activeSprite or app.sprite
if not spr then
  error("No active sprite found. Open a sprite before running this script.")
end

local p = app.params
local outPattern = p.out or "{layer}/{frame}.png"
local fromFrame = tonumber(p.fromFrame) or 1
local toFrame = tonumber(p.toFrame) or #spr.frames
local scale = tonumber(p.scale)
local includeHidden = p.includeHidden == "1" or p.includeHidden == "true"
local layersParam = p.layers or ""

if fromFrame < 1 then fromFrame = 1 end
if toFrame > #spr.frames then toFrame = #spr.frames end
if fromFrame > toFrame then
  error("Invalid frame range.")
end

local sep = app.fs.pathSeparator
local function normalize(path)
  if sep == "\\" then
    return string.gsub(path, "/", "\\")
  end
  return string.gsub(path, "\\", "/")
end

local function ensure_directory(path)
  if not path or path == "" then
    return
  end

  local built = ""
  for part in string.gmatch(path, "[^" .. sep .. "]+") do
    if built == "" then
      built = part
    else
      built = built .. sep .. part
    end

    if not app.fs.isDirectory(built) then
      pcall(app.fs.makeDirectory, built)
    end
  end
end

local function apply_frame_tokens(pattern, frameIndex)
  local value = frameIndex
  local result = pattern
  result = string.gsub(result, "{frame0001}", string.format("%04d", value))
  result = string.gsub(result, "{frame001}", string.format("%03d", value))
  result = string.gsub(result, "{frame01}", string.format("%02d", value))
  result = string.gsub(result, "{frame}", tostring(value))
  return result
end

local function is_group(layer)
  local ok, val = pcall(function() return layer.isGroup end)
  if ok and type(val) == "boolean" then
    return val
  end
  local ok2, val2 = pcall(function() return layer.layers end)
  if ok2 and type(val2) == "table" then
    return next(val2) ~= nil
  end
  return false
end

local leafLayers = {}
local function collect_layers(parent, prefix)
  for _, layer in ipairs(parent.layers) do
    local path = prefix ~= "" and (prefix .. "/" .. layer.name) or layer.name
    if is_group(layer) then
      collect_layers(layer, path)
    else
      table.insert(leafLayers, { layer = layer, path = path })
    end
  end
end

collect_layers(spr, "")

local wanted = {}
if layersParam ~= "" then
  for part in string.gmatch(layersParam, "[^|]+") do
    wanted[part] = true
  end
end

local function is_effectively_visible(layer)
  if not layer.isVisible then
    return false
  end
  local parent = layer.parent
  while parent do
    if not parent.isVisible then
      return false
    end
    parent = parent.parent
  end
  return true
end

for _, item in ipairs(leafLayers) do
  if (includeHidden or is_effectively_visible(item.layer)) and
     (next(wanted) == nil or wanted[item.path]) then
    for i = fromFrame, toFrame do
      local outputName = normalize(string.gsub(outPattern, "{layer}", item.path))
      outputName = apply_frame_tokens(outputName, i)
      ensure_directory(app.fs.filePath(outputName))
      local args = {
        ui = false,
        layer = item.path,
        textureFilename = outputName,
        frameRange = tostring(i) .. "," .. tostring(i),
      }
      if scale then args.scale = scale end
      app.command.ExportSpriteSheet(args)
    end
  end
end

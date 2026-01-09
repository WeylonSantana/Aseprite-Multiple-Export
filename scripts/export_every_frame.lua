-- Export each frame as an individual image (all layers combined).
-- Usage (PowerShell):
--   Aseprite.exe -b "file.aseprite" --script-param out='export/frame-{frame}.png' ^
--     --script "scripts/export_every_frame.lua"
-- Optional params: from, to, scale, includeHidden

local spr = app.activeSprite or app.sprite
if not spr then
  error("No active sprite found. Open a sprite before running this script.")
end

local p = app.params
local outPattern = p.out or "{frame}.png"
local fromFrame = tonumber(p.fromFrame) or 1
local toFrame = tonumber(p.toFrame) or #spr.frames
local scale = tonumber(p.scale)
local includeHidden = p.includeHidden == "1" or p.includeHidden == "true"

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

local allLayers = {}
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

local function collect_layers(parent)
  for _, layer in ipairs(parent.layers) do
    table.insert(allLayers, layer)
    if is_group(layer) then
      collect_layers(layer)
    end
  end
end

collect_layers(spr)

local previousVisibility = {}
if includeHidden then
  for _, layer in ipairs(allLayers) do
    previousVisibility[layer] = layer.isVisible
    layer.isVisible = true
  end
end

local originalFrame = app.frame
for i = fromFrame, toFrame do
  local outputName = normalize(apply_frame_tokens(outPattern, i))
  ensure_directory(app.fs.filePath(outputName))
  app.frame = spr.frames[i]
  local args = {
    ui = false,
    filename = outputName,
  }
  if scale then args.scale = scale end
  app.command.SaveFileCopyAs(args)
end
app.frame = originalFrame

if includeHidden then
  for _, layer in ipairs(allLayers) do
    layer.isVisible = previousVisibility[layer]
  end
end

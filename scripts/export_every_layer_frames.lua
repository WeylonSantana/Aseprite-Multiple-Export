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
local function parse_frame_range(value)
  if not value or value == "" then
    return nil, nil
  end
  local a, b = string.match(value, "^%s*(%d+)%s*,%s*(%d+)%s*$")
  if a and b then
    return tonumber(a), tonumber(b)
  end
  return nil, nil
end

local fromFrame = tonumber(p.fromFrame or p.fromframe or p.from) or 1
local toFrame = tonumber(p.toFrame or p.toframe or p.to) or #spr.frames
local rangeFrom, rangeTo = parse_frame_range(p.frameRange or p.framerange)
if rangeFrom and rangeTo then
  fromFrame = rangeFrom
  toFrame = rangeTo
end
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

-- Export each leaf layer using a single save with frame range.

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

local allLayers = {}
local leafLayers = {}
local function collect_layers(parent, prefix)
  for _, layer in ipairs(parent.layers) do
    local path = prefix ~= "" and (prefix .. "/" .. layer.name) or layer.name
    table.insert(allLayers, layer)
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
    local normalized = string.gsub(part, "\\", "/")
    wanted[normalized] = true
  end
end

local previousVisibility = {}
for _, layer in ipairs(allLayers) do
  previousVisibility[layer] = layer.isVisible
end

local function get_parent(layer)
  local ok, val = pcall(function() return layer.parent end)
  if ok then
    return val
  end
  return nil
end

local function is_effectively_visible(layer)
  if not previousVisibility[layer] then
    return false
  end
  local parent = get_parent(layer)
  while parent do
    if previousVisibility[parent] ~= nil and not previousVisibility[parent] then
      return false
    end
    parent = get_parent(parent)
  end
  return true
end

local function set_parent_visible(layer)
  local parent = get_parent(layer)
  while parent do
    local ok = pcall(function()
      if parent.isVisible ~= nil then
        parent.isVisible = true
      end
    end)
    if not ok then
      break
    end
    parent = get_parent(parent)
  end
end

for _, item in ipairs(leafLayers) do
  if (includeHidden or is_effectively_visible(item.layer)) and
     (next(wanted) == nil or wanted[item.path]) then
    for _, layer in ipairs(allLayers) do
      layer.isVisible = false
    end

    item.layer.isVisible = true
    set_parent_visible(item.layer)
    if app.refresh then app.refresh() end

    local outputName = normalize(string.gsub(outPattern, "{layer}", item.path))
    ensure_directory(app.fs.filePath(outputName))
    local args = {
      ui = false,
      filename = outputName,
      fromFrame = fromFrame,
      toFrame = toFrame,
    }
    if scale then args.scale = scale end
    app.command.SaveFileCopyAs(args)
  end
end

for _, layer in ipairs(allLayers) do
  layer.isVisible = previousVisibility[layer]
end

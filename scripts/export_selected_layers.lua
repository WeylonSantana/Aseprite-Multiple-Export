-- Export only selected layers (combined).
-- Usage (PowerShell):
--   Aseprite.exe -b "file.aseprite" --script-param layers='Group/LayerA|LayerB' ^
--     --script-param mode=sheet --script-param out='export/selected.png' ^
--     --script "scripts/export_selected_layers.lua"
-- Optional params: mode (sheet|frames), data, type, columns, rows, scale, from, to

local spr = app.activeSprite or app.sprite
if not spr then
  error("No active sprite found. Open a sprite before running this script.")
end

local p = app.params
local outPattern = p.out or "selected.png"
local dataName = p.data or ""
local sheetType = p.type or "packed"
local columns = tonumber(p.columns)
local rows = tonumber(p.rows)
local scale = tonumber(p.scale)
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
local mode = p.mode or "sheet"
local layersParam = p.layers or ""

if layersParam == "" then
  error("Missing 'layers' script parameter.")
end

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

-- Export selected layers either as frames or as a spritesheet.

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

local wanted = {}
for part in string.gmatch(layersParam, "[^|]+") do
  local normalized = string.gsub(part, "\\", "/")
  wanted[normalized] = true
end

local allLayers = {}
local layerMap = {}
local function collect_layers(parent, prefix)
  for _, layer in ipairs(parent.layers) do
    local path = prefix ~= "" and (prefix .. "/" .. layer.name) or layer.name
    layerMap[path] = layer
    table.insert(allLayers, layer)
    if is_group(layer) then
      collect_layers(layer, path)
    end
  end
end

collect_layers(spr, "")

local previousVisibility = {}
for _, layer in ipairs(allLayers) do
  previousVisibility[layer] = layer.isVisible
  layer.isVisible = false
end

local function set_subtree_visible(layer)
  layer.isVisible = true
  if is_group(layer) then
    for _, child in ipairs(layer.layers) do
      set_subtree_visible(child)
    end
  end
end

local function set_parent_visible(layer)
  local parent = layer.parent
  while parent do
    local ok = pcall(function()
      if parent.isVisible ~= nil then
        parent.isVisible = true
      end
    end)
    if not ok then
      break
    end
    parent = parent.parent
  end
end

for path, _ in pairs(wanted) do
  local layer = layerMap[path]
  if layer then
    set_subtree_visible(layer)
    set_parent_visible(layer)
  end
end
if app.refresh then app.refresh() end

if mode == "frames" then
  local outputName = normalize(outPattern)
  ensure_directory(app.fs.filePath(outputName))
  local args = {
    ui = false,
    filename = outputName,
    fromFrame = fromFrame,
    toFrame = toFrame,
  }
  if scale then args.scale = scale end
  app.command.SaveFileCopyAs(args)
else
  local outputName = normalize(outPattern)
  ensure_directory(app.fs.filePath(outputName))

  local args = {
    ui = false,
    textureFilename = outputName,
    type = sheetType,
  }

  if columns then args.columns = columns end
  if rows then args.rows = rows end
  if scale then args.scale = scale end
  if fromFrame and toFrame then
    args.fromFrame = fromFrame
    args.toFrame = toFrame
    args.frameRange = tostring(fromFrame) .. "," .. tostring(toFrame)
  end

  if dataName ~= "" then
    dataName = normalize(dataName)
    ensure_directory(app.fs.filePath(dataName))
    args.dataFilename = dataName
    args.listLayers = true
    args.listTags = true
    args.dataFormat = "json-array"
  end

  app.command.ExportSpriteSheet(args)
end

for _, layer in ipairs(allLayers) do
  layer.isVisible = previousVisibility[layer]
end

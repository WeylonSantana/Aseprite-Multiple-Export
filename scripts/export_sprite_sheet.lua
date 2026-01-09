-- Export a single spritesheet.
-- Usage (PowerShell):
--   Aseprite.exe -b "file.aseprite" --script-param out='export/sheet.png' ^
--     --script-param type=packed --script "scripts/export_sprite_sheet.lua"
-- Optional params: data, type, columns, rows, scale, from, to, includeHidden

local spr = app.activeSprite or app.sprite
if not spr then
  error("No active sprite found. Open a sprite before running this script.")
end

local p = app.params
local outName = p.out or "sheet.png"
local dataName = p.data or ""
local sheetType = p.type or "packed"
local columns = tonumber(p.columns)
local rows = tonumber(p.rows)
local scale = tonumber(p.scale)
local fromFrame = tonumber(p.fromFrame)
local toFrame = tonumber(p.toFrame)
local includeHidden = p.includeHidden == "1" or p.includeHidden == "true"

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

outName = normalize(outName)
ensure_directory(app.fs.filePath(outName))

local args = {
  ui = false,
  textureFilename = outName,
  type = sheetType,
}

if columns then args.columns = columns end
if rows then args.rows = rows end
if scale then args.scale = scale end
if fromFrame and toFrame then
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

if includeHidden then
  for _, layer in ipairs(allLayers) do
    layer.isVisible = previousVisibility[layer]
  end
end

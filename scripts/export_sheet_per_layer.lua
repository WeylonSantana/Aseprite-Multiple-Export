-- Export one spritesheet per layer.
-- Usage (PowerShell):
--   Aseprite.exe -b "file.aseprite" --script-param out='export/{layer}.png' ^
--     --script-param type=packed --script "scripts/export_sheet_per_layer.lua"
-- Optional params: data, type, columns, rows, scale, includeHidden, layers

local spr = app.activeSprite or app.sprite
if not spr then
  error("No active sprite found. Open a sprite before running this script.")
end

local p = app.params
local outPattern = p.out or "{layer}.png"
local dataPattern = p.data or ""
local sheetType = p.type or "packed"
local columns = tonumber(p.columns)
local rows = tonumber(p.rows)
local scale = tonumber(p.scale)
local includeHidden = p.includeHidden == "1" or p.includeHidden == "true"
local layersParam = p.layers or ""

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

local layers = {}
local function collect_layers(parent, prefix)
  for _, layer in ipairs(parent.layers) do
    local path = prefix ~= "" and (prefix .. "/" .. layer.name) or layer.name
    if is_group(layer) then
      collect_layers(layer, path)
    else
      if includeHidden or layer.isVisible then
        table.insert(layers, { layer = layer, path = path })
      end
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
if includeHidden then
  for _, item in ipairs(layers) do
    previousVisibility[item.layer] = item.layer.isVisible
    item.layer.isVisible = true
  end
end

for _, item in ipairs(layers) do
  if next(wanted) ~= nil and not wanted[item.path] then
    goto continue
  end

  local outputName = normalize(string.gsub(outPattern, "{layer}", item.path))
  ensure_directory(app.fs.filePath(outputName))

  local args = {
    ui = false,
    layer = item.path,
    textureFilename = outputName,
    type = sheetType,
  }

  if columns then args.columns = columns end
  if rows then args.rows = rows end
  if scale then args.scale = scale end
  if dataPattern ~= "" then
    local dataName = normalize(string.gsub(dataPattern, "{layer}", item.path))
    ensure_directory(app.fs.filePath(dataName))
    args.dataFilename = dataName
    args.listLayers = true
    args.listTags = true
    args.dataFormat = "json-array"
  end

  app.command.ExportSpriteSheet(args)
  ::continue::
end

if includeHidden then
  for _, item in ipairs(layers) do
    item.layer.isVisible = previousVisibility[item.layer]
  end
end

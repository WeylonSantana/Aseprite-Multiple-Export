-- Export a single spritesheet with Resize & Clone support.
-- Usage (PowerShell):
--   Aseprite.exe -b "file.aseprite" --script-param out='export/sheet.png' ^
--     --script-param type=packed --script "scripts/export_sprite_sheet.lua"
-- Optional params: data, type, columns, rows, scale, includeHidden

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
local scale = tonumber(p.scale) or 1 -- Valor padrão 1 se não informado
local includeHidden = p.includeHidden == "1" or p.includeHidden == "true"

local sep = app.fs.pathSeparator
local function normalize(path)
  if sep == "\\" then
    return string.gsub(path, "/", "\\")
  end
  return string.gsub(path, "\\", "/")
end

local function ensure_directory(path)
  if not path or path == "" then return end
  local built = ""
  for part in string.gmatch(path, "[^" .. sep .. "]+") do
    if built == "" then built = part else built = built .. sep .. part end
    if not app.fs.isDirectory(built) then
      pcall(app.fs.makeDirectory, built)
    end
  end
end

-- Função auxiliar para visibilidade recursiva
local function show_all_layers(layers)
  for _, layer in ipairs(layers) do
    layer.isVisible = true
    if layer.isGroup then
      show_all_layers(layer.layers)
    end
  end
end

-- =======================================================
-- LÓGICA PRINCIPAL (CLONE -> MODIFY -> EXPORT -> DELETE)
-- =======================================================

-- 1. Cria um clone para não afetar o sprite original
local exportSpr = Sprite(spr)
app.activeSprite = exportSpr -- Foca no clone para o comando funcionar nele

-- 2. Aplica visibilidade no clone se solicitado
if includeHidden then
  show_all_layers(exportSpr.layers)
end

-- 3. Redimensiona o sprite manualmente (Resolve o bug de scaling da API)
if scale and scale ~= 1 then
  local newWidth = math.floor(exportSpr.width * scale)
  local newHeight = math.floor(exportSpr.height * scale)
  exportSpr:resize(newWidth, newHeight)
end

-- 4. Prepara caminhos
outName = normalize(outName)
ensure_directory(app.fs.filePath(outName))

local args = {
  ui = false,
  textureFilename = outName,
  type = sheetType,
}

if columns then args.columns = columns end
if rows then args.rows = rows end

if dataName ~= "" then
  dataName = normalize(dataName)
  ensure_directory(app.fs.filePath(dataName))
  args.dataFilename = dataName
  args.listLayers = true
  args.listTags = true
  args.dataFormat = "json-array"
end

-- 5. Exporta
app.command.ExportSpriteSheet(args)

-- 6. Limpeza
exportSpr:close()       -- Fecha o clone sem salvar
app.activeSprite = spr  -- Devolve o foco ao sprite original
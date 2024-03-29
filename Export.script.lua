local filename = app.params["filename"]
local scale = app.params["scale"]
local columns = app.params["columns"]
local outputName = app.params["outputName"]
local everyLayer = app.params["everyLayer"]
local layerList = app.params["layerList"]
local exportData = app.params["exportData"]

local sprite = app.open(filename)
if sprite == nil then
    return app.alert("Could not open sprite")
end

local exportDirectory = scale .. "x/"
app.command.SpriteSize {
  ui = false,
  scale = scale,
  method = "nearest"
}

local function exportLayer(layer)
  local layerName = ""
  if layer then
    layerName = layer.name
  end
  local outputPath = exportDirectory .. outputName
  if layerName ~= "" then
    outputPath = outputPath .. "-" .. layerName
  end

  local outputFileName = outputPath .. ".png"
  local outputDataName = exportData and (outputPath .. ".json") or ""

  app.command.ExportSpriteSheet {
    ui = false,
    askOverwrite = false,
    ignoreEmpty = true,
    type = SpriteSheetType.ROWS,
    columns = columns,
    sprite = sprite,
    layer = layerName,
    textureFilename = outputFileName,
    dataFilename = outputDataName ~= "" and outputDataName or nil
  }
end

local exported = false
for _, layer in ipairs(sprite.layers) do
  layer.isVisible = true

  if everyLayer then
    exportLayer(layer)
    exported = true
  else
    if layerList and string.find(layerList, layer.name) then
      exportLayer(layer)
      exported = true
    end
  end
end

if not exported then
  exportLayer(nil)
end
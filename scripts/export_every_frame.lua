-- Export each frame as an individual image (all layers combined).
-- Usage (PowerShell):
--   Aseprite.exe -b "file.aseprite" --script-param out='export/frame-{frame}.png' ^
--     --script "scripts/export_every_frame.lua"
-- Optional params: fromFrame, toFrame, scale, includeHidden

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

-- Single save with filename pattern to export the frame range.

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

if includeHidden then
  for _, layer in ipairs(allLayers) do
    layer.isVisible = previousVisibility[layer]
  end
end

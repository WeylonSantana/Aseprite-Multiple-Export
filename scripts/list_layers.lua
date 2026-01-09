-- Write the layer list to a JSON file.
-- Parameters (passed via --script-param):
--   out=path/to/file.json
--   includeHidden=true|false

local spr = app.activeSprite or app.sprite
if not spr then
  error("No active sprite found. Open a sprite before running this script.")
end

local params = app.params
local outPath = params.out
if not outPath or outPath == "" then
  error("Missing 'out' script parameter.")
end

local includeHidden = params.includeHidden == "1" or params.includeHidden == "true"
local logPath = params.logPath or params.logpath or "output.txt"

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

local function log(message)
  local path = normalize(logPath)
  ensure_directory(app.fs.filePath(path))
  local f = io.open(path, "a")
  if f then
    f:write(message, "\n")
    f:close()
  end
end

log("list_layers.lua start")
log("out=" .. tostring(outPath))
log("includeHidden=" .. tostring(includeHidden))
log("logPath=" .. tostring(logPath))

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

local function json_escape(text)
  local escaped = string.gsub(text, "\\", "\\\\")
  escaped = string.gsub(escaped, "\"", "\\\"")
  escaped = string.gsub(escaped, "\n", "\\n")
  escaped = string.gsub(escaped, "\r", "\\r")
  escaped = string.gsub(escaped, "\t", "\\t")
  return escaped
end

local layers = {}

local function collect_layers(parent, prefix)
  log("collect_layers prefix=" .. tostring(prefix))
  for _, layer in ipairs(parent.layers) do
    log("layer=" .. tostring(layer.name) .. " visible=" .. tostring(layer.isVisible))
    if includeHidden or layer.isVisible then
      local groupPath = nil
      if prefix ~= "" then
        groupPath = prefix
      end

      table.insert(layers, { name = layer.name, group = groupPath })
      log("added layer name=" .. tostring(layer.name) .. " group=" .. tostring(groupPath))
      if is_group(layer) then
        local nextPrefix = prefix ~= "" and (prefix .. "/" .. layer.name) or layer.name
        log("descending into group " .. tostring(nextPrefix))
        collect_layers(layer, nextPrefix)
      end
    end
  end
end

collect_layers(spr, "")
log("total layers=" .. tostring(#layers))

local f = io.open(outPath, "w")
if not f then
  error("Unable to open output file: " .. outPath)
end

f:write("{\"frames\":[],\"meta\":{\"layers\":[")
for i, layer in ipairs(layers) do
  if i > 1 then
    f:write(",")
  end
  f:write("{\"name\":\"", json_escape(layer.name), "\"")
  if layer.group ~= nil then
    f:write(",\"group\":\"", json_escape(layer.group), "\"")
  end
  f:write("}")
end
f:write("]}}")
f:close()
log("list_layers.lua done")

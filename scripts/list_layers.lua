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
  for _, layer in ipairs(parent.layers) do
    if includeHidden or layer.isVisible then
      local groupPath = nil
      if prefix ~= "" then
        groupPath = prefix
      end

      table.insert(layers, { name = layer.name, group = groupPath })
      if is_group(layer) then
        local nextPrefix = prefix ~= "" and (prefix .. "/" .. layer.name) or layer.name
        collect_layers(layer, nextPrefix)
      end
    end
  end
end

collect_layers(spr, "")

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

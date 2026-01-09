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
    return true
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

local function collect_layers(parent)
  for _, layer in ipairs(parent.layers) do
    if includeHidden or layer.isVisible then
      local groupName = nil
      if layer.parent and is_group(layer.parent) then
        groupName = layer.parent.name
      end

      table.insert(layers, { name = layer.name, group = groupName })
      if is_group(layer) then
        collect_layers(layer)
      end
    end
  end
end

collect_layers(spr)

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

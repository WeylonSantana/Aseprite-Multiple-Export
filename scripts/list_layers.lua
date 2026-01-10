-- list_layers.lua
--
-- Purpose:
--   Print layer paths to stdout for the UI to consume.
--
-- Params:
--   includeHidden=true|false
--
-- Output:
--   Each layer path is printed as: LAYER:<path>

local spr = app.activeSprite or app.sprite
if not spr then
  error("No active sprite found. Open a sprite before running this script.")
end

local includeHidden = app.params.includeHidden == "1" or app.params.includeHidden == "true"

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

local function print_layer(path)
  print("LAYER:" .. path)
end

local function collect_layers(parent, prefix)
  for _, layer in ipairs(parent.layers) do
    if includeHidden or layer.isVisible then
      local path = prefix ~= "" and (prefix .. "/" .. layer.name) or layer.name
      print_layer(path)
      if is_group(layer) then
        collect_layers(layer, path)
      end
    end
  end
end

collect_layers(spr, "")

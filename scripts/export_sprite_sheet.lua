-- export_sprite_sheet.lua
--
-- Purpose:
--   Export sprite sheets using a layer list, optionally per-layer.
--
-- Usage (CLI example):
--   Aseprite.exe -b "file.ase" \
--     --script-param out="C:/path/sheet.png" \
--     --script-param type="packed" \
--     --script-param scale="1" \
--     --script-param includeHidden="true" \
--     --script-param fromFrame="1" \
--     --script-param toFrame="8" \
--     --script-param layers="LayerA|Group/LayerB" \
--     --script-param everyLayer="false" \
--     --script-param data="C:/path/sheet.json" \
--     --script "C:/path/export_sprite_sheet.lua"
--
-- Notes:
--   - The script clones the sprite, optionally trims frames, optionally resizes,
--     and exports a sprite sheet.
--   - If everyLayer=true, it exports one sheet per layer.
--   - If everyLayer=false, it combines the selected layers into a single sheet.
--   - All output is printed to the console (no file logging).

local function run_script()
  local sep = app.fs.pathSeparator

  local function normalize(path)
    if sep == "\\" then return string.gsub(path, "/", "\\") end
    return string.gsub(path, "\\", "/")
  end

  local function ensure_directory(path)
    if not path or path == "" then return end
    local built = ""
    for part in string.gmatch(path, "[^" .. sep .. "]+") do
      if built == "" then built = part else built = built .. sep .. part end
      if not app.fs.isDirectory(built) then pcall(app.fs.makeDirectory, built) end
    end
  end

  local function is_group(layer)
    local ok, val = pcall(function() return layer.isGroup end)
    if ok and type(val) == "boolean" then return val end
    local ok2, val2 = pcall(function() return layer.layers end)
    if ok2 and type(val2) == "table" then return next(val2) ~= nil end
    return false
  end

  local spr = app.activeSprite or app.sprite
  local outPattern = app.params.out
  local dataPattern = app.params.data or ""
  local sheetType = app.params.type or "packed"
  local columns = tonumber(app.params.columns)
  local rows = tonumber(app.params.rows)
  local includeHidden = app.params.includeHidden == "1" or app.params.includeHidden == "true"
  local layersParam = app.params.layers or ""
  local everyLayer = app.params.everyLayer == "1" or app.params.everyLayer == "true"
  local fromFrameArg = tonumber(app.params.fromFrame) or 1
  local toFrameArg = tonumber(app.params.toFrame) or (spr and #spr.frames or 1)

  if spr == nil or outPattern == nil or outPattern == "" then
    error("Missing required params (sprite/out).")
  end

  print("[Lua] === Export Sprite Sheet ===")
  print("[Lua] Start time: " .. os.date("%Y-%m-%d %H:%M:%S"))
  print("[Lua] Output pattern: " .. outPattern)
  print("[Lua] Data pattern: " .. tostring(dataPattern))
  print("[Lua] Sheet type: " .. tostring(sheetType))
  print("[Lua] Columns: " .. tostring(columns))
  print("[Lua] Rows: " .. tostring(rows))
  print("[Lua] Include hidden: " .. tostring(includeHidden))
  print("[Lua] Layers param: " .. tostring(layersParam))
  print("[Lua] Every layer: " .. tostring(everyLayer))
  print("[Lua] Frame range (requested): " .. tostring(fromFrameArg) .. " - " .. tostring(toFrameArg))

  local function main()
    print("[Lua] Cloning sprite...")
    local exportSpr = Sprite(spr)
    app.activeSprite = exportSpr

    local initialFrames = #exportSpr.frames
    print("[Lua] Clone frame count: " .. tostring(initialFrames))

    if fromFrameArg and toFrameArg then
      if fromFrameArg < 1 then fromFrameArg = 1 end
      if toFrameArg > initialFrames then toFrameArg = initialFrames end
      print("[Lua] Trimming frames to: " .. tostring(fromFrameArg) .. " - " .. tostring(toFrameArg))
      for i = initialFrames, 1, -1 do
        if i < fromFrameArg or i > toFrameArg then
          exportSpr:deleteFrame(exportSpr.frames[i])
        end
      end
    end

    local layerMap = {}
    local function map_layers(parent, prefix)
      for _, layer in ipairs(parent.layers) do
        local path = prefix ~= "" and (prefix .. "/" .. layer.name) or layer.name
        layerMap[path] = layer
        if is_group(layer) then
          map_layers(layer, path)
        end
      end
    end

    map_layers(exportSpr, "")

    local layersToExport = {}
    if layersParam ~= "" then
      for part in string.gmatch(layersParam, "[^|]+") do
        local normalized = string.gsub(part, "\\", "/")
        local layer = layerMap[normalized]
        if not layer then
          print("[Lua] WARNING: layer not found: " .. normalized)
        elseif is_group(layer) then
          print("[Lua] Skipping group in layers param: " .. normalized)
        else
          table.insert(layersToExport, { layer = layer, path = normalized })
        end
      end
    end

    print("[Lua] Layers to export: " .. tostring(#layersToExport))

    if #layersToExport == 0 then
      print("[Lua] ERROR: no layers to export.")
      exportSpr:close()
      app.activeSprite = spr
      return
    end

    local function hide_all_non_group(parent)
      for _, layer in ipairs(parent.layers) do
        if is_group(layer) then
          layer.isVisible = true
          hide_all_non_group(layer)
        else
          layer.isVisible = false
        end
      end
    end

    local function apply_visibility(layer, keep)
      local visible = keep[layer] == true
      if is_group(layer) then
        local childVisible = false
        for _, child in ipairs(layer.layers) do
          if apply_visibility(child, keep) then
            childVisible = true
          end
        end
        visible = visible or childVisible
      end
      layer.isVisible = visible
      return visible
    end

    local baseOutput = normalize(outPattern)
    local baseData = dataPattern ~= "" and normalize(dataPattern) or ""

    if everyLayer and string.find(baseOutput, "{layer}") == nil then
      print("[Lua] WARNING: output pattern does not include {layer}; files will be overwritten.")
    end

    if everyLayer then
      hide_all_non_group(exportSpr)

      local previousLayer = nil
      for _, item in ipairs(layersToExport) do
        if previousLayer then
          previousLayer.isVisible = false
        end

        item.layer.isVisible = true

        local outputName = baseOutput
        if string.find(outputName, "{layer}") then
          outputName = string.gsub(outputName, "{layer}", item.path)
        end

        ensure_directory(app.fs.filePath(outputName))

        local args = {
          ui = false,
          textureFilename = outputName,
          type = sheetType
        }

        if columns then args.columns = columns end
        if rows then args.rows = rows end

        if baseData ~= "" then
          local dataName = baseData
          if string.find(dataName, "{layer}") then
            dataName = string.gsub(dataName, "{layer}", item.path)
          end
          ensure_directory(app.fs.filePath(dataName))
          args.dataFilename = dataName
          args.listLayers = true
          args.listTags = true
          args.dataFormat = "json-array"
        end

        print("[Lua] Exporting layer sheet: " .. item.path)
        app.command.ExportSpriteSheet(args)

        previousLayer = item.layer
      end

      if previousLayer then
        previousLayer.isVisible = false
      end
    else
      local outputName = baseOutput
      if string.find(outputName, "{layer}") then
        print("[Lua] WARNING: output pattern includes {layer} in combined mode; replacing with 'combined'.")
        outputName = string.gsub(outputName, "{layer}", "combined")
      end
      ensure_directory(app.fs.filePath(outputName))

      print("[Lua] Building combined sprite for sheet export...")
      local tempSpr = Sprite(exportSpr.width, exportSpr.height, exportSpr.colorMode)
      local tempLayer = tempSpr.layers[1]

      while #tempSpr.frames < #exportSpr.frames do
        tempSpr:newFrame()
      end

      for i = 1, #exportSpr.frames do
        local frameImage = Image(exportSpr.spec)
        frameImage:clear()
        local drawn = false

        for _, item in ipairs(layersToExport) do
          local cel = item.layer:cel(i)
          if cel then
            frameImage:drawImage(cel.image, cel.position)
            drawn = true
          end
        end

        if drawn then
          tempSpr:newCel(tempLayer, i, frameImage, Point(0, 0))
        end
      end

      local args = {
        ui = false,
        textureFilename = outputName,
        type = sheetType
      }

      if columns then args.columns = columns end
      if rows then args.rows = rows end

      if baseData ~= "" then
        local dataName = baseData
        ensure_directory(app.fs.filePath(dataName))
        args.dataFilename = dataName
        args.listLayers = true
        args.listTags = true
        args.dataFormat = "json-array"
      end

      print("[Lua] Exporting combined sheet...")
      app.activeSprite = tempSpr
      app.command.ExportSpriteSheet(args)
      tempSpr:close()
      app.activeSprite = exportSpr
    end

    exportSpr:close()
    app.activeSprite = spr
    print("[Lua] Done.")
  end

  local status, err = xpcall(main, debug.traceback)
  print("[Lua] End time: " .. os.date("%Y-%m-%d %H:%M:%S"))

  if not status then
    print("[Lua] ERROR: script failed.")
    print("[Lua] " .. tostring(err))
    if app.activeSprite and app.activeSprite ~= spr then
      pcall(function() app.activeSprite:close() end)
    end
    app.activeSprite = spr
  end
end

run_script()

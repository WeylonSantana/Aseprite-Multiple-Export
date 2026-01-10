-- export_every_frame_every_layer.lua
--
-- Purpose:
--   Export frames using a layer list, optionally per-layer.
--
-- Usage (CLI example):
--   Aseprite.exe -b "file.ase" \
--     --script-param out="C:/path/{layer}_{frame}.png" \
--     --script-param scale="1" \
--     --script-param includeHidden="true" \
--     --script-param fromFrame="1" \
--     --script-param toFrame="8" \
--     --script "C:/path/export_every_frame_every_layer.lua"
--
-- Notes:
--   - The script clones the sprite, optionally forces all layers visible,
--     optionally trims the frame range, and optionally resizes.
--   - If everyLayer=true, it exports each layer separately.
--   - If everyLayer=false, it combines the selected layers, flattens, and exports.
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
  local layersParam = app.params.layers or ""
  local everyLayer = app.params.everyLayer == "1" or app.params.everyLayer == "true"
  local scale = tonumber(app.params.scale) or 1
  local includeHidden = app.params.includeHidden == "1" or app.params.includeHidden == "true"
  local fromFrameArg = tonumber(app.params.fromFrame) or 1
  local toFrameArg = tonumber(app.params.toFrame) or (spr and #spr.frames or 1)

  if spr == nil or outPattern == nil or outPattern == "" then
    error("Missing required params (sprite/out).")
  end

  print("[Lua] === Export Every Frame ===")
  print("[Lua] Start time: " .. os.date("%Y-%m-%d %H:%M:%S"))
  print("[Lua] Output pattern: " .. outPattern)
  print("[Lua] Layers param: " .. tostring(layersParam))
  print("[Lua] Every layer: " .. tostring(everyLayer))
  print("[Lua] Scale: " .. tostring(scale))
  print("[Lua] Include hidden: " .. tostring(includeHidden))
  print("[Lua] Frame range (requested): " .. tostring(fromFrameArg) .. " - " .. tostring(toFrameArg))
  print("[Lua] Layers param length: " .. tostring(#layersParam))

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

    if scale and scale ~= 1 then
      local newW = math.floor(exportSpr.width * scale)
      local newH = math.floor(exportSpr.height * scale)
      print("[Lua] Resizing clone: " .. exportSpr.width .. "x" .. exportSpr.height .. " -> " .. newW .. "x" .. newH)
      exportSpr:resize(newW, newH)
    end

    local finalFrames = #exportSpr.frames
    print("[Lua] Final frame count: " .. tostring(finalFrames))

    if finalFrames <= 0 then
      print("[Lua] ERROR: no frames to export.")
      exportSpr:close()
      app.activeSprite = spr
      return
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
    local layerMapCount = 0
    for _ in pairs(layerMap) do layerMapCount = layerMapCount + 1 end
    print("[Lua] Layer map count: " .. tostring(layerMapCount))

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

    local function build_frame_filename(outputName, frameIndex)
      if string.find(outputName, "{frame}") then
        return string.gsub(outputName, "{frame}", tostring(frameIndex))
      end

      local extStart = string.find(outputName, "%.[^/.]*$")
      if extStart then
        return string.sub(outputName, 1, extStart - 1) .. "_" .. frameIndex .. string.sub(outputName, extStart)
      end

      return outputName .. "_" .. frameIndex
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

    local baseOutput = normalize(outPattern)
    if everyLayer and string.find(baseOutput, "{layer}") == nil then
      print("[Lua] WARNING: output pattern does not include {layer}; files will be overwritten.")
    end

    if everyLayer then
      hide_all_non_group(exportSpr)

      local previousLayer = nil
      for index, item in ipairs(layersToExport) do
        if previousLayer then
          previousLayer.isVisible = false
        end

        item.layer.isVisible = true

        local outputName = baseOutput
        if string.find(outputName, "{layer}") then
          outputName = string.gsub(outputName, "{layer}", item.path)
        end

        ensure_directory(app.fs.filePath(outputName))
        print("[Lua] Exporting layer (" .. tostring(index) .. "/" .. tostring(#layersToExport) .. "): " .. item.path)

        local savedFrames = 0
        local emptyFrames = 0
        for i = 1, finalFrames do
          local currentFile = build_frame_filename(outputName, i)
          local cel = item.layer:cel(i)
          if cel then
            local finalImage = Image(exportSpr.spec)
            finalImage:clear()
            finalImage:drawImage(cel.image, cel.position)
            finalImage:saveAs(currentFile)
            savedFrames = savedFrames + 1
            print("[Lua] Frame " .. tostring(i) .. " saved: " .. currentFile)
          else
            emptyFrames = emptyFrames + 1
            print("[Lua] Frame " .. tostring(i) .. " empty (no cel).")
          end
        end
        print("[Lua] Layer summary: saved=" .. tostring(savedFrames) .. ", empty=" .. tostring(emptyFrames))

        previousLayer = item.layer
      end

      if previousLayer then
        previousLayer.isVisible = false
      end
    else
      local outputName = baseOutput
      ensure_directory(app.fs.filePath(outputName))
      print("[Lua] Exporting combined layers...")

      local savedFrames = 0
      local emptyFrames = 0
      for i = 1, finalFrames do
        local currentFile = build_frame_filename(outputName, i)
        local finalImage = Image(exportSpr.spec)
        finalImage:clear()

        local drawn = false
        for _, item in ipairs(layersToExport) do
          local cel = item.layer:cel(i)
          if cel then
            finalImage:drawImage(cel.image, cel.position)
            drawn = true
          end
        end

        if drawn then
          finalImage:saveAs(currentFile)
          savedFrames = savedFrames + 1
          print("[Lua] Frame " .. tostring(i) .. " saved: " .. currentFile)
        else
          emptyFrames = emptyFrames + 1
          print("[Lua] Frame " .. tostring(i) .. " empty (no cels).")
        end
      end
      print("[Lua] Combined summary: saved=" .. tostring(savedFrames) .. ", empty=" .. tostring(emptyFrames))
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

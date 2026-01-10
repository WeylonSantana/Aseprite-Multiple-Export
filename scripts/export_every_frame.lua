-- export_every_frame.lua
--
-- Purpose:
--   Export a sprite animation to individual frame files using Aseprite CLI.
--
-- Usage (CLI example):
--   Aseprite.exe -b "file.ase" \
--     --script-param out="C:/path/name_{frame}.png" \
--     --script-param scale="1" \
--     --script-param includeHidden="true" \
--     --script-param fromFrame="1" \
--     --script-param toFrame="8" \
--     --script "C:/path/export_every_frame.lua"
--
-- Notes:
--   - The script clones the sprite, optionally forces all layers visible,
--     optionally trims the frame range, and optionally resizes.
--   - It then flattens the clone and calls SaveFileCopyAs once with
--     filenameFormat to export all frames.
--   - All output is printed to the console (no file logging).

local function run_script()
  local sep = app.fs.pathSeparator

  local function normalize(path)
    if sep == "\\" then return string.gsub(path, "/", "\\") end
    return string.gsub(path, "\\", "/")
  end

  local spr = app.activeSprite or app.sprite
  local outPattern = app.params.out
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
  print("[Lua] Scale: " .. tostring(scale))
  print("[Lua] Include hidden: " .. tostring(includeHidden))
  print("[Lua] Frame range (requested): " .. tostring(fromFrameArg) .. " - " .. tostring(toFrameArg))

  local function ensure_directory(path)
    if not path or path == "" then return end
    local built = ""
    for part in string.gmatch(path, "[^" .. sep .. "]+") do
      if built == "" then built = part else built = built .. sep .. part end
      if not app.fs.isDirectory(built) then pcall(app.fs.makeDirectory, built) end
    end
  end

  local function main()
    print("[Lua] Cloning sprite...")
    local exportSpr = Sprite(spr)
    app.activeSprite = exportSpr

    if includeHidden then
      local function show_all(layers)
        for _, l in ipairs(layers) do
          l.isVisible = true
          if l.isGroup then show_all(l.layers) end
        end
      end
      show_all(exportSpr.layers)
      print("[Lua] All layers set to visible.")
    end

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

    print("[Lua] Flattening layers...")
    app.command.FlattenLayers{ visibleOnly = true }

    if scale and scale ~= 1 then
      local newW = math.floor(exportSpr.width * scale)
      local newH = math.floor(exportSpr.height * scale)
      print("[Lua] Resizing clone: " .. exportSpr.width .. "x" .. exportSpr.height .. " -> " .. newW .. "x" .. newH)
      exportSpr:resize(newW, newH)
    end

    local finalFrames = #exportSpr.frames
    print("[Lua] Final frame count: " .. tostring(finalFrames))

    if finalFrames > 0 then
      local baseOutput = normalize(outPattern)
      if baseOutput == "" then
        error("Output pattern is empty.")
      end
      ensure_directory(app.fs.filePath(baseOutput))

      local args = {
        ui = false,
        filename = baseOutput,
        filenameFormat = baseOutput,
        fromFrame = 1,
        toFrame = finalFrames
      }

      print("[Lua] Saving frames using filenameFormat...")
      app.command.SaveFileCopyAs(args)
      print("[Lua] Export completed successfully.")
    else
      print("[Lua] ERROR: no frames to export.")
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

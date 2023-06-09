local sprite = app.open(app.params["filename"])
if not sprite then
    error("Could not open sprite")
end

local frames = app.params["frames"]
local maxFrames = #sprite.frames
if not maxFrames then
    error("Could not read the number of frames")
end

local deletedFrames = 0
for i in string.gmatch(frames, "%d+") do
    local frameNumber = tonumber(i)
    if frameNumber then
        frameNumber = frameNumber - deletedFrames
        if frameNumber <= maxFrames then
            sprite:deleteFrame(frameNumber)
            deletedFrames = deletedFrames + 1
        end
    end
end

app.command.SaveFile()
app.command.CloseFile()

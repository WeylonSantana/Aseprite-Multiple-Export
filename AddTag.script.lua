local filename = app.params["filename"]
local tagname = app.params["tag"]
local startFrame = tonumber(app.params["start"])
local endFrame = tonumber(app.params["end"])

local sprite = app.open(filename)
if sprite == nil then
    return app.alert("Could not open sprite")
end

local frameTag = sprite:newTag()
frameTag.name = tagname
frameTag.fromFrame = startFrame
frameTag.toFrame = endFrame

app.command.SaveFile()
app.command.CloseFile()

local filename = app.params["filename"]
local tags = app.params["tags"]
local clean = app.params["clean"]

local sprite = app.open(filename)
if sprite == nil then
    return app.alert("Could not open sprite")
end

if clean then
	for i = #sprite.tags, 1, -1 do
        sprite:deleteTag(sprite.tags[i])
    end
end

local totalFrames = #sprite.frames

local localTags={}
for str in string.gmatch(tags, "([^,]+)") do
    table.insert(localTags, str)
end

local tagCount = totalFrames / #localTags

for i = 1, #localTags, 1 do
	local tagname = localTags[i]
	local startFrame = (i - 1) * tagCount + 1
	local endFrame = i * tagCount

	local frameTag = sprite:newTag()
	frameTag.name = tagname
	frameTag.fromFrame = startFrame
	frameTag.toFrame = endFrame
end

app.command.SaveFile()
app.command.CloseFile()

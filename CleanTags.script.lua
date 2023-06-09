local filename = app.params["filename"]

local sprite = app.open(filename)
if sprite == nil then
    return app.alert("Could not open sprite")
end

-- Deleta todas as tags existentes
for i = #sprite.tags, 1, -1 do
    sprite:deleteTag(sprite.tags[i])
end

app.command.SaveFile()
app.command.CloseFile()

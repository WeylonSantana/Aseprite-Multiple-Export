-- Obter o parâmetro do script
local filename = app.params["filename"]

-- Abrir o sprite
local sprite = app.open(filename)
if sprite == nil then
    return app.alert("Could not open sprite")
end

-- Obter o número total de quadros
local totalFrames = #sprite.frames

-- Imprimir o número total de quadros
print(totalFrames)

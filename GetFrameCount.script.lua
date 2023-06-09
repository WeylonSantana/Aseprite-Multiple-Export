-- Obter o par�metro do script
local filename = app.params["filename"]

-- Abrir o sprite
local sprite = app.open(filename)
if sprite == nil then
    return app.alert("Could not open sprite")
end

-- Obter o n�mero total de quadros
local totalFrames = #sprite.frames

-- Imprimir o n�mero total de quadros
print(totalFrames)

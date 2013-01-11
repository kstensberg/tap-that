module ( "Helper", package.seeall )

circleCount = 0
local textures = {}
local growthScale = {}


function CreateCircle ()
	local circle = MOAIGfxQuad2D.new ()
	circle:setTexture ( getTexture("media/images/circle.png") )
	circle:setRect ( -128, -128, 128, 128 )

	local circleProp = MOAIProp2D.new ()
	circleProp:setDeck ( circle )
	width, height = circleProp:getDims()


	circleProp:setColor(math.random(), math.random(), math.random())
	--circleProp:setScl(0)

	local scale = math.random(2, 8) / 10
	circleProp.growthScale = math.random(1,3)
	local randX = math.random((-SCREEN_WIDTH / 2) + (width * scale), (SCREEN_WIDTH / 2) - (width * scale))
	local randY = math.random((-SCREEN_HEIGHT / 2) + (height * scale), (SCREEN_HEIGHT / 2) - (height * scale))


	circleProp:setLoc(randX, randY)



	circleProp:setScl(scale)
	--circleCount = circleCount + 1


	return circleProp
end	

--Simple method to reuse textures in memory
function getTexture(fileName)

        local tex = textures[fileName]
        if not tex then
            tex = MOAITexture.new()
            tex:load(fileName)
            textures[fileName] = tex
        end
        return tex

end

--Audio Stuff
MOAIUntzSystem.initialize()
MOAIUntzSystem.setVolume(1)

local noteStrings = {"a", "a-sharp", "b", "c", "c-sharp", "d", "d-sharp", "e", "f", "f-sharp", "g", "g-sharp"}
local notes = {}
local noteQueue = {}

for k,v in pairs(noteStrings) do 
	notes[v] = MOAIUntzSound.new()
	notes[v]:load("media/audio/" .. v .. ".ogg")
end

function PlayNote()
	if #noteQueue == 0 then 
		for i = 1,50 do 
			table.insert(noteQueue, noteStrings[math.random(#noteStrings)])
		end
	end

	notes[noteQueue[1]]:play()

	table.remove(noteQueue, 1)
end
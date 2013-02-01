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
	circleProp:setScl(0)

	local scale = math.random(2, 8) / 10
	circleProp.growthScale = math.random(1,3)
	local randX = math.random((-SCREEN_WIDTH / 2) + (width * scale), (SCREEN_WIDTH / 2) - (width * scale))
	local randY = math.random((-SCREEN_HEIGHT / 2) + (height * scale), (SCREEN_HEIGHT / 2) - (height * scale))


	circleProp:setLoc(randX, randY)



	circleProp:moveScl(scale, scale, 1, 1)
	circleCount = circleCount + 1


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
require "src/lib/input"
require "src/lib/helper"

local game = {}
local mainLayer
local circleCount = 0
local maxCircles = 5
local width, height
game.layerTable = nil

----------------------------------------------------------------
game.onFocus = function ( self )	 

	MOAIGfxDevice.setClearColor ( 1, 1, 1, 1 )

end

----------------------------------------------------------------
game.onInput = function ( self )

	if InputMgr:up() then
 
    	local x, y = mainLayer:wndToWorld( InputMgr:getTouch () )
 		local partition = mainLayer:getPartition()
 		local pickedProp = partition:propForPoint(x, y)
 		if pickedProp then
 			Helper.PlayNote()
			mainLayer:removeProp(pickedProp)
			mainLayer:insertProp(Helper.CreateCircle())	
		end

  	end


end

----------------------------------------------------------------
game.onLoad = function ( self )

	self.layerTable = {}
	local layer = MOAILayer2D.new ()
	layer:setViewport ( viewport )
	game.layerTable [ 1 ] = { layer }
	
	layer:insertProp(Helper.CreateCircle())	

	mainLayer = layer




end

----------------------------------------------------------------
game.onLoseFocus = function ( self )
end

----------------------------------------------------------------
game.onUnload = function ( self )

	for i, layerSet in ipairs ( self.layerTable ) do

		for j, layer in ipairs ( layerSet ) do

			layer:clear ()
			layer = nil
		end
	end

	self.layerTable = nil
end

----------------------------------------------------------------
game.onUpdate = function ( self )


end

return game

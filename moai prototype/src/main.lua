require "src/lib/state"


-- open window
MOAISim.openWindow ( "Tap That!", SCREEN_WIDTH, SCREEN_HEIGHT )

viewport = MOAIViewport.new ()
viewport:setSize ( 0, 0, SCREEN_WIDTH, SCREEN_HEIGHT )
viewport:setScale ( SCREEN_UNITS_X, SCREEN_UNITS_Y )

-- open scene
StateMgr.push('src/scenes/game.lua')

StateMgr.begin()

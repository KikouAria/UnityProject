local gamePlaying = {}
local ai = require("monsterai")

-- 游戏主入口
function gamePlaying:GameStart()
	print("游戏开始")

	local player = Player.New(0,0,0)

	ai:Start(player)
end

-- 游戏定时器
function gamePlaying:GameTimer()
	ai:OnTimer()
end

-- 击中目标
function gamePlaying:onShootHit(uid)
	ai:onShootHit(uid)
end


return gamePlaying;



local monsterAi = {}

monsterAi.monstersList = {}

-- 启动AI
function monsterAi:Start(p)
	print("AI开始")

	self.player = p

	local j = 1
	for i=1, 10 do
		local x = math.random(10, 15) * j
		j = -j
		local y = math.random(10, 15) * j
		local monster = Monster.New("我是大雄"..i, x,0,y)
		table.insert(self.monstersList, monster)

	end

	local j = 1
	for i=10, 20 do
		local x = math.random(10, 15) * j
		local y = math.random(10, 15) * j
		j = -j
		local monster = Monster.New("我是大雄"..i, x,0,y)
		table.insert(self.monstersList, monster)

	end
end

-- 定时器
function monsterAi:OnTimer()

	for _, monster in ipairs(self.monstersList) do
		self:DoAction(monster)
	end

end

function monsterAi:DoAction(monster)

	local playerPos = self.player.gameObject.transform.position
	local monsterPos = monster.gameObject.transform.position

	if self:Distance(playerPos, monsterPos) > 5 then
		monster:MoveTarget(playerPos.x, playerPos.z)
		return
	end

	monster.shootPower = monster.shootPower + 1

	if monster.shootPower >= 10 then
		monster.shootPower = 0

		monster:Shoot(playerPos)
	end
end

function monsterAi:Distance(playerPos, monsterPos)
	local x1 = playerPos.x
	local y1 = playerPos.z
	local x2 = monsterPos.x
	local y2 = monsterPos.z

	return math.sqrt(math.pow((y2-y1),2)+math.pow((x2-x1),2))
end

function monsterAi:onShootHit(uid)
	if self.player.userId == uid then

		self.player.hp = self.player.hp - 1

		if self.player.hp <= 0 then
			self.player:Die()
		end
	else
		for index, monster in ipairs(self.monstersList) do
			if monster.userId == uid then

				monster.hp = monster.hp - 1

				if monster.hp <= 0 then

					table.remove(self.monstersList, index)
					monster:Die()
				end

				return
			end
		end
	end
end

return monsterAi;


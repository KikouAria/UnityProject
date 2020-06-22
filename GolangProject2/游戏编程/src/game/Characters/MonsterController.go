package Characters

import (
)
const (
	LOGIN_CREQ = 1	// 申请登陆
	LOGIN_SRES = 2	// 登陆结果
	REG_CREQ = 3	// 申请注册
	REG_SRES = 4	// 注册结果
)

const (
	LOGIN_RET_SUCC = 0 	// 登陆成功
	LOGIN_RET_NOT = 1 	// 登陆失败 用户信息不存在
	LOGIN_RET_PWD = 2 	// 登陆失败 用户密码错误
)
var monsterList []*Monster // 储存n个怪物类



// 返回怪物起始点坐标, 从monsterPosList依次中取出
func GetMonsterStartPos() [3]float32 {

	pos := monsterPosList[monsterIndex]
	monsterIndex++
	if monsterIndex >= len(monsterPosList) {
		monsterIndex = 0
	}

	return pos
}



var monsterIndex  = 0
var monsterPosList = [5][3]float32 {
	{0,0,0},
	{6,0,0},
	{-6,0,0},
	{0,0,6},
	{0,0,-6},
}

// SyncServer
package Characters

import (
	"ace"
)

type Role struct {
	Character
	Session  *ace.Session
	UserId   int
}

func CreateRole(session *ace.Session, id int, name string) *Role {
	role := &Role{}

	role.Session = session
	role.Name = name
	role.UserId = id
	role.Rotation = [3]float32{0, 180, 0}
	role.Point = GetStartPos()
	role.Health = 10

	return role
}

var index  = 0
var posList = [5][3]float32 {
	{0,0,0},
	{12,0,0},
	{-12,0,0},
	{0,0,12},
	{0,0,-12},
}

// 配置玩家出生点
func GetStartPos() [3]float32 {

	pos := posList[index]
	index++
	if index >= len(posList) {
		index = 0
	}

	return pos
}



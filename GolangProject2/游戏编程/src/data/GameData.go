package data


type PlayerData struct {
	Account string
	Passward string
	UserId int
}

// 玩家数据存储
var DataBase = make(map[int]*PlayerData)
// 玩家信息索引
var PlayerInfo = make(map[string]int)

var UserIdIndex = 0

func GetPlayerUserId() int {
	UserIdIndex++
	return UserIdIndex
}
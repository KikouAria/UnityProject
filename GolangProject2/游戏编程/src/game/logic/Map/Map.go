// Map
package Map

import (
	"ace"
	"data"
	"encoding/json"
	"fmt"
	Characters2 "game/Characters"
	"game/logic/protocol"
	"math"
	"sync"
)


var MapHander = &Handler{&sync.Mutex{},make(map[int]*Characters2.Role)}


const (
	ENTER_CREQ  = 1 // 申请进入地图
	ENTER_SRES  = 2 // 进入地图结果
	MOVE_CREQ   = 3 // 角色移动和旋转
	MOVE_SRES   = 4 // 移动和旋转结果
	EXIT_CREQ   = 5 // 角色申请退出
	EXIT_SRES   = 6 // 角色退出地图
	ATTACK_CREQ = 7 // 攻击请求
	ATTACK_SRES  = 8 // 攻击结果
	COMMON_MSG  = 9 // 通用协议
)

// 请求进入地图时的json请求格式
type EnterCReqDto struct {
	Userid int `json:"userid"`
}

// 请求进入地图结果时的json请求格式
type EnterSResDto struct {
	Name string `json:"name"`
	Userid int `json:"userid"`
	Rotation [3]float32 `json:"rotation"`
	Point [3]float32 `json:"point"`
}

// 请求角色移动时的json请求格式
type MoveCReqDto struct {
	Rotation [3]float32 `json:"rotation"`
	Point [3]float32 `json:"point"`
}

// 请求角色移动结果时的json请求格式
type MoveSResDto struct {
	Userid int `json:"userid"`
	Rotation [3]float32 `json:"rotation"`
	Point [3]float32 `json:"point"`
}

// 退出请求
type ExitCReqDto struct {
	Userid int `json:"userid"`
}

// 退出结果
type ExitSResDto struct {
	Userid int `json:"userid"`
}

// 攻击请求
type AttackCReqDto struct {
	Useridhit int        `json:"useridhit"`
	PointSrc  [3]float32 `json:"pointSrc"`
	PointDest [3]float32 `json:"pointDest"`
}

// 攻击结果
type AttackSResDto struct {
	Useridatk int        `json:"useridatk"`
	Useridhit int        `json:"useridhit"`
	PointSrc  [3]float32 `json:"pointSrc"`
	PointDest [3]float32 `json:"pointDest"`
}


type Handler struct {
	lock *sync.Mutex
	Roles map[int]*Characters2.Role
}

func (this *Handler) recovery() {
	this.lock.Unlock()

	if r := recover(); r != nil {
		fmt.Println("map recovered:", r)
	}
}

func (this *Handler) Process(session *ace.Session, model ace.DefaultSocketModel) {
	//收到角色传来的地图消息  根据传输消息中的区域码来获取 对应的地图模块 调用该模块的消息处理函数
	fmt.Println("Process model.Command=", model.Command)

	this.lock.Lock()
	defer this.recovery()

	switch model.Command {
	case ENTER_CREQ:
		this.onProcEnter(session, model)
	case MOVE_CREQ:
		this.onProcMove(session, model)
	case EXIT_CREQ:
		this.onProcExit(session, model)
	case ATTACK_CREQ:
		this.onProcAttack(session, model)
	case COMMON_MSG:
		this.onProcCommon(session, model)
	}
}

// 角色进入地图
func (this *Handler) onProcEnter(session *ace.Session, model ace.DefaultSocketModel) {
	var reqDto EnterCReqDto

	err := json.Unmarshal(model.Message, &reqDto)
	if err != nil {
		fmt.Println("onProcLogin json.Unmarshal err", err)
		return
	}

	fmt.Println("用户申请进入地图，检查角色信息:", reqDto.Userid)

	userId := reqDto.Userid
	playerData := data.DataBase[userId]

	//将新的坐标赋值给角色信息对象 并将此角色加入本地图
	role := Characters2.CreateRole(session, userId, playerData.Account)
	this.Roles[userId] = role

	session.UserId = role.UserId

	// 创建返回数据包对象
	var resDto EnterSResDto
	// 设置返回的数据包(json)的值
	resDto.Name = role.Name
	resDto.Userid = role.UserId
	resDto.Rotation = role.Rotation
	resDto.Point = role.Point

	jsonData, err := json.Marshal(&resDto)

	if err != nil {
		fmt.Println("onProcLogin json.Marshal err", err)
		return
	}


	//fmt.Println("用户申请进入地图，通知客戶端进入地图:", string(jsonData))
	//// 将进入地图的角色信息发送给已经在这个场景的所有人
	//this.broadcast(protocol.MAP, ENTER_SRES, jsonData)  // type = 地图数据, cmd = 进入场景, msg = json数据

	print("用户申请加入地图, 只通知它自己要加入地图")
	role.Session.SendMsg(protocol.MAP, ENTER_SRES, jsonData)

	// 将地图上其他人的信息发给他
	for _, roleInMap := range this.Roles {
		//将实例化的角色对象返回给登录用户
		if roleInMap.UserId != role.UserId {

			var dto EnterSResDto
			dto.Name = roleInMap.Name
			dto.Userid = roleInMap.UserId
			dto.Rotation = roleInMap.Rotation
			dto.Point = roleInMap.Point

			jsonData, err := json.Marshal(&dto)

			if err != nil {
				fmt.Println("onProcLogin roleInMap json.Marshal err", err)
				return
			}

			fmt.Println("用户进入地图，但不进行通知:", string(jsonData))
			//fmt.Println("用户进入地图，通知客戶端地图玩家进入地图:", string(jsonData))
			//role.Session.SendMsg(protocol.MAP, ENTER_SRES, jsonData)
		}
	}
}

// 对场景中的所有角色(this.Role)发送数据包
func (this *Handler) broadcast(msgType int, msgCmd int, message []byte) {
	for _, role := range this.Roles {
		//将实例化的角色对象返回给登录用户
		role.Session.SendMsg(msgType, msgCmd, message)
	}
}

// 对场景中, 除了目标(role.UserId == userId)以外的所有角色(this.Role)发送数据包
func (this *Handler) broadcastExcept(userId int, msgType int, msgCmd int, message []byte) {
	for _, role := range this.Roles {
		if userId != role.UserId {
			//将实例化的角色对象返回给登录用户
			role.Session.SendMsg(msgType, msgCmd, message)
		}
	}
}


// 角色执行移动
func (this *Handler) onProcMove(session *ace.Session, model ace.DefaultSocketModel) {
	var reqDto MoveCReqDto

	err := json.Unmarshal(model.Message, &reqDto)
	if err != nil {
		fmt.Println("onProcMove json.Unmarshal err", err)
		return
	}

	userId := session.UserId

	fmt.Println("用户在地图中移动和旋转，检查角色信息:", userId)

	//将新的坐标赋值给角色信息对象 并将此角色加入本地图
	role := this.Roles[userId]
	if role == nil {
		fmt.Println("用户在地图中移动和旋转，用户不存在:", userId)
		return
	}

	role.Point = reqDto.Point
	role.Rotation = reqDto.Rotation

	var resDto MoveSResDto
	resDto.Userid = role.UserId
	resDto.Rotation = role.Rotation
	resDto.Point = role.Point

	m, err := json.Marshal(&resDto)

	if err != nil {
		fmt.Println("onProcMove json.Marshal err", err)
		return
	}

	fmt.Println("用户在地图中移动和旋转, 但我不广播:", string(m))

	// 将信息发送给已经在这个场景的所有人
	//this.broadcast(protocol.MAP, MOVE_SRES, m)



	// 怪物执行移动
	speed := float32(0.2)
	monster := this.Roles[-1]
	moveDirection := [3]float32{role.Point[0]-monster.Point[0],
		role.Point[1]-monster.Point[1],
		role.Point[2]-monster.Point[2]}
	fmt.Print("moveDirection: ", moveDirection)
	length := float32 (math.Sqrt(float64(moveDirection[0]*moveDirection[0] +
		moveDirection[1]*moveDirection[1] +
		moveDirection[2]*moveDirection[2])))
	if (speed > length){
		speed = length
	}

	newPos := [3]float32{monster.Point[0] + speed * moveDirection[0]/length,
		monster.Point[1] + speed *moveDirection[1]/length,
		monster.Point[2] + speed *moveDirection[2]/length}

	var monsterDto MoveSResDto
	monsterDto.Userid = -1
	monsterDto.Rotation = moveDirection
	monsterDto.Point = newPos
	this.Roles[-1].Point = newPos
	m2, err := json.Marshal(&monsterDto)

	if err != nil {
		fmt.Println("onProcMove json.Marshal err", err)
		return
	}
	fmt.Print("length", length, string(m2))
	this.broadcast(protocol.MAP, MOVE_SRES, m2)
}

// 角色退出地图
func (this *Handler) onProcExit(session *ace.Session, model ace.DefaultSocketModel) {
	userId := session.UserId
	delete(this.Roles, userId)

	var dto ExitSResDto
	dto.Userid = userId

	m, err := json.Marshal(&dto)

	if err != nil {
		fmt.Println("onProcExit json.Marshal err", err)
		return
	}

	fmt.Println("用户退出地图，通知客戶端地图玩家退出地图:", string(m))
	this.broadcastExcept(userId, protocol.MAP, EXIT_SRES, m)
}


// 角色进行攻击
func (this *Handler) onProcAttack(session *ace.Session, model ace.DefaultSocketModel) {
	var reqDto AttackCReqDto

	err := json.Unmarshal(model.Message, &reqDto)
	if err != nil {
		fmt.Println("onProcAttack json.Unmarshal err", err)
		return
	}

	userId := session.UserId

	fmt.Println("用户在攻击，检查角色信息:", userId)

	var resDto AttackSResDto
	resDto.Useridatk = userId
	resDto.Useridhit = reqDto.Useridhit
	resDto.PointSrc = reqDto.PointSrc
	resDto.PointDest = reqDto.PointDest

	m, err := json.Marshal(&resDto)

	if err != nil {
		fmt.Println("onProcAttack json.Marshal err", err)
		return
	}

	fmt.Println("用户在攻击，不广播通知客戶端:", string(m))

	// 将角色攻击消息发送给已经在这个场景中除了自己的所有人
	this.broadcastExcept(userId, protocol.MAP, ATTACK_SRES, m)

	//将新的坐标赋值给角色信息对象 并将此角色加入本地图
	userIdHit := reqDto.Useridhit

	if userIdHit != 0 {
		role := this.Roles[userIdHit]
		if role == nil {
			fmt.Println("用户在攻击，用户不存在:", userIdHit)
			return
		}
		role.Health -= 1

		if role.Health <= 0 {
			var dto ExitSResDto
			dto.Userid = userIdHit

			m, err := json.Marshal(&dto)

			if err != nil {
				fmt.Println("onProcAttack Exit json.Marshal err", err)
				return
			}

			fmt.Println("用户被消灭，通知客戶端地图玩家退出地图:", string(m))
			this.broadcast(protocol.MAP, EXIT_SRES, m)

			delete(this.Roles, userIdHit)
		}
	}
}


// 角色发送信息
func (this *Handler) onProcCommon(session *ace.Session, model ace.DefaultSocketModel) {

	fmt.Println("用户发送通用消息:", string(model.Message))
	this.broadcast(protocol.MAP, COMMON_MSG, model.Message)
}


// LoginHandler
// 执行登录相关的操作
package login

import (
	"ace"
	"data"
	"encoding/json"
	"fmt"
	"game/logic/protocol"
	"sync"
)

var LoginHander = &Handler{}

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

type RegCReqDto struct {
	Username string `json:"username"`
	Password string `json:"password"`
}

type RegSResDto struct {
	Name string `json:"name"`
	Result bool `json:"result"`
	IsMonster bool `json:"isMonster"`
}

type LoginCReqDto struct {
	Username string `json:"username"`
	Password string `json:"password"`
}

type LoginSResDto struct {
	Name string `json:"name"`
	Userid int `json:"userid"`
	Result int `json:"result"`
	IsMonster bool `json:"isMonster"`
}

type Handler struct {
	lock sync.Mutex
}

func (this *Handler) recovery() {
	this.lock.Unlock()  // 线程锁解锁?

	if r := recover(); r != nil {
		fmt.Println("login recovered:", r)
	}
}

// 接受登录请求后执行的操作
func (this *Handler) Process(session *ace.Session, model ace.DefaultSocketModel) {

	fmt.Println("Process model.Command=", model.Command)

	this.lock.Lock()  // 线程锁?
	defer this.recovery()  // 逻辑处理完毕后解锁并重置

	switch model.Command {
	case LOGIN_CREQ:
		this.onProcLogin(session, model)
	case REG_CREQ:
		this.onProcRegister(session, model)
	}

}

func (this *Handler) onProcRegister(session *ace.Session, model ace.DefaultSocketModel) {
	var reqDto RegCReqDto

	err := json.Unmarshal(model.Message, &reqDto)
	if err != nil {
		fmt.Println("onProcRegister json.Unmarshal err", err)
		return
	}

	print("创建怪物登录信息")

	monsterData := &data.PlayerData{"怪物", "", -1}
	data.DataBase[-1]= monsterData
	data.PlayerInfo["怪物"] = -1

	fmt.Println("用户申请注册，创建角色信息:", reqDto.Username)

	userId := data.GetPlayerUserId()
	playerData := &data.PlayerData{reqDto.Username, reqDto.Password, userId}
	data.DataBase[userId]= playerData
	data.PlayerInfo[reqDto.Username] = userId





	var resDto RegSResDto
	resDto.Name = reqDto.Username
	resDto.Result = true
	resDto.IsMonster = false
	m, err := json.Marshal(&resDto)

	if err != nil {
		fmt.Println("onProcRegister json.Marshal err", err)
		return
	}

	fmt.Println("用户申请注册，通知客戶端註冊成功:", string(m))

	//将实例化的角色对象返回给登录用户
	session.SendMsg(protocol.LOGIN, REG_SRES, m)
}

func (this *Handler) onProcLogin(session *ace.Session, model ace.DefaultSocketModel) {
	var reqDto LoginCReqDto

	err := json.Unmarshal(model.Message, &reqDto)
	if err != nil {
		fmt.Println("onProcRegister json.Unmarshal err", err)
		return
	}

	fmt.Println("用户申请登陆，检查角色信息:", reqDto.Username)
	result := this.onCheckLogin(reqDto.Username, reqDto.Password)

	// 先创建怪物的登录
	this.SpawnMonster(session)

	var resDto LoginSResDto
	resDto.Name = reqDto.Username
	resDto.Userid = data.PlayerInfo[reqDto.Username]
	resDto.Result = result
	resDto.IsMonster = false
	m, err := json.Marshal(&resDto)

	if err != nil {
		fmt.Println("onProcLogin json.Marshal err", err)
		return
	}

	fmt.Println("用户申请登陆，通知客戶端登陆成功:", string(m))

	//将实例化的角色对象返回给登录用户
	session.SendMsg(protocol.LOGIN, LOGIN_SRES, m)




}

func (this *Handler) SpawnMonster(session *ace.Session) {
	//newMonster := Characters.CreateMonster([3]float32{0, 180, 0}, Characters.GetMonsterStartPos())

	var resDto LoginSResDto
	resDto.Name = "怪物"
	resDto.Userid = -1
	resDto.Result = LOGIN_RET_SUCC
	resDto.IsMonster = true
	m, err := json.Marshal(&resDto)

	if err != nil {
		fmt.Println("onProcLogin json.Marshal err", err)
		return
	}

	fmt.Println("模拟怪物登录成功", string(m))

	//将实例化的角色对象怪物返回给登录用户
	session.SendMsg(protocol.LOGIN, LOGIN_SRES, m)

}

func (this *Handler) onCheckLogin(username string, password string) int {

	userId := data.PlayerInfo[username]

	if userId == 0 {
		fmt.Println("用户申请登陆，用户信息不存在:", username)
		return LOGIN_RET_NOT
	}

	playerData := data.DataBase[userId]

	if playerData.Passward != password {
		fmt.Println("用户申请登陆，用户密码错误:", username)
		return LOGIN_RET_PWD
	}

	return LOGIN_RET_SUCC
}

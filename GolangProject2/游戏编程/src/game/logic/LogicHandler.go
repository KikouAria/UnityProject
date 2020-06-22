// LogicHandler
// 接受数据后的主要逻辑
package logic

import (
	"ace"
	"fmt"
	"game/logic/Map"
	"game/logic/login"
	"game/logic/protocol"
)

type GameHandler struct {

}

func (this *GameHandler) SessionOpen(session *ace.Session) {
	fmt.Println("session open", session)
}

func (this *GameHandler) SessionClose(session *ace.Session) {
	fmt.Println("session closed", session)

	// 退出地图
	this.MessageReceived(session, ace.DefaultSocketModel{protocol.MAP,Map.EXIT_CREQ, nil})
}

func (this *GameHandler) MessageReceived(session *ace.Session, message interface{}) {
	m := message.(ace.DefaultSocketModel)
	switch m.Type {
	case protocol.LOGIN: //收到登录消息
		login.LoginHander.Process(session, m)
	case protocol.MAP: //收到地图信息
		Map.MapHander.Process(session, m)
	}
}

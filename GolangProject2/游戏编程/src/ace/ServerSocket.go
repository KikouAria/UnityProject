// ServerSocket
package ace

import (
	"fmt"
	"net"
	"runtime"
)

type Handler interface {
	SessionOpen(session *Session)
	SessionClose(session *Session)
	MessageReceived(session *Session, message interface{})
}

type ServerSocket struct {
	encode  *DefaultEncode
	decode  *DefaultDecode
	handler Handler
}

// 创建服务器, 一开始调用一次
func CreateServer() *ServerSocket {
	encode := &DefaultEncode{}
	decode := &DefaultDecode{}

	return &ServerSocket{encode, decode, nil}
}


// 在目标端口开启服务器
func (server *ServerSocket) Start(port int) {
	runtime.GOMAXPROCS(runtime.NumCPU())
	addr := fmt.Sprintf(":%d", port)
	listener, err := net.Listen("tcp", addr)
	if err != nil {
		fmt.Printf("Error: %v\n", err)
		fmt.Printf("Exiting...\n")
		return
	}
	// 服务器关闭时(函数返回时)执行 (相当于using) 关闭端口监听
	defer listener.Close()

	fmt.Println("Server Start port:", addr)
	for {  // while true
		conn, err := listener.Accept()  // 阻塞执行请求监听, 获取connection对象
		if err != nil {  // 捕获异常
			fmt.Printf("Error: %v\n", err)
			fmt.Printf("Exiting...\n")
			return
		}
		session := CreateSession(conn, server.encode, server.decode)
		go clientConnection(session, server) // 创建一个新线程执行命令
	}
}

//
func clientConnection(session *Session, server *ServerSocket) {
	server.handler.SessionOpen(session)  // 钩子 这里执行Logic/LogicHandler中的SessionOpen

	for result, msg := session.Read(); result; {  // 初始化result和msg, 在result为假(结束连接)时退出循环

		if result == true {
			server.handler.MessageReceived(session, msg) // 钩子 这里执行Logic/LogicHandler中的MessageReceived
		} else {  // 若为假 则关闭连接
			session.Close()
			break
		}

		result, msg = session.Read()  // 检查连接是否还在
	}
	server.handler.SessionClose(session)  // 一样 钩子
}


// 设置handler
func (server *ServerSocket) SetHandler(handler Handler) {
	server.handler = handler
}

// AppStart
package main

import (
	"fmt"
	"ace"
	"game/logic"
)

func main() {
	fmt.Println("GameServer Start")
	server := ace.CreateServer()
	server.SetHandler(&logic.GameHandler{})
	server.Start(10100)
}



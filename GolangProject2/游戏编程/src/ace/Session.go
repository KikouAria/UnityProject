// Session
package ace

import (
	_ "fmt"
	"net"
	"encoding/binary"
	"bytes"
	"bufio"
)

type Session struct {
	Conn      net.Conn
	Reader    *bufio.Reader
	Attribute map[string]string
	Encode    *DefaultEncode
	Decode	  *DefaultDecode
	IsColse   bool
	UserId    int
}

func CreateSession(conn net.Conn, encode *DefaultEncode, decode *DefaultDecode) *Session {
	return &Session{conn,
		bufio.NewReader(conn),
		make(map[string]string),
		encode,
		decode,
		false,
		0}
}

func (session *Session) Close() {
	session.Conn.Close()
	session.IsColse = true
}

func (session *Session) SetAttribute(key string, value string) {
	session.Attribute[key] = value
}

func (session *Session) RemoveAttribute(key string) {
	delete(session.Attribute, key)
}

func (session *Session) SendMsg(t int, c int, m []byte) {
	session.Write(&DefaultSocketModel{t, c, m})
}

func (session *Session) Write(msg *DefaultSocketModel) {
	if session.IsColse {
		return
	}
	bytes := session.Encode.Encode(msg)
	session.Conn.Write(bytes)
}

/**消息接收**/
func (session *Session) Read() (bool, interface{}) {
	//读取消息长度 错误则关闭客户端链接 并从客户端列表中移除客户端信息 否则返回长度
	bufPkg, err := session.ReadPackage()

	if err != nil {
		session.Close()
		return false, nil
	}

	message := session.Decode.Decode(bufPkg)

	return true, message
}

func (session *Session) ReadPackage() ([]byte, error) {

	// Peek 返回缓存的一个切片，该切片引用缓存中前 n 个字节的数据，
	// 该操作不会将数据读出，只是引用，引用的数据在下一次读取操作之
	// 前是有效的。如果切片长度小于 n，则返回一个错误信息说明原因。
	// 如果 n 大于缓存的总大小，则返回 ErrBufferFull。
	lengthByte, err := session.Reader.Peek(4)
	if err != nil {
		return nil, err
	}

	//创建 Buffer缓冲器
	lengthBuff := bytes.NewBuffer(lengthByte)
	var length int32
	// 通过Read接口可以将buf中得内容填充到data参数表示的数据结构中
	err = binary.Read(lengthBuff, binary.LittleEndian, &length)
	if err != nil {
		return nil, err
	}
	// Buffered 返回缓存中未读取的数据的长度
	if int32(session.Reader.Buffered()) < length+4 {
		return nil, err
	}

	// 读取消息真正的内容
	pack := make([]byte, int(4+length))
	// Read 从 b 中读出数据到 p 中，返回读出的字节数和遇到的错误。
	// 如果缓存不为空，则只能读出缓存中的数据，不会从底层 io.Reader
	// 中提取数据，如果缓存为空，则：
	// 1、len(p) >= 缓存大小，则跳过缓存，直接从底层 io.Reader 中读
	// 出到 p 中。
	// 2、len(p) < 缓存大小，则先将数据从底层 io.Reader 中读取到缓存
	// 中，再从缓存读取到 p 中。
	_, err = session.Reader.Read(pack)
	if err != nil {
		return nil, err
	}
	return pack[4:], nil
}


// Default
package ace


type DefaultSocketModel struct {
	Type    int
	Command int
	Message []byte
}

type DefaultDecode struct {

}

func (decode *DefaultDecode) Decode(msg []byte) interface{} {
	buffer := NewBufferReader(msg)
	Type := buffer.ReadInt()
	Command := buffer.ReadInt()
	Message := buffer.ReadString()
	return DefaultSocketModel{Type, Command, []byte(Message)}
}

type DefaultEncode struct {

}

func (encode *DefaultEncode) Encode(msg interface{}) []byte {
	m := msg.(*DefaultSocketModel)
	var length = len(m.Message)

	buffer := NewBufferWriter()
	buffer.WriteInt(4*3+length)
	buffer.WriteInt(m.Type)
	buffer.WriteInt(m.Command)
	buffer.WriteInt(length)
	buffer.WriteBytes(m.Message)

	return buffer.Bytes()
}

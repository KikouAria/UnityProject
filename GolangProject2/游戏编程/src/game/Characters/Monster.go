package Characters

type Monster struct {
	*Role

}

func CreateMonster(rotation [3]float32, point [3]float32) *Monster{
	monster := &Monster{}

	monster.Rotation = rotation
	monster.Point = point
	monster.Health = 10


	return monster
}

func (this *Monster) TowardPlayer(){

}
using Character;

namespace Common
{
    public class KeyForDrawer: EventItem
    {
        protected override void Init()
        {
            base.Init();
            id = 110;
            name = "钥匙";
        }


        public override void InteractEvent(Player player)
        {
            base.InteractEvent(player);
            AddToInventory(player);
        }
    }
}
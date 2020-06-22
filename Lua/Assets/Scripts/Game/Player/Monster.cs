
using UnityEngine;

public class Monster 
{
    public int userId;

    public GameObject gameObject;

    public int shootPower = 0;

    public int hp = 1;

    public Monster(string name, int x, int y, int z)
    {
        userId = PlayerManager.createUid();
        GameObject prefabPlayer = PlayerManager.GetGameObject();

        // Create an instance of the player prefab at the randomly selected spawn point's position and rotation.
        Vector3 pos = new Vector3(x, y, z);
        Quaternion rot = Quaternion.Euler(0, 180, 0);

        gameObject = GameObject.Instantiate(prefabPlayer, pos, rot);
        gameObject.AddComponent<Damagable>();  // 添加自己的可被伤害组件
        gameObject.layer = LayerMask.GetMask("Shootable");  // 设置图层为可被射击

        PlayerHud playerHud = gameObject.GetComponentInChildren<PlayerHud>();
        playerHud.SetName(name);

        PlayerShooting playerShooting = gameObject.GetComponentInChildren<PlayerShooting>();
        playerShooting.SetUserId(userId);

        PlayerMovement playerMove = gameObject.GetComponent<PlayerMovement>();
        playerMove.speed = 1;

        playerShooting.isLocalPlayer = false;
    }

    public void MoveTarget(float x, float z)
    {
        Vector3 targetPos = new Vector3(x, 0, z);

        PlayerMovement playerMove = gameObject.GetComponent<PlayerMovement>();
        playerMove.MoveTarget(targetPos);
    }

    public void Shoot(Vector3 pos)
    {
        PlayerShooting playerShooting = gameObject.GetComponentInChildren<PlayerShooting>();
        playerShooting.Shoot();
    }

    public void Die()
    {
        GameObject.Destroy(gameObject);
    }

}




using UnityEngine;

public class Player
{
    public int userId;

    public int hp = 10;

    public GameObject gameObject;
    internal object transform;

    public Player(int x, int y, int z)
    {
        userId = PlayerManager.createUid();
        GameObject prefabPlayer = PlayerManager.GetGameObject();

        // Create an instance of the player prefab at the randomly selected spawn point's position and rotation.
        Vector3 pos = new Vector3(x, y, z);
        Quaternion rot = Quaternion.Euler(0, 180, 0);

        gameObject = GameObject.Instantiate(prefabPlayer, pos, rot);

        PlayerHud playerHud = gameObject.GetComponentInChildren<PlayerHud>();
        playerHud.SetName(DataManager.Instance.GetPlayerName());

        PlayerShooting playerShooting = gameObject.GetComponentInChildren<PlayerShooting>();
        playerShooting.SetUserId(userId);

        gameObject.GetComponent<PlayerMovement>().enabled = true;

        CameraFollow script = Camera.main.GetComponent<CameraFollow>();
        script.SetTarget(gameObject.GetComponent<Transform>());
        script.enabled = true;
    }

    public void Die()
    {
        GameController.Inst.isPlaying = false;

        CameraFollow script = Camera.main.GetComponent<CameraFollow>();
        script.enabled = false;

        GameObject.Destroy(gameObject);
      
    }

}




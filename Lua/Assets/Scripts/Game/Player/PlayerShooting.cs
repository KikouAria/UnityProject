using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 1;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;

    float timer;
    Ray shootRay = new Ray();
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;

    private int userId;
    public void SetUserId(int userId)
    {
        this.userId = userId;
    }
    public int GetUserId()
    {
        return this.userId;
    }

    public bool isLocalPlayer = true;

    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");
        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
    }

    void Start()
    {

    }

    void OnDestroy()
    {

    }

    void Update ()
    {
        timer += Time.deltaTime;

		if(isLocalPlayer && Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            Shoot();
        }

        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();
        }
    }

    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }

    public void Shoot()
    {
        timer = 0f;

        gunAudio.Play();

        gunLight.enabled = true;

        gunParticles.Stop();
        gunParticles.Play();

        gunLine.enabled = true;
        gunLine.SetPosition (0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        int userId = 0;
        Vector3 point = new Vector3();

        print("Shooted!");

        if (Physics.Raycast (shootRay, out shootHit, range, shootableMask))
        {
            print("Shoot Hit!");
            Damagable damagable = shootHit.collider.gameObject.GetComponent<Damagable>();
            gunLine.SetPosition(1, shootHit.point);
            if (!damagable) return;
            damagable.OnDamaged(1);
            
        }
        

        // 击中玩家或者怪物
        GameController.Inst.onShootHit(userId);

    }




}





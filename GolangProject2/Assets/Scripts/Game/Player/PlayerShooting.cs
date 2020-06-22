using UnityEngine;

public class PlayerShooting : MonoBehaviour, TListener
{
    public int damagePerShot = 20;
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
        //注册监听器
        EventManager.Instance.AddListener(Event.EVENT_TYPE.PLAYER_ATTACK, this);
    }

    void OnDestory()
    {
        //删除监听器
        EventManager.Instance.RemoveListener(Event.EVENT_TYPE.PLAYER_ATTACK, this);
    }

    void Update ()
    {
        timer += Time.deltaTime;
        if (!isLocalPlayer || FindObjectOfType<PlayerMovement>().isStun) return;
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

    void Shoot()
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

        if (Physics.Raycast (shootRay, out shootHit, range, shootableMask))
        {
            PlayerShooting playerShooting = shootHit.collider.GetComponentInChildren<PlayerShooting>();
            if (playerShooting != null)
            {
               userId = playerShooting.GetUserId();

            }
            gunLine.SetPosition (1, shootHit.point);

            point = shootHit.point;
        }
        else
        {
            point = shootRay.origin + shootRay.direction * range;
            gunLine.SetPosition (1, point);
        }

        AttackCReqDto dto = new AttackCReqDto();
        dto.useridhit = userId;
        dto.pointSrc[0] = transform.position.x;
        dto.pointSrc[1] = transform.position.y;
        dto.pointSrc[2] = transform.position.z;

        dto.pointDest[0] = point.x;
        dto.pointDest[1] = point.y;
        dto.pointDest[2] = point.z;

        string message = Coding<AttackCReqDto>.encode(dto);
        NetWorkScript.Instance.SendMessage(Protocol.MAP, MapProtocol.ATTACK_CREQ, message);
    }

    /// <summary>
    /// 事件处理接口
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="sender">发送事件的游戏组件</param>
    /// <param name="receiver">可选参数 接收事件的游戏组件</param>
    /// <param name="value">可选参数 可传递游戏数值</param>
    public bool OnEvent(Event.EVENT_TYPE eventType, Component sender, Object receiver = null, System.Object data = null)
    {
        if (eventType == Event.EVENT_TYPE.PLAYER_ATTACK)
        {
            AttackSResDto dto = data as AttackSResDto;

            if (this.userId == dto.useridatk)
            {
                PlayerShoot(dto);

                return true;
            }
        }

        return false;
    }

    void PlayerShoot(AttackSResDto dto)
    {
        timer += Time.deltaTime;

        if (timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            timer = 0f;

            gunAudio.Play();

            gunLight.enabled = true;

            gunParticles.Stop();
            gunParticles.Play();

            gunLine.enabled = true;
            gunLine.SetPosition(0, transform.position);

            shootRay.origin = transform.position;
            shootRay.direction = transform.forward;

            if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
            {
                gunLine.SetPosition(1, shootHit.point);
            }
            else
            {
                gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
            }
        }

        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }
    }

    /// <summary>
    /// 获取当前游戏对象的引用
    /// </summary>
    /// <returns>当前类游戏对象的引用（只读）</returns>
    public Object GetGameObject()
    {
        return this;
    }
}

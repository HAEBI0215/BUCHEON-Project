using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public enum PlayerLiveState
    {
        Live,
        Dead
    }
    [Header("Player Info")]
    public PlayerLiveState playerLiveState = PlayerLiveState.Live;
    private CharacterController cc;
    public float speed;
    private Vector3 lookTarget;
    public Animator anim;
    public float playerHp;
    private float maxHp;
    private float hpPer;
    public float hpSpeed;
    public Slider hpSlider;

    public enum HPState
    {
        None,
        HPDown,
        HPUp
    }
    public HPState hpState = HPState.None;

    public float jumpPower = 10f;
    public float gravity = -20;
    private float yVelocity = 0; //높이가속
    private bool isGround = true;

    [Header("Attack Info")]
    public GameObject bulletPrefab;
    public Transform firePos;
    public float bulletOffset; //총알 간격
    private float bulletTime; //격발 시간
    public float setBulletTime; //설정할 격발 시간
    public int bulletCount;
    public float bulletDamage;
    public CameraMove cam;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        maxHp = playerHp;
    }

    // Update is called once per frame
    void Update()
    {
        switch (playerLiveState)
        {
            case PlayerLiveState.Live:
                {
                    if ( Time.timeScale != 0)
                    {
                        PlayerMove();
                        PlayerRotate();
                        PlayerFire();
                        PlayerHP();
                    }
                    break;
                }
        }
    }

    void PlayerMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 pos = new Vector3(-v, 0, h) * speed;

        if (cc.isGrounded)
        {
            yVelocity = 0; //강하 속도 0으로 초기화
            isGround = true;
        }

        if (Input.GetButtonDown("Jump") && isGround == true)
        {
            yVelocity = jumpPower;
            isGround = false;
        }

        yVelocity += gravity * Time.deltaTime; //중력 적용
        pos.y = yVelocity;
        cc.Move(pos * Time.deltaTime);

        //cc.SimpleMove(pos * speed);

        anim.SetFloat("Move", Mathf.Abs(h) + Mathf.Abs(v));
    }

    void PlayerRotate()
    {
        RaycastHit hit;
        //메인카메라에서 마우스 커서의 위치에 레이를 발사하고 충돌이 일어난다면
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            lookTarget = hit.point; //충돌한 레이의 위치를 담음

            Vector3 relatiomn = new Vector3(lookTarget.x, 0, lookTarget.z) - new Vector3(transform.position.x, 0, transform.position.z);
            //캐릭터가 lookTarget을 바라보도록 회전
            Quaternion rotation = Quaternion.LookRotation(relatiomn);
            transform.rotation = rotation;
        }
    }

    void PlayerFire()
    {
        //if (Input.GetButtonDown("Fire1"))
        //{
        //    for (int i = 0; i < bulletCount; i++)
        //        CreateBullet();

        //    bulletTime = 0;
        //    anim.SetBool("Fire", true);
        //}

        //계속 누르고 있다면
        if (Input.GetButton("Fire1"))
        {
            bulletTime += Time.deltaTime;
            if (bulletTime >= setBulletTime)
            {
                for (int i = 0; i < bulletCount; i++)
                    CreateBullet();
                bulletTime = 0;
            }

            cam.CameraShakeOn();
        }

        else
        {
            if(anim.GetBool("Fire") == true)
                anim.SetBool("Fire", false);
        }

        if (Input.GetButtonUp("Fire1"))
        {
            bulletTime = 0;
            cam.CameraShakeOff();
            anim.SetBool("Fire", true);
        }
    }

    void PlayerHP()
    {
        switch (hpState)
        {
            case HPState.HPDown:
                {
                    hpSlider.value = Mathf.MoveTowards(hpSlider.value, hpPer, hpSpeed * Time.deltaTime);
                    //HP 슬라이더의 값이 hpPer로 hpSpeed의 속도로 일정하게 이동

                    if (hpSlider.value == hpPer)
                    {
                        if (hpSlider.value == 0)
                        {
                            playerLiveState = PlayerLiveState.Dead;
                            anim.SetInteger("Live", 2);
                        }
                        hpState = HPState.None;
                    }
                    break;
                }
            case HPState.HPUp:
                {
                    hpSlider.value = Mathf.MoveTowards(hpSlider.value, hpPer, hpSpeed * Time.deltaTime);
                    if (hpSlider.value == hpPer)
                    {
                        hpState = HPState.None;
                    }
                    break;
                }
        }
    }

    void CreateBullet()
    {
        //bulletOffset 반경만큼 구체 범위와 격발 위치를 더한 범위에서 무작위로 벡터값 추출
        Vector3 offset = Random.insideUnitSphere * bulletOffset + firePos.position;

        GameObject bullet = Instantiate(bulletPrefab, offset, firePos.rotation);

        bullet.GetComponent<Bullet>().bulletDamage = bulletDamage;

        Destroy(bullet, 1.5f);
    }

    public void PlayerDamageOn(float damage)
    {
        if (playerHp > damage)
            playerHp -= damage;
        else
            playerHp = 0;
        hpPer = playerHp / maxHp;
        hpState = HPState.HPDown;
    }

    public void PlayerHpUp(float hp)
    {
        maxHp += hp;
        hpPer = playerHp / maxHp;
        hpSlider.value = hpPer;
        playerHp = maxHp;
        hpPer = playerHp / maxHp;
        hpState = HPState.HPUp;
    }
    public void BulletUp(int bullet)
    {
        bulletCount = bulletCount + bullet;
    }
    public void SpeedUp(float speed)
    {
        speed = speed + speed;
    }
    public void IntervalUp(float interval)
    {
        setBulletTime = setBulletTime * interval;
    }
    public void DamageUp(float power)
    {
        power = power + power;
    }
}

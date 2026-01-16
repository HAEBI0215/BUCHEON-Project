using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Info")]
    private CharacterController cc;
    public float speed;
    private Vector3 lookTarget;
    public Animator anim;

    [Header("Attack Info")]
    public GameObject bulletPrefab;
    public Transform firePos;
    public float bulletOffset; //총알 간격
    private float bulletTime; //격발 시간
    public float setBulletTime; //설정할 격발 시간

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        PlayerRotate();
        PlayerFire();
    }

    void PlayerMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 pos = new Vector3(-v, 0, h);
        cc.SimpleMove(pos * speed);

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
        if (Input.GetButtonDown("Fire1"))
        {
            for (int i = 0; i < 3; i++)
                CreateBullet();

            anim.SetTrigger("Fire");
        }

        //계속 누르고 있다면
        if (Input.GetButton("Fire1"))
        {
            bulletTime += Time.deltaTime;
            if (bulletTime >= setBulletTime)
            {
                for (int i = 0; i < 3; i++)
                    CreateBullet();
                bulletTime = 0;
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            anim.SetTrigger("Fire");
        }
    }

    void CreateBullet()
    {
        //bulletOffset 반경만큼 구체 범위와 격발 위치를 더한 범위에서 무작위로 벡터값 추출
        Vector3 offset = Random.insideUnitSphere * bulletOffset + firePos.position;

        GameObject bullet = Instantiate(bulletPrefab, offset, firePos.rotation);

        Destroy(bullet, 1.5f);
    }

    void AnimOn(int n)
    {
        anim.SetInteger("PlayerState", n);
    }
}

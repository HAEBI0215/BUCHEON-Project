using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed;
    public float hitRange;
    public float bulletDamage;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, hitRange);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);

        HitTarget();
    }

    void HitTarget()
    {
        int layerMask = 1 << 6;
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, hitRange, Vector3.up, 0, layerMask);
        //스피어 캐스트를 그림 (위치, 반경, 방향, 거리, 레이어마스크)

        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                hit.transform.GetComponent<Zombie>().ZombieDamageOn(bulletDamage);
                Destroy(gameObject);
            }
        }
    }
}

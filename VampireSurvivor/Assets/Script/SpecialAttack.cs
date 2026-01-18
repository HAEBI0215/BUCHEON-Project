using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : MonoBehaviour
{
    public float range;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }


    void Start()
    {
        
    }

    void Update()
    {
        int layerMask = 1 << 6;
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, range, Vector3.up, 0f, layerMask);

        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                hit.transform.GetComponent<Zombie>().ZombieDamageOn(hit.transform.GetComponent<Zombie>().zombieHp * 2.0f);
            }
        }
    }
}

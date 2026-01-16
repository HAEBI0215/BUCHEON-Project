using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    public enum AttackActivate
    {
        None,
        Activate
    }
    public AttackActivate attackActivate = AttackActivate.None;

    public float attackRange;
    public float ZombieDamage;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (attackActivate)
        {
            case AttackActivate.Activate:
                {
                    int layerMask = 1 << 7; 
                    //플레이어 레이어 마스크

                    RaycastHit[] hits = Physics.SphereCastAll(transform.position, attackRange, Vector3.up, 0, layerMask);

                    if (hits.Length > 0)
                    {
                        foreach (RaycastHit hit in hits)
                        {
                            hit.transform.GetComponent<Player>().PlayerDamageOn(ZombieDamage);
                            attackActivate = AttackActivate.None;
                        }
                    }

                    break;
                }
        }
    }

    public void ZombieAttackOn()
    {
        attackActivate = AttackActivate.Activate;
    }
}

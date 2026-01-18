using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public enum LiveState
    {
        Live,
        Dead
    }
    [Header("Zombie Info")]
    public LiveState liveState = LiveState.Live;

    public enum ActionState
    {
        Idle,
        Move,
        Attack,
        Dead
    }
    public ActionState actionState = ActionState.Idle;

    private GameObject player;
    private NavMeshAgent agent;
    public Animator anim;

    [Header("Attack Info")]
    public float attackRange;
    public enum AttackState
    {
        None,
        Attack,
        Delay
    }
    public AttackState attackState = AttackState.None;
    private float attackTime;
    private float delayTime;
    public AnimationClip attackClip;
    public ZombieAttack zombieAttack;

    [Header("Zonbie Hp")]
    public float zombieHp;

    [Header("Zombie Exp")]
    public float zombieExp;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
        //공격 반경만큼 와이어 구체를 그림
    }


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        switch (liveState)
        {
            case LiveState.Live:
                {
                    Action();
                    break;
                }
        }
    }

    void Action()
    {
        switch (actionState)
        {
            case ActionState.Idle:
                {
                    if (player)
                    {
                        agent.isStopped = false;
                        AnimOn(1);
                        actionState = ActionState.Move;
                    }
                    break;
                }
            case ActionState.Move:
                { 
                    if (player)
                    {
                        agent.SetDestination(player.transform.position);
                        //플레이어를 향해 이동함

                        float dist = Vector3.Distance(transform.position, player.transform.position);
                        //좀비와 플레이어 사이의 거리를 float형으로 반환

                        if (dist <= attackRange)
                        {
                            switch (player.GetComponent<Player>().playerLiveState)
                            {
                                case Player.PlayerLiveState.Live:
                                    {
                                        agent.isStopped = true;
                                        AnimOn(2);
                                        attackState = AttackState.Attack;
                                        actionState = ActionState.Attack;
                                        break;
                                    }
                            }
                        }
                    }
                    break;
                }
            case ActionState.Attack:
                {
                    float dist = Vector3.Distance(transform.position, player.transform.position);
                    if (dist <= attackRange)
                    {
                        switch (attackState)
                        {
                            case AttackState.Attack:
                                {
                                    attackTime += Time.deltaTime;
                                    if (attackTime >= attackClip.length * 0.25f)
                                    {
                                        zombieAttack.ZombieAttackOn();
                                        AnimOn(0);
                                        attackTime = 0;
                                        delayTime = 0;
                                        attackState = AttackState.Delay;
                                    }

                                    break;
                                }
                            case AttackState.Delay:
                                {
                                    switch (player.GetComponent<Player>().playerLiveState)
                                    {
                                        case Player.PlayerLiveState.Live:
                                            {
                                                delayTime += Time.deltaTime;
                                                if (delayTime >= attackClip.length * 0.75f)
                                                {
                                                    LookPlayer();
                                                    AnimOn(2);
                                                    attackTime = 0;
                                                    delayTime = 0;
                                                    attackState = AttackState.Attack;
                                                }
                                                break;
                                            }
                                    }
                                    break;
                                }
                        }
                    }
                    else
                    {
                        StartCoroutine(ZombieMove());
                    }
                    break;
                }
        }
    }

    IEnumerator ZombieMove()
    {
        yield return new WaitForSeconds(1.0f);
        agent.isStopped = false;
        AnimOn(1);
        attackTime = 0;
        delayTime = 0;
        attackState = AttackState.None;
        actionState = ActionState.Move;
    }

    void AnimOn(int n)
    {
        anim.SetInteger("ZombieState", n);
    }

    public void ZombieDamageOn(float damage)
    {
        if (zombieHp > damage)
        {
            zombieHp -= damage;
        }
        else
        {
            GetComponent<CharacterController>().enabled = false;
            agent.isStopped = true;
            AnimOn(3);
            zombieHp = 0;
            attackTime = 0;
            delayTime = 0;
            attackState = AttackState.None;
            actionState = ActionState.Dead;
            liveState = LiveState.Dead;
            Destroy(gameObject, 5f);

            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerExpUp(zombieExp);
            //경험치 드랍
        }
    }

    void LookPlayer()
    {
        if (player)
        {
            Vector3 relation = new Vector3(player.transform.position.x, 0, player.transform.position.z) 
                - new Vector3(transform.position.x, 0, transform.position.z);
            Quaternion rotation = Quaternion.LookRotation(relation);
            transform.rotation = rotation;
        }
    }
}

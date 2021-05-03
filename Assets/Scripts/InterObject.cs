using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

[RequireComponent(typeof(AudioSource))]
public class InterObject : MonoBehaviour, IDamagable
{
    private bool alive = true;
    public float Hp = 10;
    public float MaxHP = 10;
    public EquipType EffectiveType = EquipType.None;
    public bool hasAI = true;

    [ConditionalField("hasAI")] public AI ai = AI.Idle;
    [ConditionalField("hasAI")] public float FollowDistance = 5f;
    [ConditionalField("hasAI")] public float attackRange = 5f;
    [SerializeField] public List<ChanceItem> DropList;

    private GameObject player;
    private Vector2 targetPos;
    private bool moving = false;
    [ConditionalField("hasAI")] public float attackDelay = 5f;

    private float currectAttackDelay = 0f;
    [SerializeField] private Transform customeDropLocation;
    public AudioClip hitSound, die;
    private AudioSource source;

    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Hp = MaxHP;
        if (attackRange >= FollowDistance)
            attackRange = FollowDistance / 2f;
        StartCoroutine(nameof(AiSquence));
        source = GetComponent<AudioSource>();
        if (!hasAI)
        {
            FollowDistance = 0;
            attackRange = 0;
        }
    }

    public void TakeDamage(int damage, EquipType sourceEquip = EquipType.None)
    {
        AudioPlayer.Instance.Play(hitSound);
        if (sourceEquip == EffectiveType)
            Hp -= 5;
        else
            Hp -= 2;
        Debug.Log($"{name} ({Hp}/{MaxHP})");
        if (Hp <= 0) Die();
    }

    private void LateUpdate()
    {
    }

    private void FixedUpdate()
    {
        currectAttackDelay -= 1 * Time.fixedDeltaTime;
        if (currectAttackDelay < 0) currectAttackDelay = 0;
        if (moving) transform.position = Vector2.MoveTowards(transform.position, targetPos, 5 * Time.fixedDeltaTime);
    }

    private IEnumerator AiSquence()
    {
        while (alive && hasAI)
        {
            var distance = Vector2.Distance(transform.position, player.transform.position);
            switch (ai)
            {
                case AI.Idle:
                    if (distance < FollowDistance)
                    {
                        ai = AI.Follow;
                        targetPos = player.transform.position;
                    }
                    else
                    {
                        yield return new WaitForSeconds(Random.Range(1f, 3f));
                        ai = AI.Wander;
                        //random pick location
                        targetPos = new Vector2(transform.position.x + Random.Range(-5f, 5f),
                            transform.position.y + Random.Range(-5f, 5f));
                        moving = true;
                    }

                    break;
                case AI.Wander:
                    var distance2 = Vector2.Distance(transform.position, targetPos);
                    while (moving)
                    {
                        distance2 = Vector2.Distance(transform.position, targetPos);
                        if (distance2 < 2)
                        {
                            moving = false;
                            ai = AI.Idle;
                        }

                        yield return new WaitForEndOfFrame();
                    }

                    break;
                case AI.Follow:
                    if (distance > FollowDistance)
                    {
                        moving = false;
                        ai = AI.Idle;
                    }
                    else if (distance < attackRange)
                    {
                        moving = false;
                        ai = AI.Attack;
                    }
                    else
                    {
                        targetPos = player.transform.position;
                        Debug.Log($"follow player to {targetPos}");
                        moving = true;
                    }

                    break;
                case AI.Attack:
                    if (currectAttackDelay <= 0)
                    {
                        player.GetComponent<Player>().TakeDamage(5);
                        currectAttackDelay = attackDelay;
                    }

                    yield return new WaitForSeconds(0.5f);
                    ai = AI.Follow;
                    break;
                default:
                    break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private void Die()
    {
        alive = false;
        DropManager.RandomDrop(DropList, customeDropLocation ? customeDropLocation.position : transform.position);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (hasAI)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, FollowDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}


public enum AI
{
    Idle, // just stand still
    Wander, // after wander back to idle
    Follow, // when found player switch to follow imediatly depen on ai
    Attack // when in range try to attck after attack check if can attack another time if not switch to follow
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using UnityEngine.Events;

public class InterObject : MonoBehaviour, IDamagable
{
    private bool alive = true;
    public float hp = 10;
    public float maxHp = 10;
    
    public bool onlyTakeDamageByEffective = false;
    public EquipType effectiveType = EquipType.None;
    public bool hasAI = true;

    [ConditionalField("hasAI")] public AI ai = AI.Idle;
    [ConditionalField("hasAI")] public float followDistance = 5f;
    [ConditionalField("hasAI")] public float attackRange = 5f;
    [SerializeField] public List<ChanceItem> DropList;

    private GameObject player;
    private Vector2 targetPos;
    private bool moving = false;
    [ConditionalField("hasAI")] public float attackDelay = 5f;

    private float currentAttackDelay = 0f;
    [SerializeField] private Transform customDropLocation;
    public AudioClip hitSound, die;
    private AudioSource source;

    public UnityEvent onDie;
    
    public virtual void Start()
    {
        onDie ??= new UnityEvent();
        
        player = GameObject.FindGameObjectWithTag("Player");
        hp = maxHp;
        if (attackRange >= followDistance)
            attackRange = followDistance / 2f;
        StartCoroutine(nameof(AiSquence));
        source = GetComponent<AudioSource>();
        if (!hasAI)
        {
            followDistance = 0;
            attackRange = 0;
        }
    }

    public void TakeDamage(int damage, EquipType sourceEquip = EquipType.None)
    {
        AudioPlayer.Instance?.Play(hitSound);
        if (onlyTakeDamageByEffective)
        {
            if(sourceEquip != effectiveType)
                return;
            hp -= damage;
        }
        else
        {
            if (sourceEquip == effectiveType)
                hp -= damage*2;
            else
                hp -= damage;
        }
        if (hp <= 0) Die();
    }

    private void FixedUpdate()
    {
        currentAttackDelay -= 1 * Time.fixedDeltaTime;
        if (currentAttackDelay < 0) currentAttackDelay = 0;
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
                    if (distance < followDistance)
                    {
                        ai = AI.Follow;
                        targetPos = player.transform.position;
                    }
                    else
                    {
                        yield return new WaitForSeconds(Random.Range(1f, 3f));
                        ai = AI.Wander;
                        //random pick location
                        var pos = transform.position;
                        targetPos = new Vector2(pos.x + Random.Range(-5f, 5f),
                            pos.y + Random.Range(-5f, 5f));
                        moving = true;
                    }

                    break;
                case AI.Wander:
                    while (moving)
                    {
                        var distance2 = Vector2.Distance(transform.position, targetPos);
                        if (distance2 < 2)
                        {
                            moving = false;
                            ai = AI.Idle;
                        }

                        yield return new WaitForEndOfFrame();
                    }

                    break;
                case AI.Follow:

                    if (distance < attackRange)
                    {
                        moving = false;
                        ai = AI.Attack;
                    }
                    else
                    {
                        targetPos = player.transform.position;
                        moving = true;
                    }

                    break;
                case AI.Attack:
                    if (currentAttackDelay <= 0)
                    {
                        player.GetComponent<Player>().TakeDamage(5);
                        currentAttackDelay = attackDelay;
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
        onDie.Invoke();
        DropManager.RandomDrop(DropList, customDropLocation ? customDropLocation.position : transform.position);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (hasAI)
        {
            var pos = transform.position;
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(pos, followDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pos, attackRange);
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
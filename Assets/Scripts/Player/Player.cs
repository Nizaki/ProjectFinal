using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public bool alive = true;
    public int Hp = 100;
    public int MaxHP = 100;
    public float Hunger = 100f;
    public float MaxHunger = 100f;
    public float HungerDrainRate = 0.1f;
    public float Water = 100f;
    public float MaxWater = 100f;
    public float WaterDrainRate = 0.1f;
    public float maxDarkDelay = 3;
    [HideInInspector] public UnityEvent onDeath;
    [HideInInspector] public UnityEvent<int> onTakeDamage;
    public float darkDelay = 5;
    [HideInInspector] public List<GameObject> Interactable = new List<GameObject>();
    [HideInInspector] public bool UnderLight;

    private float timer;
    [SerializeField] private GameObject Holder;
    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRange;
    public Animator punchAnim;
    public LayerMask enemy;

    public PlayerInventory inventory;

    // Start is called before the first frame update
    private void Start()
    {
        Hp = MaxHP;
        Hunger = MaxHunger;
        Water = MaxWater;
        onDeath ??= new UnityEvent();
        onTakeDamage ??= new UnityEvent<int>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!alive)
            return;
        UpdateLight();
        MoveTarget();


        var ray = Physics2D.OverlapCircleAll(transform.position, 1f);
        foreach (var item in ray)
        {
            var citem = item.GetComponent<DropItem>();
            if (citem != null)
            {
                inventory.AddItem(citem.item);
                Notification.ShowNotification($"ได้รับ {citem.item.name} 1 ชิ้น");
                Destroy(item.gameObject);
            }
        }
    }

    private void UpdateLight()
    {
        if (UnderLight || GameTime.state == TimeState.Day)
        {
            darkDelay += 0.25f * Time.deltaTime;
            if (darkDelay > maxDarkDelay)
                darkDelay = maxDarkDelay;
            return;
        }

        if (GameTime.state == TimeState.Night && !UnderLight)
        {
            darkDelay -= Time.deltaTime;
            if (darkDelay <= 0)
            {
                timer += 1 * Time.deltaTime;
                if (timer > 3f)
                {
                    TakeDamage(25);
                    timer = 0;
                }
            }

            if (darkDelay < 0) darkDelay = 0;
        }
    }

    private void MoveTarget()
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var dir = pos - Holder.transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Holder.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void Attack()
    {
        punchAnim.SetTrigger("punch");
    }

    public void DoDamage()
    {
        var toDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange);
        Debug.Log($"Attack found {toDamage.Length}");
        foreach (var item in toDamage)
        {
            IDamagable damagable = item.gameObject.GetComponent<InterObject>();
            if (damagable != null && !item.isTrigger) damagable.TakeDamage(5);
        }
    }

    public void TakeDamage(int value)
    {
        Hp -= value;
        if (Hp <= 0)
            Debug.Log("ตาย");

        onTakeDamage.Invoke(value);
    }

    private void FixedUpdate()
    {
        Hunger -= HungerDrainRate * Time.fixedDeltaTime;
        Water -= WaterDrainRate * Time.fixedDeltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }

    private GameObject GetClosetInteractable()
    {
        GameObject result = null;
        var minDist = Mathf.Infinity;
        Vector2 currentPos = transform.position;
        foreach (var go in Interactable)
        {
            var dist = Vector2.Distance(go.transform.position, currentPos);
            if (dist < minDist)
            {
                result = go;
                minDist = dist;
            }
        }

        return result;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactive")) Interactable.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactive")) Interactable.Remove(collision.gameObject);
    }

    public void Heal(int value)
    {
        Hp += value;
    }

    public void Eat(int value)
    {
        Hunger += value;
    }

    public void Drink(int value)
    {
        Water += value;
    }
}
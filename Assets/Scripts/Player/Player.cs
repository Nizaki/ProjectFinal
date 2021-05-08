using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public bool alive = true;
    public int hp = 100;
    public int maxHp = 100;
    public float hunger = 100f;
    public float maxHunger = 100f;
    public float hungerDrainRate = 0.1f;
    public float water = 100f;
    public float maxWater = 100f;
    public float waterDrainRate = 0.1f;
    public float maxDarkDelay = 3;
    [HideInInspector] public UnityEvent onDeath;
    [HideInInspector] public UnityEvent<int> onTakeDamage;
    public float darkDelay = 5;
    [HideInInspector] public List<GameObject> interactable = new List<GameObject>();
    [HideInInspector] public bool underLight;

    private float timer;
    [SerializeField] private GameObject holder;
    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRange;
    public Animator punchAnim;
    public LayerMask enemy;

    public PlayerInventory inventory;

    private PlayerUiManager ui;
    // Start is called before the first frame update
    private void Start()
    {
        hp = maxHp;
        hunger = maxHunger;
        water = maxWater;
        onDeath ??= new UnityEvent();
        onTakeDamage ??= new UnityEvent<int>();
        ui = GameObject.Find("PlayerUI")?.GetComponent<PlayerUiManager>();
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
            if (citem == null) continue;
            PickUp(citem.item);
            Destroy(item.gameObject);
        }
    }

    private void PickUp(ItemObject item)
    {
        inventory.AddItem(item);
        Notification.ShowNotification($"ได้รับ {item.name} 1 ชิ้น");
    }
    
    private void UpdateLight()
    {
        if (underLight || GameTime.state == TimeState.Day)
        {
            darkDelay += 0.25f * Time.deltaTime;
            if (darkDelay > maxDarkDelay)
                darkDelay = maxDarkDelay;
            return;
        }

        if (GameTime.state == TimeState.Night && !underLight)
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
        var dir = pos - holder.transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        holder.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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
        hp -= value;
        if (hp <= 0)
            Die();

        onTakeDamage.Invoke(value);
    }

    private void Die()
    {
        alive = false;
        ui.deathPanel.SetActive(true);
    }
    
    private void FixedUpdate()
    {
        hunger -= hungerDrainRate * Time.fixedDeltaTime;
        water -= waterDrainRate * Time.fixedDeltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactive")) interactable.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactive")) interactable.Remove(collision.gameObject);
    }

    public void Heal(int value)
    {
        hp += value;
    }

    public void Eat(int value)
    {
        hunger += value;
    }

    public void Drink(int value)
    {
        water += value;
    }

    public void Use()
    {
        var item = inventory.currentItem;
        var itemType = item.type;
        switch (itemType)
        {
            case ItemType.None:
                break;
            case ItemType.Food:
                Eat(Mathf.RoundToInt(item.data));
                inventory.RemoveItem(item, 1);
                break;
            case ItemType.Water:
                Drink(Mathf.RoundToInt(item.data));
                inventory.RemoveItem(item, 1);
                break;
            case ItemType.Health:
                Heal(Mathf.RoundToInt(item.data));
                inventory.RemoveItem(item, 1);
                break;
            case ItemType.Equip:
                break;
            case ItemType.Build:
                //build
                var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldPos.z = 0;
                if (item.prefab != null)
                {
                    Instantiate(item.prefab, worldPos, Quaternion.identity);
                    inventory.RemoveItem(item, 1);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
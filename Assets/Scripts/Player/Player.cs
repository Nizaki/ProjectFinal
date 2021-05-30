using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
    public float damage = 1f;
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
    public Transform tooltipsPos;
    public GameObject hpUp, hgUp, wtUp;
    private PlayerUiManager ui;

    private Camera cam;

    // Start is called before the first frame update
    private void Start()
    {
        hp = maxHp;
        hunger = maxHunger;
        water = maxWater;
        onDeath ??= new UnityEvent();
        onTakeDamage ??= new UnityEvent<int>();
        ui = GameObject.Find("PlayerUI")?.GetComponent<PlayerUiManager>();
        cam = Camera.main;
        StartCoroutine(nameof(NeedDamage));
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

        CheckNeed();
    }

    private void CheckNeed()
    {
        if (water <= 0)
        {
            water = 0;
        }

        if (hunger <= 0)
        {
            hunger = 0;
        }
    }

    IEnumerator NeedDamage()
    {
        while (alive)
        {
            var tempDamage = 0;
            if (water <= 0)
            {
                tempDamage += 3;
            }

            if (hunger <= 0)
            {
                tempDamage += 2;
            }
            if(tempDamage>0)
                TakeDamage(tempDamage);
            yield return new WaitForSeconds(5);
        }
    }
    
    private void PickUp(ItemObject item)
    {
        inventory.AddItem(item);
        Notification.ShowNotification($"ได้รับ {item.itemName} 1 ชิ้น");
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
        foreach (var item in toDamage)
        {
            IDamagable damagable = item.gameObject.GetComponent<InterObject>();
            if (damagable != null && !item.isTrigger)
            {
                if (hunger > 0)
                    hunger -= .5f;
                if (water > 0)
                    water -= .25f;
                if (inventory.currentItem.type == ItemType.Equip)
                    damagable.TakeDamage(Mathf.RoundToInt(damage + inventory.currentItem.data),
                        inventory.currentItem.equipType);
                else
                    damagable.TakeDamage(Mathf.RoundToInt(damage), EquipType.None);
                return;
            }
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
        //Check if can Interact with object
        var item = inventory.currentItem;
        var itemType = item.type;
        switch (itemType)
        {
            case ItemType.None:
                break;
            case ItemType.Food:
                Instantiate(hgUp, tooltipsPos);
                Eat(Mathf.RoundToInt(item.data));
                inventory.RemoveItem(item, 1);
                break;
            case ItemType.Water:
                Instantiate(wtUp, tooltipsPos);
                Drink(Mathf.RoundToInt(item.data));
                inventory.RemoveItem(item, 1);
                break;
            case ItemType.Health:
                Instantiate(hpUp, tooltipsPos);
                Heal(Mathf.RoundToInt(item.data));
                inventory.RemoveItem(item, 1);
                break;
            case ItemType.Equip:
                break;
            case ItemType.Build:
                //build
                var mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                var hit = Physics2D.Raycast(mousePos, Vector2.zero);

                if (hit.collider != null) return;
                mousePos.z = 0;
                if (item.prefab != null)
                {
                    Instantiate(item.prefab, mousePos, Quaternion.identity);
                    inventory.RemoveItem(item, 1);
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
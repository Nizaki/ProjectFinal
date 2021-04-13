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
    [HideInInspector]
    public UnityEvent onDeath;
    [HideInInspector]
    public UnityEvent<int> onTakeDamage;
    public float darkDelay = 5;
    [HideInInspector]
    public List<GameObject> Interactable = new List<GameObject>();
    [HideInInspector]
    public bool UnderLight;
    float timer;
    [SerializeField]
    GameObject Holder;
    [SerializeField]
    Transform attackPos;
    [SerializeField]
    float attackRange;
    public LayerMask enemy;
    // Start is called before the first frame update
    void Start()
    {
        Hp = MaxHP;
        Hunger = MaxHunger;
        Water = MaxWater;
        if (onDeath == null)
            onDeath = new UnityEvent();
        if (onTakeDamage == null)
            onTakeDamage = new UnityEvent<int>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!alive)
            return;


        //if (Hp <= 0)
        //{
        //    //TODO: แยกฟังค์ชั้น ตายแล้ว ต้องควบคุมไม่ได้
        //    Debug.Log("Death");
        //    alive = false;
        //    onDeath.Invoke();
        //}
        UpdateLight();
        SwingLocation();
    }

    void UpdateLight()
    {
        if (UnderLight || GameTime.state == TimeState.DAY)
        {
            darkDelay += 0.25f * Time.deltaTime;
            if (darkDelay > maxDarkDelay)
                darkDelay = maxDarkDelay;
            return;
        }

        if (GameTime.state == TimeState.NiGHT && !UnderLight)
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
            if(darkDelay < 0)
            {
                darkDelay = 0;
            }

        }
    }

    void SwingLocation()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = pos - Holder.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Holder.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Collider2D[] toDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange);
        Debug.Log(attackPos.position);
    }

    void TakeDamage(int value)
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
        Gizmos.DrawWireSphere(attackPos.position,attackRange);
    }

    GameObject GetClosetInteractable()
    {
        GameObject result = null;
        float minDist = Mathf.Infinity;
        Vector2 currentPos = transform.position;
        foreach (var go in Interactable)
        {
            float dist = Vector2.Distance(go.transform.position, currentPos);
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
        if (collision.CompareTag("Interactive"))
        {
            Interactable.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactive"))
        {
            Interactable.Remove(collision.gameObject);
        }
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

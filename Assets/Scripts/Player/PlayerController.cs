using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public Player player;
    public float speed = 5f;
    public Rigidbody2D rb;
    private Vector2 movement;
    public GameObject craftUI;
    private bool movable = true;
    private bool craftingUI;
    public Animator anime;

    public GameObject pause;
    // Start is called before the first frame update
    private void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (movable)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            anime.SetFloat("Speed",movement.sqrMagnitude);
            anime.SetFloat("Horizontal",movement.x);
            anime.SetFloat("Vertical",movement.y);
        }

        if (Input.GetKeyDown(KeyCode.C)) ToggleCraftUI();
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            player.Attack();
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            player.Use();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause.SetActive(!pause.activeSelf);
        }
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            player.running = true;
        }
        
        else
        {
            player.running = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            player.inventory.SelectSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            player.inventory.SelectSlot(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            player.inventory.SelectSlot(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            player.inventory.SelectSlot(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            player.inventory.SelectSlot(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            player.inventory.SelectSlot(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            player.inventory.SelectSlot(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            player.inventory.SelectSlot(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            player.inventory.SelectSlot(8);
        }
        
    }

    private void EnableMove(bool value)
    {
        movable = value;
    }

    private void ToggleCraftUI()
    {
        if (craftingUI)
            craftUI.transform.DOLocalMoveX(780, 0.5f).SetEase(Ease.InSine);
        else
            craftUI.transform.DOLocalMoveX(500, 0.5f).SetEase(Ease.OutSine);
        craftingUI = !craftingUI;
    }

    private void FixedUpdate()
    {
        if(player.running)
        {
            rb.velocity = movement * speed * Time.fixedDeltaTime * 2;
        }
        else
        {
            rb.velocity = movement * speed * Time.fixedDeltaTime;
        }
    }
}
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

        if (Input.GetKeyDown(KeyCode.P))
        {
            player.TakeDamage(100);
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
        rb.velocity = movement * speed * Time.fixedDeltaTime;
    }
}
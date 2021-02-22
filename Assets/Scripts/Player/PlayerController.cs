using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public PlayerController Instance;
    public float speed = 5f;
    public Rigidbody2D rb;
    Vector2 movement;
    public GameObject craftUI;

    bool movable = true;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (movable)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCraftUI();
        }
    }

    void EnableMove(bool value)
    {
        movable = value;
    }

    void ToggleCraftUI()
    {
        craftUI.SetActive(!craftUI.activeSelf);
    }

    private void FixedUpdate()
    {
        rb.velocity = movement * speed * Time.fixedDeltaTime;
    }
}
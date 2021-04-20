using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
  public PlayerController Instance;
  public Player player;
  public float speed = 5f;
  public Rigidbody2D rb;
  Vector2 movement;
  public GameObject craftUI;
  public EasyTween crafttingPanel;
  bool movable = true;
  private void Awake()
  {
    Instance = this;
  }
  // Start is called before the first frame update
  void Start()
  {
    player = GetComponent<Player>();
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
    if (Input.GetMouseButtonDown(0))
    {
      if (EventSystem.current.IsPointerOverGameObject())
      {
        return;
      }

      player.Attack();
    }
  }

  void EnableMove(bool value)
  {
    movable = value;
  }

  void ToggleCraftUI()
  {
    //craftUI.SetActive(!craftUI.activeSelf);
    crafttingPanel.OpenCloseObjectAnimation();
  }

  private void FixedUpdate()
  {
    rb.velocity = movement * speed * Time.fixedDeltaTime;
  }
}
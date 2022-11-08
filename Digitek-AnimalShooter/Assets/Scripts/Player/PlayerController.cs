using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Menu menu;
    InGameDisplay inGameDisplay;
    PlayerAimWeapon playerAimWeapon;
    public PlayerInputActions playerInputActions;

    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float collisionOffset = 0.05f;
    [SerializeField] ContactFilter2D movementFilter;

    [SerializeField] GameObject hpContainer;
    [SerializeField] //StatusBar hpBar;


    public Vector2 movementInput;
    [HideInInspector] public float lastXInput;
    [HideInInspector] public float lastYInput;


    
    private InputAction pause;
    private InputAction fire;

    public bool fired;

    private Rigidbody2D rb;

    [HideInInspector] public SpriteRenderer spriteRenderer;
    private Animator animator;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();


    public int maxHp = 100;
    public int currentHp = 100;


    void Awake()
    {
        fired = false;


        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAimWeapon = GetComponent<PlayerAimWeapon>();

        playerInputActions = new PlayerInputActions();

        //Kunne gøre det sammen.
        menu = FindObjectOfType<Menu>();
        inGameDisplay = FindObjectOfType<InGameDisplay>();

        //hpBar = hpContainer.GetComponent<StatusBar>();



        //hpBar.SetState(currentHp, maxHp);


    }

    private void OnEnable()
    {
        fire = playerInputActions.Player.Fire;
        fire.Enable();
        fire.performed += Fire;

        pause = playerInputActions.Player.Pause;
        pause.Enable();
        pause.performed += Pause;
    }

    private void OnDisable()
    {
        fire.Disable();
    }

    private void FixedUpdate()
    {
        #region Movement Input
        if (movementInput != Vector2.zero)
        {
            bool success = TryMove(movementInput);

            if (!success)
            {
                success = TryMove(new Vector2(movementInput.x, 0));
                Debug.Log("Movementinput.x");

            }

            if (!success)
            {
                success = TryMove(new Vector2(0, movementInput.y));
                Debug.Log("Movementinput.y");
            }

            //animator.SetBool("isMoving", success);
        }
        else
        {
            //animator.SetBool("isMoving", false);
        }

        if (movementInput.x != 0)
        {
            lastXInput = movementInput.x;
        }
        else if (movementInput.y != 0)
        {
            lastYInput = movementInput.y;
        }

        if (movementInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movementInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        #endregion


    }

    #region Movement
    private bool TryMove(Vector2 direction)
    {

        if (direction != Vector2.zero)
        {
            int count = rb.Cast(
                    movementInput,
                    movementFilter,
                    castCollisions,
                    moveSpeed * Time.fixedDeltaTime + collisionOffset);

            if (count == 0)
            {
                rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }

    }


    private void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }
    #endregion




    //public void TakeDamage(int damage)
    //{
    //    currentHp -= damage;

    //    if (currentHp <= 0)
    //    {
    //        Debug.Log("Game Over");
    //    }
    //    hpBar.SetState(currentHp, maxHp);
    //}

    //public void Heal(int amount)
    //{
    //    if (currentHp <= 0) { return; }

    //    currentHp += amount;
    //    if (currentHp > maxHp)
    //    {
    //        currentHp = maxHp;
    //    }
    //    hpBar.SetState(currentHp, maxHp);
    //}

    private void Pause(InputAction.CallbackContext context)
    {
        Debug.Log("Pause Startet");
        if (menu.gameIsPaused)
        {
            menu.Resume();
            Debug.Log("Resume");
        }
        else
        {
            menu.Pause();
            Debug.Log("Pause");
        }


    }

    private void Fire(InputAction.CallbackContext context)
    {
        //Check om våben reload time er 0 eller under igen, så kan man skyde.
        //Plus tid efter hver gang
        

        if (context.performed)
        {
            playerAimWeapon.HandleShooting();

        }




    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "IDamageAble")
        {
            Debug.Log("Hit da ko");
            inGameDisplay.currentScore += 5;

            inGameDisplay.currentKills += 1;

        }
        else
        {
            return;
        }
    }

}

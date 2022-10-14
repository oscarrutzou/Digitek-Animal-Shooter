using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float collisionOffset = 0.05f;
    [SerializeField] ContactFilter2D movementFilter;

    [SerializeField] GameObject hpContainer;
    [SerializeField] //StatusBar hpBar;


    public Vector2 movementInput;
    [HideInInspector] public float lastXInput;
    [HideInInspector] public float lastYInput;


    [SerializeField] Menu gameManager;



    private Rigidbody2D rb;

    [HideInInspector] public SpriteRenderer spriteRenderer;
    private Animator animator;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();


    public int maxHp = 100;
    public int currentHp = 100;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //hpBar = hpContainer.GetComponent<StatusBar>();



        //hpBar.SetState(currentHp, maxHp);


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


    private void OnMoveInput(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }
    #endregion


    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            Debug.Log("Game Over");
        }
        //hpBar.SetState(currentHp, maxHp);
    }

    public void Heal(int amount)
    {
        if (currentHp <= 0) { return; }

        currentHp += amount;
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
        //hpBar.SetState(currentHp, maxHp);
    }

    public void OnPauseInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (gameIsPaused)
            {
                Resume();
                Debug.Log("Resume");
            }
            else
            {
                Pause();
                Debug.Log("Pause");
            }
        }
    }
}

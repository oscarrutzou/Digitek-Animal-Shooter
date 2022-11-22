using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Menu menu;
    InGameDisplay inGameDisplay;
    PlayerAimWeapon playerAimWeapon;
    WeaponSwitching weaponSwitching;
    public GameObject weaponSwitchingObject;
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
    private InputAction reload;
    private InputAction numKey;
    private InputAction scrool;

    public bool fired;

    private Rigidbody2D rb;

    [HideInInspector] public SpriteRenderer spriteRenderer;
    private Animator animator;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();


    private bool switchTimeBool = false;
    [SerializeField] private float switchTime = 0.2f;
    public float _tempSwitchTime;


    void Awake()
    {
        fired = false;


        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAimWeapon = GetComponent<PlayerAimWeapon>();
        weaponSwitching = GetComponentInChildren<WeaponSwitching>();

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

        reload = playerInputActions.Player.Reload;
        reload.Enable();
        reload.performed += Reload;

        numKey = playerInputActions.Player.NumKey;
        numKey.Enable();
        numKey.performed += NumKeys;

        scrool = playerInputActions.Player.ScollWheelY;
        scrool.Enable();
        scrool.performed += ScollWheelY;
    }

    private void OnDisable()
    {
        fire.Disable();
    }

    private void Update()
    {
        if (switchTimeBool)
        {
            
            if (_tempSwitchTime >= 0)
            {
                _tempSwitchTime -= Time.deltaTime;
            }
            else
            {
                switchTimeBool = false;
            }
        }
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

    private void Reload(InputAction.CallbackContext context)
    {
        //hvis ammo er mindre end max ammo og større end 0
        if (!playerAimWeapon._isReloading && playerAimWeapon._tempAmmo < playerAimWeapon._ammo)
        {
            Debug.Log("Kan Reloade");
            playerAimWeapon.Reload();
            //Spil Reload Icon

        }
        else
        {
            Debug.Log("Reloder allerede, gør intet //");
        }
    }

    public int selectedWeapon;
    private void ScollWheelY(InputAction.CallbackContext context)
    {
        if (!switchTimeBool)
        {
            //int currentDataNumber = playerAimWeapon._dataCurrentNumber;
            int dataAmount = playerAimWeapon._dataAmount;
            int previousSelectedWeapon = weaponSwitching.selectedWeapon;

            float value;
            value = context.ReadValue<float>();


            weaponSwitching.selectedWeapon = selectedWeapon;

            if (value > 0f)
            {
                if (selectedWeapon >= weaponSwitchingObject.transform.childCount - 1)
                {
                    selectedWeapon = 0;
                }
                else
                {
                    

                    if (selectedWeapon == dataAmount)
                    {
                        selectedWeapon = 0;
                        
                    }
                    else
                    {
                        //Debug.Log("selectedWeapon++ " + selectedWeapon + " data " + dataAmount);
                        selectedWeapon++;
                    }

                }

            }
            
            if (value < 0f)
            {
                Debug.Log("down?");

                Debug.Log("selectedWeapon-- " + selectedWeapon + " data " + dataAmount);
                if (selectedWeapon <= 0)
                {
                    selectedWeapon = weaponSwitchingObject.transform.childCount - 1;
                    
                }
                else
                {
                    if (selectedWeapon == 0)
                    {
                        selectedWeapon = dataAmount;
                    }
                    else
                    {
                        selectedWeapon--;
                    }
                }
            }


            if (previousSelectedWeapon != selectedWeapon)
            {
                weaponSwitching.selectedWeapon = selectedWeapon;
                playerAimWeapon._dataCurrentNumber = selectedWeapon;
                weaponSwitching.SelectWeapon();
                Debug.Log(selectedWeapon);
                playerAimWeapon.LoadWeaponData(playerAimWeapon._data, selectedWeapon);
                switchTimeBool = true;
                _tempSwitchTime = switchTime;
            }

            //weaponSwitching.selectedWeapon = weaponSwitching.selectedWeapon % weaponSwitchingObject.transform.childCount;


            
        }
        
    }



    private void NumKeys(InputAction.CallbackContext context)
    {
        if (!switchTimeBool && !playerAimWeapon._isReloading)
        {
            
            float numKeyValueFloat; // the number key value we want from this keypress
            //int.TryParse(context.control.name, out numKeyValue);

            numKeyValueFloat = context.ReadValue<float>();

            int numKeyValueInt = (int)numKeyValueFloat;
            int dataAmount = playerAimWeapon._dataAmount;
            // Warning! If ctx.control.name can't parse as an int, numKeyValue will be 0

            if (numKeyValueInt != playerAimWeapon._dataCurrentNumber)
            {
                if (numKeyValueInt <= dataAmount && !playerAimWeapon._isReloading) //Kun hvis den har fuldt ammo, gem ammo
                {
                    playerAimWeapon._dataCurrentNumber = numKeyValueInt;
                    weaponSwitching.selectedWeapon = numKeyValueInt;
                    selectedWeapon = numKeyValueInt;
                    weaponSwitching.SelectWeapon();
                    playerAimWeapon.LoadWeaponData(playerAimWeapon._data, numKeyValueInt);
                    switchTimeBool = true;
                    _tempSwitchTime = switchTime;
                }
            }

            Debug.Log("int value of keypress is: " + numKeyValueInt);
        }
          
    }

    private void Fire(InputAction.CallbackContext context)
    {
        //Check om våben reload time er 0 eller under igen, så kan man skyde.
        //Plus tid efter hver gang
        

        if (context.performed && !playerAimWeapon._isReloading)
        {
            playerAimWeapon.HandleShooting();

        }
    }


}

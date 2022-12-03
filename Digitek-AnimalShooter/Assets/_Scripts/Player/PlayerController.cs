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


    [HideInInspector] public Vector2 movementInput;
    [HideInInspector] public float lastXInput;
    [HideInInspector] public float lastYInput;


    
    private InputAction pause;
    private InputAction fire;
    private InputAction reload;
    private InputAction numKey;
    private InputAction scrool;

    [HideInInspector] public bool fired;

    private bool timeShootsBool = false;

    private bool holdDownFire = false;

    private Rigidbody2D rb;

    [HideInInspector] public SpriteRenderer spriteRenderer;
    private Animator animator;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    public int selectedWeapon;

    private bool switchTimeBool = false;
    [SerializeField] private float switchTime = 0.2f;
    [HideInInspector] public float _tempSwitchTime;
    
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void Start()
    {
        fired = false;
        menu = FindObjectOfType<Menu>();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAimWeapon = GetComponent<PlayerAimWeapon>();
        weaponSwitching = GetComponentInChildren<WeaponSwitching>();

        inGameDisplay = FindObjectOfType<InGameDisplay>();
    }

    #region Enable + Disable
    private void OnEnable()
    {
        pause = playerInputActions.Player.Pause;
        pause.Enable();
        pause.performed += Pause;

        fire = playerInputActions.Player.Fire;
        fire.Enable();
        fire.performed += Fire;
        fire.canceled += Fire;

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
        pause.Disable();
        reload.Disable();
        numKey.Disable();
        scrool.Disable();
    }
    #endregion

    #region Update + FixedUpdate
    private void Update()
    {
        if (holdDownFire && !playerAimWeapon._isReloading && !timeShootsBool)
        {
            //Debug.Log("hold");
            playerAimWeapon.HandleShooting();
            StartCoroutine(WaitForTimeBetweenShots());
        }

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

            animator.SetBool("isMoving", success);
        }
        else
        {
            animator.SetBool("isMoving", false);
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
    #endregion


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


    private void Pause(InputAction.CallbackContext context)
    {
        if (menu.gameIsPaused)
        {
            menu.Resume();
        }
        else
        {
            menu.Pause();
        }
    }

    private void Reload(InputAction.CallbackContext context)
    {
        //hvis ammo er mindre end max ammo og større end 0
        if (!playerAimWeapon._isReloading && playerAimWeapon._tempAmmo < playerAimWeapon._ammo && !menu.gameIsPaused)
        {
            playerAimWeapon.Reload();
            //Spil Reload Icon
        }
        
    }

    private int dataAmount;
    private void ScollWheelY(InputAction.CallbackContext context)
    {
        if (!switchTimeBool && !playerAimWeapon._isReloading && !menu.gameIsPaused)
        {
            //int currentDataNumber = playerAimWeapon._dataCurrentNumber;
            dataAmount = playerAimWeapon._dataAmount;
            //Debug.Log("dataAmount" + dataAmount);
            int previousSelectedWeapon = weaponSwitching.selectedWeapon;

            float value;
            value = context.ReadValue<float>();


            weaponSwitching.selectedWeapon = selectedWeapon;

            
            if (value > 0f)
            {
                if (selectedWeapon >= dataAmount)
                {
                    selectedWeapon = 0;
                }
                else
                {
                    selectedWeapon++;
                }
            }
            
            if (value < 0f)
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


            if (previousSelectedWeapon != selectedWeapon)
            {
                weaponSwitching.selectedWeapon = selectedWeapon;
                playerAimWeapon._dataCurrentNumber = selectedWeapon;
                weaponSwitching.SelectWeapon();

                if (playerAimWeapon.tempAmmoArray[selectedWeapon] != playerAimWeapon._data[selectedWeapon].ammo)
                {
                    playerAimWeapon.LoadWeaponData(playerAimWeapon._data, selectedWeapon, false);
                }
                else
                {
                    playerAimWeapon.LoadWeaponData(playerAimWeapon._data, selectedWeapon, true);
                }

                switchTimeBool = true;
                _tempSwitchTime = switchTime;
            }
        }   
    }

    private void NumKeys(InputAction.CallbackContext context)
    {
        if (!switchTimeBool && !playerAimWeapon._isReloading && !menu.gameIsPaused)
        {
            
            float numKeyValueFloat; // the number key value we want from this keypress

            numKeyValueFloat = context.ReadValue<float>();

            selectedWeapon = (int)numKeyValueFloat; //For at sørge for at den ikke går ind og indre på andre variabler.

            dataAmount = playerAimWeapon._dataAmount;
            

            if (selectedWeapon != playerAimWeapon._dataCurrentNumber)
            {
                if (selectedWeapon <= dataAmount && !playerAimWeapon._isReloading) //Kun hvis den har fuldt ammo, gem ammo
                {
                    playerAimWeapon._dataCurrentNumber = selectedWeapon;
                    weaponSwitching.selectedWeapon = selectedWeapon;
                    
                    weaponSwitching.SelectWeapon();

                    if (playerAimWeapon.tempAmmoArray[selectedWeapon] != playerAimWeapon._data[selectedWeapon].ammo)
                    {
                        playerAimWeapon.LoadWeaponData(playerAimWeapon._data, selectedWeapon, false);
                    }
                    else
                    {
                        playerAimWeapon.LoadWeaponData(playerAimWeapon._data, selectedWeapon, true);
                    }

                    switchTimeBool = true;
                    _tempSwitchTime = switchTime;
                }
            }
        }
    }



    private void Fire(InputAction.CallbackContext context)
    {
        if (context.performed && !menu.gameIsPaused)
        {
            holdDownFire = true;
        }

        if (context.canceled)
        {
            holdDownFire = false;
            
        }
    }

    private IEnumerator WaitForTimeBetweenShots()
    {
        timeShootsBool = true;
        yield return new WaitForSeconds(playerAimWeapon._timeBetweenShoots);
        timeShootsBool = false;
    }


}

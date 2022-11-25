using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.InputSystem;
using System;

public class PlayerAimWeapon : MonoBehaviour
{
    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs
    {
        public Vector3 gunEndPointPosition;
        public Vector3 shootPosition;
    }

    

    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerShoot playerShoot;

    [SerializeField] private Transform aimTransform;
    [SerializeField] private Transform aimGunEndPointTransform;

    [HideInInspector] public Vector3 aimDirection;

    
    public WeaponData[] _data;
    public int _dataCurrentNumber;

    [HideInInspector] public int _dataAmount;


    [HideInInspector] public int _damage;
    public int _ammo;
    public int _tempAmmo;
    [HideInInspector] public float _reloadTime;
     public float _tempReloadTime;

    [HideInInspector] public bool _isReloading = false;

    public int[] tempAmmoArray;


    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerShoot = GetComponentInChildren<PlayerShoot>();

        if (_data != null)
        {
            CheckDataAmount();
            tempAmmoArray = new int[_data.Length];

            _dataCurrentNumber = 0;

            //for(int i = _dataCurrentNumber; i < _data.Length; i++)
            //{
            //    tempAmmoArray[i] = 0;
            //}

            LoadWeaponData(_data, _dataCurrentNumber, true);
            //ChangeAmmoInArray(tempAmmoArray, _dataCurrentNumber, _tempAmmo);
            
            

        }
    }



    public void Reload() //Lav til at den gemmer dens ammo, array int og gem ammo. Når den loader skal den tjekke om ammo er mindre end max også sætte current ammo til det gemte tal.
    {
        _isReloading = true;

    }

    public IEnumerator ReloadNumerator()
    {

        yield return new WaitForSeconds(_reloadTime);
    }

    void Update()
    {
        HandleAiming();

        if (_tempAmmo == 0 && !_isReloading)
        {
            Reload();
        }

        if (_isReloading)
        {
            if (_tempReloadTime >= 0)
            {
                _tempReloadTime -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Reload finished");
                _isReloading = false;
                _tempReloadTime = _reloadTime;
                _tempAmmo = _ammo;
                ChangeAmmoInArray(tempAmmoArray, _dataCurrentNumber, _tempAmmo);
            }
        }
    }

    

    //Kald hvis man samler et våben op in game.
    void CheckDataAmount()
    {
        _dataAmount = _data.Length - 1; // -1 for at få den til at være det samme som numKeys
        //Debug.Log(_dataAmount);
    }

    

    //Kald hver gang der skiftes weapon []. 
    public void LoadWeaponData(WeaponData[] data, int dataNumber, bool fullMag)
    {
        _damage = data[dataNumber].damage;
        _reloadTime = data[dataNumber].reloadTime;
        _tempReloadTime = _reloadTime;
        _ammo = data[dataNumber].ammo;

        if (tempAmmoArray[dataNumber] == 0)
        {
            _tempAmmo = _ammo;
        }
        else if (!fullMag)
        {
            _tempAmmo = tempAmmoArray[dataNumber];
        }
        else
        {
            Debug.Log(_tempAmmo + " rytryt  " + _ammo);
            _tempAmmo = _ammo;
        }

        ChangeAmmoInArray(tempAmmoArray, dataNumber, _tempAmmo);

        //if (tempAmmoBool)
        //{
        //    _tempAmmo = _ammo;
        //}
        //else
        //{
        //    ChangeAmmoInArray(tempAmmoArray, dataNumber, _tempAmmo);
        //    _tempAmmo = _ammo;
        //}
    }

    public void ChangeAmmoInArray(int[] ammoArray, int dataNumber, int tempAmmo)
    {
        ammoArray[dataNumber] = tempAmmo;
    }

    private void HandleAiming()
    {
        Vector3 mousePosition = GetMouseWorldPosition();

        aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 aimLocalScale = Vector3.one;
        if (angle > 90 || angle < -90)
        {
            aimLocalScale.y = -1f;
        }
        else
        {
            aimLocalScale.y = +1f;
        }

        aimTransform.localScale = aimLocalScale;
    }

    public void HandleShooting()
    {
        Debug.Log("Fire dat shit");
        //Play animation hvis der var en
        Vector3 mousePosition = GetMouseWorldPosition();

        OnShoot?.Invoke(this, new OnShootEventArgs {
            gunEndPointPosition = aimGunEndPointTransform.position,
            shootPosition = mousePosition  
        });
    }



    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

}

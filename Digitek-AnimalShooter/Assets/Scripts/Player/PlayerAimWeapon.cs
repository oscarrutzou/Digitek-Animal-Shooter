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

    public WeaponData[] _data;
    private GameObject weaponGameObject;
    public int _dataCurrentNumber;
    public int _dataNumber;
    public int _dataAmount;
    public bool _isReloading = false;

    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerShoot playerShoot;

    [SerializeField] private Transform aimTransform;
    [SerializeField] private Transform aimGunEndPointTransform;

    public Vector3 aimDirection;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerShoot = GetComponentInChildren<PlayerShoot>();

        if (_data != null)
        {
            CheckDataAmount(); //Bruges senere

            _dataCurrentNumber = 0;

            LoadWeaponData(_data, _dataCurrentNumber);

            //for (int i = _dataCurrentNumber; i < _dataAmount; i++)
            //{
            //    LoadWeaponData(_data, _dataNumber);
            //}

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
            }
        }
    }

    

    //Kald hvis man samler et våben op in game.
    void CheckDataAmount()
    {
        _dataAmount = _data.Length - 1; // -1 for at få den til at være det samme som numKeys
        //Debug.Log(_dataAmount);
    }

    public int _damage;
    public int _ammo;
    public int _tempAmmo;
    public float _reloadTime;
    public float _tempReloadTime;

    //Kald hver gang der skiftes weapon []. 
    public void LoadWeaponData(WeaponData[] data, int dataNumber)
    {
        _damage = data[dataNumber].damage;
        _ammo = data[dataNumber].ammo;
        _tempAmmo = _ammo;
        _reloadTime = data[dataNumber].reloadTime;
        _tempReloadTime = _reloadTime;
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

    //public IEnumerator ReloadShooting()
    //{
    //    int autoReloadCount = 0;
    //    while (true)
    //    {

    //    }

    //    if (true)
    //    {
    //        autoReloadCount = 0;

    //    }
    //    yield return null;
    //}



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

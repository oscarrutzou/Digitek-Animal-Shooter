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

    public WeaponData data;
    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerShoot playerShoot;

    [SerializeField] private Transform aimTransform;
    [SerializeField] private Transform aimGunEndPointTransform;

    public Vector3 aimDirection;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerShoot = GetComponentInChildren<PlayerShoot>();

        //if (data != null)
        //{
        //    LoadWeaponData(data);
        //}

    }
    
   

    //Kald hver gang der skiftes weapon []. 
    private void LoadWeaponData(WeaponData _data)
    {
        

    }

    private void Update()
    {

        HandleAiming();
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

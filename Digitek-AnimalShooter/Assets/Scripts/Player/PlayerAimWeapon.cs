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

    PlayerController playerController;
    private Transform aimTransform;
    private Transform aimGunEndPointTransform;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        aimTransform = transform.Find("Aim");
        aimGunEndPointTransform = aimTransform.Find("GunEndPointPosition");
    }

    private void Update()
    {

        HandleAiming();
        //HandleShooting();

    }

    private void HandleAiming()
    {
        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
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
        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();

        OnShoot?.Invoke(this, new OnShootEventArgs {
            gunEndPointPosition = aimGunEndPointTransform.position,
            shootPosition = mousePosition,  
        });
    }

}

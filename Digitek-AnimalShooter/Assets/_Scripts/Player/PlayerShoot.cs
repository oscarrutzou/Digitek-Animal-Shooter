using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private BulletRaycast bulletRaycast;

    [SerializeField] public PlayerAimWeapon playerAimWeapon;
    public InGameDisplay gameDisplay;
    //PlayerController playerController;

    [SerializeField] private Material weaponTracerMaterial;
    [SerializeField] private Sprite shootFlashSprite;

    public GameObject currentWeapon;

    [SerializeField] private float cameraShakeIntensity = 3f;
    [SerializeField] private float cameraShakeDuration = .1f;

    AudioManager audioManager;

    private void Start()
    {
        bulletRaycast = GetComponent<BulletRaycast>();

        playerAimWeapon = GetComponentInParent<PlayerAimWeapon>();
        //playerController = GetComponentInParent<PlayerController>();

        playerAimWeapon.OnShoot += PlayerAimWeapon_OnShoot;

        gameDisplay = FindObjectOfType<InGameDisplay>();
        audioManager = FindObjectOfType(typeof(AudioManager)) as AudioManager;
    }



    private void PlayerAimWeapon_OnShoot(object sender, PlayerAimWeapon.OnShootEventArgs e)
    {
        float raycastDistance = 0.2f;

        //Debug.DrawRay(e.shootPosition, playerAimWeapon.aimDirection * raycastDistance, Color.green, 0.2f);
        bulletRaycast.Shoot(e.shootPosition, playerAimWeapon.aimDirection, raycastDistance);


        CameraShake.Instance.ShakeCamera(cameraShakeIntensity, cameraShakeDuration);

        int number = Random.Range(1, 2);
        audioManager.Play("GunShot" + number);

        playerAimWeapon._tempAmmo--; //fjerner 1 hos tempammo
        playerAimWeapon.ChangeAmmoInArray(playerAimWeapon.tempAmmoArray, playerAimWeapon._dataCurrentNumber, playerAimWeapon._tempAmmo);

        gameDisplay.ammoUsed++;
        CreateWeaponTracer(e.gunEndPointPosition, e.shootPosition);

        //For at vise det flash når man skyder
        //CreateShootFlash(e.gunEndPointPosition);
    }



    private void CreateShootFlash(Vector3 spawnPosition)
    {
        World_Sprite worldSprite = World_Sprite.Create(spawnPosition, shootFlashSprite);
        FunctionTimer.Create(worldSprite.DestroySelf, 0.1f);
    }

    private void CreateWeaponTracer(Vector3 fromPosition, Vector3 targetPosition)
    {
        Vector3 dir = (targetPosition - fromPosition).normalized;
        float eulerZ = GetAngleFromVectorFloat(dir) - 90;
        float distance = Vector3.Distance(fromPosition, targetPosition);
        Vector3 tracerSpawnPosition = fromPosition + dir * distance * 0.4f;

        Material tmpWeaponTracerMaterial = new Material(weaponTracerMaterial);
        tmpWeaponTracerMaterial.SetTextureScale("_MainTex", new Vector2(1f, distance / 64f));

        //Forstår godt hvordan man laver et world mesh ligesom denne, brugte den her for at gøre det nemmere siden jeg er alene som programmør.
        World_Mesh worldMesh = World_Mesh.Create(tracerSpawnPosition, eulerZ, 1f, distance, tmpWeaponTracerMaterial, null, 10);


        //Fjern her og de andre kommenterede for at få animation til tracer.
        //Animationen skal f.eks. kunne fade ud.
        //int frame = 0;
        //float framerate = 0.016f;
        //float timer = framerate;
        //worldMesh.SetUVCoords(new World_Mesh.UVCoords(0, 0, 16, 64));

        float timer = 0.1f;
        FunctionUpdater.Create(() =>
        {
            timer -= Time.deltaTime;
            
            if (timer <= 0)
            {
                //frame++;
                //timer += framerate;
                //if (frame >= 4)
                //{
                    //worldMesh.DestroySelf();
                    //return true;
                //} else{
                //worldMesh.SetUVCoords(new World_Mesh.UVCoords(16 * frame, 0, 16, 64));
                //}
                
                worldMesh.DestroySelf(); //Slet her
                return true; //Slet her
            }
            return false;
        });

    }



    #region Math
    public float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (n < 0)
        {
            n += 360;
        }

        return n;
    }

    #endregion
}

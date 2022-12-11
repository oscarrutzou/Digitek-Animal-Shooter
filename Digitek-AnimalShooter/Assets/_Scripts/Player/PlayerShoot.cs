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
    private float raycastDistance = 0.2f;

    AudioManager audioManager;

    private void Start()
    {
        bulletRaycast = GetComponent<BulletRaycast>();

        playerAimWeapon = GetComponentInParent<PlayerAimWeapon>();
        //playerController = GetComponentInParent<PlayerController>();

        playerAimWeapon.OnShoot += PlayerAimWeapon_OnShoot;

        gameDisplay = FindObjectOfType<InGameDisplay>();
        audioManager = FindObjectOfType<AudioManager>();

    }


    //Bruger EventArgs for at gøre det nemmere.
    private void PlayerAimWeapon_OnShoot(object sender, PlayerAimWeapon.OnShootEventArgs e)
    {
        
        //Debug.DrawRay(e.shootPosition, playerAimWeapon.aimDirection * raycastDistance, Color.green, 0.2f);
        //Laver et raycast i den retning som man peger sin mus.
        bulletRaycast.Shoot(e.shootPosition, playerAimWeapon.aimDirection, raycastDistance);

        //Laver et camerashake med en bestemt Intensity og Duration.
        CameraShake.Instance.ShakeCamera(cameraShakeIntensity, cameraShakeDuration);

        //For at vælge et random nummer af gunShot lyde fra audioManageren,
        int number = Random.Range(1, 2);
        audioManager.Play("GunShot" + number);

        playerAimWeapon._tempAmmo--; //Fjerner 1 hos tempammo
        //Ændre temp ammo array til det som man har. Gemmer tempAmmo til når man skifter våben så den kan huske hvor meget man har tilbage.
        playerAimWeapon.ChangeAmmoInArray(playerAimWeapon.tempAmmoArray, playerAimWeapon._dataCurrentNumber, playerAimWeapon._tempAmmo);
        
        
        gameDisplay.ammoUsed++; //Plusser 1 hos ammo brugt
        //Laver en tracer i den retning men skyder.
        CreateWeaponTracer(e.gunEndPointPosition, e.shootPosition);
        //For at vise det flash når man skyder
        CreateShootFlash(e.gunEndPointPosition);
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

        int frame = 0;
        float framerate = 0.02f;
        float timer = framerate;
        worldMesh.SetUVCoords(new World_Mesh.UVCoords(0, 0, 16, 64));

        FunctionUpdater.Create(() =>
        {
            timer -= Time.deltaTime;
            
            if (timer <= 0)
            {
                frame++;
                timer += framerate;
                if (frame >= 4)
                {
                    worldMesh.DestroySelf();
                    return true;
                }
                else
                {
                    worldMesh.SetUVCoords(new World_Mesh.UVCoords(16 * frame, 0, 16, 64));
                }
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

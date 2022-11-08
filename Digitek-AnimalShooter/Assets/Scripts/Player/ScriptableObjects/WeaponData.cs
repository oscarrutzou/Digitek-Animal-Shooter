using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public GameObject weaponObject;
    public int damage = 10;
    public int ammo = 15;
    public float reloadTime = 3f;
}

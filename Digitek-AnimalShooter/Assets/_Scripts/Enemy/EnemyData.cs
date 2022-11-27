using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public GameObject enemyModel;
    public int health = 30;
    public int score = 5;
}

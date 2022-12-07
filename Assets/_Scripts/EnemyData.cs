using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy")]
public class EnemyData : ScriptableObject
{
    // TODO

    public Mesh mesh;
    public string displayName;

    // Statistics
    public int maxHealth;
    public float movementSpeed;
    public bool isStationary;
    public int damage;

    // AI sensor, pathfinding things
    public float repathRate;
    public float patrolRate;
    public float patrolRadius;

    public AISensor.SensorType sensorType;
    public float searchRadius;
    public float targetRadius;
    public float updateRate;
    public int angle;
}

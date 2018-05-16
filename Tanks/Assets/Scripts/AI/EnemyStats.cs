using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Strategy
{
    OFFENSIVE,
    DEFENSIVE,
    NONE
};

public class EnemyStats: MonoBehaviour
{
    public Transform turretTransform;
    public Transform turretEndTransform;
    public int mineCount;
    public int bulletCount;
    public int ricochetCount;
    public BulletSpeed bulletSpeed;
    public TankSpeed tankSpeed;
    public Intelligence tankIntelligence;
    public Strategy strategy;
}

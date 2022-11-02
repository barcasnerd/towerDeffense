using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBehaviour : MonoBehaviour
{
    private Vector3 shootDir;
    private int damage;

    public void Setup(Vector3 ShootDir, int damage)
    {
        this.shootDir = ShootDir;
        this.damage = damage;
    }

    private void Update()
    {
        float moveSpeed = 15f;
        transform.position += shootDir * moveSpeed * Time.deltaTime;
    }

    public int GetDamage()
    {
        return damage;
    }
}

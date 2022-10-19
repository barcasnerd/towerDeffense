using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSource : MonoBehaviour
{
    public int HP = 1000;

    private void Awake()
    {
        HP = 1000;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit");
        if (collision.gameObject.tag == "Bullet")
        {
            
            HP -= 20;
            Debug.Log("Hit by a bullet, new HP "+ HP);
            Destroy(collision.gameObject);
            if (HP < 0)
            {
                Destroy(this.gameObject);
                GameManager.Instance.UpdateGameState(GameManager.GameStateEnum.end);
            }
        }
    }
}

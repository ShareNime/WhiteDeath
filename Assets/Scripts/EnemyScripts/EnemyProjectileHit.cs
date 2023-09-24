using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileHit : MonoBehaviour
{
    public int damage;
    bool isColliding;
    public string nameSound;
    // Start is called before the first frame update
    private void Start()
    {
        FindObjectOfType<AudioManager>().Play(nameSound);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            if (isColliding) return;
            isColliding = true;
            collision.GetComponent<PlayerHealth>().decreaseHP(damage);
            
            Destroy(gameObject);
            //GetComponentInParent<EnemyMovement>().enemy.damage
        }
        if (collision.CompareTag("Hitable"))
        {
            Destroy(gameObject);
        }
    }
}

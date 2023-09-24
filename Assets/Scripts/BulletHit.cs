using UnityEngine;

class BulletHit : MonoBehaviour
{
    float damage;
    bool isColliding;
    private void Start()
    {
        if(FindObjectOfType<PlayerMovement>().currGun == gunState.gun1)
        {
            FindObjectOfType<AudioManager>().Play("W1Shoot");
            damage = 1;
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("W2Shoot");
            damage = 4;
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (isColliding) return;
            isColliding = true;
            Destroy(gameObject);
            
            collision.GetComponent<EnemyHit>().enemyTakeDamage(damage);
        }
        if (collision.CompareTag("Hitable"))
        {
            if (isColliding) return;
            isColliding = true;
            
            Destroy(gameObject);
        }
        if (collision.CompareTag("Target"))
        {
            if (isColliding) return;
            isColliding = true;
            
            Destroy(gameObject);
            collision.GetComponent<TargetDead>().takeDamage(damage);
        }
       
    }
    private void Update()
    {
        isColliding = false;
    }
}

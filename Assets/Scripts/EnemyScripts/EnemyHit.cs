using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{   [SerializeField]
    private float currHP;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        currHP = GetComponent<EnemyMovement>().enemy.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(currHP <= 0)
        {
            StartCoroutine(enemyDead());
        }
    }
    public void enemyTakeDamage(float damage)
    {
        currHP -= damage;
        StartCoroutine(enemyChangeColor());
    }
    IEnumerator enemyDead()
    {
        anim.SetTrigger("Dead");
        Destroy(GetComponent<EnemyMovement>());
        //GetComponent<EnemyMovement>().currMoveSpeed = 0;
        
        
        
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
    IEnumerator enemyChangeColor()
    {
        GetComponent<SpriteRenderer>().color = new Color(120, 0, 0);
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}

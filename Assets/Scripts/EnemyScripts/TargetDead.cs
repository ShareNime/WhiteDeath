using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDead : MonoBehaviour
{
    // Start is called before the first frame update
    public Enemy enemy;
    [SerializeField]
    private float currHealth; 
    void Start()
    {
        currHealth = enemy.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(currHealth <= 0)
        {
            Dead();
        }
    }
    public void takeDamage(float damage)
    {
        currHealth -= damage;
        StartCoroutine(changeColor());
        
        
    }
    void Dead()
    {
        Destroy(gameObject);
    }
    IEnumerator changeColor()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}

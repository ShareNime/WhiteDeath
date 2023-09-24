using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public Vector2 targetPos;
    public float speed;
  
    // Start is called before the first frame update
    private void Awake()
    {
        
    }
    void Start()
    {
        targetPos = FindObjectOfType<PlayerHealth>().transform.position;
        targetPos -= GetComponent<Rigidbody2D>().position;
        gameObject.transform.Rotate(0f, 0f, Mathf.Atan2(targetPos.y + 1.8f, targetPos.x) * Mathf.Rad2Deg);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position.Normalize();
        GetComponent<Rigidbody2D>().velocity = new Vector2(targetPos.x, targetPos.y+1f )* speed;
        Destroy(gameObject, 1f);
    }
}

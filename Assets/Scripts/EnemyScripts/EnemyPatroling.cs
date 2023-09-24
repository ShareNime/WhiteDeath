using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatroling : MonoBehaviour
{
    public Transform[] patrolPoints;
    private int currIndex;
    public float waitTime;

    bool once;
    
    // Update is called once per frame
    void Update()
    {
        if (transform.position != patrolPoints[currIndex].position)
        {
            transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currIndex].position, GetComponent<EnemyMovement>().enemy.moveSpeed);
        }
        else
        {
            if (once == false)
            {
                once = true;
                StartCoroutine(Wait());
            }
        }
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitTime);
        if(currIndex + 1 < patrolPoints.Length)
        {
            currIndex++;
        }
        else
        {
            currIndex = 0;
        }
    }
}

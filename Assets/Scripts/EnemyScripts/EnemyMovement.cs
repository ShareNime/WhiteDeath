using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Enemy enemy;
    
    [Header("Movement")]
    public float chaseDistance;
    public float shootDistance;
    public float gunHearDistance;
    public Transform target;
    public float currMoveSpeed;
    private Vector3 movement;
    private bool isHearing = false;
    public bool playWalkingSound = true;

    [Header("Shooting")]
    public GameObject shootProjectile;
    public float shootDelay;
    private float nextShootDelay;
    public bool isShooting = false;
    public bool canShoot = true;
    public Transform barrelPos;
    
    [Header("Patrol")]
    public Transform[] patrolPoints;
    private int currIndex;
    public float waitTime;
    [Header("Animation")]
    public Animator anim;
    bool once;

    // Start is called before the first frame update
    void Start()
    {
        currMoveSpeed = enemy.moveSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<GlobalEventScript>().isStart)
        {
            enemy.moveSpeed = 1f;
        }
        else
        {
            enemy.moveSpeed = 3f;
        }
        bool flipped = movement.x > 0;
        this.transform.rotation = Quaternion.Euler(new Vector3(0f, flipped ? 180f : 0f, 0f));
        checkHearing();
        if(Vector2.Distance(transform.position,target.position)<= chaseDistance || isHearing)
        {
            enemyAggro();
        }
        else
        {
            enemyPatrol();
        }
        if(!playWalkingSound && Vector2.Distance(transform.position, target.position) <= gunHearDistance && movement != Vector3.zero)
        {
            gameObject.GetComponent<AudioSource>().Play();
            gameObject.GetComponent<AudioSource>().volume = 0.5f;
            playWalkingSound = true;
            
        }
        else if(movement == Vector3.zero || Vector2.Distance(transform.position, target.position) >= gunHearDistance)
        {
            gameObject.GetComponent<AudioSource>().volume = 0f;
            playWalkingSound = false;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
        Gizmos.DrawWireSphere(transform.position, shootDistance);
        Gizmos.DrawWireSphere(transform.position, gunHearDistance);
    }
    void enemyPatrol()
    {
        if (transform.position != patrolPoints[currIndex].position)
        {
            transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currIndex].position,  enemy.moveSpeed * Time.deltaTime);
            movement = (patrolPoints[currIndex].position - transform.position);
            anim.SetFloat("Horizontal", movement.x);
            anim.SetFloat("Vertical", movement.y);
            anim.SetFloat("Move", 1);
        }
        else
        {
            if (once == false)
            {
                once = true;
                StartCoroutine(Wait());
            }
        }
        IEnumerator Wait()
        {
            anim.SetFloat("Move", 0);
            yield return new WaitForSeconds(waitTime);
            if (currIndex + 1 < patrolPoints.Length)
            {
                currIndex++;
            }
            else
            {
                currIndex = 0;
            }
            once = false;
        }
    }
    void enemyAggro()
    {
        //mundur
        if (Vector2.Distance(transform.position, target.position) < shootDistance)
        {
                if (!isShooting)
                {              
                transform.position = Vector2.MoveTowards(transform.position, target.position, -currMoveSpeed * Time.deltaTime);
                movement = (target.position - transform.position);
                anim.SetFloat("Horizontal",movement.x);
                anim.SetFloat("Vertical", movement.y);                      
                anim.SetFloat("Move", 1);
                }
                
                currMoveSpeed = enemy.moveSpeed;
            
        }
        else if (isHearing)
        {
            if (!isShooting)
            {
                isHearing = true;
                
                transform.position = Vector2.MoveTowards(transform.position, target.position, currMoveSpeed * Time.deltaTime);
                movement = (target.position - transform.position);


                anim.SetFloat("Horizontal", movement.x);
                anim.SetFloat("Vertical", movement.y);

                currMoveSpeed = enemy.moveSpeed;

                anim.SetFloat("Move", 1);
                if(Vector2.Distance(transform.position, target.position) >= gunHearDistance)
                {
                    isHearing = false;
                }
            }
        }
        //kejar
        else if (Vector2.Distance(transform.position, target.position) < chaseDistance && Vector2.Distance(transform.position, target.position) > shootDistance+1)
        {
                if (!isShooting)
                {
                
                transform.position = Vector2.MoveTowards(transform.position, target.position, currMoveSpeed * Time.deltaTime);
                movement = (target.position - transform.position);

                
                anim.SetFloat("Horizontal", movement.x);
                anim.SetFloat("Vertical", movement.y);
                
                currMoveSpeed = enemy.moveSpeed;
                
                anim.SetFloat("Move", 1);
                }
        }
        if(Vector2.Distance(transform.position,target.position) < chaseDistance || isHearing)
        {
            if (!isShooting && canShoot)
            {
                StartCoroutine(Shooting());
            }
            if(isShooting)
            {
                
            }
            
        }
    }
    
    IEnumerator Shooting()
    {
        if (canShoot)
        {
            canShoot = false;
            isShooting = true;
            anim.SetBool("Shooting", true);
            anim.SetFloat("Move", 0f);
            currMoveSpeed = 0;
            yield return new WaitForSeconds(1f);
            
            Instantiate(shootProjectile, barrelPos.position, Quaternion.identity);
            isShooting = false;
            anim.SetBool("Shooting", false);
            yield return new WaitForSeconds(1f);
        }
        canShoot = true;
    }
    void checkHearing()
    {
        if (target.GetComponent<PlayerMovement>().isShooting && Vector2.Distance(transform.position, target.position) <= gunHearDistance && !isHearing)
        {         
            isHearing= true;
        }
        else if(target.GetComponent<PlayerMovement>().isShooting && Vector2.Distance(transform.position, target.position) >= gunHearDistance && isHearing)
        {        
            isHearing= false;
        }      
    }
}

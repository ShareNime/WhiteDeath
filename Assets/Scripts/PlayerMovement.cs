using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public enum gunState
{
    gun1,gun2
}
public enum playerState
{
    walking, hurt, die, shooting, idle
}
class PlayerMovement : MonoBehaviour
{
    public gunState currGun;
    public playerState currState;
    public float moveSpeed = 10f;
    private Rigidbody2D RB;
    private Vector2 movement;
    public GameObject Bullet;
    public float Bulletspeed = 1f;
    public GameObject crossHair;
    public bool canWalk = true;
    public bool isShooting = false;
    public GameObject penanda;
    public Transform portalPos;
    bool playWalkSound = false;
    [Header("Guns")]
    public int gun1maxammo = 50;
    public int gun2maxammo = 5;
    private int currAmmo;
    public TextMeshProUGUI currAmmoText;
    public GameObject gun1;
    public GameObject gun2;
    public float gun1ReloadTime = 5f;
    public float gun2ReloadTime = 3f;
    public bool isReloading = false;
    public Transform barrelGun1;
    public Transform barrelGun2;
    public bool canShoot = true;
    public Slider reloadSlider;
    public GameObject gun1UI;
    public GameObject gun2UI;

    Vector2 crosshairpos;
    [Header("Animation")]
    private Animator playerAnim;
    private void Start()
    {
        currState = playerState.idle;
        currGun = gunState.gun1;
        currAmmo = PlayerPrefs.GetInt("gun1Ammo", 50);
        RB = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
    }
    private void Update()
    {

        if (FindObjectOfType<GlobalEventScript>().isStart)
        {
            moveSpeed = 3;
        }
        else
        {
            moveSpeed = 5;
        }
        var dir = portalPos.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 80;
        penanda.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        crosshairpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        crossHair.transform.position = crosshairpos;
        movement = Vector2.zero;
        movement.y = Input.GetAxisRaw("Vertical");
        movement.x = Input.GetAxisRaw("Horizontal");
        if (playWalkSound)
        {
            FindObjectOfType<AudioManager>().Play("Walking");
            playWalkSound = false;
        }
        else if(movement == Vector2.zero)
        {
            playWalkSound = true;
        }
        currAmmoText.text = currAmmo.ToString();
        if(currState != playerState.shooting)
        {
            isShooting = false;
        }
        else
        {
            isShooting = true;
        }
        if(!isReloading)
        {
            changeGun();
            Reload();
            Shooting();
            reloadSlider.value = 0;
        }
        else
        {
            if(currGun == gunState.gun1)
            {
                reloadSlider.value += Time.deltaTime * 1f;
            }
            else
            {
                reloadSlider.value += Time.deltaTime * 1f;
            }
            
        }
        if (movement == Vector2.zero)
        {
            currState = playerState.idle;
        }
        bool flipped = movement.x > 0 || crosshairpos.x - transform.position.x > 0;
        if(movement.x > 0 && crosshairpos.x - transform.position.x < 0)
        {
            flipped = false;
        }
        if (canWalk)
        {          
        this.transform.rotation = Quaternion.Euler(new Vector3(0f, flipped ? 180f : 0f, 0f));         
        }
        playerAnim.SetBool("Shoot", !canWalk);
    }
    private void FixedUpdate()
    {
        if (canWalk)
        {
            
            UpdateAnimationAndMove();
        }
        
    }
    private void UpdateAnimationAndMove()
    {
        if (movement != Vector2.zero)
        {
            this.transform.Translate(new Vector3(movement.x * moveSpeed * Time.deltaTime, movement.y * moveSpeed * Time.deltaTime), Space.World);
            currState = playerState.walking;
 
        }  
        if (canWalk)
        {   
            playerAnim.SetFloat("X", movement.x);
            playerAnim.SetFloat("Y", movement.y);
            playerAnim.SetFloat("Move", movement.magnitude);
        }
        
    }
    IEnumerator AimAndShoot()
    {
        canWalk = false;
        canShoot = false;
        yield return new WaitForSeconds(0.1f);
        isShooting = true;
        currState = playerState.shooting;
        
        Vector2 mouseCursorePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mouseCursorePos - new Vector2(barrelGun1.position.x,barrelGun1.position.y);
        lookDir.Normalize();
        GameObject bulletShooting = Instantiate(Bullet, barrelGun1.position, Quaternion.identity);
        currAmmo -= 1;
        if(currGun == gunState.gun1)
        {
            PlayerPrefs.SetInt("gun1Ammo", currAmmo);
        }
        else if(currGun == gunState.gun2)
        {
            PlayerPrefs.SetInt("gun2Ammo", currAmmo);
        }
        bulletShooting.GetComponent<Rigidbody2D>().velocity = lookDir * Bulletspeed;
        bulletShooting.transform.Rotate(0f, 0f, Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg);
        Destroy(bulletShooting, 2f);
        playerAnim.SetFloat("CrossHairX", lookDir.x);
        playerAnim.SetFloat("CrossHairY", lookDir.y);
        yield return new WaitForSecondsRealtime(0.4f);
        canShoot = true;
        canWalk = true;
    }
    void changeGun()
    {
        
        if (currGun != gunState.gun1 && Input.GetKeyDown(KeyCode.Alpha1))
        {
            FindObjectOfType<AudioManager>().Play("ChangeWeapon");
            currGun = gunState.gun1;
            gun1UI.SetActive(true);
            gun2UI.SetActive(false);
            currAmmo = PlayerPrefs.GetInt("gun1Ammo", gun1maxammo);
            gun2.SetActive(false);
            gun1.SetActive(true);
            reloadSlider.maxValue = gun1ReloadTime;
        }
        else if (currGun != gunState.gun2 && Input.GetKeyDown(KeyCode.Alpha2))
        {
            FindObjectOfType<AudioManager>().Play("ChangeWeapon");
            currGun = gunState.gun2;
            gun1UI.SetActive(false);
            gun2UI.SetActive(true);
            currAmmo = PlayerPrefs.GetInt("gun2Ammo", gun2maxammo);
            gun1.SetActive(false);
            gun2.SetActive(true);
            reloadSlider.maxValue = gun2ReloadTime;
        }
    }
    private void Reload()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            if (currGun == gunState.gun1 && currAmmo != gun1maxammo)
            {
                FindObjectOfType<AudioManager>().Play("W1Reload");
                isReloading = true;
                Invoke("ReloadGun1", gun1ReloadTime);
                reloadSlider.value = 0;
            }
            else if (currGun == gunState.gun2 && currAmmo != gun2maxammo)
            {
                FindObjectOfType<AudioManager>().Play("W2Reload");
                isReloading = true;
                Invoke("ReloadGun2", gun2ReloadTime);
                reloadSlider.value = 0;
            }
        }
    }
    void Shooting()
    {
        if (currAmmo != 0 && currState != playerState.walking)
        {
            
            if (Input.GetKeyDown("space") && canShoot)
            {
                StartCoroutine(AimAndShoot());             
            }
        }
    }
    void ReloadGun1()
    {
        currAmmo = gun1maxammo;
        PlayerPrefs.SetInt("gun1Ammo", gun1maxammo);
        
        isReloading = false; 
    }
    void ReloadGun2()
    {
        currAmmo = gun2maxammo;
        PlayerPrefs.SetInt("gun2Ammo", gun2maxammo);
        
        isReloading = false;  
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    private int currHealth;
    public GameObject PlayerDeadAnim;
    public Slider HealthSlider;
    private bool LowHPSound = true;
    private bool once = false;
    private void Start()
    {
        currHealth = maxHealth;
        HealthSlider.maxValue = maxHealth;
    }
    private void Update()
    {
        HealthSlider.value = currHealth;
        if(currHealth <= 0)
        {
            PlayerDead();
        }
        if(currHealth <= 3 && LowHPSound)
        {
            FindObjectOfType<AudioManager>().Play("LowHP");
            LowHPSound = false;
        }
    }
    public void decreaseHP(int damage)
    {
        currHealth -= damage;
    }
    void PlayerDead()
    {
        if (!once)
        {
            FindObjectOfType<AudioManager>().Play("Dead");
            FindObjectOfType<AudioManager>().Stop("LowHP");
            once = true;
        }
        transform.position = new Vector2(999f, 999f);
        gameObject.GetComponent<PlayerMovement>().moveSpeed = 0;
        PlayerDeadAnim.SetActive(true);
    }
}

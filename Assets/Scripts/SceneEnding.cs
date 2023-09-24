using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneEnding : MonoBehaviour
{
    public float changeTime;
    public GameObject trigger;
    public GameObject CreditScene;
    public SpriteRenderer badai;
   
    
    // Start is called before the first frame update


    // Update is called once per frame

    void Update()
    {
        changeTime -= Time.deltaTime;
        if (changeTime <= 0)
        {
            trigger.SetActive(true);
            CreditScene.SetActive(true); 
                
        }
    }
    
}

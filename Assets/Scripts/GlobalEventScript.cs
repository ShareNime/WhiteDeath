using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEventScript : MonoBehaviour
{
    private float eventDuration;
    private float eventStartTime;
    public bool isStart = true;
    public GameObject eventGameobject;
    private bool blizzardSoundPlay = true;
    // Start is called before the first frame update
    void Start()
    {
        eventDuration = Random.Range(10f, 30f);
        eventStartTime = Random.Range(60f, 100f);
        StartCoroutine(eventNow());
        gameObject.GetComponent<AudioSource>().volume = 0f;
    }
    private void Update()
    {
        if (!isStart)
        {
            
        }
    }
    IEnumerator eventNow()
    {
        while (true)
        {
            yield return new WaitForSeconds(eventStartTime);
            isStart = true;
            eventGameobject.SetActive(true);
            if (blizzardSoundPlay)
            {
                gameObject.GetComponent<AudioSource>().Play();
                gameObject.GetComponent<AudioSource>().volume = 1f;
                
                blizzardSoundPlay = false;
            }
            yield return new WaitForSeconds(eventDuration);
            isStart = false;
            eventGameobject.SetActive(false);
            blizzardSoundPlay = true;
            gameObject.GetComponent<AudioSource>().volume = 0f;

        }
        
    }
}

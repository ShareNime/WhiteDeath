using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayScript : MonoBehaviour
{
    public string BGMName;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Play(BGMName);
    }

    // Update is called once per frame
    
}

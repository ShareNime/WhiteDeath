using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerBlack : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;
    bool isPlayed = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Update()
    {
        if (isPlayed != true)
        {
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            isPlayed = true;
            
        }
    }
}

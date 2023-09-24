using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;
    public bool playerInRange;
    public bool Interact = false;
    public GameObject InteractButton;
    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }
    private void Update()
    {
        if(playerInRange)
        {
            if(DialogueManager.GetInstance().dialogueIsPlaying == false)
            {
                InteractButton.SetActive(true);
            }
            visualCue.SetActive(true);
            if (Interact)
            {
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                Interact = false;
                InteractButton.SetActive(false);
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
            InteractButton.SetActive(false);
        }
    }
    public void interact()
    {
        Interact = true;
    }
}

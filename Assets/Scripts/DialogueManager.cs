using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [Header("Param")]
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Load Globals JSON")]
    [SerializeField] private TextAsset loadGlobalsJSON;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField]private GameObject continueIcon;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Animator portraitAnimator;
    [SerializeField] private GameObject animationPanel;
    [SerializeField] private Animator animationPlay;
    
    private Animator layoutAnimator;
    private static DialogueManager instance;

    private Story currentStory;

    [HideInInspector]public bool dialogueIsPlaying = false;

    //private bool ContinueButton = false;
    private bool canContinueToNextLine = false;
    private Coroutine displayLineCoroutine;


    //public GameObject Joystick;
    //public GameObject InteractButton;
    private bool submitButton = false;
    [Header("Choose UI")]
    [SerializeField] private GameObject[] choices;

    private TextMeshProUGUI[] choicesText;

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";
    private const string MOVESCENE_TAG = "scene";
    private const string ANIMNAME_TAG = "animation";

    private DialogueVariables DialogueVariables;
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the Scene");
        }
        instance = this;
        //InteractButton.SetActive(false);
        DialogueVariables = new DialogueVariables(loadGlobalsJSON);
    }
    public static DialogueManager GetInstance()
    {
        return instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        //get all of the choicces text
        layoutAnimator = dialoguePanel.GetComponent<Animator>();
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach(GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }
    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        //Joystick.SetActive(false);
        //InteractButton.SetActive(false);

        DialogueVariables.StartListening(currentStory);

        //reset to default
        displayNameText.text = "???";
        portraitAnimator.Play("default");
        layoutAnimator.Play("left");
        ContinueStory();
    }
    private void ExitDialogueMode()
    {
        
        DialogueVariables.StopListening(currentStory);
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        //Joystick.SetActive(true);
        //InteractButton.SetActive(false);
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            submitButton = true;
        }
        if (!dialogueIsPlaying)
        {
            return;
        }
        if (canContinueToNextLine && submitButton && currentStory.currentChoices.Count == 0)
        {
            submitButton = false;
            ContinueStory();
        }
        
    }
    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            if(displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
            //display choices if any
            //DisplayChoices();
            //handle tags
            HandleTags(currentStory.currentTags);
        }
        else
        {
            ExitDialogueMode();
        }
    }
    private IEnumerator AnimationPlaying(string name)
    {
        canContinueToNextLine = false;
        animationPanel.SetActive(true);
        animationPlay.Play(name);
        yield return new WaitForSecondsRealtime(animationPlay.GetCurrentAnimatorStateInfo(0).length + animationPlay.GetCurrentAnimatorStateInfo(0).normalizedTime);
        canContinueToNextLine = true;
        animationPanel.SetActive(false);
    }
    private IEnumerator DisplayLine(string line)
    {

        //empty the line
        dialogueText.text = "";
        continueIcon.SetActive(false);
        HideChoices();
        canContinueToNextLine = false;
        //continueButton.SetActive(false);

        bool isAddingRichTextTag = false;

        //print letter by letter
        foreach(char letter in line.ToCharArray())
        {
            if (submitButton)
            {
                submitButton = false;
                dialogueText.text = line;                
                break;
            }
            if(letter == '<' || isAddingRichTextTag)
            {
                isAddingRichTextTag = true;
                dialogueText.text += letter;
                if(letter == '>')
                {
                    isAddingRichTextTag = false;
                }
            }
            else
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
            
        }

        continueIcon.SetActive(true);
        DisplayChoices();

        canContinueToNextLine = true;
    }
    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if(splitTag.Length!= 2)
            {
                Debug.LogError("Tag could not be appropriately parsed" + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    break;
                case PORTRAIT_TAG:
                    portraitAnimator.Play(tagValue);
                    break;
                case LAYOUT_TAG:
                    layoutAnimator.Play(tagValue);
                    break;
                case MOVESCENE_TAG:
                    
                    SceneManager.LoadScene(int.Parse(tagValue));
                    
                    break;
                case ANIMNAME_TAG:
                    StartCoroutine(AnimationPlaying(tagValue));
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }
    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        //defensive check
        if(currentChoices.Count > choices.Length)
        {
            Debug.LogError("Kebanyakan");
        }

        int index = 0;
        //untuk menampilkan choice
        foreach(Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        //untuk menyembunyikan UI choice yang tidak terpakai

        for(int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }
    }
    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            ContinueStory();
        }
        
    }
    
    public Ink.Runtime.Object GetVariableState(string variableName)
    {
        Ink.Runtime.Object variableValue = null;
        DialogueVariables.variables.TryGetValue(variableName, out variableValue);
        if(variableValue == null)
        {
            Debug.LogWarning("Ink Variable was found to be null:" + variableName);
        }
        return variableValue;
    }
    private void HideChoices()
    {
        foreach (GameObject choiceButton in choices)
        {
            choiceButton.SetActive(false);
        }
    }
}

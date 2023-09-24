using UnityEngine;
using UnityEngine.UI;
using TMPro;
class TutorialQuest : MonoBehaviour
{
    public Quest quest;
    public GameObject questWindow;
    public GameObject questUI;
    private GameObject[] objective;
    private int maxPopulation;
    public GameObject portalFinish;
    public string nameTag;
    public GameObject arrowTarget;
    [Header("Quest Window")]
    public TextMeshProUGUI titletext;
    public TextMeshProUGUI descriptiontext;
    public TextMeshProUGUI Goaltext;
    [Header("Quest UI Mini")]
    public TextMeshProUGUI titletextmini;
    public TextMeshProUGUI descriptiontextmini;
    public TextMeshProUGUI currProgresstextmini;
    
    private void Start()
    {
        openQuestWindow();
        maxPopulation = GameObject.FindGameObjectsWithTag(nameTag).Length;
        
        titletextmini.text = quest.title;
        descriptiontextmini.text = quest.description;
        currProgresstextmini.text = quest.currProgress.ToString() + "/" + quest.goal.ToString();
        
    }

    private void Update()
    {
        objective = GameObject.FindGameObjectsWithTag(nameTag);
        
        currProgresstextmini.text = Mathf.Abs(objective.Length - maxPopulation).ToString() + "/" + quest.goal.ToString()  ;
        if (Mathf.Abs(objective.Length - maxPopulation) == quest.goal)
        {
            questFinish();
        }
    }
    public void openQuestWindow()
    {
        questWindow.SetActive(true);
        quest.isActive = false;
        titletext.text = quest.title;
        descriptiontext.text = quest.description;
        Goaltext.text = quest.goal.ToString();
    }
    public void acceptQuest()
    {
        questWindow.SetActive(false);
        questUI.SetActive(true);
        quest.isActive = true;
    }
    public void questFinish()
    {
        titletextmini.color = Color.gray;
        descriptiontextmini.color = Color.gray;
        currProgresstextmini.color = Color.gray;
        portalFinish.SetActive(true);
        arrowTarget.SetActive(true);
    }
}

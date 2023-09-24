using UnityEngine;

class Tutorial : MonoBehaviour
{
    public GameObject[] WASDPure;
    public GameObject[] WASDPressed;
    public bool[] isPressed;
    private void Start()
    {
        for(int i = 0; i < 4; i++)
        {
            isPressed[i] = false;
        }
    }

    private void Update()
    {
        buttonCheck();
    }
    void buttonCheck()
    {
        if (Input.GetKeyDown("w"))
        {
            WASDPure[0].SetActive(false);
            WASDPressed[0].SetActive(true);
            isPressed[0] = true;

        }
        else if (Input.GetKeyUp("w"))
        {
            WASDPure[0].SetActive(true);
            WASDPressed[0].SetActive(false);
        }
        else if (Input.GetKeyDown("a"))
        {
            WASDPure[1].SetActive(false);
            WASDPressed[1].SetActive(true);
            isPressed[1] = true;

        }
        else if (Input.GetKeyUp("a"))
        {
            WASDPure[1].SetActive(true);
            WASDPressed[1].SetActive(false);
        }
        else if (Input.GetKeyDown("s"))
        {
            WASDPure[2].SetActive(false);
            WASDPressed[2].SetActive(true);
            isPressed[2] = true;

        }
        else if (Input.GetKeyUp("s"))
        {
            WASDPure[2].SetActive(true);
            WASDPressed[2].SetActive(false);
        }
        else if (Input.GetKeyDown("d"))
        {
            WASDPure[3].SetActive(false);
            WASDPressed[3].SetActive(true);
            isPressed[3] = true;

        }
        else if (Input.GetKeyUp("d"))
        {
            WASDPure[3].SetActive(true);
            WASDPressed[3].SetActive(false);
        }
        else if (Input.GetKeyDown("space"))
        {
            WASDPure[4].SetActive(false);
            WASDPressed[4].SetActive(true);
            isPressed[4] = true;
        }
        else if (Input.GetKeyUp("space"))
        {
            WASDPressed[4].SetActive(false);
            WASDPure[4].SetActive(true);
        }
    }
}

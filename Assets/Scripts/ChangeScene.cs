using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeScene : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update

    public void pindah(int number)
    {
        StartCoroutine(MoveScene(number));
    }
    public void retry()
    {
        StartCoroutine(MoveScene(SceneManager.GetActiveScene().buildIndex-1));
    }
    IEnumerator MoveScene(int number)
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(number);
    }
    
    
    public void StartTransition()
    {
        anim.SetTrigger("Change");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}

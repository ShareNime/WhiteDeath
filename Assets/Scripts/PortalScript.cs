using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PortalScript : MonoBehaviour
{
    Scene currScene;
    public Animator anim;
    public GameObject UI;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            currScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currScene.buildIndex + 1);
            UI.SetActive(false);
            anim.SetTrigger("Change");
        }
    }
}

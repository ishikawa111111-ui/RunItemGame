using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class TitleController : MonoBehaviour
{
    public string newScene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewScene() 
    {
        if (newScene != null)
        {
            StartCoroutine(NextScene());
        }
    }


    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(newScene);
    }
}

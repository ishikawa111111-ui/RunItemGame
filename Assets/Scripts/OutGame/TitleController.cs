using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;


public class TitleController : MonoBehaviour
{
    public string nextScene; //NewGameで移動するシーン名

    public string loadScene = "Intermission";   //装備画面
    public Button loadButton;   //Loadボタンが押せないこともあるようにする

    public UpgradeData[] allUpgrades;

    public void NewScene() 
    {
        GameData.ResetData();

        if (nextScene != null)
        {
            StartCoroutine(NextScene());
            //遅らせるためにこの書き方。即ならSceneManager.LoadScene(nextScene);でもOK
        }
    }

    public void LoadGame()
    {
        GameData.Load(allUpgrades);
        StartCoroutine(LoadScene());    //遅らせるためにこの書き方
    }



    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(nextScene);
    }
    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(loadScene);
    }
}

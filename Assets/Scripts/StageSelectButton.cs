using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectButton : MonoBehaviour
{

    public string nextScene;

    void Start()
    {
        //いったんボタンの文字を消す
        GetComponentInChildren<TextMeshProUGUI>().text = "";

        //ステージクリア済みなら★つける
        if (GameData.clearStages.Contains(nextScene))
        {
            GetComponentInChildren<TextMeshProUGUI>().text = "★";

        }
        GetComponentInChildren<TextMeshProUGUI>().text += nextScene;
    }

    public void Departure()
    {
        SceneManager.LoadScene(nextScene);
    }

}

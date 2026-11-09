using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //ゲームのステータス用のテキスト
    public GameObject statusText;

    //スライダー関連
    public GameObject sliderPanel;
    public Slider lifeSlider;

    //バレット関連
    public GameObject bulletPanel;
    public TextMeshProUGUI bulletText;

    //ボタン関連
    public GameObject buttonPanel;
    public GameObject retryButton;
    public GameObject nextButton;

    public string nextScene;

    void Start()
    {
        //ボタンを非表示にする
        buttonPanel.SetActive(false);
        //時間差でステータス文字を非表示にする
        StartCoroutine(DisplayChange());

        UpdateLife();   //プレイヤー体力を反映（開始時は満タンになる）
        UpdateBullet(); //残弾数を反映（）
    }

    IEnumerator DisplayChange()
    {
        yield return new WaitForSeconds(1.0f);
        statusText.SetActive(false);
    }

    public void UpdateLife()    //Lifeを更新
    {
        //PlayerRunすくkリプとを参照できるようにする
        PlayerRun player = GameObject.FindAnyObjectByType<PlayerRun>();
        lifeSlider.value = player.Life();   //現Lifeをスライダーに反映
    }

    public void UpdateBullet()  //Bulletを更新
    {
        PlayerShooter player = GameObject.FindAnyObjectByType<PlayerShooter>();
        bulletText.text = player.GetRemaining().ToString(); //残弾をテキストに代入
    }

    public void Retry()
    {
        //現Sceneの情報を取得
        string currentScene = SceneManager.GetActiveScene().name;
        //取得した現シーンを読み直す
        SceneManager.LoadScene(currentScene);
    }
    public void GameOverDisplay()
    {
        buttonPanel.SetActive(true);    //ボタンパネル表示させる
        statusText.SetActive(true);     //テキスト表示させる
        statusText.GetComponent<TextMeshProUGUI>().text = "GAME OVER";

        sliderPanel.SetActive(false);   //Life非表示
        bulletPanel.SetActive(false);   //弾数非表示
        nextButton.SetActive(false);   //NEXT非表示
    }

    public void Next()
    {
        ////取得した現シーンを読み直す
        SceneManager.LoadScene(nextScene);
    }
    public void GameClearDisplay()
    {
        buttonPanel.SetActive(true);    //ボタンパネル表示させる
        statusText.SetActive(true);     //テキスト表示させる
        statusText.GetComponent<TextMeshProUGUI>().text = "GAME CLEAR";

        sliderPanel.SetActive(false);   //Life非表示
        bulletPanel.SetActive(false);   //弾数非表示
        retryButton.SetActive(false);   //retry非表示
    }

}

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //ゲームのステータス用のテキスト
    public GameObject statusText;

    //ライフ・スライダー関連
    public GameObject sliderPanel;
    public Slider lifeSlider;
    public TextMeshProUGUI lifeText;

    //バレット関連
    public GameObject bulletPanel;
    public TextMeshProUGUI bulletText;

    //デバッグ関連・臨機応変にいろいろ使う
    public TextMeshProUGUI debugText;

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

        PlayerRun player = GameObject.FindAnyObjectByType<PlayerRun>();
        lifeSlider.maxValue = player.Life();   //現LifeをスライダーのMAXに反映

        UpdateLife();   //プレイヤー体力を反映（開始時は満タンになる）
        UpdateBullet(); //残弾数を反映（）


        UpdateDebug();  //debug表示
    }

    IEnumerator DisplayChange()
    {
        yield return new WaitForSeconds(1.0f);
        statusText.SetActive(false);
    }

    public void UpdateLife()    //Lifeを更新
    {
        //PlayerRunスクリプトを参照できるようにする
        PlayerRun player = GameObject.FindAnyObjectByType<PlayerRun>();
        lifeSlider.value = player.Life();   //現Lifeをスライダーに反映
        lifeText.text = player.Life().ToString(); //ライフをテキストに代入
    }

    public void UpdateBullet()  //Bulletを更新
    {
        PlayerShooter player = GameObject.FindAnyObjectByType<PlayerShooter>();
        bulletText.text = player.GetRemaining().ToString(); //残弾をテキストに代入
    }

    public void UpdateDebug()   //左上のデバッグ表示・臨機応変にいろいろ使う
    {
        /*
        GameObject player = GameObject.FindAnyObjectByType<GameObject>();
        //debugText.text = //

        Vector3 pos = player.transform.position;
        debugText.text = pos.ToString();
        */
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

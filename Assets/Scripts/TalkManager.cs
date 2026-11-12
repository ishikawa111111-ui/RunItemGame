using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TalkManager : MonoBehaviour
{
    //スクリプタブルオブジェクトを呼んでくる
    public TalkData currentTalkData;

    //UI側のパーツ
    public GameObject talkPanel; //会話枠
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI messageText;
    public Image faceImage;

    public float textSpeed = 0.04f;


    private int currentIndex = -1; //現在表示している会話の番号
    //マイナス１から始めると0番から始められて都合がいい

    private bool isTyping = false;


    private string currentFullText = "";

    private Coroutine typingCoroutine;


    public string nextScene;

    InputAction submitAction;

    void Start()
    {
        InputActionMap actionMap = GetComponent <PlayerInput>().currentActionMap;   //現マップ（UI)の情報を取得
        submitAction = actionMap.FindAction("Submit");  //UIマップで定義されているSubmitを参照

        //最初のメッセージ更新
        AdvanceTalk();
    }

    void Update()
    {
        if (submitAction.WasCompletedThisFrame())
        {
            //メッセージを更新
            AdvanceTalk();

        }
    }

    public void AdvanceTalk()
    {
        if(currentTalkData==null || currentTalkData.talks.Count == 0)
        {
            return;
        }

        if (isTyping)   //ボタンが押されたならば
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);    //メッセージ表示途中なら、コルーチンを停止
            messageText.text = currentFullText; //テキストを全表示する
            isTyping = false;   //フラグオフ
            return;
        }

        currentIndex ++; //番号を１追加

        //対象としているtalkdataの持っている数をIndexが上回ったら終了
        if (currentIndex >= currentTalkData.talks.Count)
        {
            EndTalk();
            return;
        }

        //トークパネルが実在していれば
        if(talkPanel!=null && talkPanel.activeSelf)
        {
            talkPanel.SetActive(true);  //トークUIを表示するよ
        }

        //対象データベースに記載されているリストのIndex番目を参照してくる
        //SingleTalkクラスには、名前、メッセージ、顔イメージ
        SingleTalk talk = currentTalkData.talks[currentIndex];

        if (nameText != null) nameText.text = talk.name; 
        //データベースから抜き出したSingleTalkクラスのnameをUIに代入＊空なら何も表示されない

        if(faceImage != null)
        {
            //データベースから抜き出したSingleTalkクラスの顔イメージがあれば表示、なければ表示しない
            faceImage.gameObject.SetActive(talk.faceImage != null);
            faceImage.sprite = talk.faceImage; //あれば差し替え nullならnull
        }

        currentFullText = talk.message; //抜き出したSingleTalkクラスのメッセージを変数に
        // messageText.text = currentFullText;//メッセージUIに反映_テキストを全部一気に表示させている方式

        typingCoroutine = StartCoroutine(TypeTextCoroutine(currentFullText));
    }

    IEnumerator TypeTextCoroutine(string text)
    {
        isTyping = true;    //表示途中
        messageText.text= ""; //まずテキストの内容を一旦リセット

        foreach(char c in text)
        {
            messageText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping=false; //表示完了
    }


    void EndTalk()
    {
        currentIndex = -1;  //再利用できるようにリセットする

        if (talkPanel != null)  //talkPanelが存在しているなら
        {
            talkPanel.SetActive(false); //お前は消す
        }

        if (nextScene != null)
        {
            StartCoroutine(NextScene());
        }
    }

    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(0.3f); //0.3秒後に次へ
        SceneManager.LoadScene(nextScene);
    }

}

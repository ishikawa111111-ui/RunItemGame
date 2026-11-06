using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public enum GameState
{
    None,
    IsPlay,
    GameCrear,
    GameOver,
    GameEnd,

}


public class GameManager : MonoBehaviour
{
    public static GameState gameState;  //自作した型を扱うStatic変数

    //private GameObject mixingCamera;

    public CinemachineCamera[] cameras;             //(2)のやり方

    public PlayableDirector playableDirector;   
    //(3)のやり方   インスペクタにタイムラインをアタッチするだけ＊メインカメラにBrainは必要
    //意図的に作っておいたTimelineを再生する

    void Start()
    {
        gameState = GameState.IsPlay;

        //mixingCamera = GameObject.FindGameObjectWithTag("MixingCamera");    //mixingカメラを探して
        //mixingCamera.SetActive(false);  //mixingカメラは非表示

        Camera.main.GetComponent<CinemachineBrain>().enabled = false;      //mainのBrainを無効化
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.GameOver)
        {
            //UIにゲームオーバー表示
            UIController controller = GameObject.FindAnyObjectByType<UIController>();   //UIControllerを探し出して
            controller.GameOverDisplay();   //これを実行

            //mixingCamera.SetActive(true);
            Camera.main.GetComponent<CinemachineBrain>().enabled = true;   //maincameraのBrainを復活 (2)(3)どちらのやり方でも必要
            StartCoroutine(GameOverCamera());

            //(2)のやり方
            //cameras[1].Priority = 2;    
            //[1]=CinemachineCamera2、元Pliorityは0を2にして、CinemachineCamera1のPriority0より高くしている
            //ここに直接書くやり方では即camera2に移行してしまい、camera1が飛ばされたように見える


            gameState = GameState.GameEnd;  //ループでGameOverが発動しないようにGameEndに変更している
        }
    }
    IEnumerator GameOverCamera()
    {
        //(2)のやり方
        yield return null;  //1フレーム待ってから
        cameras[1].Priority = 2;
    }
}

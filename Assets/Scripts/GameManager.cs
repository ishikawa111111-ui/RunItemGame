using Unity.Cinemachine;
using UnityEngine;

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

    private GameObject mixingCamera;


    void Start()
    {
        gameState = GameState.IsPlay;

        mixingCamera = GameObject.FindGameObjectWithTag("MixingCamera");    //mixingカメラを探して
        mixingCamera.SetActive(false);  //mixingカメラは非表示
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

            mixingCamera.SetActive(true);
            Camera.main.GetComponent<CinemachineBrain>().enabled = true;   //mainのBrainを無効化

            gameState = GameState.GameEnd;
        }
    }
}

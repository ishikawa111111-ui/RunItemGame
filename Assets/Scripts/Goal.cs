using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.gameState = GameState.GameClear;    //元がstatic変数なので書き換えられる
            //↓staticでない場合の書き方
            //GameManager gm=GameObject.FindAnyObjectByType<GameManager>();
            //gm= GameState.GameCrear;
        }
    }
}

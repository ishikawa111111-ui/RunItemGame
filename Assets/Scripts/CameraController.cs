using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    Vector3 diff;   //対象とカメラの維持するべき距離

    //public GameObject target;   //カメラの対象＝Player
    GameObject player;
    [Header("カメラ追従スピード")]
    public float followSpeed = 5.0f; //カメラの追従スピード

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        //最初に維持するべき距離を定義（Player位置-カメラ位置）
        //diff = target.transform.position - transform.position;
        diff = player.transform.position - transform.position;
    }

    void LateUpdate()   //LateUpdateはUpdate、FixedUpdateの後に実行される＝プレイヤーが動いた後にカメラが動く
    {
        //Lerpらーぷ　現在地から、目的地へ移動する
        //プレイヤーがどこにいても、一定の位置をキープする
        transform.position = Vector3.Lerp(
            transform.position, //現地点A
            player.transform.position - diff, //目的地B 
            //target.transform.position - diff, //目的地B 
            Time.deltaTime * followSpeed); //A→Bへの進行に対して、どれだけ進んだ位置にいるべきかの進捗率
            //プレイヤーと離れるほど素早く移動、近いとゆっくり（現在地A→目的地Bに向けて就職していく）
            //最終的には限りなく近づき続けるだけでイコールにはならない
        //現在はChinemaSeaneのコンポーネントでカメラコントローラするのが基本
    }
}

using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int enemyLife = 3;
    public GameObject enemyBody;   //見た目オブジェクトの情報

    //無敵時間の秒数
    public float freezeTime = 1.0f;

    //ダメージ中かどうか　ダメージ中のコルーチンを参照する変数
    Coroutine damageStart;


    void Update()
    {
        //もしダメージコルーチン変数がnullでなければ（＝ダメージ中ならば）、点滅処理
        if (damageStart != null)
        {
            //valが一定周期で正か負か、値を決める。三角関数と時間
            float val = Mathf.Sin(Time.time * 50);  //三角関数(Sin)を使って一定周期(リズム)で切り替える

            if (val > 0)   // valが正の値なら
            {
                //見た目のMeshRendererをON
                GetComponent<MeshRenderer>().enabled = true;
            }
            else            // valが正の値ではない(=だいたい負)
            {
                //見た目のMeshRendererをON
                GetComponent<MeshRenderer>().enabled = false;
            }
            return; // 現フレームのUpdateを中断する（Updateは次フレームでまた動くけどね）
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(damageStart==null)   //ダメージ中（点滅中）でなければ
        {
            if (other.gameObject.tag == "Bullet")
            {
                enemyLife--;    //体力が１減る
                if (enemyLife <= 0)
                {
                    Destroy(gameObject);
                }
                else //体力が残っていれば
                {
                    damageStart = StartCoroutine(DamageStart());
                }
            }

        }
    }

    IEnumerator DamageStart()
    {
        //freezTime秒待ってから、ダメージコルーチン変数を解放
        yield return new WaitForSeconds(freezeTime); //
        damageStart = null; //コルーチン発動中である証明だった変数を解放

        GetComponent<MeshRenderer>().enabled = true;    //点滅中に消えて終わらないように見た目を戻す
    }
}

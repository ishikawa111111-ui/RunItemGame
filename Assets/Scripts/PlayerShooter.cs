using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [Header("弾")]
    public GameObject bullet;

    InputAction shootAction;    //Attackボタンの割り振り

    Coroutine shootStart;   //シュートされているかどうかを知るためのコルーチン型の変数

    [Header("弾速・インターバル時間")]
    public float bulletSpeed = 10.0f;
    public float shootInterval = 0.5f;

    const int startRemaining = 10;  //初期弾数
    private int bulletRemaining = startRemaining;   //現在の弾数

    //UIなどに弾数を伝えるアクセサメソッド
    public int GetRemaining()
    {
        return bulletRemaining;
    }

    //弾数を回復するメソッド
    public void AddBullet(int num)  //引数にnum、回復する量が入る
    {
        bulletRemaining += num;
    }

    /*
    public PlayerShooter(int bulletRemaining)
    {
        this.bulletRemaining = bulletRemaining;
    }
    */


    void Start()
    {
        //PlayerInputコンポーネントの現Mapを取得
        InputActionMap actionMap = GetComponent<PlayerInput>().currentActionMap;

        //現Mapの「Attack」アクションを取得
        shootAction = actionMap.FindAction("Attack");

        //this.bulletRemaining = startRemaining;

    }

    void Update()
    {
        //アタックが押されたメソッド
        //if (shootAction.IsPressed())    //新しいInputシステム書き方(IsPressed 押しっぱなし)
        if (shootAction.WasPerformedThisFrame() && bulletRemaining > 0)    //新しいInputシステム書き方（Was～１回押す）
        {
                Shoot();
        }

    }
    void Shoot()
    {
        if (shootStart == null) //インターバル中でなければ
        {
            bulletRemaining--;  //弾1減らす


            UIController controller = GameObject.FindAnyObjectByType<UIController>();
            controller.UpdateBullet();


            Vector3 pos = transform.position + new Vector3(0.0f, 1.0f, 0.5f);  //勝手に追加。弾の発射位置を調整


            //攻撃開始Bulletプレハブ生成とZ前方へシュート
            //インターバルとなるコルーチンの開始
            //GameObject _bullet = (GameObject)Instantiate(
            //    bullet, //プレハブ
            //    transform.position, //生成位置
            //    Quaternion.identity);   //回転させない
            GameObject _bullet = (GameObject)Instantiate(
                bullet, //プレハブ
                pos, //生成位置
                Quaternion.identity);   //回転させない

            Rigidbody rbody =_bullet.GetComponent<Rigidbody>();  //生成したもののRigidbody
            Debug.Log(bulletSpeed);
            rbody.AddForce(new Vector3(0,0,bulletSpeed), ForceMode.Impulse);  //前シュート！
            //打ち出すときにみにチェックしているので、Time.deltatimeは点けなくてもPCによる性能差は出ない


            /*  間違えた！！！
            Vector3 moveDirection = Vector3.zero;
            moveDirection.x = -5;
            */

            shootStart = StartCoroutine(ShootStart());  //コルーチンスタート
        }
        Debug.Log(bulletRemaining);

    }

    //インターバル用
    IEnumerator ShootStart()  //特定の時間（インターバル）だけ入力を待つコルーチン
    {
        //連射できないように0.5秒待つ
        yield return new WaitForSeconds(shootInterval); //いーるどりたーん＝0.5秒待ってから、
        //コルーチン変数の解放
        shootStart = null; //コルーチン発動中である証明だった変数を解放
    }
}

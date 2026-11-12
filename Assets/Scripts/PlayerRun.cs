using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRun : MonoBehaviour
{
    const int MinLane = -2;       //最小レーン番号
    const int MaxLane =  2;       //最大レーン番号
    const float LaneWidth = 2.0f; //レーン幅

    const int DefaultLife = 3;  //最大体力
    const float StunDuration = 0.5f; //硬直時間

    CharacterController controller; //CjharacterControllerを参照できる変数
    [SerializeField] private GameObject animaterBody;//アニメーターを持っているだろうGameObjectの取得に改造(追加)
    Animator animator;  //Anbimatorコンポーネントの取得

    //Vector3 moveDirection = Vector3.zero;    //Moveメソッドに与える方向。zero=Vector3(0f,0f,0f)と同じ
    // newで作成（x, y, zはfloat型）
    Vector3 moveDirection = new Vector3(0f, 0f, 0f);


    private int targetLane; //移動先レーン

    private int maxLife = DefaultLife;
    private int life = DefaultLife; //現体力   →アクセサ、ライフメソッドを作成
    private float recoverTime = 0.0f;   //硬直カウントダウン


    [Header("重力・前進・横移動・ジャンプ力")]
    public float gravity;
    public float speedZ;    //z(前＝z奥方向)への移動
    public float speedX;    //横移動スピード
    public float speedJump; //ジャンプ力

    [Header("前進加速力")]
    public float accelerationZ = 10.0f; //加速
    Coroutine resetIntervalCol; //インプットのインターバルを置くための変数

    //public float speedRotate = 3.0f;   //勝手に追加、回転速度


    private InputAction moveAction; //Moveアクションを参照するための変数
    private InputAction jumpAction; //Jumpアクションを参照するための変数

    public float knockBackForceZ = -6f;

    //[Header("プレイヤーの見た目")]→animaterBodyで取得しているものを使いまわせるのでいらなかった
    //public GameObject playerBody;


    //体力の残数を参照メソッド
    public int Life()
    {
        return this.life;
    }

    //体力最大を参照メソッド 使わないかも
    public int MaxLife()
    {
        return this.maxLife;
    }

    public void RecoverLife(int val)    //アクセサ
    {
        life += val;
        if (life > maxLife)
        {
            life = maxLife;
        }

    }

    //硬直中かどうかを判断するメソッド
    bool IsStun()   //bool型なので、何か値を返す。publicが付いていないので、このスクリプトから呼ばれることがわかる
    {
        return recoverTime > 0.0f || life <= 0;//リカバータイムが残っている（硬直継続中）
    }

    void Start()
    {
        //装備リストにLifeUpがあれば＋2（それ以外は0）
        maxLife = DefaultLife + GameData.GetValue(UpgradeType.LifeUp);
        life = maxLife; //ライフを最大値で初期化

        //Playerに付いているコンポーネントの取得
        controller = GetComponent<CharacterController>();

        //animator = GetComponent<Animator>(); 下記に変更
        animator = animaterBody.GetComponent<Animator>();

        //PlayerInput コンポーネントに設定されている現Mapは何かを取得＝（Playerマップ）
        InputActionMap actionmap = GetComponent<PlayerInput>().currentActionMap;
        moveAction = actionmap.FindAction("Move"); //現Map上にいるMoveというアクションを取得
        jumpAction = actionmap.FindAction("Jump"); //現Map上にいるJumpというアクションを取得

    }

    void Update()
    {
        if (GameManager.gameState == GameState.GameEnd) return;

        //勝手にデバッグ用に機能を追加してみた①
        if (moveAction.ReadValue<Vector2>().y > 0 && Time.timeScale != 10.0f)
        {
            Time.timeScale = 5.0f; //上が押されたら、高速化
        }

        if (moveAction.ReadValue<Vector2>().y < 0 && Time.timeScale != 0.0f) //追加！
        {
            //Time.timeScale = 0.0f; //下が押されたら、一時停止（ポーズ）
            Time.timeScale = 0.3f; //下が押されたら、スロー
        }

        if (moveAction.ReadValue<Vector2>().x != 0) //移動しようとしたら（移動できなくても）
            //if (moveAction.ReadValue<Vector2>().x != 0 || jumpAction.WasPressedThisFrame()) //移動しようとしたら（移動できなくても）
        {
                Time.timeScale = 1.0f;  //通常速度に戻す
        }
        //勝手にデバッグ機能を追加してみた②
        //UIController controller = GameObject.FindAnyObjectByType<UIController>();
        //controller.UpdateDebug();
        //Vector3 pos = transform.position;
        //Debug.Log($"pos:" + pos);

        //↑ここまで


        //点滅処理
        if (IsStun())
        {                        
            //valが一定周期で正か負か、値を決める。三角関数と時間
            float val = Mathf.Sin(Time.time * 50);  //三角関数(Sin)を使って一定周期(リズム)で切り替える

            if (val > 0)   // valが正の値なら
            {
                //見た目のMeshRendererをON
                //animaterBody.GetComponent<MeshRenderer>().enabled = true;
                animaterBody.SetActive(true);
            }
            else            // valが正の値ではない(=だいたい負)
            {
                //見た目のMeshRendererをON
                animaterBody.SetActive(false);
            }
        }
        else //ダメージ中ではない時、姿を現す
        {
            animaterBody.SetActive(true);
        }


        //CharacterControllerがすでに接地判断isGroundedを持っている。接地＝true

        if (controller.isGrounded)  //接地しているならば、
        {
            //if (Input.GetAxis("Vertical") > 0.0f)   //上方向にアナログ0以上のキー入力があれば、
            //if (Input.GetAxis("Vertical") != 0.0f)   //入力が0以外(上か下)ならば、
            //if(moveAction.ReadValue<Vector2>().y != 0.0f)   //上と同じ。新しいInputシステムの書き方
            //{
            //        // moveDirection.z = Input.GetAxis("Vertical") * speedz;   //前後移動・古い書き方
            //        moveDirection.z = moveAction.ReadValue<Vector2>().y * speedZ; //新しいInputシステムの書き方
            //}
            //else //上以外の入力ならば、
            //{
            //    moveDirection.z = 0;    //z方向（前後）には動かない
            //}

            //横回転は、Rotateメソッドで直接　Y軸回転させる。（ポールのイメージ）
            //transform.Rotate(0, Input.GetAxis("Horizontal") * 300 * Time.deltaTime * speedRotate, 0);
            //transform.Rotate(0, moveAction.ReadValue<Vector2>().x * 300 * Time.deltaTime * speedRotate, 0);
            //transform.Rotate(0, moveAction.ReadValue<Vector2>().x * 300 * Time.deltaTime * 3.0f, 0);

            //目的のレーン番号に向かって動く　これｈUPdateで書く
            //右レーン移動、左レーン移動はそれぞれメソッドを作る
            if (resetIntervalCol == null)   //コルーチンの待ち時間が発動中でなければ、以下の入力処理
            {
                if (moveAction.ReadValue<Vector2>().x < 0) MoveToLeft();   //左のレーン番号に変更
                else if (moveAction.ReadValue<Vector2>().x > 0) MoveToRight();    //左のレーン番号に変更
            }


            //ジャンプ押されている間
            //if (Input.GetButton("Jump"))  //古い書き方
            //if (jumpAction.IsPressed())    //新しいInputシステム書き方(連続ジャンプ）
            if (jumpAction.WasPressedThisFrame())    //新しいInputシステム書き方（１回ジャンプ）
            {
                    moveDirection.y = speedJump;    //y方向（向き）をセッティング

                // animator.SetTrigger("Jump"); //ジャンプアニメ（今はないので一次的にコメント）
            }
            if(IsStun())　//
            {
                moveDirection.x = 0f;
                moveDirection.z = Mathf.Min(moveDirection.z + 0.05f, 0f);
                recoverTime -= Time.deltaTime;
            }
            else //スタン中以外
            {
                //対象とするレーンをもとにmoveDirectionX（横移動）を決める
                float ratioX = (targetLane * LaneWidth - transform.position.x) / LaneWidth;
                //対象のレーンが遠いほど移動が速く、近いと遅くなる（減速・ゆるやかに見える）
                moveDirection.x = ratioX * speedX;

                //自動前進
                float acceleratedZ = moveDirection.z + (accelerationZ * Time.deltaTime);
                //前フレームまでのmoveZの力に加速accelaを（毎フレーム）足している

                moveDirection.z = Mathf.Clamp(accelerationZ, 0, speedZ);
                //Clampは、加減上限が決まっている ↑下限は第二引数の0、上限は第三引数のspeedZ（=3.0f）
            }
        }


            //常に重力がかかっている
            moveDirection.y -= gravity;
            //moveDirection.y -= gravity * Time.deltaTime;  //二重にdeltaTimeかかっているで消した


            //TransformaDirection（ワールド座標の向きを、グローバルな向きの座標に変換）
            Vector3 globalDirection = transform.TransformDirection(moveDirection);

            //セッティングしたmoveDirectionの値を使ってMoveメソッドの発動
            //＊実際にPlayerが動く　Time.deltaTimeでPC性能差によらないように動作安定
            //controller.Move(moveDirection * Time.deltaTime);

            controller.Move(globalDirection * Time.deltaTime);

            //地面にいるのであれば、Y成分の力を毎回 0にリセット（重力がかかり続けないように）
            if (controller.isGrounded) moveDirection.y = 0;

            //アニメの制御
            // animator.SetBool("run", moveDirection.z > 0.0f);
            // 今はアニメがないのでコメントアウト

        }
   
    public void MoveToLeft()
    {
        if (IsStun()) return;
        if (targetLane > MinLane) //現レーンが最小でなければ、
        {
            targetLane--;//対象レーン番号をひとつ減らす

            //次の入力認識するまえのインタ＾-バルを保つコルーチンの発動
            resetIntervalCol = StartCoroutine(ResetIntervalCol());   
            //StartCoroutine=スタートコルーチン何かの処理を別で開始させる=
        }
    }
    public void MoveToRight()
    {
        if (IsStun()) return;
        if (targetLane < MaxLane) //現レーンが最大でなければ、
        {
            targetLane++;//対象レーン番号をひとつ増やす
        }
    }

    public void Jump()
    {
        if (IsStun()) return;   //スタン中なら何もしない（できない）
        if (controller.isGrounded)
        {
            moveDirection.y = speedJump;
            animator.SetTrigger("Jump");
        }
    }


    IEnumerator ResetIntervalCol()  //特定の時間（インターバル）だけ入力を待つコルーチン
    {
        yield return new WaitForSeconds(0.1f); //0.1秒待ってから、
        resetIntervalCol = null; //コルーチン発動中である証明だった変数を解放
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (IsStun()) return;

        if (hit.gameObject.tag == "Enemy")
        {
            moveDirection.z = knockBackForceZ;  //ノックバック
            LifeDown(); //体力減少
            recoverTime = StunDuration; //硬直時間をrecoverTimeにセット

        } 
    }

    public void LifeDown()
    {
        life--;
        UIController controller = GameObject.FindAnyObjectByType<UIController>();
        controller.UpdateLife();

        if (life <= 0)  //ライフなくなったら
        {
            //ステータス変更
            GameManager.gameState = GameState.GameOver;
        }
    }
}

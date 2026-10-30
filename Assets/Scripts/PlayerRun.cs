using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRun : MonoBehaviour
{
    const int MinLane = -2;       //最小レーン番号
    const int MaxLane =  2;       //最大レーン番号
    const float LaneWidth = 2.0f; //レーン幅


    CharacterController controller; //CjharacterControllerを参照できる変数
    [SerializeField] private GameObject animaterBody;//アニメーターを持っているだろうGameObjectの取得に改造(追加)
    Animator animator;  //Anbimatorコンポーネントの取得

    Vector3 moveDirection= Vector3.zero;    //Moveメソッドに与える方向。zero=Vector3(0,0,0)と同じ
    int targetLane; //移動先レーン

    [Header("重力・前進・横移動・ジャンプ力")]
    public float gravity;
    public float speedZ;    //z(前＝z奥方向)への移動
    public float speedX;    //横移動スピード
    public float speedJump; //ジャンプ力

    [Header("前進加速力")]
    public float accelerationZ = 10.0f;//加速
    Coroutine resetIntervalCol; //インプットのインターバルを置くための変数

    //public float speedRotate = 3.0f;   //勝手に追加、回転速度


    private InputAction moveAction; //Moveアクションを参照するための変数
    private InputAction jumpAction; //Jumpアクションを参照するための変数

    void Start()
    {
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

            //対象とするレーンをもとにmoveDirectionX（横移動）を決める
            float ratioX = (targetLane * LaneWidth - transform.position.x) / LaneWidth;
            //対象のレーンが遠いほど移動が速く、近いと遅くなる（減速・ゆるやかに見える）
            moveDirection.x = ratioX * speedX;

            //自動前進
            float acceleratedZ = moveDirection.z + (accelerationZ * Time.deltaTime);    
            //前フレームまでのmoveZの力に加速accelaを（毎フレーム）足している

            moveDirection.z = Mathf.Clamp(accelerationZ, 0, speedZ);
            //Clampは、加減上限が決まっている ↑下限は第二引数の0、上限は第三引数のspeedZ（=3.0f）

            //ジャンプ押されている間
            //if (Input.GetButton("Jump"))  //古い書き方
            if (jumpAction.IsPressed())    //新しいInputシステム書き方
            {
                moveDirection.y = speedJump;    //y方向（向き）をセッティング

                // animator.SetTrigger("Jump"); //ジャンプアニメ（今はないので一次的にコメント）
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
        if (targetLane < MaxLane) //現レーンが最大でなければ、
        {
            targetLane++;//対象レーン番号をひとつ増やす
        }
    }

    IEnumerator ResetIntervalCol()  //特定の時間（インターバル）だけ入力を待つコルーチン
    {
        yield return new WaitForSeconds(0.1f); //0.1秒待ってから、
        resetIntervalCol = null; //コルーチン発動中である証明だった変数を解放
    }
}

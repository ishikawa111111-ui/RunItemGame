using UnityEngine;

public class PlayerFreeMove : MonoBehaviour
{
    CharacterController controller; //CjharacterControllerを参照できる変数


    [SerializeField] private GameObject animaterBody;
    //アニメーターを持っているだろうGameObjectの取得に改造(追加)

    Animator animator;  //Anbimatorコンポーネントの取得

    Vector3 moveDirection= Vector3.zero;    //Moveメソッドに与える方向。zero=Vector3(0,0,0)と同じ

   public float gravity;
   public float speedz;    //z(前＝z奥方向)への移動
   public float speedJump; //ジャンプ力

   public float speedRotate = 3.0f;   //勝手に追加、回転速度

    void Start()
    {
        //Playerに付いているコンポーネントの取得
        controller = GetComponent<CharacterController>();

        //animator = GetComponent<Animator>(); 下記に変更
        animator = animaterBody.GetComponent<Animator>();

    }

    void Update()
    {
        //CharacterControllerがすでに接地判断isGroundedを持っている。接地＝true

        if (controller.isGrounded)  //接地しているならば、
        {
            //if (Input.GetAxis("Vertical") > 0.0f)   //上方向にアナログ0以上のキー入力があれば、
            if (Input.GetAxis("Vertical") != 0.0f)   //入力が0以外(上か下)ならば、
            {
                    moveDirection.z = Input.GetAxis("Vertical") * speedz;   //前後移動
            }
            else //上以外の入力ならば、
            {
                moveDirection.z = 0;    //z方向（前後）には動かない
            }

            //横回転は、Rotateメソッドで直接　Y軸回転させる。（ポールのイメージ）
            transform.Rotate(0, Input.GetAxis("Horizontal") * speedRotate, 0);

            //ジャンプ押されている間
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = speedJump;    //y方向（向き）をセッティング
                
                // animator.SetTrigger("Jump"); //ジャンプアニメ（今はないので一次的にコメント）
            }


            //常に重力がかかっている
            moveDirection.y -= gravity * Time.deltaTime;


            //TransformaDirection（ワールド座標の向きを、グローバルな向きの座標に変換）
            Vector3 globalDirection = transform.TransformDirection(moveDirection);

            //セッティングしたmoveDirectionの値を使ってMoveメソッドの発動
            //＊実際にPlayerが動く　Time.deltaTimeでPC性能差によらないように動作安定
            controller.Move(moveDirection * Time.deltaTime);

            controller.Move(globalDirection * Time.deltaTime);

            //地面にいるのであれば、Y成分の力を毎回 0にリセット（重力がかかり続けないように）
            if (controller.isGrounded) moveDirection.y = 0;

            //アニメの制御
            // animator.SetBool("run", moveDirection.z > 0.0f);
            // 今はアニメがないのでコメントアウト

        }
    }
}

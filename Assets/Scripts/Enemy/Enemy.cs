using System.Collections;
using TMPro;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    private int enemyLife = 5;
    public GameObject enemyBody;   //見た目オブジェクトの情報

    //無敵時間の秒数
    public float freezeTime = 0.1f;

    //ダメージ中かどうか　ダメージ中のコルーチンを参照する変数
    Coroutine damageStart;

    public GameObject effectObject;


    [SerializeField] private TextMeshProUGUI enemyLpTxt; //敵の体力表示テキスト
    //public TextMeshProUGUI numText;
    //public int value;


    private Material _originalColorMat; // 元の色用のマテリアル
    private Renderer _rend;


    [Header("Flash")]
    public Renderer[] renderers;
    public Color flashColor = Color.yellow;
    public float flashDuration = 0.2f;
    private Color[] originalColors;


    void Start()
    {
        //numText.text = value.ToString();
        //enemyLpTxt.text = enemyLife.ToString();

        //_rend = GetComponent<Renderer>();
        //_rend.material.color = Color.yellow;
        //GetComponent<Renderer>().material.color = Color.red;

        // 元の色を保存
        originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].material.color;
        }
    }

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
                enemyBody.GetComponent<MeshRenderer>().enabled = true;

                //_rend.material.color = _originalColorMat.color; //元の色

            }
            else            // valが正の値ではない(=だいたい負)
            {
                //見た目のMeshRendererをOFF
                enemyBody.GetComponent<MeshRenderer>().enabled = false;

                //_rend.material.color = Color.yellow;

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

                Flash();

                if (enemyLife <= 0)
                {
                    Destroy(gameObject);
                    CreateEffect();
                }
                else //体力が残っていれば
                {
                    damageStart = StartCoroutine(DamageStart());
                }
            }
            if (other.gameObject.tag == "Slash")    //Slashは即撃破
            {
                Destroy(gameObject);
                CreateEffect() ;
            }

            UpdateEnemyLife();  //頭上の表示を最新に変える
        }
    }

    IEnumerator DamageStart()
    {
        //freezTime秒待ってから、ダメージコルーチン変数を解放
        yield return new WaitForSeconds(freezeTime); //
        damageStart = null; //コルーチン発動中である証明だった変数を解放

        enemyBody.GetComponent<MeshRenderer>().enabled = true;    //点滅中に消えて終わらないように見た目を戻す
        //_rend.material.color = _originalColorMat.color; //元の色
    }

    void CreateEffect() //敵死亡時のやつ
    {
        Instantiate(
            effectObject,
            transform.position,     //この場所
            Quaternion.identity);   //回転させない

    }

    private void UpdateEnemyLife()    //Lifeを更新
    {
        //lifeSlider.value = player.Life();   //現Lifeをスライダーに反映
        //enemyLpTxt.text = enemyLife.ToString(); //ライフをテキストに代入

        // 値が変わったら更新
        //numText.text = value.ToString();
    }

    System.Collections.IEnumerator Flash()
    {
        foreach (var r in renderers)
            r.material.color = flashColor;

        yield return new WaitForSeconds(flashDuration);

        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.color = originalColors[i];
    }
}

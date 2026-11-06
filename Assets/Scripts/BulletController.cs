using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("弾の消滅時間")]
    public float deleteTime = 3.0f;

    private void OnTriggerEnter(Collider other) //other何か　にぶつかったら
    {
        Destroy(gameObject);
    }

    void Start()
    {
        Destroy(gameObject, deleteTime);
    }

    /* 以下は無駄。Destroy(gameObject, deleteTime);←同じ処理はこれだけでよかった…
    void Update()
    {
        if (deleteTime > 0)//削除時間が残っているなら、
        {
            deleteTime -= Time.deltaTime;  //時間を減らして
            return; //処理を抜ける
        }
        else
        {
            Destroy(gameObject);
        }
        ;
    }
    */
}

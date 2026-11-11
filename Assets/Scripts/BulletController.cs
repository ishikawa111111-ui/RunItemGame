using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("弾の消滅時間")]
    public float deleteTime = 3.0f;

    private void OnTriggerEnter(Collider other) //other何か　にぶつかったら
    {
        if (GameData.HasUpgrade(UpgradeType.PierceBullet) && other.gameObject.tag == "Enemy")   //貫通弾装備していたら
            { 
                return; //リターンで終了
            }
        Destroy(gameObject);    //貫通弾を持っていないと、この弾は消える
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

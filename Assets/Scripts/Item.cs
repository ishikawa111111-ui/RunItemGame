using UnityEngine;

public enum ItemType
{
    Life,
    Bullet,
    Upgrade
}

public class Item : MonoBehaviour
{

    //変数　自分自身のタイプitemType・回復量(int)、補充量(int)valueを作成
    public ItemType itemType;  //自作した型を扱うStatic変数

    public int value = 1;


    //Life回復はPlayerRun
    //弾の補充はPlayerShooter
    //スフィアの色で見分けられるように

    //コライダーを取得→OnTriggerでいい
    //Playerタグとぶつかったら
    //ItemTypeをチェックして、それぞれの回復へ
    //Itemタグはどこで使う？　＞弾との接触かな


    public UpgradeData upgradeData; //生成したデータの情報を参照できる

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            switch (itemType)
            {
            case ItemType.Life:
                    other.gameObject.GetComponent<PlayerRun>().RecoverLife(value);
                    FindAnyObjectByType<UIController>().UpdateLife(); //UI更新 ←一応残しておくけど、UI側で更新した方がいいよね
                    break;

            case ItemType.Bullet:
                    other.gameObject.GetComponent<PlayerShooter>().AddBullet(value);
                    FindAnyObjectByType<UIController>().UpdateBullet(); //UI更新
                    break;
            case ItemType.Upgrade:
                    //管理リストに入手した情報を加える
                    GameData.AddUpgrade(upgradeData);
                    break;
            }
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

//拡張アイテムの種類
public enum UpgradeType
{
    LifeUp,         //最大ライフUP
    PierceBullet,   //貫通弾
    BulletUp        //初期段数UP
}

//拡張アイテム1つ文のデータ(TalkDataと同じScriptableObject)
[CreateAssetMenu(fileName ="NewUpgradeData", menuName ="ScriptableObject/UpgradeData",order =2)]

public class UpgradeData : ScriptableObject
{
    public string upgradeName;  //表示名（アイコン名）

    [TextArea(2, 5)]
    public string description;  //説明文
                               
    public UpgradeType upgradeType; //種類
    public int value;   //効果の大きさ（Life回復量など）
}

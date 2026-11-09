using UnityEngine;

public class TrapGenerator : MonoBehaviour
{
    //const int MinLane = -2; //最小レーン番号
    //const int MaxLane = 2; //最大レーン番号
    //const float LaneWidth = 2.0f; //レーン幅
    const float StageChipSize = 30.0f; //ステージの奥行(Z)のサイズ

    public GameObject[] trapPrefabs; //生成するトラップの種類（ここを入れ替える）
    public float minInterval = 5.0f; //トラップを置く間隔(Z)の最小値
    public float maxInterval = 15.0f; //トラップを置く間隔(Z)の最大値
    public float offsetZ = 5.0f; //最初のトラップをどこから置くかのずらし

    public float posX = 0.0f; //トラップを置く高さ
    public float posY = 0.5f; //トラップを置く高さ
    //↑不要？一応つけておく
    public float spawnRate = 0.3f; //トラップが出る確率（0〜1）

    void Start()
    {
        //ステージの手前の端(-15)から、奥の端(15)まで、ランダムな間隔でトラップを置く
        //1つ置くたびに、minInterval〜maxIntervalの間でランダムに進める
        for (float z = -StageChipSize / 2 + offsetZ; z < StageChipSize / 2; z += Random.Range(minInterval, maxInterval))
        {
            //確率に外れたら、今回は置かずに次へ
            if (Random.value > spawnRate) continue;

            //配列からランダムにトラップの種類を選ぶ（今は１種類だけど）
            int index = Random.Range(0, trapPrefabs.Length);

            //ステージの位置を基準に、生成する座標を決める
            //Vector3 pos = transform.position + new Vector3(lane * LaneWidth, posY, z);
            Vector3 pos = transform.position + new Vector3(posX, posY, z);

            //Debug.Log(pos);

            //ステージの子として生成（ステージが消えると一緒に消える）
            Instantiate(trapPrefabs[index], pos, Quaternion.identity, transform);
            //Instantiate(trapPrefabs, pos, Quaternion.identity, transform);    //１種類なのでこれでも同じ
        }
    }
}
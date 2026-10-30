using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    const int StageChipSize = 30;   //ステージの奥行(Z)のサイズ

    int currentChipIndex;   //生成済みのフロア番号（現在どこまで作成したか）

    Transform character; //対象となるPlayerのTransform（位置を確認）publicでは、なくした。その代わりStartで探させている
    //public Transform character; //対象となるPlayerのTransform（位置を確認）

    public GameObject[] stageChips; //ここは配列。生成されるフロアの種類

    public int startChipIndex = 1;  //最初のフロア番号。初期は1
    
    public int preInstantiate = 5;  //予備で作っておく数（画面奥に見える範囲）とりあえず５個先まで

    public List<GameObject> generatedStageList = new List<GameObject>();    //配列ではなくListなので何個でも増やせる
    //生成されたフロア情報が順番に記帳される管理リスト（通り過ぎると抹消される）
    //Listは中身が消えると詰まっていく。便利

    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player").transform;   //Playerを検索し、Transformを取得
        currentChipIndex = startChipIndex - 1;  //現在基準のフロアを-1にする
        UpdateStage(preInstantiate);    //まず最初に作成するフロアの数
    }


    void Update()
    {
        //プレイヤーのZをフロアサイズ30で割ると、何番目のフロアにいるかわかる
        int charaPositionIndex = (int)(character.position.z / StageChipSize);

        //Playerの現在フロア番号＋予備フロア番号が、生成したフロア番号を上回るならば、
        if (charaPositionIndex + preInstantiate > currentChipIndex)
        {
            UpdateStage(charaPositionIndex + preInstantiate);   //新しくフロアを生成する
        }
    }

    void UpdateStage(int toChipIndex) //フロア生成メソッド（引数：ここまで作っておいてほしいというフロア番号）
    {
        if (toChipIndex <= currentChipIndex) return;    //現在作ってあるフロア数を下回っていた場合は何もしない（基本発生しえないはず、不要）

        //現在生成済みフロア番号＋１からはじめ、作るべきフロア番号にたどり着くまで繰り返す
        for (int i = currentChipIndex + 1; i <= toChipIndex; i++)   //引数の数になるまでフロア生成を繰り替えす
        {
            GameObject stageObject = GeneratedStage(i);
            generatedStageList.Add(stageObject);    //リストの末尾に記帳される
        }

        //リスト元の数（５）＋２＝７個まではＯＫ。８個以上のフロア数になったら消す。
        while (generatedStageList.Count > preInstantiate + 2) DestroyOldestStage();
        //点検。必要な数に減少するまで、フロア削除メソッドをwhile繰り返す
        currentChipIndex = toChipIndex;//生成したフロア番号の更新
    }

    GameObject GeneratedStage(int chipIndex)//実際にフロアをインスタンティエイト、ＧａｍｅＯｂｊｅｃｔとして返す
    {
        //　配列からランダムにチョイス
        int nextStageChip = Random.Range(0, stageChips.Length);

        //引数のフロア番号に対応した座標に、配列から選ばれたフロアを生成
        //=(GameObject)は、昔の書き方なので消しても問題ない
        //GameObject stageObject = (GameObject)Instantiate(
        GameObject stageObject = (GameObject)Instantiate(
            stageChips[nextStageChip], 
            new Vector3(0, 0, chipIndex * StageChipSize), 
            Quaternion.identity);   //回転させない

        return stageObject;//生成と同時にＧａｍｅＯｂｊｅｃｔを返す。生成されたフロア情報を戻り値として返す
    }

    void DestroyOldestStage()//管理リストから古いフロアを①リストから②実際にオブジェクトも　削除するメソッド
    {
        GameObject oldStage = generatedStageList[0];    //管理リストの戦闘[0]盤
        generatedStageList.RemoveAt(0); //①リストから抹消
        Destroy(oldStage);  //②オブジェクトも削除
    }
}

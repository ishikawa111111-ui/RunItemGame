using UnityEngine;

public class EnemyEffect : MonoBehaviour
{
    [Header("消滅までの時間")]     //敵が死ぬ時の星エフェクト
    public float deleteTime = 1.0f;

    void Start()
    {
        Destroy(gameObject, deleteTime);
    }
}
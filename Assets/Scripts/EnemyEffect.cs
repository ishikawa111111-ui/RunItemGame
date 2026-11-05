using UnityEngine;

public class EnemyEffect : MonoBehaviour
{
    [Header("消滅までの時間")]
    public float deleteTime = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, deleteTime);
    }
}
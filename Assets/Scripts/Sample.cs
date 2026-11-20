using UnityEngine;

public class DamageReceiver : MonoBehaviour//, //IDamageable
{
    public int maxHP = 100;
    public int currentHP;

    [Header("Damage UI")]
    public GameObject damagePopupPrefab;
    public Transform popupSpawnPoint;

    [Header("Flash")]
    public Renderer[] renderers;
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;

    private Color[] originalColors;

    void Awake()
    {
        currentHP = maxHP;

        // 元の色を保存
        originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].material.color;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        ShowDamagePopup(damage);
        StartCoroutine(Flash());

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void ShowDamagePopup(int damage)
    {
        if (damagePopupPrefab == null) return;

        Vector3 pos = popupSpawnPoint != null
            ? popupSpawnPoint.position
            : transform.position + Vector3.up;

        GameObject popupObj = Instantiate(damagePopupPrefab, pos, Quaternion.identity);
        //popupObj.GetComponent<DamagePopup>().Setup(damage);
    }

    System.Collections.IEnumerator Flash()
    {
        foreach (var r in renderers)
            r.material.color = flashColor;

        yield return new WaitForSeconds(flashDuration);

        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.color = originalColors[i];
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
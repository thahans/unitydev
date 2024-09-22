using System.Collections;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public float offScreenThreshold = -10f; // Ekran�n d���na ��k�� e�i�i
    public float destroyDelay = 3f;          // Yok olma s�resi

    void Update()
    {
        // E�er mart� ekran�n solundan ��kt�ysa
        if (transform.position.x < offScreenThreshold)
        {
            StartCoroutine(DestroyAfterDelay());
        }
    }

    // Belirli bir s�re sonra yok olma coroutine'i
    private IEnumerator DestroyAfterDelay()
    {
        // 3 saniye bekle
        yield return new WaitForSeconds(destroyDelay);
        // Ku�u yok et
        Destroy(gameObject);
    }
}

using System.Collections;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public float offScreenThresholdLeft = -10f; // Ekran�n sol d���na ��k�� e�i�i
    public float offScreenThresholdRight = 10f;  // Ekran�n sa� d���na ��k�� e�i�i
    public float destroyDelay = 3f;               // Yok olma s�resi

    void Update()
    {
        // E�er ku� ekran�n solundan veya sa��ndan ��kt�ysa
        if (transform.position.x < offScreenThresholdLeft || transform.position.x > offScreenThresholdRight)
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

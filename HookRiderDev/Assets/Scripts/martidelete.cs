using System.Collections;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public float offScreenThreshold = -10f; // Ekranýn dýþýna çýkýþ eþiði
    public float destroyDelay = 3f;          // Yok olma süresi

    void Update()
    {
        // Eðer martý ekranýn solundan çýktýysa
        if (transform.position.x < offScreenThreshold)
        {
            StartCoroutine(DestroyAfterDelay());
        }
    }

    // Belirli bir süre sonra yok olma coroutine'i
    private IEnumerator DestroyAfterDelay()
    {
        // 3 saniye bekle
        yield return new WaitForSeconds(destroyDelay);
        // Kuþu yok et
        Destroy(gameObject);
    }
}

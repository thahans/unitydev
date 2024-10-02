using System.Collections;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public float offScreenThresholdLeft = -10f; // Ekranýn sol dýþýna çýkýþ eþiði
    public float offScreenThresholdRight = 10f;  // Ekranýn sað dýþýna çýkýþ eþiði
    public float destroyDelay = 3f;               // Yok olma süresi

    void Update()
    {
        // Eðer kuþ ekranýn solundan veya saðýndan çýktýysa
        if (transform.position.x < offScreenThresholdLeft || transform.position.x > offScreenThresholdRight)
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

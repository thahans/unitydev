using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookShoot : MonoBehaviour
{
    public Transform hookPrefab; // Fırlatılacak hook objesi
    public Transform triangle;   // Hook'un nereye fırlatılacağını gösteren ok
    public float hookSpeed = 10f; // Hook'un hızını ayarlıyoruz
    public LayerMask wallLayer; // Duvarların Layer'ı

    private Transform currentHook; // Mevcut hook referansı
    private bool isHooked = false; // Hook duvara çarptı mı?
    private Vector3 hookTarget; // Hook'un hedef noktası

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentHook == null)
        {
            ShootHook();
        }

        if (isHooked && currentHook != null)
        {
            // Karakteri duvara doğru çek
            transform.position = Vector3.MoveTowards(transform.position, hookTarget, hookSpeed * Time.deltaTime);

            // Eğer karakter duvara ulaştıysa, sekme işlemini başlat
            if (Vector3.Distance(transform.position, hookTarget) < 0.1f)
            {
                StartCoroutine(ReleaseHook());
            }
        }
    }

    void ShootHook()
    {
        // Hook'u üçgenin ucuna doğru fırlat
        currentHook = Instantiate(hookPrefab, triangle.position, Quaternion.identity);

        // Hook'un hedef noktasını üçgenin yönü doğrultusunda belirle
        hookTarget = triangle.position + triangle.up * 10f; // 10 birim uzağa fırlatılacak (mesafeyi isteğine göre ayarlayabilirsin)

        // Hook'u ileri doğru hareket ettir
        StartCoroutine(MoveHook());
    }

    IEnumerator MoveHook()
    {
        while (!isHooked)
        {
            currentHook.position = Vector3.MoveTowards(currentHook.position, hookTarget, hookSpeed * Time.deltaTime);

            // Hook duvara çarptı mı?
            RaycastHit2D hit = Physics2D.Raycast(currentHook.position, currentHook.up, 0.1f, wallLayer);
            if (hit.collider != null)
            {
                isHooked = true;
                hookTarget = hit.point; // Duvara çarpan noktayı hedef yap
            }

            yield return null;
        }
    }

    IEnumerator ReleaseHook()
    {
        yield return new WaitForSeconds(1f); // 1 saniye sonra hooku bırak

        // Hook'u yok et ve karakteri geri sek
        Destroy(currentHook.gameObject);
        isHooked = false;

        // Karakteri başlangıç noktasına geri döndür
        Vector3 startPosition = new Vector3(0, transform.position.y, transform.position.z); // X ekseninde başlangıç noktası
        while (Vector3.Distance(transform.position, startPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, hookSpeed * Time.deltaTime);
            yield return null;
        }
    }
}

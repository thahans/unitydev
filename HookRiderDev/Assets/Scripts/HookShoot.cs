using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookShoot : MonoBehaviour
{
    public Transform hookPrefab; // Fırlatılacak hook objesi
    public Transform triangle;   // Hook'un nereye fırlatılacağını gösteren ok
    public float hookSpeed = 10f; // Hook'un hızını ayarlıyoruz
    public LayerMask wallLayer;  // Duvarların Layer'ı
    public float bounceHeight = 5f;  // Zıplama yüksekliği
    public float bounceDuration = 1f;  // Zıplama süresi
    public float gravity = -9.8f;  // Yer çekimi değeri
    public float raycastDistance = 0.05f;  // Raycast'in hassasiyetini artırmak için mesafe

    private Transform currentHook;  // Mevcut hook referansı
    private bool isHooked = false;  // Hook duvara çarptı mı?
    private Vector3 hookTarget;     // Hook'un hedef noktası
    private Vector3 initialHookScale; // Hook'un ilk boyutu
    private bool isRetracting = false; // Karakteri çekme işlemi başlıyor mu?
    private bool isBouncing = false; // Parabolik hareket başladı mı?
    private Vector3 startPosition; // Karakterin başlangıç pozisyonu
    private bool isGameStarted = false; // Oyun başladı mı? kontrolü
    private bool isSlowMotionTriggered = false; // Ağır çekim tetiklendi mi?

    void Start()
    {
        startPosition = transform.position;  // Karakterin başlangıç konumunu kaydet
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentHook == null)
        {
            ShootHook();
        }

        if (isRetracting && currentHook != null)
        {
            // Hook'u kısalt ve karakteri duvara doğru çek
            RetractHook();
        }

        // Karakterin Z rotasyonunu sürekli sıfırla (takla atmaması için)
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    IEnumerator MoveHook()
    {
        while (!isHooked)
        {
            // Hook'un pozisyonunu ve uzunluğunu uzat
            currentHook.position = Vector3.MoveTowards(currentHook.position, hookTarget, hookSpeed * Time.deltaTime);

            // Hook'u uzat (Sadece Y ekseninde uzaması gerekiyor)
            currentHook.localScale = new Vector3(initialHookScale.x, Vector3.Distance(currentHook.position, transform.position), initialHookScale.z);

            // Hook duvara çarptı mı?
            RaycastHit2D hit = Physics2D.Raycast(currentHook.position, currentHook.up, raycastDistance, wallLayer);
            if (hit.collider != null)
            {
                isHooked = true;
                hookTarget = hit.point; // Duvara çarpan noktayı hedef yap
                isRetracting = true; // Çekme işlemini başlat

                // Hook'un ileri gitmesini durdur
                StopCoroutine(MoveHook());
            }

            yield return null;
        }
    }

    void RetractHook()
    {
        // Karakteri duvara doğru çek
        transform.position = Vector3.MoveTowards(transform.position, hookTarget, hookSpeed * Time.deltaTime);

        // Hook'u kısalt
        currentHook.localScale = new Vector3(initialHookScale.x, Vector3.Distance(currentHook.position, transform.position), initialHookScale.z);

        // Eğer karakter duvara ulaştıysa, hook'u serbest bırak ve parabolik zıplamayı başlat
        if (Vector3.Distance(transform.position, hookTarget) < 0.8f)
        {
            ReleaseHook();
        }
    }

    void ReleaseHook()
    {
        if (currentHook != null)
        {
            Destroy(currentHook.gameObject);
            currentHook = null;
        }

        isHooked = false;
        isRetracting = false;

        // Parabolik zıplama tamamlandı, ağır çekimi başlat
        StartCoroutine(BounceBack());
    }

    IEnumerator SlowMotionAfterBounce()
    {
        // Ağır çekime geç
        Time.timeScale = 0.1f;

        float elapsedTime = 0f;
        while (elapsedTime < 3f)
        {
            elapsedTime += Time.unscaledDeltaTime;

            // Eğer oyuncu 3 saniye içinde tekrar hook atarsa
            if (Input.GetMouseButtonDown(0))
            {
                Time.timeScale = 1f;
                yield break;
            }

            yield return null;
        }

        // 3 saniye boyunca etkileşim olmazsa karakter ölecek
        Time.timeScale = 1f;
        CharacterFallsAndDies();

        // Ağır çekim sona erdi, tetikleyici bayrağı sıfırla
        isSlowMotionTriggered = false;
    }

    void CharacterFallsAndDies()
    {
        // Karakterin ölmesi ve oyun durumu
        Debug.Log("Karakter öldü!");
        Time.timeScale = 0f;
        ShowDeathScreen();
    }

    IEnumerator BounceBack()
    {
        isBouncing = true;
        float elapsedTime = 0f;
        Vector3 wallPosition = transform.position;  // Duvara çarpılan pozisyonu kaydet
        Vector3 peakPosition = new Vector3((startPosition.x + wallPosition.x) / 2, Mathf.Max(wallPosition.y + bounceHeight, bounceHeight), startPosition.z);  // Parabolün zirve noktası

        while (elapsedTime < bounceDuration)
        {
            float t = elapsedTime / bounceDuration;

            // Yatay hareket (X ekseni) için lineer interpolasyon
            float newX = Mathf.Lerp(wallPosition.x, startPosition.x, t);

            // Dikey hareket (Y ekseni) için parabolik bir interpolasyon sağlıyoruz
            float newY = Mathf.Lerp(wallPosition.y, startPosition.y, t) + bounceHeight * Mathf.Sin(Mathf.PI * t);  // Parabolik yay için Sin fonksiyonu

            // Karakterin yeni pozisyonunu ayarla
            transform.position = new Vector3(newX, newY, transform.position.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Karakter başlangıç pozisyonuna ulaşır
        transform.position = startPosition;
        isBouncing = false;
    }

    void ShootHook()
    {
        if (!isGameStarted)
        {
            // Oyun başlıyor
            isGameStarted = true;
        }

        currentHook = Instantiate(hookPrefab, triangle.position, triangle.rotation);
        hookTarget = triangle.position + triangle.up * 10f;
        initialHookScale = currentHook.localScale;

        StartCoroutine(MoveHook());
    }

    void ShowDeathScreen()
    {
        // UI ölüm ekranını açma kodu buraya yazılabilir
        // örneğin: deathScreenUI.SetActive(true);
    }

    // Karakterin ağır çekim bölgesine girdiğini kontrol eden tetikleyici
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SlowMotionZone") && !isSlowMotionTriggered)
        {
            isSlowMotionTriggered = true;  // Ağır çekim tetiklendi
            StartCoroutine(SlowMotionAfterBounce());
        }
    }

    // Karakter ağır çekim bölgesinden çıktığında bayrağı sıfırla
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("SlowMotionZone"))
        {
            isSlowMotionTriggered = false;  // Ağır çekim bölgesinden çıktı, tekrar tetiklenebilir
        }
    }
}

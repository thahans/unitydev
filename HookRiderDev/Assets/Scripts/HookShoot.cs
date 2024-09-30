using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // TextMeshPro için gerekli kütüphane

public class HookShoot : MonoBehaviour
{
    public Transform hookPrefab; 
    public Transform triangle;   
    public float hookSpeed = 10f; 
    public LayerMask wallLayer;  
    public float bounceHeight = 5f;  
    public float bounceDuration = 1f;  
    public float gravity = -9.8f;  
    public float raycastDistance = 0.05f;  

    public TextMeshProUGUI scoreText;  // Skoru göstermek için TextMeshPro referansı
    private int score = 0;  // Skor değişkeni

    private Transform currentHook;  
    private bool isHooked = false;  
    private Vector3 hookTarget;     
    private Vector3 initialHookScale; 
    private bool isRetracting = false; 
    private bool isBouncing = false; 
    private Vector3 startPosition; 
    private bool isGameStarted = false; 
    private bool isSlowMotionTriggered = false;
    
    public HookAim hookAimScript;  // HookAim scriptine referans
    private bool isInSlowMotionZone = false;  // Karakter slow motion bölgesinde mi?

    // Yeni eklenen değişkenler
    public Transform slowMotionZoneTransform;  // Görünmez kare için referans
    public float slowMotionZoneMoveSpeed = 0.5f;  // Karenin yavaşça aşağıya kayma hızı
    private Vector3 slowMotionZoneStartPos;  // Karenin başlangıç pozisyonu
    private bool isZoneMovingDown = false;  // Kare aşağıya kayıyor mu?

    void Start()
    {
        startPosition = transform.position;  
        slowMotionZoneStartPos = slowMotionZoneTransform.position;  // Karenin başlangıç pozisyonunu kaydediyoruz
        UpdateScoreText();  // Oyun başladığında skoru güncelle
    }

    void Update()
    {
        // Sadece "isInSlowMotionZone" true olduğunda hook atabilir
        if (Input.GetMouseButtonDown(0) && currentHook == null && isInSlowMotionZone)
        {
            ShootHook();
        }

        if (isRetracting && currentHook != null)
        {
            RetractHook();
        }

        transform.rotation = Quaternion.Euler(0, 0, 0);

        // Eğer kare hareket ediyorsa onu yavaşça aşağıya kaydır
        if (isZoneMovingDown)
        {
            MoveSlowMotionZoneDown();
        }
    }

    void MoveSlowMotionZoneDown()
    {
        // Kareyi yavaşça aşağıya doğru hareket ettir
        slowMotionZoneTransform.position += Vector3.down * slowMotionZoneMoveSpeed * Time.deltaTime;
        
        // Karakteri de kare ile beraber aşağıya hareket ettir
        transform.position += Vector3.down * slowMotionZoneMoveSpeed * Time.deltaTime;
    }

    void ResetSlowMotionZonePosition()
    {
        slowMotionZoneTransform.position = slowMotionZoneStartPos;  // Kareyi başlangıç pozisyonuna döndür
    }

    IEnumerator SlowMotionAfterBounce()
    {
        Time.timeScale = 0.1f;
        isZoneMovingDown = true;  // Kareyi aşağıya kaydırmaya başla

        float elapsedTime = 0f;
        while (elapsedTime < 3f)
        {
            elapsedTime += Time.unscaledDeltaTime;

            if (Input.GetMouseButtonDown(0))
            {
                Time.timeScale = 1f;
                isSlowMotionTriggered = false;
                isZoneMovingDown = false;  // Hareketi durdur
                ResetSlowMotionZonePosition();  // Kareyi başlangıç pozisyonuna geri döndür
                yield break;
            }

            yield return null;
        }

        // Slow motion süresi bittiğinde zaman hızını normale döndür
        Time.timeScale = 1f;
        isSlowMotionTriggered = false;
        isZoneMovingDown = false;  // Hareketi durdur
        ResetSlowMotionZonePosition();  // Kareyi başlangıç pozisyonuna geri döndür
    }

    IEnumerator BounceBack()
    {
        isBouncing = true;
        float elapsedTime = 0f;
        Vector3 wallPosition = transform.position;
        Vector3 peakPosition = new Vector3((startPosition.x + wallPosition.x) / 2, Mathf.Max(wallPosition.y + bounceHeight, bounceHeight), startPosition.z);

        while (elapsedTime < bounceDuration)
        {
            float t = elapsedTime / bounceDuration;
            float newX = Mathf.Lerp(wallPosition.x, startPosition.x, t);
            float newY = Mathf.Lerp(wallPosition.y, startPosition.y, t) + bounceHeight * Mathf.Sin(Mathf.PI * t);

            transform.position = new Vector3(newX, newY, transform.position.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = startPosition;
        isBouncing = false;
        ResetSlowMotionZonePosition();  // Duvara sektiğinde kareyi başlangıç noktasına döndür
    }

    void ShootHook()
    {
        if (!isGameStarted)
        {
            isGameStarted = true;
        }

        currentHook = Instantiate(hookPrefab, triangle.position, triangle.rotation);
        hookTarget = triangle.position + triangle.up * 10f;
        initialHookScale = currentHook.localScale;

        StartCoroutine(MoveHook());
    }

    void ShowDeathScreen()
    {
        // Ölüm ekranı işlemleri
        Time.timeScale = 0; // Oyunu durdur
        // Ölüm ekranı UI'sını göster (bu kısmı ihtiyacına göre düzenle)
        Debug.Log("Ölüm ekranı açıldı!");
    }

    IEnumerator MoveHook()
    {
        while (!isHooked)
        {
            currentHook.position = Vector3.MoveTowards(currentHook.position, hookTarget, hookSpeed * Time.deltaTime);
            currentHook.localScale = new Vector3(initialHookScale.x, Vector3.Distance(currentHook.position, transform.position), initialHookScale.z);

            RaycastHit2D hit = Physics2D.Raycast(currentHook.position, currentHook.up, raycastDistance, wallLayer);
            if (hit.collider != null)
            {
                isHooked = true;
                hookTarget = hit.point;
                isRetracting = true;
                StopCoroutine(MoveHook());
                
                // Başarılı hook sonrası puan ekle
                AddScore(1);  
            }

            yield return null;
        }
    }

    void RetractHook()
    {
        transform.position = Vector3.MoveTowards(transform.position, hookTarget, hookSpeed * Time.deltaTime);
        currentHook.localScale = new Vector3(initialHookScale.x, Vector3.Distance(currentHook.position, transform.position), initialHookScale.z);

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

        StartCoroutine(BounceBack());
    }

    // Slow motion bölgesine giriş ve çıkışları kontrol et
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("SlowMotionZone"))
        {
            isSlowMotionTriggered = true;
            isInSlowMotionZone = true;

            hookAimScript.SetTriangleActive(true);
            StartCoroutine(SlowMotionAfterBounce());
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("SlowMotionZone"))
        {
            isInSlowMotionZone = false;
            hookAimScript.SetTriangleActive(false);
        }
    }

    // Skoru artır ve ekrana yansıt
    void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    // Skor metnini güncelle
    void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }
}

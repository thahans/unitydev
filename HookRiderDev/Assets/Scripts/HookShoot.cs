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

    public TextMeshProUGUI scoreText;  
    private int score = 0;  

    private Transform currentHook;  
    private bool isHooked = false;  
    private Vector3 hookTarget;     
    private Vector3 initialHookScale; 
    private bool isRetracting = false; 
    private bool isBouncing = false; 
    private Vector3 startPosition; 
    private bool isGameStarted = false; 
    private bool isSlowMotionTriggered = false;
    
    public HookAim hookAimScript;  
    private bool isInSlowMotionZone = false;  

    // Yeni eklenen değişkenler
    public Transform slowMotionZoneTransform;  
    public float slowMotionZoneMoveSpeed = 0.9f;  
    private Vector3 slowMotionZoneStartPos;  
    private bool isZoneMovingDown = false;  

    private bool isCoroutineRunning = false;  // Coroutine'un çalışıp çalışmadığını kontrol etmek için
    private bool hasFallen = false;  // Karakterin düştüğünü kontrol etmek için

    void Start()
    {
        startPosition = transform.position;  
        slowMotionZoneStartPos = slowMotionZoneTransform.position;  
        UpdateScoreText();  
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentHook == null && isInSlowMotionZone)
        {
            ShootHook();
        }

        if (isRetracting && currentHook != null)
        {
            RetractHook();
        }

        transform.rotation = Quaternion.Euler(0, 0, 0);

        if (isZoneMovingDown)
        {
            MoveSlowMotionZoneDown();
        }
    }

    void MoveSlowMotionZoneDown()
    {
        slowMotionZoneTransform.position += Vector3.down * slowMotionZoneMoveSpeed * Time.deltaTime;
        
        // Karakteri de kare ile beraber aşağıya hareket ettir
        transform.position += Vector3.down * slowMotionZoneMoveSpeed * Time.deltaTime;
    }

    void ResetSlowMotionZonePosition()
    {
        slowMotionZoneTransform.position = slowMotionZoneStartPos;  
    }

    IEnumerator SlowMotionAfterBounce()
    {
        Time.timeScale = 0.1f;
        isZoneMovingDown = true;  

        float elapsedTime = 0f;
        while (elapsedTime < 3f)
        {
            elapsedTime += Time.unscaledDeltaTime;

            if (Input.GetMouseButtonDown(0))
            {
                Time.timeScale = 1f;
                isSlowMotionTriggered = false;
                isZoneMovingDown = false;  
                ResetSlowMotionZonePosition();  
                yield break;
            }

            yield return null;
        }

        Time.timeScale = 1f;
        isSlowMotionTriggered = false;
        isZoneMovingDown = false;  
        ResetSlowMotionZonePosition();  

        // Altındaki karenin collider'ını kaldır ve karakteri düşür
        RemoveColliderAndDropCharacter();
    }

    void RemoveColliderAndDropCharacter()
    {
        // Eğer karakter henüz düşmemişse
        if (!hasFallen)
        {
            hasFallen = true;  // Düşme işleminin başladığını belirt

            // Burada görünmez karenin collider'ını kaldırıyoruz
            // Eğer görünmez kare bir GameObject ise
            // Destroy(slowMotionZoneTransform.gameObject); // veya collider'ını kapatabilirsin
            slowMotionZoneTransform.GetComponent<Collider2D>().enabled = false;

            // Karakterin aşağı düşmesini sağla
            StartCoroutine(DropCharacter());
        }
    }

    IEnumerator DropCharacter()
    {
        float fallDuration = 2f;  // 2 saniyelik düşüş süresi
        float elapsedTime = 0f;
        Vector3 initialPosition = transform.position;

        while (elapsedTime < fallDuration)
        {
            float t = elapsedTime / fallDuration;
            transform.position = Vector3.Lerp(initialPosition, initialPosition + Vector3.down * 30f, t);  // Aşağı doğru düşüş
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Düşüş tamamlandıktan sonra ölüm ekranını göster
        ShowDeathScreen();
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
        ResetSlowMotionZonePosition();  
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
        Time.timeScale = 0; 
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("SlowMotionZone"))
        {
            isSlowMotionTriggered = true;
            isInSlowMotionZone = true;

            hookAimScript.SetTriangleActive(true);
            isZoneMovingDown = true;  // Karayı aşağıya kaydırmaya başla
            if (!isCoroutineRunning)  // Coroutine'u bir kez başlat
            {
                StartCoroutine(SlowMotionAfterBounce());
                isCoroutineRunning = true;  // Coroutine çalışıyor
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("SlowMotionZone"))
        {
            isInSlowMotionZone = false;
            hookAimScript.SetTriangleActive(false);
            isZoneMovingDown = false;  // Karayı durdur
            ResetSlowMotionZonePosition();  // Kareyi başlangıç noktasına döndür
            isCoroutineRunning = false;  // Coroutine çalışmayı bıraksın
        }
    }

    void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }
}

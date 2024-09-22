using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public GameObject birdPrefab;    // Kuþ prefab'i
    public Transform[] spawnPoints;  // Spawnlanacak noktalar
    public float birdSpeed = 5f;     // Kuþun hýzý

    // Kuþun spawnlanacaðý saniyeleri belirten bir liste
    public List<float> spawnTimes = new List<float> { 2f, 5f, 10f, 20f }; // Ýstediðin saniyeleri buraya ekle

    private int currentIndex = 0;    // Mevcut kontrol edilen saniye indeksi
    private float timer = 0f;        // Sayaç

    // Baþlangýçta çalýþan fonksiyon
    void Start()
    {
        // Coroutine baþlat, saniye sayacý çalýþsýn
        StartCoroutine(SpawnBirdAtSpecificTimes());
    }

    // Belirli saniyelerde kuþ spawnlayan coroutine
    IEnumerator SpawnBirdAtSpecificTimes()
    {
        while (currentIndex < spawnTimes.Count)
        {
            // Zamaný artýr
            timer += Time.deltaTime;

            // Eðer sýradaki spawn zamaný geldiyse kuþ spawnla
            if (timer >= spawnTimes[currentIndex])
            {
                SpawnBird(); // Kuþ spawnlama fonksiyonunu çalýþtýr
                currentIndex++; // Sýradaki zaman dilimine geç
            }

            yield return null; // Bir sonraki frame'i bekle
        }
    }

    // Kuþ spawnlayan fonksiyon
    public void SpawnBird()
    {
        // Rastgele bir spawn noktasý seç
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Kuþu spawnla
        GameObject bird = Instantiate(birdPrefab, spawnPoint.position, Quaternion.identity);

        

        // Rastgele saða veya sola gitmesini saðla
        int direction = 0;

        // Rigidbody2D ile hareket etmesi
        Rigidbody2D rb = bird.GetComponent<Rigidbody2D>();

        if (direction == 0)
        {
            // Sola git
            rb.velocity = new Vector2(-birdSpeed, 0);
            // Kuþun saða bakmasý için ters çevir
            
        }
        else
        {
            // Saða git
            rb.velocity = new Vector2(birdSpeed, 0);
        }
    }
}

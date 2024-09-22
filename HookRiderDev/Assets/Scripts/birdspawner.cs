using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public GameObject birdPrefab;    // Ku� prefab'i
    public Transform[] spawnPoints;  // Spawnlanacak noktalar
    public float birdSpeed = 5f;     // Ku�un h�z�

    // Ku�un spawnlanaca�� saniyeleri belirten bir liste
    public List<float> spawnTimes = new List<float> { 2f, 5f, 10f, 20f }; // �stedi�in saniyeleri buraya ekle

    private int currentIndex = 0;    // Mevcut kontrol edilen saniye indeksi
    private float timer = 0f;        // Saya�

    // Ba�lang��ta �al��an fonksiyon
    void Start()
    {
        // Coroutine ba�lat, saniye sayac� �al��s�n
        StartCoroutine(SpawnBirdAtSpecificTimes());
    }

    // Belirli saniyelerde ku� spawnlayan coroutine
    IEnumerator SpawnBirdAtSpecificTimes()
    {
        while (currentIndex < spawnTimes.Count)
        {
            // Zaman� art�r
            timer += Time.deltaTime;

            // E�er s�radaki spawn zaman� geldiyse ku� spawnla
            if (timer >= spawnTimes[currentIndex])
            {
                SpawnBird(); // Ku� spawnlama fonksiyonunu �al��t�r
                currentIndex++; // S�radaki zaman dilimine ge�
            }

            yield return null; // Bir sonraki frame'i bekle
        }
    }

    // Ku� spawnlayan fonksiyon
    public void SpawnBird()
    {
        // Rastgele bir spawn noktas� se�
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Ku�u spawnla
        GameObject bird = Instantiate(birdPrefab, spawnPoint.position, Quaternion.identity);

        

        // Rastgele sa�a veya sola gitmesini sa�la
        int direction = 0;

        // Rigidbody2D ile hareket etmesi
        Rigidbody2D rb = bird.GetComponent<Rigidbody2D>();

        if (direction == 0)
        {
            // Sola git
            rb.velocity = new Vector2(-birdSpeed, 0);
            // Ku�un sa�a bakmas� i�in ters �evir
            
        }
        else
        {
            // Sa�a git
            rb.velocity = new Vector2(birdSpeed, 0);
        }
    }
}

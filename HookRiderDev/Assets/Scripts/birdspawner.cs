using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public GameObject birdPrefabLeft;  // Soldan saða giden kuþ prefab'i
    public GameObject birdPrefabRight; // Saðdan sola giden kuþ prefab'i
    public Vector2[] spawnPoints;      // Kuþlarýn spawnlanacaðý 6 farklý X ve Y koordinatlarý

    public float minSpawnTime = 2f;    // Min spawn süresi
    public float maxSpawnTime = 5f;    // Max spawn süresi
    public float birdSpeed = 5f;       // Kuþlarýn hýzý

    private float spawnTimer = 0f;     // Spawn için sayaç
    private float randomSpawnTime;     // Rastgele spawn zamaný

    void Start()
    {
        // Ýlk rastgele spawn zamanýný ayarla
        randomSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    void Update()
    {
        // Sayaç ilerlesin
        spawnTimer += Time.deltaTime;

        // Eðer sayaç rastgele belirlenen spawn süresine ulaþtýysa kuþ spawnla
        if (spawnTimer >= randomSpawnTime)
        {
            SpawnBird();
            spawnTimer = 0f; // Sayaç sýfýrlanýr
            randomSpawnTime = Random.Range(minSpawnTime, maxSpawnTime); // Yeni bir rastgele zaman ayarlanýr
        }
    }

    // Kuþ spawnlayan fonksiyon
    public void SpawnBird()
    {
        int x = Random.Range(0, spawnPoints.Length);
        Vector2 spawnPoint = spawnPoints[x];

        int direction;

        if (0 <= x && x <= 2) // Eðer x 0, 1 veya 2 ise
        {
            direction = 1; // Soldan saða gidecek
        }
        else
        {
            direction = 0; // Saðdan sola gidecek
        }

        GameObject bird;
        if (direction == 0)
        {
            // Saðdan sola giden kuþ
            bird = Instantiate(birdPrefabLeft, spawnPoint, Quaternion.identity);
            // Kuþu sola hareket ettir
            Rigidbody2D rb = bird.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(-birdSpeed, 0);
        }
        else
        {
            // Soldan saða giden kuþ
            bird = Instantiate(birdPrefabRight, spawnPoint, Quaternion.identity);
            // Kuþu saða hareket ettir
            Rigidbody2D rb = bird.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(birdSpeed, 0);
        }
    }

}

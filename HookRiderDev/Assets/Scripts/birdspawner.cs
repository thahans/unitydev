using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public GameObject birdPrefabLeft;  // Soldan sa�a giden ku� prefab'i
    public GameObject birdPrefabRight; // Sa�dan sola giden ku� prefab'i
    public Vector2[] spawnPoints;      // Ku�lar�n spawnlanaca�� 6 farkl� X ve Y koordinatlar�

    public float minSpawnTime = 2f;    // Min spawn s�resi
    public float maxSpawnTime = 5f;    // Max spawn s�resi
    public float birdSpeed = 5f;       // Ku�lar�n h�z�

    private float spawnTimer = 0f;     // Spawn i�in saya�
    private float randomSpawnTime;     // Rastgele spawn zaman�

    void Start()
    {
        // �lk rastgele spawn zaman�n� ayarla
        randomSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    void Update()
    {
        // Saya� ilerlesin
        spawnTimer += Time.deltaTime;

        // E�er saya� rastgele belirlenen spawn s�resine ula�t�ysa ku� spawnla
        if (spawnTimer >= randomSpawnTime)
        {
            SpawnBird();
            spawnTimer = 0f; // Saya� s�f�rlan�r
            randomSpawnTime = Random.Range(minSpawnTime, maxSpawnTime); // Yeni bir rastgele zaman ayarlan�r
        }
    }

    // Ku� spawnlayan fonksiyon
    public void SpawnBird()
    {
        int x = Random.Range(0, spawnPoints.Length);
        Vector2 spawnPoint = spawnPoints[x];

        int direction;

        if (0 <= x && x <= 2) // E�er x 0, 1 veya 2 ise
        {
            direction = 1; // Soldan sa�a gidecek
        }
        else
        {
            direction = 0; // Sa�dan sola gidecek
        }

        GameObject bird;
        if (direction == 0)
        {
            // Sa�dan sola giden ku�
            bird = Instantiate(birdPrefabLeft, spawnPoint, Quaternion.identity);
            // Ku�u sola hareket ettir
            Rigidbody2D rb = bird.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(-birdSpeed, 0);
        }
        else
        {
            // Soldan sa�a giden ku�
            bird = Instantiate(birdPrefabRight, spawnPoint, Quaternion.identity);
            // Ku�u sa�a hareket ettir
            Rigidbody2D rb = bird.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(birdSpeed, 0);
        }
    }

}

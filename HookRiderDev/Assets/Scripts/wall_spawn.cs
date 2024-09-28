using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    public GameObject wallPrefab;   // Duvar�n prefab'�
    public float spawnInterval = 2f;  // Duvar�n ne s�kl�kla spawnlanaca��
    public Transform spawnPoint;    // Duvar�n spawnlanaca�� pozisyon
    public float wallLifetime = 5f; // Duvar�n ka� saniye sonra yok olaca��

    void Start()
    {
        // Duvarlar� belirli aral�klarla spawnlamak i�in bir coroutine ba�lat�yoruz
        StartCoroutine(SpawnWalls());
    }

    IEnumerator SpawnWalls()
    {
        while (true)
        {
            // Duvar prefab'�n� belirlenen noktada spawnla
            GameObject spawnedWall = Instantiate(wallPrefab, spawnPoint.position, Quaternion.identity);

            // Duvar� belirtilen s�re sonra yok et
            Destroy(spawnedWall, wallLifetime);

            // Bir sonraki spawn i�in belirlenen s�reyi bekle
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}

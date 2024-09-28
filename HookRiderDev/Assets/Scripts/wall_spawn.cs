using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    public GameObject wallPrefab;   // Duvarýn prefab'ý
    public float spawnInterval = 2f;  // Duvarýn ne sýklýkla spawnlanacaðý
    public Transform spawnPoint;    // Duvarýn spawnlanacaðý pozisyon
    public float wallLifetime = 5f; // Duvarýn kaç saniye sonra yok olacaðý

    void Start()
    {
        // Duvarlarý belirli aralýklarla spawnlamak için bir coroutine baþlatýyoruz
        StartCoroutine(SpawnWalls());
    }

    IEnumerator SpawnWalls()
    {
        while (true)
        {
            // Duvar prefab'ýný belirlenen noktada spawnla
            GameObject spawnedWall = Instantiate(wallPrefab, spawnPoint.position, Quaternion.identity);

            // Duvarý belirtilen süre sonra yok et
            Destroy(spawnedWall, wallLifetime);

            // Bir sonraki spawn için belirlenen süreyi bekle
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}

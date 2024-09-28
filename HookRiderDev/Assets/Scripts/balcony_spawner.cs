using UnityEngine;

public class WallSpawnerScript : MonoBehaviour
{
    public GameObject wallPrefab;  // Duvar prefab'ý
    private float spawnRate;       // Duvarlarýn kaç saniyede bir spawnlanacaðý
    private float timer = 0f;

    void Start()
    {
        // Ýlk baþta spawnRate'i rastgele ayarla
        spawnRate = Random.Range(2f, 5f);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            SpawnWall();
            timer = 0f; // Zamanlayýcýyý sýfýrla

            // Bir sonraki spawn için spawnRate'i tekrar rastgele ayarla (2 ile 10 saniye arasýnda)
            spawnRate = Random.Range(2f, 5f);
        }
    }

    void SpawnWall()
    {
        // Duvarýn y ekseninde sabit bir konumda gelmesi
        float randomY = 6.81f;

        // Duvarýn spawnlanacaðý pozisyon
        Vector3 spawnPosition = new Vector3(transform.position.x, randomY, 0f);

        // Duvarý belirtilen pozisyonda spawnla
        Instantiate(wallPrefab, spawnPosition, Quaternion.identity);
    }
}

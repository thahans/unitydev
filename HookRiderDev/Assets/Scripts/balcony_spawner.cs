using UnityEngine;

public class WallSpawnerScript : MonoBehaviour
{
    public GameObject wallPrefab;  // Duvar prefab'ý
    private float spawnRate;       // Duvarlarýn kaç saniyede bir spawnlanacaðý
    private float timer = 0f;

    // Spawn süresi aralýðý
    public float spawnerLowerSecond = 2f;
    public float spawnerUpperSecond = 5f;

    // Ýlk duvar için spawn süresi aralýðý
    public float initialLowerSecond = 1f;
    public float initialUpperSecond = 2f;

    // Yeni eklenen x pozisyonu aralýðý
    public float spawnLowerX = -5f;  // Minimum x pozisyonu
    public float spawnUpperX = 5f;   // Maksimum x pozisyonu

    void Start()
    {
        // Ýlk spawnRate'i ilk duvar için ayarla
        spawnRate = Random.Range(initialLowerSecond, initialUpperSecond);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            SpawnWall();
            timer = 0f; // Zamanlayýcýyý sýfýrla

            // Bir sonraki spawn için spawnRate'i tekrar rastgele ayarla
            spawnRate = Random.Range(spawnerLowerSecond, spawnerUpperSecond);
        }
    }

    void SpawnWall()
    {
        // Duvarýn y ekseninde sabit bir konumda gelmesi
        float randomY = 6.81f;

        // Duvarýn spawnlanacaðý rastgele x pozisyonu (seçilen iki x deðeri arasýnda)
        float randomX = Random.Range(spawnLowerX, spawnUpperX);

        // Duvarýn spawnlanacaðý pozisyon
        Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);

        // Duvarý belirtilen pozisyonda spawnla
        Instantiate(wallPrefab, spawnPosition, Quaternion.identity);
    }
}

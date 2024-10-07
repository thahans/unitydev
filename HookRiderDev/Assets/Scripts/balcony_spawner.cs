using UnityEngine;

public class WallSpawnerScript : MonoBehaviour
{
    public GameObject wallPrefab;  // Duvar prefab'�
    private float spawnRate;       // Duvarlar�n ka� saniyede bir spawnlanaca��
    private float timer = 0f;

    // Spawn s�resi aral���
    public float spawnerLowerSecond = 2f;
    public float spawnerUpperSecond = 5f;

    // Yeni eklenen x pozisyonu aral���
    public float spawnLowerX = -5f;  // Minimum x pozisyonu
    public float spawnUpperX = 5f;   // Maksimum x pozisyonu

    void Start()
    {
        // �lk ba�ta spawnRate'i rastgele ayarla
        spawnRate = Random.Range(spawnerLowerSecond, spawnerUpperSecond);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            SpawnWall();
            timer = 0f; // Zamanlay�c�y� s�f�rla

            // Bir sonraki spawn i�in spawnRate'i tekrar rastgele ayarla
            spawnRate = Random.Range(spawnerLowerSecond, spawnerUpperSecond);
        }
    }

    void SpawnWall()
    {
        // Duvar�n y ekseninde sabit bir konumda gelmesi
        float randomY = 6.81f;

        // Duvar�n spawnlanaca�� rastgele x pozisyonu (se�ilen iki x de�eri aras�nda)
        float randomX = Random.Range(spawnLowerX, spawnUpperX);

        // Duvar�n spawnlanaca�� pozisyon
        Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);

        // Duvar� belirtilen pozisyonda spawnla
        Instantiate(wallPrefab, spawnPosition, Quaternion.identity);
    }
}

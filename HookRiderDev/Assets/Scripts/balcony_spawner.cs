using UnityEngine;

public class WallSpawnerScript : MonoBehaviour
{
    public GameObject wallPrefab;  // Duvar prefab'�
    private float spawnRate;       // Duvarlar�n ka� saniyede bir spawnlanaca��
    private float timer = 0f;
    public float spawnerlowersecond = 2f;
    public float spawneruppersecond = 5f;

    void Start()
    {
        // �lk ba�ta spawnRate'i rastgele ayarla
        spawnRate = Random.Range(spawnerlowersecond, spawneruppersecond);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            SpawnWall();
            timer = 0f; // Zamanlay�c�y� s�f�rla

            // Bir sonraki spawn i�in spawnRate'i tekrar rastgele ayarla (2 ile 10 saniye aras�nda)
            spawnRate = Random.Range(2f, 5f);
        }
    }

    void SpawnWall()
    {
        // Duvar�n y ekseninde sabit bir konumda gelmesi
        float randomY = 6.81f;

        // Duvar�n spawnlanaca�� pozisyon
        Vector3 spawnPosition = new Vector3(transform.position.x, randomY, 0f);

        // Duvar� belirtilen pozisyonda spawnla
        Instantiate(wallPrefab, spawnPosition, Quaternion.identity);
    }
}

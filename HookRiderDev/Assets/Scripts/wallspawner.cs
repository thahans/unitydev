using UnityEngine;

public class WallSpawnerScript : MonoBehaviour
{
    public GameObject wallPrefab1;  // Duvar 1
    public GameObject wallPrefab2;  // Duvar 2
    public GameObject wallPrefab3;  // Duvar 3
    public GameObject wallPrefab4;  // Duvar 4

    public float spawnRate = 2f;   // Duvarlar�n ka� saniyede bir gelece�i
    public float spawnHeight = 5f; // Duvarlar�n ortaya ��kaca�� y�ksekli�in aral���

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            SpawnWall();
            timer = 0f; // Zamanlay�c�y� s�f�rla
        }
    }

    void SpawnWall()
    {
        // Duvar�n rastgele bir y y�ksekli�inden gelmesi
        float randomY = 6.81f;

        // Duvar�n pozisyonu: (x, y, z)
        Vector3 spawnPosition = new Vector3(transform.position.x, randomY, 0f);

        // Rastgele bir duvar se�mek i�in
        int randomWall = Random.Range(1, 5);  // 1 ve 4 aras� (5 dahil de�il)

        GameObject selectedWallPrefab;

        switch (randomWall)
        {
            case 1:
                selectedWallPrefab = wallPrefab1;
                break;
            case 2:
                selectedWallPrefab = wallPrefab2;
                break;
            case 3:
                selectedWallPrefab = wallPrefab3;
                break;
            case 4:
                selectedWallPrefab = wallPrefab4;
                break;
            default:
                selectedWallPrefab = wallPrefab1;
                break;
        }

        // Rastgele se�ilen duvar� spawn et
        Instantiate(selectedWallPrefab, spawnPosition, Quaternion.identity);
    }
}

using UnityEngine;

public class WallSpawnerScript : MonoBehaviour
{
    public GameObject wallPrefab1;  // Duvar 1
    public GameObject wallPrefab2;  // Duvar 2
    public GameObject wallPrefab3;  // Duvar 3
    public GameObject wallPrefab4;  // Duvar 4

    public float spawnRate = 2f;   // Duvarlarýn kaç saniyede bir geleceði
    public float spawnHeight = 5f; // Duvarlarýn ortaya çýkacaðý yüksekliðin aralýðý

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            SpawnWall();
            timer = 0f; // Zamanlayýcýyý sýfýrla
        }
    }

    void SpawnWall()
    {
        // Duvarýn rastgele bir y yüksekliðinden gelmesi
        float randomY = 6.81f;

        // Duvarýn pozisyonu: (x, y, z)
        Vector3 spawnPosition = new Vector3(transform.position.x, randomY, 0f);

        // Rastgele bir duvar seçmek için
        int randomWall = Random.Range(1, 5);  // 1 ve 4 arasý (5 dahil deðil)

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

        // Rastgele seçilen duvarý spawn et
        Instantiate(selectedWallPrefab, spawnPosition, Quaternion.identity);
    }
}

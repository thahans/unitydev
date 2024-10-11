using TMPro;
using UnityEngine;

public class Difficulty : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Skor için TextMeshPro objesi
    private int currentScore;

    // Sol ve sað balkon spawner'larýnýn referanslarý
    public GameObject balcony_spawner_left;
    public GameObject balcony_spawner_right;
    public GameObject bird_spawner;

    public GameObject WallPrefab;
    public GameObject ReverseWallPrefab;
    public GameObject BalconyPrefab;
    public GameObject ReverseBalconyPrefab;

    // Bu scriptlere eriþmek için deðiþkenler
    private BalconySpawner leftBalconySpawnerScript;
    private BalconySpawner rightBalconySpawnerScript;
    private BirdSpawner birdspawnerScript;

    private float Speed;

    void Start()
    {
        // Sol ve sað balkon spawner'larýna ait balcony_spawner scriptine eriþim
        leftBalconySpawnerScript = balcony_spawner_left.GetComponent<BalconySpawner>();
        rightBalconySpawnerScript = balcony_spawner_right.GetComponent<BalconySpawner>();
        birdspawnerScript = bird_spawner.GetComponent<BirdSpawner>();
    }

    void Update()
    {
        // Skoru string'den integer'a çevir
        currentScore = int.Parse(scoreText.text);

        // Skor bazlý zorluk ayarlarýný güncelle
        UpdateDifficulty();
    }

    void UpdateDifficulty()
    {
        // Örnek zorluk ayarlarý:
        if (currentScore > 10)
        {
            leftBalconySpawnerScript.spawnerLowerSecond = 1.0f; // Sol spawner için
            leftBalconySpawnerScript.spawnerUpperSecond = 3.0f;

            rightBalconySpawnerScript.spawnerLowerSecond = 1.0f; // Sað spawner için
            rightBalconySpawnerScript.spawnerUpperSecond = 3.0f;

            birdspawnerScript.minSpawnTime = 1;
            birdspawnerScript.maxSpawnTime = 4;
            birdspawnerScript.birdSpeed = 3;

            Speed = 3.2f;

            // Gelecekte instantiate edilecek objeler için
            SetPrefabSpeeds(Speed);

            // Sahnedeki tüm mevcut objeleri güncelle
            UpdateExistingObjects(Speed);
        }
        else if (currentScore > 5)
        {
            leftBalconySpawnerScript.spawnerLowerSecond = 1.0f; // Sol spawner için
            leftBalconySpawnerScript.spawnerUpperSecond = 3.1f;

            rightBalconySpawnerScript.spawnerLowerSecond = 1.0f; // Sað spawner için
            rightBalconySpawnerScript.spawnerUpperSecond = 3.1f;

            birdspawnerScript.minSpawnTime = 1.0f;
            birdspawnerScript.maxSpawnTime = 4.5f;
            birdspawnerScript.birdSpeed = 2.5f;

            Speed = 3.1f;

            // Gelecekte instantiate edilecek objeler için
            SetPrefabSpeeds(Speed);

            // Sahnedeki tüm mevcut objeleri güncelle
            UpdateExistingObjects(Speed);
        }
    }

    // Prefab'larýn içindeki speed deðiþkenini ayarlamak için bir fonksiyon
    void SetPrefabSpeeds(float speed)
    {
        // Prefab instantiate edildiðinde objelerin speed'ini ayarlýyoruz
        WallMovementScript wallScript = WallPrefab.GetComponent<WallMovementScript>();
        WallMovementScript reverseWallScript = ReverseWallPrefab.GetComponent<WallMovementScript>();
        WallMovementScript balconyScript = BalconyPrefab.GetComponent<WallMovementScript>();
        WallMovementScript reverseBalconyScript = ReverseBalconyPrefab.GetComponent<WallMovementScript>();

        if (wallScript != null) wallScript.fallSpeed = speed;
        if (reverseWallScript != null) reverseWallScript.fallSpeed = speed;
        if (balconyScript != null) balconyScript.fallSpeed = speed;
        if (reverseBalconyScript != null) reverseBalconyScript.fallSpeed = speed;
    }

    // Sahnedeki tüm mevcut objeleri güncellemek için
    void UpdateExistingObjects(float speed)
    {
        // Sahnedeki tüm duvar ve balkon objelerini bul ve fallSpeed deðerlerini güncelle
        WallMovementScript[] allWallsAndBalconies = FindObjectsOfType<WallMovementScript>();

        foreach (WallMovementScript obj in allWallsAndBalconies)
        {
            obj.fallSpeed = speed;
        }
    }
}

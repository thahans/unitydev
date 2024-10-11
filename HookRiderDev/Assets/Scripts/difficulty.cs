using TMPro;
using UnityEngine;

public class Difficulty : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Skor i�in TextMeshPro objesi
    private int currentScore;

    // Sol ve sa� balkon spawner'lar�n�n referanslar�
    public GameObject balcony_spawner_left;
    public GameObject balcony_spawner_right;
    public GameObject bird_spawner;

    public GameObject WallPrefab;
    public GameObject ReverseWallPrefab;
    public GameObject BalconyPrefab;
    public GameObject ReverseBalconyPrefab;

    // Bu scriptlere eri�mek i�in de�i�kenler
    private BalconySpawner leftBalconySpawnerScript;
    private BalconySpawner rightBalconySpawnerScript;
    private BirdSpawner birdspawnerScript;

    private float Speed;

    void Start()
    {
        // Sol ve sa� balkon spawner'lar�na ait balcony_spawner scriptine eri�im
        leftBalconySpawnerScript = balcony_spawner_left.GetComponent<BalconySpawner>();
        rightBalconySpawnerScript = balcony_spawner_right.GetComponent<BalconySpawner>();
        birdspawnerScript = bird_spawner.GetComponent<BirdSpawner>();
    }

    void Update()
    {
        // Skoru string'den integer'a �evir
        currentScore = int.Parse(scoreText.text);

        // Skor bazl� zorluk ayarlar�n� g�ncelle
        UpdateDifficulty();
    }

    void UpdateDifficulty()
    {
        // �rnek zorluk ayarlar�:
        if (currentScore > 10)
        {
            leftBalconySpawnerScript.spawnerLowerSecond = 1.0f; // Sol spawner i�in
            leftBalconySpawnerScript.spawnerUpperSecond = 3.0f;

            rightBalconySpawnerScript.spawnerLowerSecond = 1.0f; // Sa� spawner i�in
            rightBalconySpawnerScript.spawnerUpperSecond = 3.0f;

            birdspawnerScript.minSpawnTime = 1;
            birdspawnerScript.maxSpawnTime = 4;
            birdspawnerScript.birdSpeed = 3;

            Speed = 3.2f;

            // Gelecekte instantiate edilecek objeler i�in
            SetPrefabSpeeds(Speed);

            // Sahnedeki t�m mevcut objeleri g�ncelle
            UpdateExistingObjects(Speed);
        }
        else if (currentScore > 5)
        {
            leftBalconySpawnerScript.spawnerLowerSecond = 1.0f; // Sol spawner i�in
            leftBalconySpawnerScript.spawnerUpperSecond = 3.1f;

            rightBalconySpawnerScript.spawnerLowerSecond = 1.0f; // Sa� spawner i�in
            rightBalconySpawnerScript.spawnerUpperSecond = 3.1f;

            birdspawnerScript.minSpawnTime = 1.0f;
            birdspawnerScript.maxSpawnTime = 4.5f;
            birdspawnerScript.birdSpeed = 2.5f;

            Speed = 3.1f;

            // Gelecekte instantiate edilecek objeler i�in
            SetPrefabSpeeds(Speed);

            // Sahnedeki t�m mevcut objeleri g�ncelle
            UpdateExistingObjects(Speed);
        }
    }

    // Prefab'lar�n i�indeki speed de�i�kenini ayarlamak i�in bir fonksiyon
    void SetPrefabSpeeds(float speed)
    {
        // Prefab instantiate edildi�inde objelerin speed'ini ayarl�yoruz
        WallMovementScript wallScript = WallPrefab.GetComponent<WallMovementScript>();
        WallMovementScript reverseWallScript = ReverseWallPrefab.GetComponent<WallMovementScript>();
        WallMovementScript balconyScript = BalconyPrefab.GetComponent<WallMovementScript>();
        WallMovementScript reverseBalconyScript = ReverseBalconyPrefab.GetComponent<WallMovementScript>();

        if (wallScript != null) wallScript.fallSpeed = speed;
        if (reverseWallScript != null) reverseWallScript.fallSpeed = speed;
        if (balconyScript != null) balconyScript.fallSpeed = speed;
        if (reverseBalconyScript != null) reverseBalconyScript.fallSpeed = speed;
    }

    // Sahnedeki t�m mevcut objeleri g�ncellemek i�in
    void UpdateExistingObjects(float speed)
    {
        // Sahnedeki t�m duvar ve balkon objelerini bul ve fallSpeed de�erlerini g�ncelle
        WallMovementScript[] allWallsAndBalconies = FindObjectsOfType<WallMovementScript>();

        foreach (WallMovementScript obj in allWallsAndBalconies)
        {
            obj.fallSpeed = speed;
        }
    }
}

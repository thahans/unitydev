using System.Collections;
using UnityEngine;
using TMPro;  // TextMeshPro için gerekli kütüphane

public class CountdownManager : MonoBehaviour
{
    public TextMeshProUGUI countdownText;  // Geri sayım UI öğesi
    public float countdownTime = 3f;  // 3'ten geri sayacak süre
    private bool gameStarted = false;  // Oyunun başladığını kontrol eden değişken

    void Start()
    {
        Time.timeScale = 0f;  // Oyun başlar başlamaz her şeyi durdur
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        float currentTime = countdownTime;

        // Geri sayım devam ederken
        while (currentTime > 0)
        {
            countdownText.text = currentTime.ToString("0");  // Geri sayımı güncelle
            yield return new WaitForSecondsRealtime(1f);  // Zamanı normal akışında bekle, çünkü timeScale = 0
            currentTime--;
        }

        // Sayaç bittiğinde "GO!" mesajını göster
        countdownText.text = "GO!";
        yield return new WaitForSecondsRealtime(1f);  // 1 saniye bekle

        // Sayaç bittiğinde oyunu başlat
        countdownText.gameObject.SetActive(false);
        Time.timeScale = 1f;  // Oyun akmaya başlasın
        gameStarted = true;
    }
}

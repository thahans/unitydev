using UnityEngine;

public class WallMovementScript : MonoBehaviour
{
    public float fallSpeed = 2f; // Duvar�n a�a��ya do�ru hareket h�z�
    public bool isGameStarted = false; // Oyun başladığında true olacak

    void Update()
    {
        // Duvar�n yava� yava� a�a��ya do�ru hareketi (y ekseninde)
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);

            // Sadece oyun başladıysa duvarlar ve zemin hareket etsin
        if (isGameStarted)
        {
            transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
        }
    }

    // Duvar ekran�n alt�na indi�inde yok edilsin (performans a��s�ndan �nemli)
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}

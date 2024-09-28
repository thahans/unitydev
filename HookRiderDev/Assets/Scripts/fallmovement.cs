using UnityEngine;

public class WallMovementScript : MonoBehaviour
{
    public float fallSpeed = 2f; // Duvarýn aþaðýya doðru hareket hýzý

    void Update()
    {
        // Duvarýn yavaþ yavaþ aþaðýya doðru hareketi (y ekseninde)
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
    }

    // Duvar ekranýn altýna indiðinde yok edilsin (performans açýsýndan önemli)
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookAim : MonoBehaviour
{
    public Transform triangle; // Üçgenin referansı
    public Transform centerPoint; // Yayın merkezi (karakterin kafası)
    public float radius = 1f; // Yayın yarıçapı
    public float rotationSpeed = 100f; // Üçgenin hareket hızı
    private float angle = 90f; // Üçgenin baştaki açısı (ortadan başlayacak)

    private bool movingRight = true;

    void Update()
    {
        MoveTriangle();
    }

    void MoveTriangle()
    {
        float rotationAmount = rotationSpeed * Time.deltaTime;

        if (movingRight)
        {
            angle -= rotationAmount;
            if (angle <= -90f) // Sol sona ulaştığında
            {
                angle = -90f;
                movingRight = false; // Sağ tarafa dön
            }
        }
        else
        {
            angle += rotationAmount;
            if (angle >= 90f) // Sağ sona ulaştığında
            {
                angle = 90f;
                movingRight = true; // Sol tarafa dön
            }
        }

        // Üçgenin yeni pozisyonunu hesapla (X ve Y koordinatlarını yer değiştiriyoruz)
        float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
        float y = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

        // Üçgenin pozisyonunu yay üzerinde güncelle (X ve Y koordinatlarını yer değiştiriyoruz)
        triangle.position = new Vector3(centerPoint.position.x + y, centerPoint.position.y + x, triangle.position.z);

        // Üçgenin rotasını ayarla (sivri ucun doğru noktayı gösterecek şekilde)
        triangle.up = new Vector3(y, x, 0).normalized; // Üçgenin sivri ucunu doğru noktayı gösterecek şekilde ayarla
    }
}

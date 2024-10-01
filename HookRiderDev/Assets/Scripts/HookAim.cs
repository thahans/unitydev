using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookAim : MonoBehaviour
{
    public Transform triangle; 
    public Transform centerPoint; 
    public float radius = 1f; 
    public float rotationSpeed = 100f; 
    private float angle = 90f; 
    private bool movingRight = true;
    private bool isActive = true;  // Üçgenin aktif olup olmadığını kontrol etmek için

    void Update()
    {
        if (isActive)
        {
            MoveTriangle();
        }
    }

    public void SetTriangleActive(bool active)
    {
        isActive = active;
        triangle.gameObject.SetActive(active);  // Üçgenin görünürlüğünü ayarla
    }

    void MoveTriangle()
    {
        float rotationAmount = rotationSpeed * Time.deltaTime;

        if (movingRight)
        {
            angle -= rotationAmount;
            if (angle <= -90f)
            {
                angle = -90f;
                movingRight = false;
            }
        }
        else
        {
            angle += rotationAmount;
            if (angle >= 90f)
            {
                angle = 90f;
                movingRight = true;
            }
        }

        float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
        float y = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

        triangle.position = new Vector3(centerPoint.position.x + y, centerPoint.position.y + x, triangle.position.z);
        triangle.up = new Vector3(y, x, 0).normalized;
    }
}

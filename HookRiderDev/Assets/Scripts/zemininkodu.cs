using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float lifetime = 5f; // Nesnenin ne kadar s�re sonra yok olaca��n� belirler

    void Start()
    {
        // Nesneyi belirlenen s�re sonra yok et
        Destroy(gameObject, lifetime);
    }
}

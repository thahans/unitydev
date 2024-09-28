using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float lifetime = 5f; // Nesnenin ne kadar süre sonra yok olacaðýný belirler

    void Start()
    {
        // Nesneyi belirlenen süre sonra yok et
        Destroy(gameObject, lifetime);
    }
}

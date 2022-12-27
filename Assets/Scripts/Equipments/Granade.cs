using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{

    [SerializeField] float fuseDuration;
    float explosionTimer;

    private void Start()
    {
        explosionTimer = fuseDuration + Time.time;    
    }

    private void Update()
    {
        if(explosionTimer < Time.time)
        {
            Explode();
        }
    }

    private void Explode()
    {
        Debug.Log("Boom");
        Destroy(gameObject);
    }
}

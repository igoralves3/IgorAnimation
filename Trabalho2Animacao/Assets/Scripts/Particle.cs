//using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class Particle : MonoBehaviour
{

    public Vector3 position;
    public float speedX;
    public float speedY;
    public float size;
    public SpriteRenderer spriteRenderer;
    public int restantTime;
    public bool ativa = true;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        restantTime = 120;
        ativa = true;

        speedX = Random.Range(-5f,5f);
        speedY = Random.Range(-5f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        restantTime--;
        if (restantTime <= 0)
        {
            ativa = false;
        }
        else
        {
           
            transform.position += new Vector3(speedX * Time.deltaTime, speedY * Time.deltaTime,0);
        }
    }
}

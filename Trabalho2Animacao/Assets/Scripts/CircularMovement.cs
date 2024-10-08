using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CircularMovement : MonoBehaviour
{

    public float radius = 5f; // Raio do c�rculo
    public float speed = 1f; // Velocidade do movimento
    private float angle = 0f; // �ngulo atual


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Incrementa o �ngulo com base na velocidade e no tempo
        angle += speed * Time.deltaTime;

        // Calcula a nova posi��o
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;

        // Atualiza a posi��o do objeto
        //transform.position = new Vector3(x, transform.position.y, z);
        transform.position = new Vector3(x, y, transform.position.z);

    }
}

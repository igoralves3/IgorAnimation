using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CurvedMovement : MonoBehaviour
{

    public Transform pointA; // Ponto inicial
    public Transform pointB; // Ponto final
    public Transform controlPoint; // Ponto de controle para a curva
    public float duration = 2.0f; // Duração da translação
    private float t = 0.0f; // Valor que varia de 0 a 1

    private int delayFrames = 0;

    void Start()
    {
       
        delayFrames = 0;
        

    }

    // Update is called once per frame
    void Update()
    {

        if (t < 1.0f)
        {
            delayFrames++;
            if (delayFrames >= 10) {
                delayFrames = 0;

                t += Time.deltaTime / duration;
                Vector3 position = CalculateBezierPoint(t, pointA.position, controlPoint.position, pointB.position);
                transform.position = position;
            }
        }

    }

    // Função para calcular um ponto na curva de Bézier
    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        return Mathf.Pow(1 - t, 2) * p0 +
               2 * (1 - t) * t * p1 +
               Mathf.Pow(t, 2) * p2;
    }

}

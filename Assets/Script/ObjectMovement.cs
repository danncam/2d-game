using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
 * 
 * v1.0
 * Este script serve para modificar a posição de um objeto de cena (Plataforma, Item, Projétil, Inimigo...) 
 * Sem a necessidade de adicionar um RigidBody2D ao GameObject
 * As variáveis moveSpeed e magnitude movem o Object horizontalmente por uma velocidade até certa magnitude, e volta.
 * 
 * v1.1
 * Adicionada a possibilidade de usar coordenadas polares, em que dado um ângulo, o objeto move-se através dele
 * 
 * v1.2
 * a opção goAhead move infinitamente um objeto
 */

public class ObjectMovement : MonoBehaviour
{
    public float moveSpeed, magnitude; // relativa a posição do gameObject
    public bool goAhead = false;
    public bool usePolarCoordinates;
    
    bool Switch = true;

    [Range(0, 360)]
    [SerializeField]
    private int angle = 90;

    // Update is called once per frame
    void Update()
    {

        if (usePolarCoordinates)
        {

            Vector3 moveDelta = VectorFromAngle(angle, magnitude);

            if (transform.position.x > magnitude)
                Switch = false;
            if (transform.position.x < -magnitude)
                Switch = true;
            if (transform.position.y > magnitude)
                Switch = false;
            if (transform.position.y < -magnitude)
                Switch = true;

            if (Switch)
                transform.Translate(moveDelta * moveSpeed * Time.deltaTime, 0);
            else
                transform.Translate(-moveDelta * moveSpeed * Time.deltaTime, 0);

        }
        else  {
            if (goAhead)
            {
                transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
            } else
            {
                if (transform.position.x > magnitude)
                    Switch = false;
                if (transform.position.x < -magnitude)
                    Switch = true;

                if (Switch)
                    transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
                else
                    transform.position = new Vector2(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y);

            }

        }


    }

    // este método retorna um Vector3 à partir de um ângulo (em graus) e uma magnitude
    Vector3 VectorFromAngle(float theta, float magnitude)
    {
        float newx = Mathf.Cos(Mathf.Deg2Rad * theta) * magnitude;
        float newy = Mathf.Sin(Mathf.Deg2Rad * theta) * magnitude;
        return new Vector3(newx, newy);
    }


}
    
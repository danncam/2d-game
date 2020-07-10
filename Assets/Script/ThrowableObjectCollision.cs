using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThrowableObjectCollision : MonoBehaviour
{
    /**
     * 
     * Este scripe trata da colisão dos objetos lançados pelo Player
     * Os objetos lançados devem colidir com um inimigo
     * e a pontuação do Score deve aumentar
     * 
     * v1.0
     * Este script é adicionado aos Prefabs que são lançados pelo Player 
     * Estes objetos são instanciados em PlayerObjectLauncher.cs e
     * lançados na direção em que o Player está olhando
     * 
     *
     * v1.2
     * A váriavel m_FacingRight presente em CharacterCollision2D nem sempre está atualizada a cada frame
     * fazendo com que o objeto lançado se choque com o próprio Player
     * A correção foi criar uma condição em que se houver colisão com o player, 
     * este objeto troque sua direção
     * 
     * Criado um evento SumScore para enviar a informação ao Score
     * 
     */

    private ObjectMovement movement;

    // evento SumScore enviará a informação de colisão para o Score
    public static event Action<int> SumScore = delegate { };

    private void Awake()
    {
        movement = this.GetComponentInParent<ObjectMovement>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // se colide com um inimigo...
        if (collision.collider.CompareTag("WalkingEnemy") ||
            collision.collider.CompareTag("FallingEnemy"))
            {
            Destroy(collision.gameObject); // destrói o objeto que colidiu
            Destroy(gameObject); // destrói-se

            // envia a informação ao score
            SumScore(10);


        }

        // se o objeto lançado colidir com o próprio player
        // troca a direção
        if (collision.collider.CompareTag("Player"))
        {
            movement.moveSpeed *= -1;
        }
    }

}

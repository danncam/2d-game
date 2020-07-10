using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * v1.1
 * Este script torna possível ao Player lançar objetos em direção aos inimigos
 * O script recebe um GameObject (Prefab) e um valor para o ritmo de lançamento
 * O GameObject deve ter adicionado o script ObjectMovement.c
 * 
 * v1.2
 * Recebe a informação da direção atual do Player (direita ou esquerda) através do evento Fire definido em CharacterController2D
 * 
 */


public class PlayerObjectLauncher : MonoBehaviour
{
    [Header("Throwable Items")]
    public GameObject fruit;
    public float fireRate = 0.5f;

    private ObjectMovement movement;
    private bool launchProjectile = false;
    private float nextFire = 0f;

    private void Start()
    {
        // O objeto lançado recebe as informações de movimento através do script configurado em seu Prefab
        movement = fruit.GetComponent<ObjectMovement>();

        // Inscrição do método FacingRight no evento Fire em CharacterController2D
        CharacterController2D.Fire += FacingRight;
    }


    // Recebe a direção atual do Player e chama o método Launch
    private void FacingRight(bool direction)
    {
        Launch(direction);
    }
    private void Update()
    {
        
        // Se pressionar o botão..
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            // deve lançar
            launchProjectile = true;
            nextFire = Time.time + fireRate;
        }
        else
        {
            launchProjectile = false;
        }
    }

    /* Instancia novos itens lançaveis de acordo com a direção do Player */
    public void Launch(bool launchDirection)
    {
        // se deve lançar
        if (launchProjectile)
        {
            // verifica a direção
            if (launchDirection)
            {
                // instancia o objeto à direita do Player
                var apple = Instantiate(fruit, new Vector2(transform.position.x + 1.5f, transform.position.y), transform.rotation);
                movement.moveSpeed = 10; // o objeto move-se para a direita
                Destroy(apple, 5); // destrói o objeto depois de 5 segundos
            }
            else
            {
                // instancia o objeto à esquerda do Player
                var apple = Instantiate(fruit, new Vector2(transform.position.x - 1.5f, transform.position.y), transform.rotation);
                movement.moveSpeed = -10; // o objeto move-se para a esquerda
                Destroy(apple, 5); // destrói o objeto depois de 5 segundos
            }
            // a variável apple não existe aqui
        }

    }
}

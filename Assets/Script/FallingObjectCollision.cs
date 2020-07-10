using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * 
 * O script FallingObject.cs refere-se a todos os objetos que partem de Flying Skull
 * Estes objetos são instanciados em OffspringGenerator.cs a partir de Prefabs temporários (itens e inimigos caindo pelo mapa)
 */
public class FallingObjectCollision : MonoBehaviour
{
    GameObject desappearing;
    
    private void Start()
    {

        // FallingObject.cs é um script carregado por um Prefab.
        // Estes objetos são criados em Runtime e precisam ser inicializados diretamente pelo código, e não no Inspector
        desappearing = Resources.Load("Desappearing") as GameObject;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // esta condição serve para que quando este objeto tocar em Bottom seja destruído
        if (collision.collider.CompareTag("Bottom"))
        {
            Destroy(gameObject);
        }

        // esta condição verifica a colisão entre os objetos de cena
        if (
            // diferentemente dos objetos de tag Item e Falling Enemy
            // um inimigo com a tag Walking Enemy não é destruído quando encosta nas plataformas
            collision.collider.CompareTag("Platform") && !this.CompareTag("WalkingEnemy") ||

            // esta condição impede que os objetos se acumulem em cima um do outro
            // destruindo Item e Falling Enemy se cairem em cima de um Walking Enemy
            collision.collider.CompareTag("WalkingEnemy") && (this.CompareTag("Item") || this.CompareTag("FallingEnemy"))

            ) {
                Instantiate(desappearing, transform.position, transform.rotation); // instancia um efeito de unspawn
                Destroy(gameObject); // destrói este objeto
        }
    }
}

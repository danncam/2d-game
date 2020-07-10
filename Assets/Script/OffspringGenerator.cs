using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffspringGenerator : MonoBehaviour
{
    [Header("Items")]
    public GameObject fruit;

    [Header("Enemies")]
    public GameObject bee;
    public GameObject shoom;
    public GameObject trunk;
    [Space(10)]
    public bool rain = true; 
    public float waitTime = 0.5f;
    public float spawnTime = 1f;


    // Update is called once per frame
    void Start()
    {
        InvokeRepeating("SpawnObject", spawnTime, waitTime); // chama o metodo repetidas vezes
    }

    private void SpawnObject()
    {

        // lógica simples: se o número gerado aleatoriamente for par, instancia um Item, se for impar, instancia um inimigo
        if(Random.Range(0, 2) % 2 == 0)
            Instantiate(fruit, transform.position, transform.rotation); // instancia novos objetos
        else
        {
            // o caso de instanciar um inimigo é definido pelo aleatório num range de 5. 
            // Deste modo, o inimigo definido como default ganha maior probabilidade de ser gerado
            int rand = Random.Range(0, 5) % 5;
            switch (rand) {
                case 1:
                    Instantiate(shoom, transform.position, transform.rotation);
                    break;
                case 2:
                    Instantiate(trunk, transform.position, transform.rotation);
                    break;
                    // ganha maior probabilidade de ser instanciado
                default:
                    Instantiate(bee, transform.position, transform.rotation);
                    break;
            }
                
            
        }
            


        if (!rain) // habilitavel pelo usuario
        {
            CancelInvoke("SpawnObject"); // cancela a repetição
        }
    }

}

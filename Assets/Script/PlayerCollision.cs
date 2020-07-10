using UnityEngine;
using UnityEngine.Events;

/**
 * v1.0
 * Este script verifica a colisão entre o Player e outros objetos de cena, como itens e inimigos
 * As checagens de colisão são feitas pela Tag do GameObject definida em Inspector
 * Após a colisão, modifica o Score
 * 
 * v1.2
 * Adicionado um evento HitEnemy para tratar da animação de quando o player colide com um inimigo.
 * Esta animação é reproduzida somente uma vez à partir de qualquer outro estado quando o trigger hit é acionado.
 * 
 * v1.3 final
 * Adicionada a condição de término do jogo pela colisão entre o Player e o Checkpoint
  */

public class PlayerCollision : MonoBehaviour
{
    // inicializados pelo Inspector
    public Transform startPosition;
    public GameObject appearing;
    public GameObject checkpoint;
    private GameObject enemy;

    private Score score;

    [Header("Player Movement: Hit Enemy")]
    [Space]

    public UnityEvent HitEvent;

    private void Awake()
    {
        // Score é componente de Player
        score = GetComponentInParent<Score>();

        // Instancia o evento
        if (HitEvent == null)
            HitEvent = new UnityEvent();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.collider.name);
        // condição em que Bottom entra na área de colisão
        if(collision.collider.CompareTag("Bottom")) {
            transform.position = startPosition.position; // transforma a posição dele em startPosition
            Instantiate(appearing, startPosition.position, startPosition.rotation); // instancia um efeito de spawn que é destruído pelo script DestroyOnExit
            score.setText(-1); // altera o score
        }

        // estas condições tratam da colisão com o player
        // fazem a verificação se este objeto é inimigo ou item através da tag em Inspector
        // após a colisão, a variável de Score é alterada para positivo ou negativo
        if (collision.collider.CompareTag("Item"))
        {
            Destroy(collision.gameObject); // destrói o objeto que colidiu com player
            score.setText(10); // +10 no score
        }

        // se o player encostou no inimigo
        // existem dois tipos de inimigos: os que caem na plataforma e os que passam direto
        if (collision.collider.CompareTag("FallingEnemy") || collision.collider.CompareTag("WalkingEnemy"))
        {
            Destroy(collision.gameObject); // destrói o objeto que colidiu com player

            HitEvent.Invoke(); // chama a função para modificar a animação

            score.setText(-10); // -10 no score
        }

        // esta condição mantém o player em cima da plataforma
        // configurando a posição atual dele relativa a da plataforma
        if (collision.collider.CompareTag("Platform"))
        {
            this.transform.parent = collision.transform;
        }

        // colisão entre o Player e o Checkpoint
        if (collision.collider.CompareTag("Checkpoint"))
        {
            // desabilita o inimigo no céu
            enemy = GameObject.FindGameObjectWithTag("Enemy");
            Destroy(enemy);

            // desabilita o movimento do Player
            this.GetComponentInParent<CharacterController2D>().enabled = false;
            this.GetComponentInParent<PlayerMovement>().enabled = false;

            // reproduz a animação da Flag
            checkpoint.GetComponentInParent<Animator>().SetTrigger("reach");

            

        }

        // o código abaixo funciona se o script for adicionado em Bottom
        /* condição em que Player entra na área de colisão. public Transform transform_out; é necessário
        if (collision.collider.CompareTag("Player"))
        {
            transform_out.transform.position = startPosition.position; // transforma a posição dele em startPosition
            Debug.Log(collision.transform);
        }

        foreach(Transform t in transform) {
            Debug.Log(t.name);
        }*/

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform"))
        {
            this.transform.parent = null;
        }
    }

}

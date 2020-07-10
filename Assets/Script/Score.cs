using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System; // necessário para o evento delegate em ThrowableObjectCollision

public class Score : MonoBehaviour
{

    /**
     * Este script trata apenas da alteração do Score presente no Canvas
     * 
     * v1.0 
     * método públic setText recebe a informação do Player quando ele colide com algum objeto de cena
     * Este script é um componente do Player e é chamado pelo PlayerCollision
     * 
     * v1.2
     * Recebe a informação do evento SumScore presente em ThrowableObjectCollision
     * E altera o Score
     * 
         */

    public Text textScore;

    private void Start()
    {
        // inscrição do método setText em ThrowableObjectCollision
        // quando ocorre uma colisão entre o objeto lançado pelo Player e um inimigo
        // o score é alterado pelo evento estático sem que este Score tenha que ser chamado na própria classe ThrowableObjectCollision
        ThrowableObjectCollision.SumScore += setText;

    }

    // Recebe um valor positivo ou negativo
    // Converte de String para Inteiro e soma o valor
    // Converte o resultado para String e armazena na variável do Score
    public void setText(int v)
    {
        Int32.TryParse(textScore.text, out int numValue);
        textScore.text = (numValue + v).ToString();
    }
}

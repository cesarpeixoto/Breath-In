/* ========================================================================================================================
Classe responsável pela interatividade Arrastar / Puxar.

Ao receber uma colisão, o script checa se veio do Player. Caso positivo, estabelece a função SetInteractive no callback do
método de interatividade do Player (Implementado na classe base). A colisão ocorre sempre antes da tecla ser pressionada, 
garantindo a integridade do método. Uma vez acionada, o script altera o estado do Player para DragMovimentState e estabelece 
as referências necessárias para que a ação ocorra no objeto atachado. Também é responsável pelo monitoramento da tecla, 
uma vez que esta for solta, todas as referências são liberadas, e o estado do Player retorna para default.
O scrit também garante o componente de física necessário, bem como sua configuração.
//-------------------------------------------------------------------------------------------------------------------------

26/09/2016 - Otimização.
Para otimização, a checagem da tecla pressionada saiu do método update, para uma Coroutine, de forma que a execução desta
checagem, agora só occore quando houver interatividade, liberando processamento quando o objeto estiver ocioso.

//-------------------------------------------------------------------------------------------------------------------------
Futuras implmentações:
Podemos definir aqui, a velocidade em que o objeto será movido, setando a variável na maquina de estados.
Podemos definir outras configurações em Rigidbody, caso houver necessidade.
//-------------------------------------------------------------------------------------------------------------------------

$Creator: Cesar Peixoto $
$Notice: (C) Copyright 2016 by Cesar Peixoto. All Rights Reserved. $     Inicializado: 23/09/2016 - Finalizado em 26/09/2016
=========================================================================================================================== */


using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Rigidbody))]
public class DragInteractiveBehaviour : InteractiveBehaviour
{    
    public bool activated = false;                  // Auxilio para debug.

    // Referência dos componentes externos.
    private Transform _transform = null;
    private Rigidbody _rigidbody = null;

    //---------------------------------------------------------------------------------------------------------------
    // Inicializa as referências.
    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    //---------------------------------------------------------------------------------------------------------------
    // Função para o callback, vinculando o objeto ao PLayer e preparando o objeto para ser arrastado ou puxado.
    public override void SetInteractive(/*EK.StateController controller*/)
    {
        _rigidbody.isKinematic = false;
        _controller.currentState = _controller.dragMovimentState;                    // Atualiza maquina de estados.
        _controller.dragMovimentState.dragObject = _rigidbody;                       // Passa referência deste objeto para ser movido.
        activated = true;
        _controller.Interaction = null;
        StartCoroutine(WaitingKeyUp());
        _controller.getAnimator().SetBool("OnDrag", true);
    }

    //---------------------------------------------------------------------------------------------------------------
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _controller = collision.gameObject.GetComponent<EK.StateController>();  // Estabelece referência do controlador de estados.
            _controller.Interaction = SetInteractive;                               // Estabelece o callback para esta interação.
        }
    }

    //---------------------------------------------------------------------------------------------------------------
    // Encerra a interatividade, desvinculando o Player deste objeto.
    private void UnLinkPlayer()
    {
        if(_controller != null)
        {
            _controller.Interaction = null;                                         // Estabelece o callback como null.
            _controller.currentState = _controller.defaultMovimentState;            // Estabelece o estado para default.
            _controller.dragMovimentState.dragObject = null;                        // Estabelece a referência do objeto para null.
            _controller.getAnimator().SetBool("OnDrag", false);                     // TALVEZ ISSO DEVA SAIR DAQUI E PASSAR PARA STATUS.
            _controller = null;                                                     // Estabelece a referência do controlador de estados para null.
            _rigidbody.isKinematic = true;                                          // Estabelece o objeto como Kinematic.
            activated = false;                                                      // Para debug.
        }
    }

    //---------------------------------------------------------------------------------------------------------------
    // Executa a função de desvinculo, no momento em que a tecla deixar de ser pressionada.
    IEnumerator WaitingKeyUp()
    {
        yield return new WaitUntil(() => Input.GetButtonUp("Fire2"));                // Aguarda até a tecla deixar de ser pressioanda.
        UnLinkPlayer();                                                             // Executa função de desvinculo.
    }

    //---------------------------------------------------------------------------------------------------------------
}

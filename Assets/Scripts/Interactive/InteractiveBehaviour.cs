using UnityEngine;
using System.Collections;

public abstract class InteractiveBehaviour : MonoBehaviour
{
    //referência do controlador de estados.
    protected EK.StateController _controller = null;

    public abstract void SetInteractive(EK.StateController controller);

    //---------------------------------------------------------------------------------------------------------------
    // Se houver colisão com o Player, ajusta o callback para esta interatividade.
    protected virtual void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _controller = collision.gameObject.GetComponent<EK.StateController>();  // Estabelece referência do controlador de estados.
            _controller.Interaction = SetInteractive;                               // Estabelece o callback para esta interação.
        }
    }

    // Use this for initialization
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }
}

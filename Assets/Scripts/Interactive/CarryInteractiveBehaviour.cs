using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Rigidbody))]
public class CarryInteractiveBehaviour : InteractiveBehaviour
{
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
    }

    //---------------------------------------------------------------------------------------------------------------
    public override void SetInteractive(/*EK.StateController controller*/)
    {
        if(_controller.carryingObject != null)
        {
            _controller.carryingObject.transform.SetParent(null);
            _controller.carryingObject.transform.position = this.transform.position;
            _controller.carryingObject.SetActive(true);
        }
        _controller.carryingObject = this.gameObject;
        //_controller.carryingObject.transform.SetParent(_transform);
        _controller.carryingObject.SetActive(false);
        // _controller.Interaction = null;
        _controller.LightAct(1f);
        _controller.Interaction = null;
    }

    //---------------------------------------------------------------------------------------------------------------
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EK.StateController controller = collision.gameObject.GetComponent<EK.StateController>();
            controller.Interaction = null;
            //controller.carryingObject = null;
            if (_controller != null)
                _controller = null;
        }
    }

    //---------------------------------------------------------------------------------------------------------------
    protected override void OnDestroy()
    {
        if (_controller != null)
        {
            _controller.carryingObject = null;
            _controller.LightAct(0f);
        }
            
        base.OnDestroy();
    }
    //---------------------------------------------------------------------------------------------------------------
}

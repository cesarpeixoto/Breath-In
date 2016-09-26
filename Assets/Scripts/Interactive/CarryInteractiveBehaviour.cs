using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Rigidbody))]
public class CarryInteractiveBehaviour : InteractiveBehaviour
{
    // Referência dos componentes externos.
    private Transform _transform = null;
    private Rigidbody _rigidbody = null;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
    }

    public override void SetInteractive(EK.StateController controller)
    {
        if(_controller.carryingObject != null)
        {
            _controller.carryingObject.transform.SetParent(null);
            _controller.carryingObject.transform.position = this.transform.position;
            _controller.carryingObject.SetActive(true);
        }
        _controller.carryingObject = this.gameObject;
        _controller.carryingObject.transform.SetParent(_transform);
        _controller.carryingObject.SetActive(false);
        _controller.Interaction = null;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(_controller != null)
            {
                _controller.Interaction = null;
                _controller = null;
            }
            
        }
    }

}

using UnityEngine;
using System.Collections;
using System;

public class DragInteractiveBehaviour : InteractiveBehaviour
{
    private EK.StateController _controller = null;
    private Vector3 direction = Vector3.zero;

    public override void SetInteractive(EK.StateController controller)
    {
        //transform.SetParent(controller.transform);
        // Aqui muda a maquina de estados.
        //_controller = controller;
        GetComponent<Rigidbody>().isKinematic = false;
        controller.currentState = controller.dragMovimentState;
        controller.dragMovimentState.dragObject = GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            _controller = collision.gameObject.GetComponent<EK.StateController>();
            _controller.Interaction = SetInteractive;
        }
    }

    //private void OnCollisionExit(Collision collision)
    //{
    //    //UnLinkPlayer();
    //}

    protected override void Update()
    {
        base.Update();
        if(!Input.GetButton("Fire2") && _controller != null)
        {
            UnLinkPlayer();
        }
    }

    private void UnLinkPlayer()
    {
        if(_controller != null)
        {
            _controller.Interaction = null;
            transform.SetParent(null);
            _controller.currentState = _controller.defaultMovimentState;
            _controller.dragMovimentState.dragObject = null;
            _controller = null;
            GetComponent<Rigidbody>().isKinematic = true;            
        }
    }


}

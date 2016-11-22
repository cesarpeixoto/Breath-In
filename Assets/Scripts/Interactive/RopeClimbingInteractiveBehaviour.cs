using UnityEngine;
using System.Collections;
using EK;
using System;

public class RopeClimbingInteractiveBehaviour : InteractiveBehaviour
{
    public static bool active = false;
    public SphereCollider ropectrl = null;
    public float xPosition = 0;
    bool moving = false;

    protected override void Start()
    {
        base.Start();
        xPosition = this.transform.localPosition.x;
    }


    public override void SetInteractive(/*StateController controller*/)
    {
        Debug.Log(_controller);
        DeativeCapsuleCollider(_controller.GetComponent<CapsuleCollider>());
        _controller.transform.SetParent(this.transform);
        _controller.GetComponent<Rigidbody>().isKinematic = true;        
        active = true;
        _controller.getAnimator().SetBool("OnClimbRope", true);
        config(_controller);
        _controller.transform.localPosition = Vector3.zero;
        _controller.transform.Translate(0, 1f, 0, Space.Self);

        _controller.currentState = _controller.climbRopeMovimentState;
        _controller.Interaction = _controller.climbRopeMovimentState.OnActionController;
        _controller.climbRopeMovimentState.ropeNode = this.GetComponent<Rigidbody>();

        Debug.Log("Entrou");

        /*
        tratar pegando o bounds do colisore ajustando os offsets para ficar legal...

        */
        // rope descer para y = -1.738
        // meshe -1.748
        // capsule center y = -0.83

    }

    void config(StateController controller)
    {
        controller.transform.FindChild("Mesh").localPosition = new Vector3(controller.transform.FindChild("Mesh").localPosition.x, -1.748f, controller.transform.FindChild("Mesh").localPosition.z);
        controller.GetComponent<CapsuleCollider>().center = new Vector3(controller.GetComponent<CapsuleCollider>().center.x, -0.83f, controller.GetComponent<CapsuleCollider>().center.z);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GameController") && active)
            other.transform.parent.GetComponent<StateController>().currentState.OnTriggerEnter(this.GetComponent<SphereCollider>());        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("GameController") && !active)
            other.transform.parent.GetComponent<StateController>().Interaction = null;
    }

    public void DeativeCapsuleCollider(Collider collider, bool ignore = true)
    {
        //StateController controller = _controller;
        BoxCollider[] boxColliders = transform.root.GetComponentsInChildren<BoxCollider>();
        foreach (var item in boxColliders)
        {
            Physics.IgnoreCollision(collider, item, ignore);
        }
    }


}

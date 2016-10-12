using UnityEngine;
using System.Collections;
using EK;
using System;

public class RopeClimbingInteractiveBehaviour : InteractiveBehaviour
{

    //Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());

   
    public static bool active = false;
    public SphereCollider ropectrl = null;

    bool moving = false;

    public override void SetInteractive(StateController controller)
    {
        DeativeCapsuleCollider();
        _controller.transform.SetParent(this.transform);
        _controller.GetComponent<Rigidbody>().isKinematic = true;        
        active = true;
        _controller.getAnimator().SetBool("OnClimbRope", true);
        //Physics.IgnoreCollision(_controller.GetComponent<CapsuleCollider>(), GetComponent<BoxCollider>());
        //_controller.gameObject.layer = 8;

        config(controller);
        _controller.transform.localPosition = Vector3.zero;
        _controller.transform.Translate(0, 1f, 0, Space.Self);

        _controller.currentState = _controller.climbRopeMovimentState;

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
        //controller.transform.FindChild("Rope").localPosition = new Vector3(controller.transform.FindChild("Rope").localPosition.x, -1.738f, controller.transform.FindChild("Rope").localPosition.z);
        controller.transform.FindChild("Mesh").localPosition = new Vector3(controller.transform.FindChild("Mesh").localPosition.x, -1.748f, controller.transform.FindChild("Mesh").localPosition.z);
        controller.GetComponent<CapsuleCollider>().center = new Vector3(controller.GetComponent<CapsuleCollider>().center.x, -0.83f, controller.GetComponent<CapsuleCollider>().center.z);

    }


    Vector3 onNodePosition = Vector3.zero;
    protected override void Update()
    {
        //moving = false;
        //if (_controller != null && active)
        //{
        //    float v = Input.GetAxis("Vertical") * Time.deltaTime * 0.3f;
        //    if(v != 0)
        //    {
        //        //Vector3 direction = (transform.localPosition - _controller.transform.localPosition).normalized * Time.deltaTime * 0.3f *-1;
        //        //Vector3 direction = transform.localPosition.normalized * Time.deltaTime * 0.3f * -1;
        //        //onNodePosition = _controller.transform.TransformPoint(Vector3.zero);
        //        _controller.transform.Translate(0, v, 0, Space.Self);
                
        //        //onNodePosition.y = _controller.transform.position.y;
        //        float temp = _controller.transform.position.y;
        //        _controller.transform.localPosition = Vector3.zero;
        //        //float temp = v * Time.deltaTime;// * 0.3f;
        //        //Vector3 pos = new Vector3(transform.position.x, _controller.transform.position.y + temp, transform.position.z);
        //        Vector3 pos = new Vector3(_controller.transform.position.x, temp, _controller.transform.position.z);

        //        //_controller.transform.position = onNodePosition;// pos;
        //        _controller.transform.position = pos;
        //    }

        //    //_controller.transform.localPosition = new Vector3(transform.localPosition.x, _controller.transform.localPosition.y +0.1f, transform.localPosition.z);

        //    //_controller.transform.localPosition = new Vector3(0, _controller.transform.localPosition.y, 0);
        //    moving = true;

        //    float h = Input.GetAxis("Horizontal");
        //    if (h != 0)
        //        GetComponent<Rigidbody>().AddForce(new Vector3(-h, 0, 0));

        //}
    }

    //protected override void OnCollisionStay(Collision collision)
    //{
    //    base.OnCollisionStay(collision);
    //    if (collision.gameObject.CompareTag("Player") && active)
    //    {
    //        _controller.transform.SetParent(this.transform);
    //        Physics.IgnoreCollision(_controller.GetComponent<CapsuleCollider>(), GetComponent<BoxCollider>());
    //    }
    //}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GameController") && active)
            other.transform.parent.GetComponent<StateController>().currentState.OnTriggerEnter(this.GetComponent<SphereCollider>());        
    }

    void DeativeCapsuleCollider()
    {
        BoxCollider[] boxColliders = transform.root.GetComponentsInChildren<BoxCollider>();
        foreach (var item in boxColliders)
        {
            Physics.IgnoreCollision(_controller.GetComponent<CapsuleCollider>(), item);
        }
    }



    //protected void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("GameController") && active)
    //        _controller.transform.SetParent(this.transform);
    //}



}

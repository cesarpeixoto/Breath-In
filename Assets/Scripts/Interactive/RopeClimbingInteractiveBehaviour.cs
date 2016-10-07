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
        _controller.getAnimator().SetTrigger("New Trigger");
        //Physics.IgnoreCollision(_controller.GetComponent<CapsuleCollider>(), GetComponent<BoxCollider>());
        //_controller.gameObject.layer = 8;
        _controller.transform.localPosition = Vector3.zero;
        _controller.transform.Translate(0, 0.1f, 0, Space.Self);

        Debug.Log("Entrou");
    }

    protected override void Update()
    {
        moving = false;
        if (_controller != null && active)
        {
            float v = Input.GetAxis("Vertical") * Time.deltaTime * 0.3f;
            _controller.transform.Translate(0, v, 0, Space.Self);
            moving = true;

        }
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
        {
            //if(other.transform.parent.GetComponent<Rigidbody>().velocity.y != 0)
            //{
                other.transform.parent.SetParent(this.transform);
            other.transform.parent.localPosition = new Vector3(0, 0.25f, 0);//Vector3.zero;

                Debug.Log("Colidiu no " + gameObject.name);
            //}
                
        }

        
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

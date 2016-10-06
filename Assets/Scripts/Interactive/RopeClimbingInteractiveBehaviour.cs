using UnityEngine;
using System.Collections;
using EK;
using System;

public class RopeClimbingInteractiveBehaviour : InteractiveBehaviour
{

    //Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());

   
    public static bool active = false;
    public SphereCollider ropectrl = null;

    public override void SetInteractive(StateController controller)
    {
        _controller.transform.SetParent(this.transform);
        _controller.GetComponent<Rigidbody>().isKinematic = true;        
        active = true;
        Physics.IgnoreCollision(_controller.GetComponent<CapsuleCollider>(), GetComponent<BoxCollider>());
        //_controller.gameObject.layer = 8;
        _controller.transform.localPosition = Vector3.zero;
        _controller.transform.Translate(0, 0.1f, 0, Space.Self);

        Debug.Log("Entrou");
    }

    protected override void Update()
    {
        if(_controller != null && active)
        {
            float v = Input.GetAxis("Vertical") * Time.deltaTime * 0.3f;
            _controller.transform.Translate(0, v, 0, Space.Self);
        }
    }

    protected override void OnCollisionStay(Collision collision)
    {
        base.OnCollisionStay(collision);
        if (collision.gameObject.CompareTag("Player") && active)
        {
            _controller.transform.SetParent(this.transform);
            Physics.IgnoreCollision(_controller.GetComponent<CapsuleCollider>(), GetComponent<BoxCollider>());
        }            
    }


    //protected void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player") && active)
    //        Physics.IgnoreCollision(_controller.GetComponent<CapsuleCollider>(), GetComponent<BoxCollider>());
    //}



}

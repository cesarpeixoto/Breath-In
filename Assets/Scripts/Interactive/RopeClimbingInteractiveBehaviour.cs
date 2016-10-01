using UnityEngine;
using System.Collections;
using EK;
using System;

public class RopeClimbingInteractiveBehaviour : InteractiveBehaviour
{
    public override void SetInteractive(StateController controller)
    {
        _controller.transform.SetParent(this.transform);
        _controller.GetComponent<Rigidbody>().isKinematic = true;
        _controller.transform.Translate(0, 0.1f, 0);

        Debug.Log("Entrou");
    }

    protected override void Update()
    {
        if(_controller != null)
        {
            float v = Input.GetAxis("Vertical") * Time.deltaTime * 0.3f;

            _controller.transform.Translate(0, v, 0);

        }
    }



}

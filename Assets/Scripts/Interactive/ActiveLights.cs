using UnityEngine;
using System.Collections;

public class ActiveLights : InteractiveBehaviour
{
    public bool Active = false;
    public bool isenergy = true;

    public GameObject light1;
    public GameObject light2;

    public override void SetInteractive()
    {
        // Testar se o personagem tem uma bateria!!!
        if(_controller.carryingObject != null)
            StartCoroutine(_controller.EnergyClock(false));
    }

    void OnCollisionExit(Collision collision)
    {
        if (_controller != null)
        {
            _controller.Interaction = null;
            _controller = null;
        }
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (_controller != null)
        {
            float param = _controller.getAnimator().GetFloat("InteractCurve"); //_animator.GetFloat("InteractCurve")
            if (param == 1f)
                Active = true;
        }


        if (Active)
        {
            light1.SetActive(true);
            light2.SetActive(true);
            Destroy(_controller.carryingObject);
            _controller.carryingObject = null;
            _controller.LightAct(0f);
            _controller.Interaction = null;
            _controller = null;
            Active = false;
        }
    }
}

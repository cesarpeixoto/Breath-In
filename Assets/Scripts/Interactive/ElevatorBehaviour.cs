using UnityEngine;
using System.Collections;

public class ElevatorBehaviour : InteractiveBehaviour
{

    public bool Active = false;
    public Vector3 endposition = Vector3.zero;
    Vector3 origin = Vector3.zero;
    public float animTime = 2f;
    float elapsedTime = 0f;

    protected override void Start()
    {
        base.Start();
        origin = transform.localPosition;
    }


    public override void SetInteractive()
    {
        // Testar se o personagem tem uma bateria!!!

        StartCoroutine(_controller.EnergyClock(false));
    }

    void OnCollisionExit(Collision collision)
    {
        //if (_controller != null && Active == false)
        //{
        //    _controller.Interaction = null;
        //    _controller = null;
        //}
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if(_controller != null)
        {
            float param = _controller.getAnimator().GetFloat("InteractCurve"); //_animator.GetFloat("InteractCurve")
            if (param == 1f)
                Active = true;
        }
        

        if (Active)
        {
            _controller.ekState = EK.EKSubState.Dead;
            _controller.transform.SetParent(transform);
            elapsedTime += Time.deltaTime;

            Vector3 temp = Vector3.Lerp(origin, endposition, elapsedTime / animTime);
            transform.localPosition = temp;

            if(elapsedTime >= animTime)
            {
                elapsedTime = animTime;
                _controller.ekState = EK.EKSubState.Idle;
                Active = false;
                _controller.transform.SetParent(null);
                _controller.Interaction = null;
                _controller = null;
            }


        }
    }
}

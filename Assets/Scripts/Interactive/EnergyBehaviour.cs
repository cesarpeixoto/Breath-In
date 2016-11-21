using UnityEngine;
using System.Collections;
using System;

public class EnergyBehaviour : InteractiveBehaviour
{
    public override void SetInteractive()
    {
        // Testar se o personagem tem uma bateria!!!

        StartCoroutine(_controller.EnergyClock());        
    }

    void OnCollisionExit(Collision collision)
    {
        if (_controller != null)
        {
            _controller.Interaction = null;
            _controller = null;
        }
    }


}

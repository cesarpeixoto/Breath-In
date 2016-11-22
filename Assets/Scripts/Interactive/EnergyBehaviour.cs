using UnityEngine;
using System.Collections;
using System;

public class EnergyBehaviour : InteractiveBehaviour
{
    public bool isenergy = true;
    public override void SetInteractive()
    {
        // Testar se o personagem tem uma bateria!!!

        StartCoroutine(_controller.EnergyClock(isenergy));        
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

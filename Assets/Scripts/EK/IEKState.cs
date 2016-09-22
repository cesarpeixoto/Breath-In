using UnityEngine;
using System.Collections;


namespace EK
{    
    public interface IEKState
    {
        void OnJumpController();
        void OnActionController();
        void OnMovimentController(Vector3 direction);
        void OnCrouchingController();
        void Update();
        void FixedUpdate();
        void OnCollisionEnter(Collision collision);
        void OnTriggerEnter(Collider collider);
    }
}

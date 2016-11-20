using UnityEngine;
using System.Collections;
using System;

namespace EK
{

    public class DragMovimentState : IEKState
    {
        // Referência do controlador de estados
        private StateController _stateController = null;
        public Rigidbody dragObject = null;
        private CardinalDirection _forwardDirection;
        private float speed = 2f;

        // Referência dos componentes externos.
        private Transform _transform;
        private Rigidbody _rigidbody;
        private Animator _animator;

        public DragMovimentState(StateController stateController)
        {
            _stateController = stateController;
            _transform = _stateController.getTransform();
            _rigidbody = _stateController.getRigidbody();
            _animator = _stateController.getAnimator();
        }


        public void OnMovimentController(Vector3 direction)
        {
            _forwardDirection = _stateController.cardinalState;
            bool moveCondition = _stateController.defaultMovimentState.GetCardinalDirection(direction) == _forwardDirection || isOpposedDirection(direction);
            if (_stateController.isGrounded && moveCondition)
            {
                direction = direction.normalized;
                direction *= speed;
                _rigidbody.velocity = direction;
                dragObject.velocity = direction;
            }

        }


        private bool isOpposedDirection(Vector3 direction)
        {
            CardinalDirection cardialDirection = _stateController.defaultMovimentState.GetCardinalDirection(direction); 
            if ((int)cardialDirection % 2 == 0)                                   
                return (int)cardialDirection - 1 == (int)_forwardDirection;
            else                                                                  
                return (int)cardialDirection + 1 == (int)_forwardDirection;
        }

        public void FixedUpdate()
        {
            
        }

        public void OnActionController()
        {
            
        }

        public void OnCollisionEnter(Collision collision)
        {
            
        }

        public void OnCrouchingController()
        {
            
        }

        public void OnJumpController()
        {
            
        }
        
        public void OnTriggerEnter(Collider collider)
        {
            
        }

        public void Update()
        {
            
        }
    }

}

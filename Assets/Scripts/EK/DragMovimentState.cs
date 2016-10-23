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

        private Vector3 moviment = Vector3.zero;

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

        Vector3 forward;
        public void OnMovimentController(Vector3 direction)
        {
            //_forwardDirection = _stateController.cardinalState;
            //bool moveCondition = _stateController.defaultMovimentState.GetCardinalDirection(direction) == _forwardDirection || isOpposedDirection(direction);
            forward = direction.normalized;
            //bool moveCondition = _transform.forward == forward || _transform.forward.normalized == (forward * -1);
            moviment = direction;
            bool moveCondition = Vector3.SqrMagnitude(_transform.forward - forward) < 0.1 || Vector3.SqrMagnitude(_transform.forward - (forward * -1f)) < 0.1;

            if (_stateController.isGrounded && moveCondition)
            {
                Debug.Log("Empurrando");
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
            //moviment.x = Input.GetAxisRaw("Horizontal");
            //moviment.y = Input.GetAxisRaw("Vertical");

            //if (_stateController.cardinalState == CardinalDirection.Right || _stateController.cardinalState == CardinalDirection.Up)
            //    moviment *= -1;

            if (_transform.forward.x > 0.1 || _transform.forward.z > 0.1)
                moviment *= -1f;

            _animator.SetFloat("InputX", moviment.x, 0.1f, Time.deltaTime);
            _animator.SetFloat("InputZ", moviment.z, 0.1f, Time.deltaTime);
        }
    }

}

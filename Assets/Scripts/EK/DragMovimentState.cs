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
        private float speed = 2f;
        private Vector3 forward;
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
        
        public void OnMovimentController(Vector3 direction)
        {
            forward = direction.normalized;
            moviment = direction;
            bool moveCondition = _stateController.ekState != EKSubState.Crouching && _stateController.ekState != EKSubState.Standing &&
                                 _stateController.ekState != EKSubState.Falling && _stateController.ekTrasitionState == EKTrasitionState.None;
            bool directionCondition = Vector3.SqrMagnitude(_transform.forward - forward) < 0.1 || Vector3.SqrMagnitude(_transform.forward - (forward * -1f)) < 0.1;

            if (_stateController.isGrounded && directionCondition && moveCondition)
            {
                direction *= Time.deltaTime * speed;
                _transform.Translate(direction, Space.World);
                dragObject.transform.Translate(direction, Space.World);
            }
        }

        public void Update()
        {
            if (_transform.forward.x > 0.1 || _transform.forward.z > 0.1)
                moviment *= -1f;

            _animator.SetFloat("InputX", moviment.x, 0.1f, Time.deltaTime);
            _animator.SetFloat("InputZ", moviment.z, 0.1f, Time.deltaTime);
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

    }

}

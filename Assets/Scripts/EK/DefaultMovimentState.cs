using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace EK
{

    [System.Serializable]
    public class DefaultMovimentState : IEKState
    {
        // Referência do controlador de estados
        private StateController _stateController = null;

        // Atributos de movimento do EK
        private float speed = 3.2f;
        private float jumpForce = 380.0f;

        // Variáveis de auxilio
        private bool moveCondition = false;                                         // Condição para movimento.
        private float lastTimeJumped = 0f;                                           //Controle do tempo entre saltos

        // Referência dos componentes externos.
        private Transform _transform;
        private Rigidbody _rigidbody;
        private Animator  _animator;

        //---------------------------------------------------------------------------------------------------------------
        // Construtor base.
        public DefaultMovimentState(StateController stateController)
        {
            _stateController = stateController;
            _transform = _stateController.getTransform();
            _rigidbody = _stateController.getRigidbody();
            _animator = _stateController.getAnimator();
            lastTimeJumped = Time.time;
        }

        Vector3 velocity;
        bool canmove = false;
        //---------------------------------------------------------------------------------------------------------------
        public void OnMovimentController(Vector3 direction)
        {
            // Estabelece a condição de movimento.
            moveCondition = _stateController.ekState != EKSubState.Crouching && _stateController.ekState != EKSubState.Standing &&
                            _stateController.ekState != EKSubState.Falling && _stateController.ekState != EKSubState.Jumping;

            if (_stateController.isGrounded && moveCondition)
            {
                velocity = direction;
                canmove = true;
            }
            else
                canmove = false;

        }

        

        //---------------------------------------------------------------------------------------------------------------
        public void OnJumpController()
        {
            if (_stateController.isGrounded && Time.time > lastTimeJumped + 0.2f)
            {
                _animator.SetBool("OnJump", true);
                _stateController.isGrounded = false;                
                _rigidbody.velocity *= 1.15f;
                _rigidbody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
               // _rigidbody.drag = 0f;
                lastTimeJumped = Time.time;

            }
        }




        //---------------------------------------------------------------------------------------------------------------
        public void OnCrouchingController()
        {
            if (_stateController.isCrouching)
            {
                //if (_stateController.CanStandUp())
                    _stateController.StandUp();
                    _stateController.isCrouching = false;
                //else
                //    return;
            }
            else
            {
                _stateController.StandDown();
                _stateController.isCrouching = true;
            }                
            //_stateController.isCrouching = !_stateController.isCrouching;
            _animator.SetBool("OnCrouching", _stateController.isCrouching);
        }
        

        public void Update()
        {
            _animator.SetFloat("InputX", Input.GetAxisRaw("Horizontal"), 0.1f, Time.deltaTime);
            _animator.SetFloat("InputZ", Input.GetAxisRaw("Vertical"), 0.1f, Time.deltaTime);

            if(Mathf.Abs(_animator.GetFloat("InputX")) > 0.3f || Mathf.Abs(_animator.GetFloat("InputZ")) > 0.3f)
                _animator.SetBool("IsMoving", true);
            else
                _animator.SetBool("IsMoving", false);

            if (velocity.sqrMagnitude <= 0.01f)
                _animator.SetBool("IsStoped", true);
            else
                _animator.SetBool("IsStoped", false);

        }


        //FixedUpdate is called every fixed framerate frame.
        public void FixedUpdate()
        {
            if(canmove && _stateController.isGrounded)
            {
                Vector3 vel = velocity * speed;
                _stateController.transform.forward = Vector3.Slerp(_stateController.transform.forward, velocity.normalized, Time.fixedDeltaTime * 6.5f);
                vel.y = _rigidbody.velocity.y;
                _rigidbody.velocity = vel;
                canmove = false;
            }            
        } 

        //---------------------------------------------------------------------------------------------------------------
        // Métodos da interface sem implementação neste estado.        
        //public void FixedUpdate() { } //FixedUpdate is called every fixed framerate frame.
        public void OnActionController() {}
        public void OnCollisionEnter(Collision collision) {}
        public void OnTriggerEnter(Collider collider) {}
        //---------------------------------------------------------------------------------------------------------------
    }

}


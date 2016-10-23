﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace EK
{

    // TODO: Acertar a animação do salto!!!

    //cardinal point

    // Estados de rotação.
    public enum CardinalDirection { None, Left, Right, Up, Down, UpLeft, DownRight, UpRight, DownLeft }

    [System.Serializable]
    public class DefaultMovimentState : IEKState
    {
        // Referência do controlador de estados
        private StateController _stateController = null;

        // Controle de rotação do EK
        private CardinalDirection _cardinalState;
        private CardinalDirection _lastCardinalState;
        private Dictionary<CardinalDirection, Quaternion> _cardinalRefs = new Dictionary<CardinalDirection, Quaternion>();

        // Atributos de movimento do EK
        private float speed = 3.2f;
        private float jumpForce = 110.0f;

        // Variáveis para cronômetro.
        private float _elapsedTime = 0.0f;
        private const float _animRotateDuration = 0.6f;

        // Variáveis de auxilio
        private bool moveCondition = false;                                         // Condição para movimento.

        // Referência dos componentes externos.
        private Transform _transform;
        private Rigidbody _rigidbody;
        private Animator  _animator;

        //---------------------------------------------------------------------------------------------------------------
        // Construtor base.
        public DefaultMovimentState(StateController stateController)
        {
            // Alimentando dicinário de referências para rotação.
            _cardinalRefs.Add(CardinalDirection.None, Quaternion.Euler(0, 0, 0));
            _cardinalRefs.Add(CardinalDirection.Down, Quaternion.Euler(0, 0, 0));
            _cardinalRefs.Add(CardinalDirection.DownLeft, _cardinalRefs[CardinalDirection.Down] * Quaternion.Euler(0, 45, 0));
            _cardinalRefs.Add(CardinalDirection.DownRight, _cardinalRefs[CardinalDirection.Down] * Quaternion.Euler(0, -45, 0));
            _cardinalRefs.Add(CardinalDirection.Up, _cardinalRefs[CardinalDirection.Down] * Quaternion.Euler(0, 180, 0));
            _cardinalRefs.Add(CardinalDirection.UpLeft, _cardinalRefs[CardinalDirection.Down] * Quaternion.Euler(0, 135, 0));
            _cardinalRefs.Add(CardinalDirection.UpRight, _cardinalRefs[CardinalDirection.Down] * Quaternion.Euler(0, 225, 0));
            _cardinalRefs.Add(CardinalDirection.Left, _cardinalRefs[CardinalDirection.Down] * Quaternion.Euler(0, 90, 0));
            _cardinalRefs.Add(CardinalDirection.Right, _cardinalRefs[CardinalDirection.Down] * Quaternion.Euler(0, -90, 0));

            _stateController = stateController;
            _transform = _stateController.getTransform();
            _rigidbody = _stateController.getRigidbody();
            _animator = _stateController.getAnimator();
            _lastCardinalState = CardinalDirection.None;
        }

        Vector3 velocity;
        bool canmove = false;
        //---------------------------------------------------------------------------------------------------------------
        public void OnMovimentController(Vector3 direction)
        {
            // Estabelece a condição de movimento.
            moveCondition = _stateController.ekState != EKSubState.Crouching && _stateController.ekState != EKSubState.Standing &&
                            _stateController.ekState != EKSubState.Falling;

            //if (_stateController.isGrounded && moveCondition)
            //{
            //    direction = direction.normalized;
            //    SetCardinalDirection(direction);
            //    direction *= speed;
            //    _rigidbody.velocity = direction;
            //}
            //else
            //    if (isOpposedDirection(direction) && !_stateController.isGrounded)
            //    {
            //        _rigidbody.drag = 4f;
            //       // _animator.SetBool("OnFalling", true); Está dando um comportamento estranho nas anins
            //    }                    

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
            // TODO: Estabelecer um tempo de intervalo entre os saltos???
          
            if (_stateController.isGrounded)
            {
                _animator.SetBool("OnJump", true);
                _stateController.isGrounded = false;                
                _rigidbody.velocity *= 1.15f;
                _rigidbody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
                _rigidbody.drag = 0f;
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
            if(Mathf.Abs(_animator.GetFloat("InputX")) > 0.1f || Mathf.Abs(_animator.GetFloat("InputZ")) > 0.1f)
                _animator.SetBool("IsMoving", true);
            else
                _animator.SetBool("IsMoving", false);
        }

        //---------------------------------------------------------------------------------------------------------------
        // Retorna o ponto cardeal da direção.
        public CardinalDirection GetCardinalDirection(Vector3 direction)
        {
            if (direction.x > 0 && direction.z == 0)
                return CardinalDirection.Left;
            else if (direction.x < 0 && direction.z == 0)
                return CardinalDirection.Right;
            else if (direction.z > 0 && direction.x == 0)
                return CardinalDirection.Down;
            else if (direction.z < 0 && direction.x == 0)
                return CardinalDirection.Up;
            else if (direction.z < 0 && direction.x > 0)
                return CardinalDirection.UpLeft;
            else if (direction.z < 0 && direction.x < 0)
                return CardinalDirection.UpRight;
            else if (direction.z > 0 && direction.x > 0)
                return CardinalDirection.DownLeft;
            else if (direction.z > 0 && direction.x < 0)
                return CardinalDirection.DownRight;
            else if(direction.z == 0 && direction.x == 0)
                return _cardinalState;
            else
                return CardinalDirection.None;               
        }

        //---------------------------------------------------------------------------------------------------------------
        // Retorna se a direção inserida é oposta a direção atual.
        private bool isOpposedDirection(Vector3 direction)
        {
            CardinalDirection cardialDirection = GetCardinalDirection(direction); // Considerando que Enum são constantes numéricas:
            if ((int)cardialDirection % 2 == 0)                                   // Se a constante é par, seu oposto é a subtração de um,
                return (int)cardialDirection - 1 == (int)_cardinalState;
            else                                                                  // Se a constante é impar, seu oposto é a soma de um.
                return (int)cardialDirection + 1 == (int)_cardinalState;
        }

        //---------------------------------------------------------------------------------------------------------------
        // Checa o input para estabelecer a direção cardeal.
        private void SetCardinalDirection(Vector3 direction)
        {            
            _cardinalState = GetCardinalDirection(direction);
            if (_lastCardinalState != _cardinalState)                             // Se a direção for diferente, haverá rotação, 
            {
                _elapsedTime = 0.0f;                                              // portanto, zera o cronometro.
                _stateController.StartChildCoroutine(LookTo());                   // Inicia a Coroutine pelo controlador MonoBehaviour.
            }                
            _lastCardinalState = _cardinalState;                                  // Atualizar estado anterior.
            _stateController.cardinalState = _cardinalState;                      // Exibe a carinalidade no controlador de estados.
        }

        //---------------------------------------------------------------------------------------------------------------
        // Aplica a rotação apontando para o ponto cardeal atual.
        public IEnumerator LookTo()
        {
            while (_elapsedTime != _animRotateDuration)
            {
                _elapsedTime += Time.deltaTime;
                if (_elapsedTime >= _animRotateDuration)
                    _elapsedTime = _animRotateDuration;
                _transform.rotation = Quaternion.Lerp(_transform.rotation, _cardinalRefs[_cardinalState], _elapsedTime / _animRotateDuration);
                yield return null;
            }
        }


        // velocity = direction;
        //canmove = true;
        //FixedUpdate is called every fixed framerate frame.
        public void FixedUpdate()
        {
            if(canmove)
            {
                Vector3 vel = velocity * speed;
                _stateController.transform.forward = Vector3.Slerp(_stateController.transform.forward, velocity.normalized, Time.fixedDeltaTime * 6.5f);
                vel.y = _rigidbody.velocity.y;
                _rigidbody.velocity = vel;
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


using UnityEngine;
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
        private bool moveCondition = false;

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

        //---------------------------------------------------------------------------------------------------------------
        public void OnMovimentController(Vector3 direction)
        {

            moveCondition = _stateController.ekState != EKState.Crouching && _stateController.ekState != EKState.Standing &&
                            _stateController.ekState != EKState.Falling;

            if (_stateController.isGrounded && moveCondition)
            {
                direction = direction.normalized;
                SetCardinalDirection(direction);

                // TODO: Retirar depois do Teste.
                _stateController.state = _cardinalState;
                direction *= speed;
                _rigidbody.velocity = direction;
            }
            else
                if (isOpposedDirection(direction) && !_stateController.isGrounded)
                {
                    _rigidbody.drag = 4f;
                   // _animator.SetBool("OnFalling", true); Está dando um comportamento estranho nas anins
                }                    
        }

        //---------------------------------------------------------------------------------------------------------------
        public void OnJumpController()
        {
            // TODO: Estabelecer um tempo de intervalo entre os saltos???
            if (_stateController.isGrounded)
            {
                _animator.SetBool("OnGround", false);
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


        public void OnActionController()
        {
            throw new NotImplementedException();
        }

        public void OnCollisionEnter(Collision collision)
        {
            throw new NotImplementedException();
        }

        
        
        public void OnTriggerEnter(Collider collider)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            //throw new NotImplementedException();
        }
        
        //---------------------------------------------------------------------------------------------------------------
        // Retorna o ponto cardeal da direção.
        private CardinalDirection GetCardinalDirection(Vector3 direction)
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
                _elapsedTime = 0.0f;                                              // portanto, zera o cronometro.

            _lastCardinalState = _cardinalState;
        }

        //---------------------------------------------------------------------------------------------------------------
        // Aplica a rotação apontando para o ponto cardeal atual.
        private void LookTo(Quaternion direction)
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= _animRotateDuration)
            {
                _elapsedTime = _animRotateDuration;
                
                // TODO: Colocar uma variável RunRotate urgente!!!!!
                //_cardinalState = CardinalDirection.None;
            }
            _transform.rotation = Quaternion.Lerp(_transform.rotation, direction, _elapsedTime / _animRotateDuration);
        }

        //---------------------------------------------------------------------------------------------------------------
        // FixedUpdate is called every fixed framerate frame. Aplica o LookTo de acordo com o ponto cardeal atual.
        public void FixedUpdate()
        {
            if (_cardinalState != CardinalDirection.None)
                LookTo(_cardinalRefs[_cardinalState]);
        }
        //---------------------------------------------------------------------------------------------------------------
    }

}


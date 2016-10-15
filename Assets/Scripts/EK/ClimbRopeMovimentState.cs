using UnityEngine;
using System.Collections;
using EK;
using System;

public class ClimbRopeMovimentState : IEKState
{
    private StateController _stateController;
    private float _speed = 0.3f;

    public Rigidbody ropeNode = null;
    private Vector3 _inNodePosition = Vector3.zero;
    private float _inNodeY = 0.0f;

    private Transform _transform = null;
    private Animator _animator = null;

    //---------------------------------------------------------------------------------------------------------------
    // Construtor base.
    public ClimbRopeMovimentState(StateController stateController)
    {
        _stateController = stateController;
        _transform = stateController.getTransform();
        _animator = stateController.getAnimator();
    }

    //---------------------------------------------------------------------------------------------------------------
    public void OnMovimentController(Vector3 direction)
    {
        if(direction.z != 0)                                                            // Move o player no sentido vertical.
        {
            direction.z *= Time.deltaTime * _speed * -1f;

            if (_stateController.isGrounded && (direction.z < 0))
            {
                
            }
            else
                _transform.Translate(0, direction.z, 0, Space.Self);

            _inNodeY = _transform.position.y;                                           // Armazena posição global no eixo Y pós movimento.
            _transform.localPosition = Vector3.zero;                                    // Armazena posição global, no centro do node.
            _inNodePosition = _transform.position;
            _inNodePosition.y = _inNodeY;
            _transform.position = _inNodePosition;                                      // Atualiza a posição global.
        }            
        if(direction.x != 0)
            if(this.ropeNode)
                this.ropeNode.AddForce(new Vector3(direction.x * 15f, 0, 0));           // impõe forca no node da corda para balança-la.
    }

    //---------------------------------------------------------------------------------------------------------------
    public void OnTriggerEnter(Collider collider)
    {
        _transform.SetParent(collider.transform);
        _stateController.climbOffset = new Vector3(0, -1.83f, 0);
        ropeNode = collider.GetComponent<Rigidbody>();
    }

    //---------------------------------------------------------------------------------------------------------------
    public void Update()
    {
        _animator.SetFloat("InputX", Input.GetAxisRaw("Horizontal"), 0.1f, Time.deltaTime);
        _animator.SetFloat("InputZ", Input.GetAxisRaw("Vertical"), 0.1f, Time.deltaTime);
    }

    //---------------------------------------------------------------------------------------------------------------
    public void OnActionController()
    {
        Vector3 velocity = ropeNode.velocity;        
        RopeClimbingInteractiveBehaviour.active = false;
        _stateController.Interaction = null;
        _stateController.transform.SetParent(null);
        _stateController.climbOffset = Vector3.zero;
        _transform.rotation = Quaternion.Euler(Vector3.zero);
        _animator.SetBool("OnClimbRope", false);
        _stateController.GetComponent<Rigidbody>().isKinematic = false;

        velocity *= 4f;
        _stateController.GetComponent<Rigidbody>().AddForce(velocity * 8f);  
        _stateController.currentState = _stateController.defaultMovimentState;
        _stateController.GetComponent<Rigidbody>().drag = 0;
        _stateController.GetComponent<Rigidbody>().velocity = velocity;
        _stateController.GetComponent<Rigidbody>().AddForce(new Vector3(velocity.x, velocity.y, 0), ForceMode.Impulse);
        //_stateController.currentState = _stateController.defaultMovimentState;

        _stateController.ReactiveRopeCollision();
        //_stateController.Invoke("ReactiveRopeCollision", 0.5f);

    }

    public void ReactiveRopeCollision()
    {
        this.ropeNode.GetComponent<RopeClimbingInteractiveBehaviour>().DeativeCapsuleCollider(_stateController.GetComponent<CapsuleCollider>(), false);
        _stateController.climbRopeMovimentState.ropeNode = null;
    }

    //---------------------------------------------------------------------------------------------------------------
    public void FixedUpdate()
    {
    }

    

    //---------------------------------------------------------------------------------------------------------------
    public void OnCollisionEnter(Collision collision)
    {
    }

    //---------------------------------------------------------------------------------------------------------------
    public void OnCrouchingController()
    {
    }

    //---------------------------------------------------------------------------------------------------------------
    public void OnJumpController()
    {
    }
    
    

    
}

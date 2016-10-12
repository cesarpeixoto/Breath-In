using UnityEngine;
using System.Collections;
using EK;
using System;

public class ClimbRopeMovimentState : IEKState
{
    private StateController _stateController;
    private RopeClimbingInteractiveBehaviour _climbingBehaviour = null;
    private float _speed = 0.3f;
    private float _direction = 1f;

    public Rigidbody ropeNode = null;
    private Vector3 _inNodePosition = Vector3.zero;
    private float _inNodeY = 0.0f;

    private Transform _transform = null;

    //---------------------------------------------------------------------------------------------------------------
    // Construtor base.
    public ClimbRopeMovimentState(StateController stateController)
    {
        _stateController = stateController;
        _transform = stateController.getTransform();
    }

    //---------------------------------------------------------------------------------------------------------------
    public void OnMovimentController(Vector3 direction)
    {
        if(direction.z != 0)                                                            // Move o player no sentido vertical.
        {
            direction.z *= Time.deltaTime * _speed * -1f;
            _transform.Translate(0, direction.z, 0, Space.Self);
            _inNodeY = _transform.position.y;                                           // Armazena posição global no eixo Y pós movimento.
            _transform.localPosition = Vector3.zero;                                    // Armazena posição global, no centro do node.
            _inNodePosition = _transform.position;
            _inNodePosition.y = _inNodeY;
            _transform.position = _inNodePosition;                                      // Atualiza a posição global.
        }            
        if(direction.x != 0)
            if(this.ropeNode)
                this.ropeNode.AddForce(new Vector3(direction.x * 15f, 0, 0));                // impõe forca no node da corda para balança-la.
    }

    //---------------------------------------------------------------------------------------------------------------
    public void OnTriggerEnter(Collider collider)
    {
        _transform.SetParent(collider.transform);
        ropeNode = collider.GetComponent<Rigidbody>();
    }

    //---------------------------------------------------------------------------------------------------------------
    public void Update()
    {
    }

    //---------------------------------------------------------------------------------------------------------------
    public void FixedUpdate()
    {
    }

    public void OnActionController()
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

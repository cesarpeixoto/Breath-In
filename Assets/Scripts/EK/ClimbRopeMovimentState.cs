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
    private float _rotationDefaultInY = 0.0f;
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
        if(direction.x != 0)                                                            // Move o player no sentido vertical.
        {
            direction.x *= Time.deltaTime * _speed;

            if (_stateController.isGrounded && (direction.z < 0))
            {
                
            }
            else
                _transform.Translate(0, direction.x, 0, Space.Self);

            _inNodeY = _transform.position.y;                                           // Armazena posição global no eixo Y pós movimento.
            _transform.localPosition = Vector3.zero;                                    // Armazena posição global, no centro do node.
            _inNodePosition = _transform.position;
            _inNodePosition.y = _inNodeY;
            _transform.position = _inNodePosition;                                      // Atualiza a posição global.
        }            
        if(direction.z != 0)
            if(this.ropeNode)
                this.ropeNode.AddForce(new Vector3(0, 0, direction.z * 15f));           // impõe forca no node da corda para balança-la.
    }

    //---------------------------------------------------------------------------------------------------------------
    public void OnTriggerEnter(Collider collider)
    {
        _rotationDefaultInY = _transform.rotation.eulerAngles.y;
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

        RopeClimbingInteractiveBehaviour.active = false;
        _stateController.Interaction = null;
        _stateController.transform.SetParent(null);

        // Depois da nova mecanica de movimento
        _stateController.climbOffset = Vector3.zero;                
        _animator.SetBool("OnClimbRope", false);
        _stateController.GetComponent<Rigidbody>().isKinematic = false;
        _transform.rotation.eulerAngles.Set(0, _rotationDefaultInY, 0);

        //float velMagnitude = ropeNode.velocity.magnitude;
        ////velMagnitude = Mathf.Lerp(4.7f, 0f, velMagnitude / 4.7f);
        //Debug.Log(velMagnitude);
        //Vector3 velocity = _transform.forward;
        //velocity.y += 0.1f;
        //velocity *= velMagnitude * 3f;
        //Debug.Log(velocity);

        Vector3 direction = ropeNode.velocity.normalized + Vector3.up * 0.3f;
        float speed = Mathf.Abs(ropeNode.transform.localPosition.x) - ropeNode.GetComponent<RopeClimbingInteractiveBehaviour>().xPosition;

        Vector3 velocity = direction * speed * 5f;
        velocity.x *= 3f;
        //_stateController.GetComponent<Rigidbody>().AddForce(velocity * 8f);  
        _stateController.currentState = _stateController.defaultMovimentState;
        _stateController.GetComponent<Rigidbody>().velocity = velocity;
        Debug.Log("Corpo " + _stateController.GetComponent<Rigidbody>().velocity);
        //_stateController.GetComponent<Rigidbody>().AddForce(new Vector3(velocity.x, velocity.y, velocity.z), ForceMode.Impulse);
        _stateController.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.Acceleration);
        Debug.Log("Corpo2 " + _stateController.GetComponent<Rigidbody>().velocity);

        //Onde estava isso???????
        _transform.FindChild("Mesh").localPosition = Vector3.zero;
        _stateController.GetComponent<CapsuleCollider>().center = new Vector3(0, 0.9f, 0);
        _transform.Translate(0, -1.748f, 0);
        _transform.rotation = Quaternion.Euler(0, _rotationDefaultInY, 0);

        _stateController.StartChildCoroutine(ReactiveRopeCollision());
        _stateController.GetComponent<Rigidbody>().velocity = velocity;
    }

    //public void ReactiveRopeCollision()
    //{
    //    this.ropeNode.GetComponent<RopeClimbingInteractiveBehaviour>().DeativeCapsuleCollider(_stateController.GetComponent<CapsuleCollider>(), false);
    //    _stateController.climbRopeMovimentState.ropeNode = null;
    //}

    public IEnumerator ReactiveRopeCollision()
    {
        yield return new WaitForSeconds(0.3f);
        this.ropeNode.GetComponent<RopeClimbingInteractiveBehaviour>().DeativeCapsuleCollider(_stateController.GetComponent<CapsuleCollider>(), false);
        _stateController.climbRopeMovimentState.ropeNode = null;
        _transform.rotation.eulerAngles.Set(0, _rotationDefaultInY, 0);
        
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

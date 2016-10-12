using UnityEngine;
using System.Collections;

public class InputHandle : MonoBehaviour
{
    //private MovimentHandle _moviment = null;
    private EK.StateController _stateController = null;

    private float _horizontal = 0.0f;
    private float _vertical = 0.0f;
    private float _horizontalRaw = 0.0f;
    private float _verticalRaw = 0.0f;
    //private Animator _animator;
    //private Rigidbody _rigidbody;

    public bool isFolling = false;

    private void Awake()
    {
        //_moviment = GetComponent<MovimentHandle>();
        //_animator = GetComponent<Animator>();
        //_rigidbody = GetComponent<Rigidbody>();
        _stateController = GetComponent<EK.StateController>();
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        _horizontalRaw = Input.GetAxis("Horizontal");
        _verticalRaw = Input.GetAxis("Vertical");

        

        //float temp = Input.GetAxisRaw("Horizontal");

        // como não faz parte, precisa só ocorrer não não estiver agachado!!!

        // Vamos checar se horizontal 

        //_stateController.getAnimator().SetFloat("Velocity", temp, 0.1f, Time.deltaTime);



        //if (!_stateController.isCrouching)
        //    _stateController.getAnimator().SetFloat("Velocity", _stateController.getRigidbody().velocity.magnitude, 0.1f, Time.deltaTime);

        //isFolling = _stateController.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("hard_landing");



        if (Input.GetAxis("Jump") != 0)
        {
            //_moviment.OnJump();
            _stateController.currentState.OnJumpController();
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            _stateController.currentState.OnCrouchingController();
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            _stateController.OnActionController();
        }
        else if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Vertical") == 0)
                _stateController.currentState.OnMovimentController(new Vector3(-_horizontal, 0, 0));
                //_stateController.OnMovimentController(new Vector3(-_horizontal, 0, 0), new Vector3(_horizontalRaw, 0, 0));
            else if (Input.GetAxisRaw("Vertical") != 0 && Input.GetAxisRaw("Horizontal") == 0)
                _stateController.currentState.OnMovimentController(new Vector3(0, 0, -_vertical));
                //_stateController.OnMovimentController(new Vector3(0, 0, -_vertical), new Vector3(0, 0, _verticalRaw));
            else
                _stateController.currentState.OnMovimentController(new Vector3(-_horizontal, 0, -_vertical));
                //_stateController.OnMovimentController(new Vector3(-_horizontal, 0, -_vertical), new Vector3(_horizontalRaw, 0, _verticalRaw));
        }
        //else
        //    _stateController.axis = Vector3.zero;
            //_stateController.currentState.OnMovimentController(Vector3.zero);
            
    }

    private void FixedUpdate()
    {
        
    }



}

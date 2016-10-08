/*
A questão é descer o conjunto inteiro, e deixar só o object no topo...
Isso resolve a questão de ele subir...


*/



using UnityEngine;
using System.Collections;
using UnityEditor;

namespace EK
{
    public enum EKSubState { Idle, Runing, Jumping, Standing, Crouching, Crouched, Falling }

    public class StateController : MonoBehaviour
    {
        public delegate void InteractiveHandle(EK.StateController controller) ;
        public InteractiveHandle Interaction;

        // Referência do objeto carregado.
        [HideInInspector]
        public GameObject carryingObject = null;


        // Flags de estados
        public bool isGrounded = true;
        public bool isCrouching = false;
        public Vector3 axis = Vector3.zero;
        public Vector3 axisRaw = Vector3.zero;

        [SerializeField]
        public IEKState currentState;
        public CardinalDirection cardinalState;
        public EKSubState ekState = EKSubState.Idle;


        private float _capsuleHeight = 0.0f;
        private Vector3 _capsuleCenter = Vector3.zero;
        private float _crouchCapsuleHeight = 1.30f;
        private Vector3 _crouchCapsuleCenter = new Vector3(0, 0.67f,0);

        // Referências de componentes externos.
        private Transform _transform = null;
        private Rigidbody _rigidbody = null;
        private Animator  _animator = null;
        private CapsuleCollider _capsuleCollider = null;

        [HideInInspector]
        public DefaultMovimentState defaultMovimentState;
        public DragMovimentState dragMovimentState;

        // delcarar o dragMovimentState aqui!!!!
        
        // Use this for initialization
        void Awake()
        {
            _transform = GetComponent<Transform>();
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            _capsuleCollider = GetComponent<CapsuleCollider>();
            _capsuleHeight = _capsuleCollider.height;
            _capsuleCenter = _capsuleCollider.center;
            this.defaultMovimentState = new DefaultMovimentState(this);
            this.currentState = this.defaultMovimentState;
            this.dragMovimentState = new DragMovimentState(this);            
        }

        public void OnActionController()
        {
            if(Interaction != null)
                Interaction(this);
            else
            {
                // Implementar som de que não tem ação disponível
            }

        }

        // Update is called once per frame
        private void Update()
        {
            this.currentState.Update();
        }

        private void FixedUpdate()
        {
            this.currentState.FixedUpdate();
            CheckGroundStatus();
        }

        //
        public void StandUp()
        {
            _capsuleCollider.height = _capsuleHeight;
            _capsuleCollider.center = _capsuleCenter;
        }

        public void StandDown()
        {
            _capsuleCollider.height = _crouchCapsuleHeight;
            _capsuleCollider.center = _crouchCapsuleCenter;
        }


        public bool CanStandUp()
        {
            Ray ray = new Ray(_rigidbody.position + Vector3.up * _capsuleCollider.radius * 0.5f, Vector3.up);
            float rayLength = _capsuleHeight - _capsuleCollider.radius * 0.5f;
            if(Physics.SphereCast(ray, _capsuleCollider.radius/2, rayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                return true;
            else
                return false;
        }

        //---------------------------------------------------------------------------------------------------------------
        // Checa e estabelece se o Player está em contato com o chão.
        private void CheckGroundStatus()
        {
            RaycastHit hitInfo;
            Vector3 offset = Vector3.up * 0.4f;
            if (Physics.Raycast(_transform.position + offset, Vector3.down, out hitInfo, 0.5f))
            {
                this.isGrounded = true;
                _animator.SetBool("OnGround", isGrounded);
                _animator.SetBool("OnFalling", !isGrounded);
                _rigidbody.drag = 1.8f;                                
            }
            else
                this.isGrounded = false;                                
        }

        public void OnMovimentController(Vector3 direction, Vector3 directionRaw)
        {
            // aqui sempre entra
            this.axis = direction;//_transform.InverseTransformDirection(direction);
            this.axisRaw = directionRaw;//_transform.InverseTransformDirection(directionRaw);
            this.currentState.OnMovimentController(axis);
        }
       
        public Transform getTransform()
        {
            return _transform;
        }

        public Rigidbody getRigidbody()
        {
            return _rigidbody;
        }

        public Animator getAnimator()
        {
            return _animator;
        }



        

    }
}

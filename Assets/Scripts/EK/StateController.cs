/*
A questão é descer o conjunto inteiro, e deixar só o object no topo...
Isso resolve a questão de ele subir...


*/



using UnityEngine;
using System.Collections;
using UnityEditor;

namespace EK
{
    public enum EKSubState { Idle, Runing, Jumping, Standing, Crouching, Crouched, Falling, Draging }
    public enum EKTrasitionState { None, BeginDrag }

    public class StateController : MonoBehaviour
    {
        // Callback das ações de iteração.
        public delegate void InteractiveHandle();
        public InteractiveHandle Interaction;

        // Flags auxiliares de Estado.
        public bool OnGround = true;
        private float _groundDistance = 0.0f;

        public RaycastHit groundHit;



        // Referência do objeto carregado.
        [HideInInspector]
        public GameObject carryingObject = null;

        // Flags de estados
        //public bool _isGrounded = true;
        public bool isGrounded = true;


        public bool isCrouching = false;
        public Vector3 axis = Vector3.zero;
        public Vector3 axisRaw = Vector3.zero;

        [SerializeField]
        public IEKState currentState;
        public EKSubState ekState = EKSubState.Idle;
        public EKTrasitionState ekTrasitionState = EKTrasitionState.None;

        // Referências de componentes externos.
        #region "Componentes Externos"
        private Transform _transform = null;
        private Rigidbody _rigidbody = null;
        private Animator _animator = null;
        private CapsuleCollider _capsuleCollider = null;
        #endregion

        // Instancias da maquina de estados.
        #region "Maquina de estados"
        [HideInInspector]
        public DefaultMovimentState defaultMovimentState;
        [HideInInspector]
        public DragMovimentState dragMovimentState;
        [HideInInspector]
        public ClimbRopeMovimentState climbRopeMovimentState;
        #endregion

        // Referências auxiliares
        #region "Auxiliares Maquina de Estados"
        public Vector3 climbOffset = Vector3.zero;
        public Transform leftHand = null;
        public Transform rightHand = null;

        private float _capsuleHeight = 0.0f;
        private Vector3 _capsuleCenter = Vector3.zero;
        private float _crouchCapsuleHeight = 1.30f;
        private Vector3 _crouchCapsuleCenter = new Vector3(0, 0.67f, 0);
        #endregion

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
            this.climbRopeMovimentState = new ClimbRopeMovimentState(this);
        }

        public void OnActionController()
        {
            if (Interaction != null)
                Interaction(/*this*/);
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



            CheckGroundDistance();

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
            if (Physics.SphereCast(ray, _capsuleCollider.radius / 2, rayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                return true;
            else
                return false;
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

        public void StartChildCoroutine(IEnumerator callback)
        {
            StartCoroutine(callback);
        }


        // É feito, ridiculo mas necessario no momento...
        public void ReactiveRopeCollision()
        {
            Invoke("ReactiveRopeCollisionAux", 0.4f);
        }

        void ReactiveRopeCollisionAux()
        {
            climbRopeMovimentState.ReactiveRopeCollision();
        }


        public Vector3 input = Vector3.zero;
        private Vector3 _velocity = Vector3.zero;
        public void OnMovimentController(Vector3 direction)
        {
            direction = Vector3.ClampMagnitude(direction, 1f);
            _velocity = Camera.main.transform.forward * direction.z + Camera.main.transform.right * direction.x;
            _velocity.y = 0;
            this.currentState.OnMovimentController(_velocity);
        }

        //---------------------------------------------------------------------------------------------------------------
        // Checa e estabelece se o Player está em contato com o chão.
        private void CheckGroundStatus()
        {
            RaycastHit hitInfo;
            Vector3 offset = Vector3.up * 0.4f;
            int layer = 1 << 0;
            if (Physics.Raycast(_transform.position + offset + climbOffset, Vector3.down, out hitInfo, 0.5f, layer))
            {
                this.isGrounded = true;
                _animator.SetBool("OnGround", isGrounded);
                _animator.SetBool("OnFalling", !isGrounded);
                _rigidbody.drag = 1.8f;
            }
            else
                this.isGrounded = false;
            _animator.SetBool("OnGround", isGrounded);
            _animator.SetBool("OnFalling", !isGrounded);

        }

        //---------------------------------------------------------------------------------------------------------------
        // Checa e estabelece se o Player está em contato com o chão e qual a distancia.
                
        private void CheckGroundDistance()
        {            
            float radius = _capsuleCollider.radius * 0.9f;                                          // Raio 10% menor que do capsule colider.            
            Vector3 spherePosition = _transform.position + (Vector3.up * _capsuleCollider.radius);  // Posição de origem para o SphereCast de acordo com CapsuleCollider
            Ray sphereRay = new Ray(spherePosition, Vector3.down);

            if (Physics.SphereCast(sphereRay, radius, out this.groundHit, 0.03f))                         // Checa se o personagem está em alguma superficie.
            {
                this.OnGround = true;
                _groundDistance = 0.0f;
            }
            else
            {
                Ray distanceRay = new Ray(_transform.position, Vector3.down);
                if (Physics.Raycast(distanceRay, out this.groundHit, 2f))
                    _groundDistance = this.groundHit.distance;
                else
                    _groundDistance = 2f;
            }
#if UNITY_EDITOR
            DebugSpherePosition = spherePosition;
            DebugSphereRadius = radius;
#endif
        }

        //---------------------------------------------------------------------------------------------------------------
        // Para Debug.

#if UNITY_EDITOR
        Vector3 DebugSpherePosition = Vector3.zero;
        float DebugSphereRadius = 0.0f;
        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(DebugSpherePosition, DebugSphereRadius);
        }
#endif


    }
}

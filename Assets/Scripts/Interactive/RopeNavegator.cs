using UnityEngine;
using System.Collections;
using EK;

public class RopeNavegator : MonoBehaviour
{
    private StateController _stateController;
    private RopeClimbingInteractiveBehaviour _climbingBehaviour = null;
    private float _speed = 0.3f;
    private float _direction = 1f;

    public Rigidbody ropeNode = null;
    private Vector3 _inNodePosition = Vector3.zero;
    private float _inNodeY = 0.0f;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMovimentController(Vector3 direction)
    {
        _direction = direction.y >= 0 ? 1f : -1f;                                       // Estabelece a direção do movimento.
        if (direction.y != 0)                                                            // Move o player no sentido vertical.
        {
            direction.y *= Time.deltaTime * _speed;
            _stateController.getTransform().Translate(0, direction.y, 0, Space.Self);
            _inNodeY = _stateController.getTransform().position.y;                      // Armazena posição global no eixo Y pós movimento.
            _stateController.getTransform().localPosition = Vector3.zero;               // Armazena posição global, no centro do node.
            _inNodePosition = _stateController.getTransform().position;
            _inNodePosition.y = _inNodeY;
            _stateController.getTransform().position = _inNodePosition;                 // Atualiza a posição global.
        }
        if (direction.x != 0)
            this.ropeNode.AddForce(new Vector3(direction.x, 0, 0));                     // impõe forca no node da corda para balança-la.
    }


}

using UnityEngine;
using System.Collections.Generic;

public class CameraController : MonoBehaviour 
{
	public Transform target;								// Referência para o GameObject que será seguido.
	public float movimentSpeed = 0.1f;						// Velocidade do movimento da Camera.
    public float distance = 12f;
    Transform _transform;

    void Start()
    {
        _transform = transform;
    }

	// Update is called once per frame
	void LateUpdate () 
	{
		if (target != null)																		// Checa se a referencia não é nula.
		{
            Vector3 destination = new Vector3(target.position.x - distance, target.position.y +3f, 		// Cria um vetor para posição de destino, mantendo o eixo z inalterado.
                                                target.position.z);
            _transform.position = Vector3.Lerp (transform.position, destination, movimentSpeed);	// Faz o movimento da Camera seguir para nova posição com fluidez, na velocidade de lerpSpeed.
		}

        
	}
}

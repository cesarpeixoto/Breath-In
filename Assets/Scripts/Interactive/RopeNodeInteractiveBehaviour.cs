using UnityEngine;
using System.Collections;

public class RopeNodeInteractiveBehaviour : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Entrou");
        Debug.Log(gameObject.name);
        //GetComponent<Rigidbody>().isKinematic = true;
    }


    private void OnCollisionExit(Collision collision)
    {
        //GetComponent<Rigidbody>().isKinematic = false;
    }


}

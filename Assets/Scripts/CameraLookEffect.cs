using UnityEngine;
using System.Collections.Generic;

public class CameraLookEffect : MonoBehaviour {

    Transform _transform;
    public Transform playerCenter, playerTop, playerBotton;
    List<Renderer> modifiedRenderers = new List<Renderer>();

	void Start () {
        _transform = transform;
	}
	
	void Update () {
        List<RaycastHit> hits = new List<RaycastHit>();
        Vector3 dirTop = (playerTop.position - _transform.position).normalized;
        Vector3 dirCenter = (playerCenter.position - _transform.position).normalized;
        Vector3 dirBotton = (playerBotton.position - _transform.position).normalized;

        float dist = Vector3.Distance(playerCenter.position, _transform.position);

        foreach(RaycastHit hit in Physics.RaycastAll(_transform.position, dirTop, dist))
        {
            hits.Add(hit);
        }
        foreach (RaycastHit hit in Physics.RaycastAll(_transform.position, dirCenter, dist))
        {
            hits.Add(hit);
        }
        foreach (RaycastHit hit in Physics.RaycastAll(_transform.position, dirBotton, dist))
        {
            hits.Add(hit);
        }


        for (int i = 0; i < hits.Count; i++)
        {
            RaycastHit hit = hits[i];
            Renderer rend = hit.transform.GetComponent<Renderer>();

            if (rend)
            {
                // Change the material of all hit colliders
                // to use a transparent shader.
                rend.material.shader = Shader.Find("Transparent/Diffuse");
                Color tempColor = rend.material.color;
                tempColor.a = 0.3F;
                rend.material.color = tempColor;
                modifiedRenderers.Add(rend);
            }
        }

        List<int> indexesToRemove = new List<int>();
        for(int i = 0; i<modifiedRenderers.Count; i++)
        {
            bool isInArray = false;
            for(int j = 0; j<hits.Count; j++)
            {
                if(hits[j].transform.GetComponent<Renderer>() == modifiedRenderers[i])
                {
                    isInArray = true;
                    break;
                }
            }
            if(!isInArray)
            {
                modifiedRenderers[i].material.shader = Shader.Find("Legacy Shaders/Bumped Diffuse");

                Color tempColor = modifiedRenderers[i].material.color;
                tempColor.a = 1F;
                modifiedRenderers[i].material.color = tempColor;

                indexesToRemove.Add(i);
            }
        }
        for(int i = indexesToRemove.Count - 1; i>0; i--)
        {
            modifiedRenderers.RemoveAt(indexesToRemove[i]);
        }
    }

}

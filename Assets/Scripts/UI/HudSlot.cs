using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HudSlot : MonoBehaviour
{
    enum SlotStatus { Activated, Deactivated }
    public GameObject[] activeObjects;
    public Sprite[] activeSprite;

    // Use this for initialization
    void Start()
    {
        Status(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Activad(bool state)
    {
        activeObjects[0].SetActive(state);
    }

    public void Status(bool newState)
    {
        foreach (var item in activeObjects)
            item.SetActive(newState);
    }


}

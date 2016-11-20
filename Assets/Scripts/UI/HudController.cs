using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HudController : MonoBehaviour
{
    public Image HudEnergyStatus;
    public HudSlot HudEnergyController;
    public Image HudBreathStatus;
    public HudSlot HudBreathController;


    // Use this for initialization
    void Start()
    {
        EK.StateController.OnSetBreath += SetBreath;
        EK.StateController.OnSetEnergy += SetEnergy;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetEnergy(float value)
    {
        HudEnergyStatus.fillAmount = value;

        if (HudEnergyStatus.fillAmount > 0)
            HudEnergyController.Status(true);
        else if (HudEnergyStatus.fillAmount == 0)
            HudEnergyController.Status(false);
    }

    private void SetBreath(float value)
    {
        HudBreathStatus.fillAmount = value;

        if (HudBreathStatus.fillAmount > 0)
            HudBreathController.Status(true);
        else if (HudBreathStatus.fillAmount == 0)
            HudBreathController.Status(false);
    }



}

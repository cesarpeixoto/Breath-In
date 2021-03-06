﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HudController : MonoBehaviour
{
    public Image HudEnergyStatus;
    public HudSlot HudEnergyController;
    public Image HudBreathStatus;
    public HudSlot HudBreathController;


    // Use this for initialization
    void Awake()
    {
        EK.StateController.OnSetBreath += SetBreath;
        EK.StateController.OnSetEnergy += SetEnergy;
        EK.StateController.OnEnergyActive += ActiveEnergy;
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void ActiveEnergy(float value)
    {
        bool act = false;
        if (value == 0f)
            act = false;
        else
            act = true;
        HudEnergyController.Activad(act);
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

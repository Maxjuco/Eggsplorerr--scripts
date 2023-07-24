using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_NightVision : A_Skill
{
    [SerializeField] GameObject nightVisionfilter;
    [SerializeField] LightController lightController;

    bool isNightVisionOn = false;

    public override void OnButtonPressed()
    {
        ToggleNightVision();
    }

    public override void OnButtonReleased()
    {
        return;
    }

    public override void OnSkillDeselected()
    {
        isNightVisionOn = false;
        Deactivate();
    }

    void ToggleNightVision()
    {
        isNightVisionOn = !isNightVisionOn;

        if (isNightVisionOn) Activate();
        else Deactivate();
        
    }

    void Activate()
    {
        nightVisionfilter.SetActive(true);
        lightController.ActivateNightVision();
    }

    void Deactivate()
    {
        nightVisionfilter.SetActive(false);
        lightController.DeactivateNightVision();
    }
}

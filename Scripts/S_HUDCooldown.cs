using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_HUDCooldown : MonoBehaviour
{
    [SerializeField] S_PlayerControllerNew playerController;
    [SerializeField] Image image;

    [SerializeField] Color defaultColor;
    [SerializeField] Color cooldownColor;

    private void Start()
    {
        image.color = defaultColor;
    }

    void Update()
    {
        A_Skill skill = playerController.skillList[playerController.skillSelected];

        // Render
        if(skill.cooldownTimer <= 0)
        {
            image.fillAmount = 1f;
            image.color = defaultColor;
        } else
        {
            image.fillAmount = 1 - (skill.cooldownTimer / skill.cooldown);
            image.color = cooldownColor;
            // Debug.Log(image.fillAmount);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Camouflage : A_Skill
{
    [SerializeField] S_PlayerControllerNew playerController;
    [SerializeField] GameObject playerGraphics;
    [SerializeField] GameObject camoGraphics;

    public bool isCamouflaged { get; private set; }

    private void Update()
    {
        // Cooldown
        if(this.cooldownTimer > 0)
        {
            this.cooldownTimer -= Time.deltaTime;
        } else
        {
            this.cooldownTimer = 0;
        }

        // Activation time
        if (this.activationTimer > 0)
        {
            this.activationTimer -= Time.deltaTime;
        }
        else
        {
            this.Deactivate();
        }

        // Debug.Log("Cooldown : " + this.cooldownTimer);
        // Debug.Log("ActivationTime : " + this.activationTimer);
        // Debug.Log("--------------------------------------");
    }

    public override void OnButtonPressed()
    {
        if(this.cooldownTimer <= 0f)
        {
            this.cooldownTimer = this.cooldown;
            Activate();
        }
    }

    public override void OnButtonReleased()
    {
        return;
    }

    public override void OnSkillDeselected()
    {
        Deactivate();
    }

    void Activate()
    {
        this.isCamouflaged = true;

        playerGraphics.SetActive(false);
        camoGraphics.SetActive(true);

        this.activationTimer = this.activationTime;
        this.playerController.canMove = false;

        // TODO: Si besoin ajouter un mouvement arrière de la camera pour voir la roche de l'exterieur
    }

    void Deactivate()
    {
        this.isCamouflaged = false;

        playerGraphics.SetActive(true);
        camoGraphics.SetActive(false);

        this.activationTimer = 0;
        this.playerController.canMove = true;
    }
}

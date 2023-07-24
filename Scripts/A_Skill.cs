using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class A_Skill : MonoBehaviour
{
    [SerializeField] protected float activationTime = -1f;
    [SerializeField] public float cooldown = -1f;

    public float cooldownTimer = 0;
    public float activationTimer = 0;

    public abstract void OnButtonPressed();

    public abstract void OnButtonReleased();

    public abstract void OnSkillDeselected();
}

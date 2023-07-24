using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessTrigger : MonoBehaviour
{
    [SerializeField] LightController lightController;
    [SerializeField] bool isExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (isExit)
            {
                lightController.PlayerLeavesDarkness();
            }
            else
            {
                lightController.PlayerEnterDarkness();
            }
        }
    }
}
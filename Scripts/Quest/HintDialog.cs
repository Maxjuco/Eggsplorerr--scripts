using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintDialog : MonoBehaviour
{

    [SerializeField] CanPick canPick;
    [SerializeField] GameObject notificationPanel;
    private string thoughtsText;

    private S_PlayerPickUp playerPickUp;

    private void Awake()
    {
        if(tag == "Wolf")
        {
            playerPickUp = GameObject.FindGameObjectWithTag("Player").GetComponent<S_PlayerPickUp>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if((tag != "Rabbit" && !playerPickUp.canPickRacoon) || (canPick != null && !canPick.pickable))
                StartCoroutine(DisplayHint());
        }
    }

    private IEnumerator DisplayHint()
    {
        switch (tag)
        {
            case "Rabbit":
                thoughtsText = "Ce lapin court trop vite ! Il me faudrait un appât pour l'attraper.";
                break;
            case "Wolf":
                thoughtsText = "Ce loup est très territorial. Peut-être que je pourrais le distraire avec un appât ?";
                break;
        }
        notificationPanel.SetActive(true);
        notificationPanel.GetComponentInChildren<TextMeshProUGUI>().SetText(thoughtsText);
        yield return new WaitForSeconds(2f);
        notificationPanel.SetActive(false);
    }
}

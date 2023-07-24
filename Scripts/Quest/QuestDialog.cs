using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class QuestDialog : MonoBehaviour
{

    Collider detectionArea;



    public GameObject dialogBox;
    public GameObject notificationBox;

    public bool goalIsReach;

    public bool inReach;

    public int countSentences = 0;


    private void Awake()
    {
        detectionArea = GetComponent<SphereCollider>();
        dialogBox = GameObject.FindGameObjectsWithTag("DialogBox")[0];
        dialogBox.SetActive(false);
        notificationBox= GameObject.FindGameObjectsWithTag("DialogBox")[0];
        notificationBox.SetActive(false);

        goalIsReach = false;
        inReach = false;
    }

    // Update is called once per frame
    void Update()
    {
    }


    private void OnTriggerEnter(Collider other)
    {
        inReach = true;
        notificationBox.SetActive(true);
        TextMeshProUGUI txtNotif = notificationBox.transform.Find("NotificationText").GetComponent<TextMeshProUGUI>();
        txtNotif.SetText("APPUYER SUR [T] POUR PARLER");

    }
    private void endQuest()
    {
        dialogBox.SetActive(true);

        TextMeshProUGUI txt = dialogBox.transform.Find("DialogText").GetComponent<TextMeshProUGUI>();//.GetChild(dialogBox.transform.childCount - 1).GetComponent<TextMeshProUGUI>();
        Debug.Log(txt);
        txt.SetText("VIEIL HOMME:\n BRAVO, TON INITIATION EST TERMINÉE !");

        StartCoroutine(loadDelay());
    }

    private IEnumerator giveQuest()
    {
        dialogBox.SetActive(true);

        TextMeshProUGUI txt = dialogBox.transform.Find("DialogText").GetComponent<TextMeshProUGUI>();//.GetChild(dialogBox.transform.childCount - 1).GetComponent<TextMeshProUGUI>();
        notificationBox?.SetActive(false);
        switch (countSentences)
        {
            case 0:
                txt.SetText("VIEIL HOMME:\n VA RÉCUPÉRER L'OEUF MAGIQUE DANS CETTE GROTTE !");
                countSentences = 1;
                break;
            case 1:
                txt.SetText("VIEIL HOMME:\n LA GROTTE EST SOMBRE ET GARDÉE PAR UN IMMENSE GARDIEN.");
                countSentences = 2;
                break;
            case 2:
                txt.SetText("VIEIL HOMME:\n MAIS UN OEUF DE POUVOIR SE CACHE DANS CETTE VALLÉE. IL POURRA PEUT-ÊTRE T'AIDER !");
                yield return new WaitForSeconds(4f);
                countSentences = 0;
                dialogBox.SetActive(false);
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogBox?.SetActive(false);
            notificationBox?.SetActive(false);
        }

        inReach = false;
    }

    private void OnTriggerStay(Collider other)
    {
        Vector3 lookAtPosition = other.transform.position;
        lookAtPosition.y = transform.position.y;
        this.transform.parent.transform.LookAt(lookAtPosition);
    }

    public void DialogWithPlayer()
    {
        //if victory condition not reach:
        if (!goalIsReach && inReach)
            StartCoroutine(giveQuest());
        else if(goalIsReach)
            endQuest();
    }

    public void unlockSkill(int i)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<S_PlayerControllerNew>().skillUnlocked.Add(i);
    }

    public void getFirstEgg()
    {
        notificationBox.SetActive(true);
        TextMeshProUGUI txt = notificationBox.transform.Find("NotificationText").GetComponent<TextMeshProUGUI>();
        txt.SetText("VISION NOCTURNE DÉBLOQUÉE !");
        unlockSkill(0);
        StartCoroutine(displayDelay(txt, false));
    }

    public void getCaveEgg()
    {
        notificationBox.SetActive(true);
        TextMeshProUGUI txt = notificationBox.transform.Find("NotificationText").GetComponent<TextMeshProUGUI>();
        txt.SetText("CAMOUFLAGE DÉBLOQUÉ !");
        goalIsReach = true;
        unlockSkill(1);
        StartCoroutine(displayDelay(txt, true));
    }

    private IEnumerator displayDelay(TextMeshProUGUI txt, bool isQuestGoal)
    {
        
        yield return new WaitForSeconds(2);
        if (!isQuestGoal)
            notificationBox.SetActive(false);
        else
        {
            txt.SetText("RAPPORTEZ L'OEUF AU VIEIL HOMME");

            yield return new WaitForSeconds(2);
            notificationBox.SetActive(false);
        }

    }

    private IEnumerator loadDelay()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

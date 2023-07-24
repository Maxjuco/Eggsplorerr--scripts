using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class S_RacoonController : MonoBehaviour
{
    [SerializeField] float delay = 3f;
    [SerializeField] GameObject hint;
    [SerializeField] Cinemachine.CinemachineVirtualCamera Vcam;
    [SerializeField] ParticleSystem eggParticle;
    
    private Animator anim;
    private CharacterController characterController;
    private bool isStanding = false;

    public bool raccoonFled = false;

    [SerializeField] GameObject notificationPanel;
    private string thoughtsText;

    IEnumerator Start()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        while(true)
        {
            yield return new WaitForSeconds(delay);

            if (Random.Range(0, 10) < 2) isStanding = !isStanding; // 20% chance de changer de position
            anim.SetBool("isStanding", isStanding);
            anim.SetInteger("idleAnimIndex", Random.Range(0, 4));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            anim.SetBool("isHiding", true);
            hint.SetActive(true);

            //display charachter thoughts :
            StartCoroutine(displayThoughts());
        } 
        else if (other.gameObject.layer == LayerMask.NameToLayer("Wolf"))
        {
            StartCoroutine(onWolfApproaching(other.gameObject));
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            anim.SetBool("isHiding", false);
            hint.SetActive(false);
        }
    }

    private IEnumerator onWolfApproaching(GameObject wolf)
    {
        Debug.Log("Wolf coming !");

        // TODO: Bouger la caméra devant
        Vcam.Priority = 100;
        yield return new WaitForSeconds(1f);
        eggParticle.Play();

        hint.SetActive(true);
        transform.rotation = Quaternion.LookRotation(transform.position - wolf.transform.position);

        anim.SetTrigger("RunAway");
        StartCoroutine(RunAway());

        yield return new WaitForSeconds(2f);
        Vcam.Priority = 1;
        Destroy(gameObject);
    }

    private IEnumerator RunAway()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<S_PlayerPickUp>().canPickRacoon = true;
        while (true) {
            characterController.Move((transform.forward * 5f + Vector3.down * 9.8f) * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator displayThoughts()
    {
        thoughtsText = "JE NE PEUX PAS M'APPROCHER DE CET OEUF, CE RATON LAVEUR NE BOUGERA PAS.";
        notificationPanel.SetActive(true);
        notificationPanel.GetComponentInChildren<TextMeshProUGUI>().SetText(thoughtsText);
        yield return new WaitForSeconds(2f);
        thoughtsText = "PEUT-ÊTRE QU'UN PRÉDATEUR POURRAIT LE FAIRE FUIR ?";
        notificationPanel.GetComponentInChildren<TextMeshProUGUI>().SetText(thoughtsText);
        yield return new WaitForSeconds(2f);
        notificationPanel.SetActive(false);
    }
}

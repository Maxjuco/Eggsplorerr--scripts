using System;
using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class S_PlayerPickUp : MonoBehaviour
{

    private PlayerInput _playerInput;

    [SerializeField]
    private Transform cameraLocation;
    [SerializeField]
    public Transform pickUpLocation;
    private LayerMask pickableObjectMask;
    private LayerMask collectibleObjectMask;
    private bool canPick = false;
    public bool canPickRacoon = false;
    [SerializeField]
    public AudioSource AudioItemPickUp;
    [SerializeField]
    public AudioSource AudioEggPickUp;
    [SerializeField]
    public AudioSource AudioItemDrop;
    [SerializeField]
    public int maxPickUpDistance = 5;
    [SerializeField]
    private int moveForce = 1;
    [SerializeField]
    private int throwForce = 1;

    [SerializeField]
    public Animator animator;

    public GameObject _heldObject;

    private void Awake()
    {
        this._playerInput = GetComponent<PlayerInput>();
        this.pickableObjectMask = LayerMask.GetMask("Rabbit");
        this.collectibleObjectMask = LayerMask.GetMask("Collectibles");
    }

    // Update is called once per frame
    void Update()
    {
        //checkPick();
    }

    private void checkPick()
    {

        //moving the object : 
        if (_heldObject != null)
        {//an object is picked 
            MoveObject();
        }
    }

    public void OnPick(InputAction.CallbackContext cxt)
    {
        LayerMask carrotMask = LayerMask.GetMask("Carrot");
        if (cxt.performed){
            //check if the player is looking at a pickable object
            RaycastHit lookDirection;
            if ((Physics.Raycast(cameraLocation.transform.position, cameraLocation.transform.forward, out lookDirection, maxPickUpDistance, pickableObjectMask) || Physics.Raycast(cameraLocation.transform.position, cameraLocation.transform.forward, out lookDirection, maxPickUpDistance, carrotMask)) && _heldObject == null)
            {
                
                if(lookDirection.transform.gameObject.GetComponent<CanPick>() == null)
                {
                    AudioItemPickUp.Play();
                    PickUpObject(lookDirection.transform.gameObject);
                }
                else if (lookDirection.transform.gameObject.GetComponent<CanPick>().Pickable)
                {
                    {
                        AudioItemPickUp.Play();
                        PickUpObject(lookDirection.transform.gameObject);
                    }
                }
                return;
            }
            else if (Physics.Raycast(cameraLocation.transform.position, cameraLocation.transform.forward, out lookDirection, maxPickUpDistance, collectibleObjectMask))
            {
                Debug.Log("Collecting items !");
                AudioEggPickUp.Play();
                CollectItem(lookDirection.transform.gameObject);
            }
            else
            {
                
                if (_heldObject != null)
                {//an object is picked 
                    AudioItemDrop.Play();
                        DropObject();
                }
                else
                    Debug.Log("no object to pick up !");
            }
        }
    }

    private void CollectItem(GameObject gameObject)
    {
        //indicate to the QuestDialog to display what item have been pickup : 
        QuestDialog questDialog = GameObject.FindGameObjectWithTag("QuestManager").GetComponent<QuestDialog>();
        if (gameObject.name == "RacoonEgg" && canPickRacoon)
        {
            questDialog.getFirstEgg();
            gameObject.SetActive(false);
        }
        if (gameObject.name == "CaveEgg")
        {
            questDialog.getCaveEgg();
            gameObject.SetActive(false);
        }
    }

    private void PickUpObject(GameObject pickedObj)
    {
        Rigidbody objRig = pickedObj.GetComponent<Rigidbody>();
        if (objRig)
        {
            objRig.isKinematic = true;
            objRig.useGravity = false;
            objRig.drag = 20; //higher the drag is the harder is the object to move
            objRig.angularDrag = 0.5f;

            objRig.transform.position = pickUpLocation.transform.position;
            objRig.transform.parent = pickUpLocation;
            _heldObject = pickedObj;

            //set animation : 
            animator.SetLayerWeight(1, .9f);

            pickedObj.transform.localEulerAngles = new Vector3(90, 0, 140);

            if (objRig.TryGetComponent(out ParticleSystem particleSystem))
            {
                particleSystem.Stop(false);
            }
        }
    }

    private void MoveObject()
    {
        if (Vector3.Distance(_heldObject.transform.position, pickUpLocation.position) > 0.1f)
        {
            Vector3 moveDir = (pickUpLocation.position - _heldObject.transform.position);
            _heldObject.GetComponent<Rigidbody>().AddForce(moveDir * moveForce);
        }
    }

    private void DropObject()
    {
        Rigidbody objRig = _heldObject.GetComponent<Rigidbody>();
        objRig.isKinematic = false;
        objRig.useGravity = true;
        objRig.drag = 0;
        objRig.angularDrag = 0.05f;

        objRig.transform.parent = null;
        objRig.AddForce(pickUpLocation.transform.forward * throwForce);
        _heldObject = null;
        animator.SetLayerWeight(1, 0f);

        if (objRig.TryGetComponent(out ParticleSystem particleSystem))
        {
            particleSystem.Play();
        }
    }
}

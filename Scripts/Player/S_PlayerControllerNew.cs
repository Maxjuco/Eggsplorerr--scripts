using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class S_PlayerControllerNew : MonoBehaviour
{
    CharacterController characterController;
    PlayerInput playerInput;

    // Health/Damage
    [Header("Health/Damage")]
    [SerializeField] Volume hitMarker1;
    [SerializeField] Volume hitMarker2;
    [SerializeField] Volume hitMarker3;

    // Skills
    [Header("Skills")]
    [SerializeField] S_RadialMenuController radialMenuController;
    [SerializeField] public List<A_Skill> skillList;
    [SerializeField] GameObject NightVisionSprite;
    [SerializeField] GameObject CamouflageSprite;

    [HideInInspector] public int skillSelected = 0; // Remplacer par une r�f�rence au script du skill ou d'une fonction si besoin
    [HideInInspector] public List<int> skillUnlocked;
    [HideInInspector] public GameObject notificationBox;



    // Movements
    [Header("Movement")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float sprintSpeedBonus = 1.5f;
    Vector3 currentMovement;
    [HideInInspector] public bool canMove = true;

    // Jump
    [Header("Jump")]
    [SerializeField] float jumpHeight = 1f;
    [SerializeField] float coyoteTime = 0.1f;
    [SerializeField] float jumpBufferTime = 0.1f;
    float coyoteTimeCounter;
    float jumpBufferCounter;
    bool jumpButtonPressed;
    bool isJumping;

    float gravityValue = 9.81f;

    // Camera
    [Header("Camera")]
    [SerializeField] GameObject cameraObject;
    [SerializeField] float cameraSensibilityX;
    [SerializeField] float cameraSensibilityY;
    [SerializeField] float maxAngleY;
    [SerializeField] float minAngleY;
    Vector2 currentMouseMovement;
    float verticalRotation = 0f;

    //Animator:
    [Header("Graphic Model")]
    [SerializeField] Animator animator;
    [SerializeField] Transform eyes;

    private void Awake()
    {
        this.characterController = GetComponent<CharacterController>();
        this.playerInput = GetComponent<PlayerInput>();
        this.skillUnlocked = new List<int>();
        this.notificationBox = GameObject.FindGameObjectsWithTag("DialogBox")[0];
        this.notificationBox.SetActive(false);
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        this.ProcessLook();
        this.ProcessMovement();
        this.ProcessAnimation();
    }

    private void ProcessAnimation()
    {
        //if(hips.localPosition.y > 82f)
        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Jump")
        {
            cameraObject.transform.position = new Vector3(cameraObject.transform.position.x, eyes.position.y+0.5f, cameraObject.transform.position.z);
        }
        else
        {
            cameraObject.transform.localPosition = new Vector3(0, 0, 0);
        }

        if (currentMovement.x != 0f || currentMovement.z != 0f)
        {
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Running", false);
        }
    }

    void ProcessMovement()
    {
        // Jumping
        if (this.characterController.isGrounded)
        {
            isJumping = false;
            currentMovement.y = -gravityValue * Time.deltaTime;
            coyoteTimeCounter = 0f;
        }
        else
        {
            currentMovement.y -= gravityValue * Time.deltaTime;
            coyoteTimeCounter += Time.deltaTime; // Count the time in air
        }

        if (jumpButtonPressed)
        {
            jumpBufferCounter = 0;
            jumpButtonPressed = false;
        }
        else
        {
            jumpBufferCounter += Time.deltaTime; // Count the time after jump button pressed
        }

        if (!isJumping && coyoteTimeCounter < coyoteTime && jumpBufferCounter < jumpBufferTime)
        {
            isJumping = true;
            currentMovement.y = Mathf.Sqrt(jumpHeight * gravityValue);
        }

        // Apply move
        this.characterController.Move(this.transform.rotation * currentMovement * Time.deltaTime * this.moveSpeed);
    }

    void ProcessLook()
    {
        // Left / Right
        this.transform.Rotate(this.transform.up, this.currentMouseMovement.x * cameraSensibilityX * Time.deltaTime);

        // Up / Down
        this.verticalRotation -= this.currentMouseMovement.y * cameraSensibilityX * Time.deltaTime;
        this.verticalRotation = Mathf.Clamp(this.verticalRotation, this.minAngleY, this.maxAngleY);
        Vector2 targetAngle = cameraObject.transform.eulerAngles;
        targetAngle.x = this.verticalRotation;
        this.cameraObject.transform.eulerAngles = targetAngle;
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (!isJumping && canMove)
        {

            animator.SetTrigger("Jumping");
            jumpButtonPressed = true;
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (!canMove)
        {
            currentMovement = Vector3.zero;
            return;
        }

        Vector2 inputValue = ctx.ReadValue<Vector2>();
        this.currentMovement.x = inputValue.x;
        this.currentMovement.z = inputValue.y;
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        this.currentMouseMovement = ctx.ReadValue<Vector2>();
    }

    public void OnSkillMenuPress(InputAction.CallbackContext ctx)
    {
        if (!radialMenuController) return;

        if (ctx.started) // Bouton enfoncé
        {
            radialMenuController.OpenMenu();
            playerInput.actions.FindActionMap("UI").Enable();
        }
        else if (ctx.canceled)  // Bouton relaché
        {
            int newSkillSelected = radialMenuController.currentOptionSelected;
            //check if the skill is unlocked : 
            if (skillUnlocked.Contains(newSkillSelected))
            {
                int currentSkill = this.skillSelected;
                this.skillSelected = newSkillSelected;
                if (currentSkill < skillList.Count && currentSkill != skillSelected)
                {
                    skillList[currentSkill].OnSkillDeselected();
                }
                switch (this.skillSelected)
                {
                    case 0:
                        NightVisionSprite.SetActive(true);
                        CamouflageSprite.SetActive(false);
                        break;
                    case 1:
                        NightVisionSprite.SetActive(false);
                        CamouflageSprite.SetActive(true);
                        break;
                    default:
                        NightVisionSprite.SetActive(false);
                        CamouflageSprite.SetActive(false);
                        break;
                }
                Debug.Log("Selected skill " + this.skillSelected);
            }
            else
            {
                Debug.Log("This skill is still locked : " + this.skillSelected);
                StartCoroutine(DisplaySkillNotUnlocked());

            }

            radialMenuController.CloseMenu();
            playerInput.actions.FindActionMap("UI").Disable();
        }
    }

    private IEnumerator DisplaySkillNotUnlocked()
    {
        this.notificationBox.SetActive(true);
        TextMeshProUGUI txt = this.notificationBox.transform.Find("NotificationText").GetComponent<TextMeshProUGUI>();
        txt.SetText("Ce pouvoir n'est pas encore débloqué !");
        yield return new WaitForSeconds(2);

        this.notificationBox.SetActive(false);
    }

    public void OnSkillPress(InputAction.CallbackContext ctx)
    {
        if (skillSelected < skillList.Count && skillUnlocked.Contains(skillSelected))
        {
            if (ctx.started)
            {
                skillList[skillSelected].OnButtonPressed();
            }
            else if (ctx.canceled)
            {
                skillList[skillSelected].OnButtonReleased();
            }
        }
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            moveSpeed += sprintSpeedBonus;
        }
        else if (ctx.canceled)
        {
            moveSpeed -= sprintSpeedBonus;
        }
    }

    public void OnTalk(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
            GameObject.FindGameObjectWithTag("QuestManager").GetComponent<QuestDialog>().DialogWithPlayer();
    }

    public void UpdateHitMarker(int health)
    {
        switch (health)
        {
            case 1:
                hitMarker1.enabled = false;
                hitMarker2.enabled = false;
                hitMarker3.enabled = true;
                break;
            case 2:
                hitMarker1.enabled = false;
                hitMarker2.enabled = true;
                hitMarker3.enabled = false;
                break;
            case 3:
                hitMarker1.enabled = true;
                hitMarker2.enabled = false;
                hitMarker3.enabled = false;
                break;
            default:
                hitMarker1.enabled = false;
                hitMarker2.enabled = false;
                hitMarker3.enabled = false;
                break;
        }
    }
}


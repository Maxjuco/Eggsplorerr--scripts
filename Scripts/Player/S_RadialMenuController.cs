using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class S_RadialMenuController : MonoBehaviour
{
    [SerializeField] GameObject radialMenuUI;
    [SerializeField] Image[] options;

    [SerializeField] Color baseColor;
    [SerializeField] Color hoverColor;

    Vector2 currentMousePosition;
    bool isMenuOpened = false;
    public int currentOptionSelected { get; private set; } = 0;
    
    void Update()
    {
        if(isMenuOpened)
        {
            this.ProcessMouse();
            this.HoverEffect();
        }
    }

    void ProcessMouse()
    {
        Vector2 mousePosition = new Vector2(currentMousePosition.x - Screen.width / 2f, currentMousePosition.y - Screen.height / 2f);
        mousePosition.Normalize();

        float angle = Mathf.Atan2(mousePosition.y, -mousePosition.x);
        if (angle < 0)
        {
            angle += Mathf.PI * 2;
        }

        SelectOptions(angle);
    }

    void SelectOptions(float angleRad)
    {
        float optionAngle = Mathf.PI * 2 / 6;
        currentOptionSelected = (int) Mathf.Floor(angleRad / optionAngle);
    }

    void HoverEffect()
    {
        for (int i = 0; i < options.Length; i++)
        {
            if (i == currentOptionSelected)
            {
                options[i].GetComponent<Outline>().enabled = true;
            }
            else
            {
                options[i].GetComponent<Outline>().enabled = false;
            }
        }
    }

    public void OpenMenu()
    {
        isMenuOpened = true;
        radialMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseMenu()
    {
        isMenuOpened = false;
        radialMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnMouseMove(InputAction.CallbackContext ctx)
    {
        this.currentMousePosition = ctx.ReadValue<Vector2>();
    }
}

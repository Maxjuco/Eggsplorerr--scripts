using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class exitCredit : MonoBehaviour
{

    public void onScrollEnd()
    {
        Debug.Log("FIN");

        SceneManager.LoadScene("MainMenu2");
    }
}

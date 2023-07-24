using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cinemachine;

public class S_TrailerCamManager : MonoBehaviour
{
    [SerializeField] List<CinemachineVirtualCamera> vcamList;
    [SerializeField] List<float> timePerCamera;

    private void Start()
    {
        StartCoroutine(ShowCameraOneByOne());
    }

    IEnumerator ShowCameraOneByOne()
    {
        int i = 0;
        while(i < vcamList.Count)
        {
            yield return new WaitForSeconds(timePerCamera[i]);
            vcamList[i].gameObject.SetActive(false);
            i++;
        }
    }
}

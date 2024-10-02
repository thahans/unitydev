using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonManager : MonoBehaviour
{
    public GameObject InfoSc;

    public void oyungiris()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    public void infopageactive()
    {
        InfoSc.SetActive(true);
    }

    public void closeinfopage()
    {
        InfoSc.SetActive(false);
    }
}

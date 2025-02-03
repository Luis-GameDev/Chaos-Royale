using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitButtons : MonoBehaviour
{
    [SerializeField] private GameObject login;
    [SerializeField] private GameObject register;
    [SerializeField] private GameObject buttons;

    public void LoginButton()
    {
        login.SetActive(true);
        register.SetActive(false);
        buttons.SetActive(false);
    }

    public void RegisterButton()
    {
        login.SetActive(false);
        register.SetActive(true);
        buttons.SetActive(false);
    }

    public void BackButton()
    {
        login.SetActive(false);
        register.SetActive(false);
        buttons.SetActive(true);
    }

    public void Exit() {
        # if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        # else
        Application.Quit();
        # endif
    }
}

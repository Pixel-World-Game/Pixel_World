using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUi : MonoBehaviour
{
    public Button StartBnt;

    public Button ExitBtn; 

    void Start()
    {
        StartBnt.onClick.AddListener(() =>
        {
            //SceneManager.LoadScene("Game1");
        });

        ExitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}

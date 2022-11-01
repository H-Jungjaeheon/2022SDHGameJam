using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public GameObject GameStart_Button;
    public GameObject Setting_Button;
    public GameObject Setting_Window;

    public void Setting()
    {
        GameStart_Button.gameObject.SetActive(false);
        Setting_Button.gameObject.SetActive(false);
        Setting_Window.gameObject.SetActive(true);
    }

    public void Setting_Window_OK()
    {
        Setting_Window.gameObject.SetActive(false);
        GameStart_Button.gameObject.SetActive(true);
        Setting_Button.gameObject.SetActive(true);
    }
}

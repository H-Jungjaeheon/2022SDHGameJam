using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public GameObject GameStart_Button;
    public GameObject Setting_Button;
    public GameObject Setting_Window;
    public GameObject Credit_Window;

    public AudioMixer Effectmixer;      // 오디오 변수 설정
    public AudioMixer BackGroundmixer;
    public Slider Effectslider;
    public Slider BackGroundslider;
    public Text EffectSoundNum;
    public Text BackGroundSoundNum;
    private int Sound_Num1 = 0;
    private int Sound_Num2 = 0;

    void Start()
    {
        Screen.SetResolution(1600, 900, true);
        First();
    }

    void Update()
    {
        EffectSoundNum.text = Sound_Num1.ToString();
        BackGroundSoundNum.text = Sound_Num2.ToString();
    }

    public void Setting_Reset() // 설정 초기화
    {
        Effectslider.value = Sound_Num1 = 3;
        BackGroundslider.value = Sound_Num2 = 2;
        Effectmixer.SetFloat("MusicVol", 5);
        BackGroundmixer.SetFloat("MusicVol", 3);
        Screen.SetResolution(1600, 900, true);
    }

    public void EffectSetLevel(float silderValue1)  // 환경음 설정
    {
        Effectmixer.SetFloat("MusicVol", Mathf.Log10(silderValue1) * 10);
    }

    public void BackGroundSetLevel(float silderValue2)  // 배경음 설정
    {
        BackGroundmixer.SetFloat("MusicVol", Mathf.Log10(silderValue2) * 10);
    }

    public void EffectSoundNumRight_Control()   // 효과음 점점 크게
    {
        Sound_Num1++;
        Effectslider.value = Sound_Num1;
        if (Sound_Num1 >= 10)
        {
            Sound_Num1 = 10;
        }
    }

    public void EffectSoundNumLeft_Control()    // 효과음 점점 작게
    {
        Sound_Num1--;
        Effectslider.value = Sound_Num1;
        if (Sound_Num1 <= 0)
        {
            Sound_Num1 = 0;
        }
    }

    public void BackGroundSoundNumRight_Control()   // 배경음 점점 크게
    {
        Sound_Num2++;
        BackGroundslider.value = Sound_Num2;
        if (Sound_Num2 >= 10)
        {
            Sound_Num2 = 10;
        }
    }

    public void BackGroundSoundNumLeft_Control()    // 배경음 점점 작게
    {
        Sound_Num2--;
        BackGroundslider.value = Sound_Num2;
        if (Sound_Num2 <= 0)
        {
            Sound_Num2 = 0;
        }
    }

    public void Setting()   // 설정 창 활성화
    {
        GameStart_Button.gameObject.SetActive(false);
        Setting_Button.gameObject.SetActive(false);
        Setting_Window.gameObject.SetActive(true);
    }

    public void Setting_Window_OK() // 메인 화면 활성화
    {
        Setting_Window.gameObject.SetActive(false);
        GameStart_Button.gameObject.SetActive(true);
        Setting_Button.gameObject.SetActive(true);
    }

    private void First()    // 처음 설정 값
    {
        Effectslider.value = Sound_Num1 = 3;
        BackGroundslider.value = Sound_Num2 = 2;
        Effectmixer.SetFloat("MusicVol", 5);
        BackGroundmixer.SetFloat("MusicVol", 3);
    }

    public void CreditOpen()    // 크레딧
    {
        Setting_Window.gameObject.SetActive(false);
        Credit_Window.gameObject.SetActive(true);
    }

    public void CreditClose()
    {
        Setting_Window.gameObject.SetActive(true);
        Credit_Window.gameObject.SetActive(false);
    }
}

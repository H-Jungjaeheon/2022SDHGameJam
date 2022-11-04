using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QnAButton : MonoBehaviour
{
    [SerializeField]
    [Tooltip("ù ��° ������")]
    private GameObject firstPage;

    [SerializeField]
    [Tooltip("�� ��° ������")]
    private GameObject secondPage;

    [Tooltip("���� �ؽ�Ʈ")]
    public TextMeshProUGUI QuestionText;

    [Tooltip("�亯 �ؽ�Ʈ")]
    public TextMeshProUGUI AnswerText;

    public int nowQuestionIndex;

    public void PageChange(bool isTurnToFirstPage)
    {
        firstPage.SetActive(isTurnToFirstPage);
        secondPage.SetActive(!isTurnToFirstPage);
    }

    public void Destroy()
    {
        IngameManager.Instance.determiningDuplicateQuestions[nowQuestionIndex] = false;
        Destroy(gameObject); 
    }
}

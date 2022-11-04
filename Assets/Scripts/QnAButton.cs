using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QnAButton : MonoBehaviour
{
    [SerializeField]
    [Tooltip("첫 번째 페이지")]
    private GameObject firstPage;

    [SerializeField]
    [Tooltip("두 번째 페이지")]
    private GameObject secondPage;

    [Tooltip("질문 텍스트")]
    public TextMeshProUGUI QuestionText;

    [Tooltip("답변 텍스트")]
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

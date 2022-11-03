using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public enum NowGameState
{
    GameReady,
    Gaming,
    GameEnd
}

public class IngameManager : Singleton<IngameManager>
{
    [Header("���� ���� ����")]
    [Tooltip("���� ����")]
    public string nowAnswer;

    [Tooltip("�Է��� ����")]
    private string nowInputAnswer;
    
    [Space(20)]

    [Header("���� ���� ����")]
    [SerializeField]
    [Tooltip("���� �����")]
    private string[] questions;
    
    [SerializeField]
    [Tooltip("���� ���信 ���� �������� ��¥ ��")]
    private Button[] questionCorrectAnswer;

    [SerializeField]
    [Tooltip("���� �������� �ѱ�� ��ư ������Ʈ")]
    private GameObject goNextQuestionButtonObj;

    [SerializeField]
    [Tooltip("���� �������� �ѱ�� ��ư")]
    private Button goNextQuestionButton;

    [SerializeField]
    [Tooltip("���� ��ư �ؽ�Ʈ")]
    private TextMeshProUGUI[] answerButtonText;

    [SerializeField]
    [Tooltip("���� ��ư ������Ʈ")]
    private GameObject questionButtonsObj;

    [SerializeField]
    [Tooltip("���� ��ư")]
    private Button[] questionButtons;


    private bool[] determiningDuplicateQuestions = new bool[25];

    private int[] buttonQuestionIndex = new int[3];

    [Space(20)]

    [SerializeField]
    [Tooltip("�ΰ��� - �⺻���� UI���� ���� ������Ʈ")]
    private GameObject basicUisObj;

    [SerializeField]
    [Tooltip("���� �Է� â ������Ʈ")]
    private GameObject InputAnswerObj;

    [SerializeField]
    [Tooltip("�������� �ȳ� �����׿� ���� �̹��� ��Ʈ Ʈ������")]
    private RectTransform stageGuideImageRectTransform;

    [SerializeField]
    [Tooltip("��� ��Ͽ� ��ũ�� �� ��Ʈ Ʈ������")]
    private RectTransform qnaScrollViewRectTransform;

    [SerializeField]
    [Tooltip("���ΰ��� ��ǳ�� ������Ʈ")]
    private GameObject detectiveSpeechBubble;

    [SerializeField]
    [Tooltip("������ ��ǳ�� ������Ʈ")]
    private GameObject suspectSpeechBubble;

    [SerializeField]
    [Tooltip("���ΰ��� �ؽ�Ʈ")]
    private Text detectiveText;

    [SerializeField]
    [Tooltip("������ �ؽ�Ʈ")]
    private Text suspectText;

    [SerializeField]
    [Tooltip("���� ����� �ϴ� ���� �ȳ� �ؽ�Ʈ")]
    private TextMeshProUGUI typeOfAnswerText;

    [SerializeField]
    [Tooltip("���� �������� �ȳ� �ؽ�Ʈ")]
    private TextMeshProUGUI stageText;

    [SerializeField]
    [Tooltip("���� �������� ���� ���� �ȳ� �ؽ�Ʈ")]
    private TextMeshProUGUI stageGuideText;

    [SerializeField]
    [Tooltip("�ִ� ���ѽð�")]
    private int maxLimitTime;

    [SerializeField]
    [Tooltip("���ѽð� �̹���")]
    private Image limitTimeImageBar;

    private float nowLimitTime;

    private bool isTurnOn;

    public float NowLimitTime
    {
        get { return nowLimitTime; }
        set 
        {
            if (value > 0)
            {
                nowLimitTime = value;
            }
            else
            {
                nowLimitTime = 0;
            }
            limitTimeImageBar.fillAmount = nowLimitTime / maxLimitTime;
        }
    }

    private NowGameState nowGameState;

    private void Awake()
    {
        nowLimitTime = maxLimitTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        typeOfAnswerText.text = $"{GameManager.Instance.nowFindThingType} ��(��) ������!";
        stageText.text = $"STAGE{GameManager.Instance.nowStageIndex}";
        stageGuideText.text = $"������ ���� ���� {GameManager.Instance.nowFindThingType}�� �˾Ƴ���!";
        StartCoroutine(RandQuestionSetting()); //���� �Ŀ� �ٷιٷ� ����
        RandAnserSetting();
        StartCoroutine(StartAnim());
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    private void RandAnserSetting()
    {
        int nowRandIndex = Random.Range(0, 5);

        nowAnswer = GameManager.Instance.nowStageCorrectAnswerList[nowRandIndex];
    }

    IEnumerator StartAnim()
    {
        WaitForSeconds sayDelay = new WaitForSeconds(2.5f);

        nowGameState = NowGameState.GameReady;

        suspectSpeechBubble.SetActive(true);
        suspectText.DOText("�� ���� ���...\n�������� �����ô�.", 1.5f);

        yield return sayDelay;

        detectiveSpeechBubble.SetActive(true);
        detectiveText.DOText("���� ������ �����ϰڽ��ϴ�.\n�����ϰ� �����ֽñ� �ٶ��ϴ�.", 1.5f);

        yield return sayDelay;

        suspectSpeechBubble.SetActive(false);
        detectiveSpeechBubble.SetActive(false);

        stageGuideImageRectTransform.DOAnchorPos(new Vector3(0,0,0), 1.5f);

        yield return sayDelay;

        stageGuideImageRectTransform.DOAnchorPos(new Vector3(0,-645,0), 0.5f);

        yield return sayDelay;

        isTurnOn = true;
        qnaScrollViewRectTransform.DOAnchorPos(new Vector3(0, -60, 0), 0.25f);
        basicUisObj.SetActive(true);
        StartCoroutine(Timer());
        nowGameState = NowGameState.Gaming;

        questionButtonsObj.SetActive(true);
    }

    IEnumerator RandQuestionSetting() //���� ���� ����
    {
        int randIndex;
        for (int nowIndex = 0; nowIndex < 3; nowIndex++)
        {
            while (true)
            {
                randIndex = Random.Range(0, 25);
                if (determiningDuplicateQuestions[randIndex] == false)
                {
                    int nowRandIndex = randIndex;
                    int nowButtonIndex = nowIndex;

                    answerButtonText[nowIndex].text = questions[randIndex];
                    buttonQuestionIndex[nowButtonIndex] = nowRandIndex;
                    questionButtons[nowIndex].onClick.AddListener(() => AnswerButtonClick(nowRandIndex));
                    break;
                }
            }
        }
        yield return null;
    }

    IEnumerator Timer()
    {
        while (true)
        {
            NowLimitTime -= Time.deltaTime;
            if (NowLimitTime <= 0)
            {
                break;
            }
            yield return null;
        }
    }

    public void MoveScrollView()
    {
        if (nowGameState == NowGameState.Gaming)
        {
            if (isTurnOn)
            {
                isTurnOn = false;
                qnaScrollViewRectTransform.DOAnchorPos(new Vector3(-500, -60, 0), 0.25f);
            }
            else
            {
                isTurnOn = true;
                qnaScrollViewRectTransform.DOAnchorPos(new Vector3(0, -60, 0), 0.25f);
            }
        }
    }

    public void InputAnswerObjSetActive(bool isSetActiveTrue)
    {
        if (isSetActiveTrue)
        {
            InputAnswerObj.SetActive(isSetActiveTrue);
        }
        else
        {
            InputAnswerObj.SetActive(isSetActiveTrue);
        }
    }

    public void AnswerButtonClick(int nowAnswerIndex) //���� �ε���
    {
        determiningDuplicateQuestions[nowAnswerIndex] = true;
        questionButtonsObj.SetActive(false);
        StartCoroutine(RandQuestionSetting());

        StartCoroutine(QNA(nowAnswerIndex));
    }

    IEnumerator QNA(int nowAnswerIndex)
    {
        WaitForSeconds sayDelay = new WaitForSeconds(2f);
        int randAnswer = Random.Range(1, 8);
        
        detectiveText.text = "";
        detectiveSpeechBubble.SetActive(true); //���ΰ� ���â ����
        detectiveText.DOText($"{questions[nowAnswerIndex]}", 1.5f);

        yield return sayDelay;

        suspectText.text = "";
        suspectSpeechBubble.SetActive(true); //���� ���â ����

        SuspectAnswer(randAnswer);

        yield return sayDelay;

        goNextQuestionButtonObj.SetActive(true);
    }

    public void PressGoNextQuestionButton()
    {
        detectiveSpeechBubble.SetActive(false);
        suspectSpeechBubble.SetActive(false);
        goNextQuestionButtonObj.SetActive(false);
        questionButtonsObj.SetActive(true);
    }

    private void SuspectAnswer(int answerKind)
    {
        switch (answerKind)
        {
            case 1:
                suspectText.DOText($"�½��ϴ�.", 1.5f);
                break;
            case 2:
                suspectText.DOText($"�ణ �׷����ϴ�.", 1.5f);
                break;
            case 3:
                suspectText.DOText($"�ƴմϴ�.", 1.5f);
                break;
            case 4:
                suspectText.DOText($"�ణ �ƴմϴ�.", 1.5f);
                break;
            case 5:
                suspectText.DOText($"�ָ��մϴ�.", 1.5f);
                break;
            case 6:
                suspectText.DOText($"�������� �ʽ��ϴ�.", 1.5f);
                break;
            case 7:
                suspectText.DOText($"��Ȳ�� ���� �ٸ� �� �����ϴ�.", 1.5f);
                break;
        }
    }
}

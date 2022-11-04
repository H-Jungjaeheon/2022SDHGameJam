using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

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

    [Space(20)]

    [Header("���� ���� ����")]
    [SerializeField]
    [Tooltip("���� �����")]
    private string[] questions;
    
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

    [HideInInspector]
    public bool[] determiningDuplicateQuestions = new bool[25];

    private int[] nowSelectedIndexes = new int[3];

    private int rightNowAnswerIndex;

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
    [Tooltip("�������� ���� �� ��� �ؽ�Ʈ")]
    private TextMeshProUGUI stageEndText;

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
    [Tooltip("�ۼ��� ���� �ؽ�Ʈ")]
    private InputField wroteAnswer;

    [SerializeField]
    [Tooltip("�ִ� ���ѽð�")]
    private int maxLimitTime;

    [SerializeField]
    [Tooltip("���ѽð� �̹���")]
    private Image limitTimeImageBar;

    [SerializeField]
    [Tooltip("��ũ�� ����Ʈ ������Ʈ")]
    private GameObject scrollContent;

    [SerializeField]
    [Tooltip("��ũ�� ����Ʈ�� �߰��� QnA ������Ʈ")]
    private GameObject QnAObj;

    [SerializeField]
    [Tooltip("���� ������ ���� â ������Ʈ")]
    private GameObject gameEndObj;

    private float nowLimitTime;

    private bool isTurnOn;

    private bool isOpenInputAnswerObjAble;

    public bool isAnswering;

    private bool isSkillUseAble;

    private bool nowSkillUsing;

    private TextMeshProUGUI nowQnAObjAnswerText;

    private QnAButton qnaComponent;

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
                stageEndText.text = "Game Over...";
                nowLimitTime = 0;
                gameEndObj.SetActive(true);
            }
            limitTimeImageBar.fillAmount = nowLimitTime / maxLimitTime;
        }
    }
    
    [SerializeField]
    [Tooltip("��ų ��Ÿ�� �̹���")]
    private Image skillCoolTimeImage;

    [SerializeField]
    [Tooltip("�ִ� ��ų ��Ÿ��")]
    public float maxSkillCoolTime;

    public float nowSkillCoolTime;

    public float NowSkillCoolTime
    {
        get { return nowSkillCoolTime; }
        set 
        {
            if (value <= 0)
            {
                nowSkillCoolTime = 0;
            }
            nowSkillCoolTime = value;
            skillCoolTimeImage.fillAmount = NowSkillCoolTime / maxSkillCoolTime;
        }
    }

    private NowGameState nowGameState;

    private void Awake()
    {
        isOpenInputAnswerObjAble = true;
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

    // Update is called once per frame
    void Update()
    {
        Timer();
        if (NowSkillCoolTime > 0)
        {
            NowSkillCoolTime -= Time.deltaTime;
        }
    }

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
        
        nowGameState = NowGameState.Gaming;

        questionButtonsObj.SetActive(true);
    }

    IEnumerator RandQuestionSetting() //���� ���� ����
    {
        int randIndex = 0;
        
        
        for (int nowIndex = 0; nowIndex < 3; nowIndex++)
        {
            while (true)
            {
                randIndex = Random.Range(0, 25);
                if (determiningDuplicateQuestions[randIndex] == false)
                {
                    determiningDuplicateQuestions[randIndex] = true;
                    answerButtonText[nowIndex].text = questions[randIndex];
                    nowSelectedIndexes[nowIndex] = randIndex;
                    break;
                }
            }

        }

        for (int nowIndex = 0; nowIndex < nowSelectedIndexes.Length; nowIndex++)
        {
            int nowButtonIndex = nowIndex;
            questionButtons[nowButtonIndex].onClick.AddListener(() => AnswerButtonClick(nowSelectedIndexes[nowButtonIndex], nowButtonIndex));
        }

        yield return null;
    }

    private void Timer()
    {
        if (NowLimitTime > 0 && nowGameState == NowGameState.Gaming)
        {
            NowLimitTime -= Time.deltaTime;
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
        if (isOpenInputAnswerObjAble)
        {
            InputAnswerObj.SetActive(isSetActiveTrue);
            questionButtonsObj.SetActive(!isSetActiveTrue);
        }
    }

    public void AnswerButtonClick(int nowAnswerIndex, int buttonIndex) //���� �ε���
    {
        isOpenInputAnswerObjAble = false;
        if (isAnswering == false)
        {
            isAnswering = true;

            for (int nowIndex = 0; nowIndex < 3; nowIndex++)
            {
                if (nowIndex != buttonIndex)
                {
                    determiningDuplicateQuestions[nowSelectedIndexes[nowIndex]] = false;
                }
            }

            determiningDuplicateQuestions[nowAnswerIndex] = true;
            questionButtonsObj.SetActive(false);

            StartCoroutine(QNA(nowAnswerIndex));
            StartCoroutine(RandQuestionSetting());
        }
    }

    IEnumerator QNA(int nowAnswerIndex)
    {
        WaitForSeconds sayDelay = new WaitForSeconds(2f);
        int probabilityOfTheAnswer = Random.Range(1, 101);
        int randAnswer = Random.Range(1, 8);
        
        detectiveText.text = "";
        detectiveSpeechBubble.SetActive(true); //���ΰ� ���â ����
        detectiveText.DOText($"{questions[nowAnswerIndex]}", 1.5f);
        rightNowAnswerIndex = nowAnswerIndex;

        yield return sayDelay;

        suspectText.text = "";
        suspectSpeechBubble.SetActive(true); //���� ���â ����

        if (probabilityOfTheAnswer <= 45)
        {
            var correctAnswerInstance = CorrectAnswer.Instance;

            switch (nowAnswer)
            {
                case "����":
                    SuspectAnswer(correctAnswerInstance.CorrectAnswerWhenPark[nowAnswerIndex]);
                    break;
                case "������":
                    SuspectAnswer(correctAnswerInstance.CorrectAnswerWhenConstructionSite[nowAnswerIndex]);
                    break;
                case "����":
                    SuspectAnswer(correctAnswerInstance.CorrectAnswerWhenAlleys[nowAnswerIndex]);
                    break;
                case "��":
                    SuspectAnswer(correctAnswerInstance.CorrectAnswerWhenDesertedHouse[nowAnswerIndex]);
                    break;
                case "�б�":
                    SuspectAnswer(correctAnswerInstance.CorrectAnswerWhenSchool[nowAnswerIndex]);
                    break;
            }
        }
        else
        {
            SuspectAnswer(randAnswer);
        }

        yield return sayDelay;

        isAnswering = false;

        GameObject nowSpawnQnAObj = Instantiate(QnAObj, transform.position, QnAObj.transform.rotation);
        nowSpawnQnAObj.transform.SetParent(scrollContent.transform);
        qnaComponent = nowSpawnQnAObj.GetComponent<QnAButton>();

        qnaComponent.nowQuestionIndex = nowAnswerIndex;
        qnaComponent.QuestionText.text = detectiveText.text;
        qnaComponent.AnswerText.text = suspectText.text;
        nowSpawnQnAObj.transform.localScale = new Vector3(1, 1, 1);

        nowQnAObjAnswerText = qnaComponent.AnswerText;
        nowQnAObjAnswerText.text = qnaComponent.AnswerText.text;

        isSkillUseAble = true;
        goNextQuestionButtonObj.SetActive(true);
    }

    public void PressGoNextQuestionButton()
    {
        isSkillUseAble = false;
        isOpenInputAnswerObjAble = true;
        detectiveSpeechBubble.SetActive(false);
        suspectSpeechBubble.SetActive(false);
        goNextQuestionButtonObj.SetActive(false);
        questionButtonsObj.SetActive(true);
    }

    public void IsCorrectAnswer()
    {
        StartCoroutine(CorrectAnswerAnim());
    }

    IEnumerator CorrectAnswerAnim()
    {
        isOpenInputAnswerObjAble = false;
        if (wroteAnswer.text == nowAnswer)
        {
            InputAnswerObj.SetActive(false);
            stageEndText.text = "Game Clear!";
            gameEndObj.SetActive(true);
        }
        else
        {
            InputAnswerObj.SetActive(false);
            wroteAnswer.text = "";
            suspectText.text = "";
            NowLimitTime -= 25;
            suspectSpeechBubble.SetActive(true);
            suspectText.DOText($"���� �װ��� ���� �ʾҽ��ϴ�..", 1.5f);
            yield return new WaitForSeconds(2);
            goNextQuestionButtonObj.SetActive(true);
        }
        yield return null;
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

    private void SkillSuspectAnswer(int answerKind) //�ؽ�Ʈ �ٲٱ�
    {
        switch (answerKind)
        {
            case 1:
                nowQnAObjAnswerText.text = $"�½��ϴ�.";
                break;
            case 2:
                nowQnAObjAnswerText.text = $"�ణ �׷����ϴ�.";
                break;
            case 3:
                nowQnAObjAnswerText.text = $"�ƴմϴ�.";
                break;
            case 4:
                nowQnAObjAnswerText.text = $"�ణ �ƴմϴ�.";
                break;
            case 5:
                nowQnAObjAnswerText.text = $"�ָ��մϴ�.";
                break;
            case 6:
                nowQnAObjAnswerText.text = $"�������� �ʽ��ϴ�.";
                break;
            case 7:
                nowQnAObjAnswerText.text = $"��Ȳ�� ���� �ٸ� �� �����ϴ�.";
                break;
        }
    }

    public void UseSkill() => StartCoroutine(SkillEvent());

    IEnumerator SkillEvent()
    {
        if (nowSkillCoolTime <= 0 && isSkillUseAble && nowSkillUsing == false)
        {
            print("tlfgod");

            WaitForSeconds sayDelay = new WaitForSeconds(2f);

            nowSkillUsing = true;

            detectiveText.text = "";
            detectiveSpeechBubble.SetActive(true); //���ΰ� ���â ����
            detectiveText.DOText($"���� �� ���� �½��ϱ�?", 1.5f);

            yield return sayDelay;

            suspectText.text = "";
            suspectSpeechBubble.SetActive(true); //���� ���â ����
            suspectText.DOText($"..�̰� �´� ���Դϴ�.", 1.5f);

            var correctAnswerInstance = CorrectAnswer.Instance;

            switch (nowAnswer)
            {
                case "����":
                    SkillSuspectAnswer(correctAnswerInstance.CorrectAnswerWhenPark[rightNowAnswerIndex]);
                    break;
                case "������":
                    SkillSuspectAnswer(correctAnswerInstance.CorrectAnswerWhenConstructionSite[rightNowAnswerIndex]);
                    break;
                case "����":
                    SkillSuspectAnswer(correctAnswerInstance.CorrectAnswerWhenAlleys[rightNowAnswerIndex]);
                    break;
                case "��":
                    SkillSuspectAnswer(correctAnswerInstance.CorrectAnswerWhenDesertedHouse[rightNowAnswerIndex]);
                    break;
                case "�б�":
                    SkillSuspectAnswer(correctAnswerInstance.CorrectAnswerWhenSchool[rightNowAnswerIndex]);
                    break;
            }

            yield return sayDelay;

            goNextQuestionButtonObj.SetActive(true);
            nowSkillUsing = false;
            nowSkillCoolTime = maxSkillCoolTime;
            yield return null;
        }
    }

    public void GoToTitle() => SceneManager.LoadScene("Title");

    public void ReStart() => SceneManager.LoadScene("Ingame");
}

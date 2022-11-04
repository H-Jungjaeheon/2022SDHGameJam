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
    [Header("정답 관련 변수")]
    [Tooltip("현재 정답")]
    public string nowAnswer;

    [Space(20)]

    [Header("질문 관련 변수")]
    [SerializeField]
    [Tooltip("질문 내용들")]
    private string[] questions;
    
    [SerializeField]
    [Tooltip("다음 질문으로 넘기기 버튼 오브젝트")]
    private GameObject goNextQuestionButtonObj;

    [SerializeField]
    [Tooltip("다음 질문으로 넘기기 버튼")]
    private Button goNextQuestionButton;

    [SerializeField]
    [Tooltip("질문 버튼 텍스트")]
    private TextMeshProUGUI[] answerButtonText;

    [SerializeField]
    [Tooltip("질문 버튼 오브젝트")]
    private GameObject questionButtonsObj;

    [SerializeField]
    [Tooltip("질문 버튼")]
    private Button[] questionButtons;

    [HideInInspector]
    public bool[] determiningDuplicateQuestions = new bool[25];

    private int[] nowSelectedIndexes = new int[3];

    private int rightNowAnswerIndex;

    [Space(20)]

    [SerializeField]
    [Tooltip("인게임 - 기본적인 UI들을 모은 오브젝트")]
    private GameObject basicUisObj;

    [SerializeField]
    [Tooltip("정답 입력 창 오브젝트")]
    private GameObject InputAnswerObj;

    [SerializeField]
    [Tooltip("스테이지 안내 오프닝에 쓰일 이미지 렉트 트랜스폼")]
    private RectTransform stageGuideImageRectTransform;

    [SerializeField]
    [Tooltip("대답 기록용 스크롤 뷰 렉트 트랜스폼")]
    private RectTransform qnaScrollViewRectTransform;

    [SerializeField]
    [Tooltip("주인공의 말풍선 오브젝트")]
    private GameObject detectiveSpeechBubble;

    [SerializeField]
    [Tooltip("범인의 말풍선 오브젝트")]
    private GameObject suspectSpeechBubble;

    [SerializeField]
    [Tooltip("주인공의 텍스트")]
    private Text detectiveText;

    [SerializeField]
    [Tooltip("범인의 텍스트")]
    private Text suspectText;

    [SerializeField]
    [Tooltip("스테이지 종료 시 결과 텍스트")]
    private TextMeshProUGUI stageEndText;

    [SerializeField]
    [Tooltip("현재 맞춰야 하는 주제 안내 텍스트")]
    private TextMeshProUGUI typeOfAnswerText;

    [SerializeField]
    [Tooltip("현재 스테이지 안내 텍스트")]
    private TextMeshProUGUI stageText;

    [SerializeField]
    [Tooltip("현재 스테이지 정답 내용 안내 텍스트")]
    private TextMeshProUGUI stageGuideText;

    [SerializeField]
    [Tooltip("작성한 정답 텍스트")]
    private InputField wroteAnswer;

    [SerializeField]
    [Tooltip("최대 제한시간")]
    private int maxLimitTime;

    [SerializeField]
    [Tooltip("제한시간 이미지")]
    private Image limitTimeImageBar;

    [SerializeField]
    [Tooltip("스크롤 리스트 오브젝트")]
    private GameObject scrollContent;

    [SerializeField]
    [Tooltip("스크롤 리스트에 추가될 QnA 오브젝트")]
    private GameObject QnAObj;

    [SerializeField]
    [Tooltip("게임 끝나면 띄우는 창 오브젝트")]
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
    [Tooltip("스킬 쿨타임 이미지")]
    private Image skillCoolTimeImage;

    [SerializeField]
    [Tooltip("최대 스킬 쿨타임")]
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
        typeOfAnswerText.text = $"{GameManager.Instance.nowFindThingType} 을(를) 맞혀라!";
        stageText.text = $"STAGE{GameManager.Instance.nowStageIndex}";
        stageGuideText.text = $"취조를 통해 범행 {GameManager.Instance.nowFindThingType}를 알아내자!";
        StartCoroutine(RandQuestionSetting()); //질문 후에 바로바로 실행
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
        suspectText.DOText("거 형사 양반...\n빨리빨리 끝냅시다.", 1.5f);

        yield return sayDelay;

        detectiveSpeechBubble.SetActive(true);
        detectiveText.DOText("이제 취조를 시작하겠습니다.\n솔직하게 답해주시길 바랍니다.", 1.5f);

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

    IEnumerator RandQuestionSetting() //랜덤 질문 세팅
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

    public void AnswerButtonClick(int nowAnswerIndex, int buttonIndex) //질문 인덱스
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
        detectiveSpeechBubble.SetActive(true); //주인공 대사창 띄우기
        detectiveText.DOText($"{questions[nowAnswerIndex]}", 1.5f);
        rightNowAnswerIndex = nowAnswerIndex;

        yield return sayDelay;

        suspectText.text = "";
        suspectSpeechBubble.SetActive(true); //범인 대사창 띄우기

        if (probabilityOfTheAnswer <= 45)
        {
            var correctAnswerInstance = CorrectAnswer.Instance;

            switch (nowAnswer)
            {
                case "공원":
                    SuspectAnswer(correctAnswerInstance.CorrectAnswerWhenPark[nowAnswerIndex]);
                    break;
                case "공사장":
                    SuspectAnswer(correctAnswerInstance.CorrectAnswerWhenConstructionSite[nowAnswerIndex]);
                    break;
                case "골목길":
                    SuspectAnswer(correctAnswerInstance.CorrectAnswerWhenAlleys[nowAnswerIndex]);
                    break;
                case "폐가":
                    SuspectAnswer(correctAnswerInstance.CorrectAnswerWhenDesertedHouse[nowAnswerIndex]);
                    break;
                case "학교":
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
            suspectText.DOText($"저는 그곳에 있지 않았습니다..", 1.5f);
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
                suspectText.DOText($"맞습니다.", 1.5f);
                break;
            case 2:
                suspectText.DOText($"약간 그렇습니다.", 1.5f);
                break;
            case 3:
                suspectText.DOText($"아닙니다.", 1.5f);
                break;
            case 4:
                suspectText.DOText($"약간 아닙니다.", 1.5f);
                break;
            case 5:
                suspectText.DOText($"애매합니다.", 1.5f);
                break;
            case 6:
                suspectText.DOText($"생각나지 않습니다.", 1.5f);
                break;
            case 7:
                suspectText.DOText($"상황에 따라서 다른 것 같습니다.", 1.5f);
                break;
        }
    }

    private void SkillSuspectAnswer(int answerKind) //텍스트 바꾸기
    {
        switch (answerKind)
        {
            case 1:
                nowQnAObjAnswerText.text = $"맞습니다.";
                break;
            case 2:
                nowQnAObjAnswerText.text = $"약간 그렇습니다.";
                break;
            case 3:
                nowQnAObjAnswerText.text = $"아닙니다.";
                break;
            case 4:
                nowQnAObjAnswerText.text = $"약간 아닙니다.";
                break;
            case 5:
                nowQnAObjAnswerText.text = $"애매합니다.";
                break;
            case 6:
                nowQnAObjAnswerText.text = $"생각나지 않습니다.";
                break;
            case 7:
                nowQnAObjAnswerText.text = $"상황에 따라서 다른 것 같습니다.";
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
            detectiveSpeechBubble.SetActive(true); //주인공 대사창 띄우기
            detectiveText.DOText($"정말 그 답이 맞습니까?", 1.5f);

            yield return sayDelay;

            suspectText.text = "";
            suspectSpeechBubble.SetActive(true); //범인 대사창 띄우기
            suspectText.DOText($"..이게 맞는 답입니다.", 1.5f);

            var correctAnswerInstance = CorrectAnswer.Instance;

            switch (nowAnswer)
            {
                case "공원":
                    SkillSuspectAnswer(correctAnswerInstance.CorrectAnswerWhenPark[rightNowAnswerIndex]);
                    break;
                case "공사장":
                    SkillSuspectAnswer(correctAnswerInstance.CorrectAnswerWhenConstructionSite[rightNowAnswerIndex]);
                    break;
                case "골목길":
                    SkillSuspectAnswer(correctAnswerInstance.CorrectAnswerWhenAlleys[rightNowAnswerIndex]);
                    break;
                case "폐가":
                    SkillSuspectAnswer(correctAnswerInstance.CorrectAnswerWhenDesertedHouse[rightNowAnswerIndex]);
                    break;
                case "학교":
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

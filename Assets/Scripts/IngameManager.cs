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
    [Header("정답 관련 변수")]
    [Tooltip("현재 정답")]
    public string nowAnswer;

    [Tooltip("입력한 정답")]
    private string nowInputAnswer;
    
    [Space(20)]

    [Header("질문 관련 변수")]
    [SerializeField]
    [Tooltip("질문 내용들")]
    private string[] questions;
    
    [SerializeField]
    [Tooltip("현재 정답에 대한 질문들의 진짜 답")]
    private Button[] questionCorrectAnswer;

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


    private bool[] determiningDuplicateQuestions = new bool[25];

    private int[] buttonQuestionIndex = new int[3];

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
    [Tooltip("현재 맞춰야 하는 주제 안내 텍스트")]
    private TextMeshProUGUI typeOfAnswerText;

    [SerializeField]
    [Tooltip("현재 스테이지 안내 텍스트")]
    private TextMeshProUGUI stageText;

    [SerializeField]
    [Tooltip("현재 스테이지 정답 내용 안내 텍스트")]
    private TextMeshProUGUI stageGuideText;

    [SerializeField]
    [Tooltip("최대 제한시간")]
    private int maxLimitTime;

    [SerializeField]
    [Tooltip("제한시간 이미지")]
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
        typeOfAnswerText.text = $"{GameManager.Instance.nowFindThingType} 을(를) 맞혀라!";
        stageText.text = $"STAGE{GameManager.Instance.nowStageIndex}";
        stageGuideText.text = $"취조를 통해 범행 {GameManager.Instance.nowFindThingType}를 알아내자!";
        StartCoroutine(RandQuestionSetting()); //질문 후에 바로바로 실행
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
        StartCoroutine(Timer());
        nowGameState = NowGameState.Gaming;

        questionButtonsObj.SetActive(true);
    }

    IEnumerator RandQuestionSetting() //랜덤 질문 세팅
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

    public void AnswerButtonClick(int nowAnswerIndex) //질문 인덱스
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
        detectiveSpeechBubble.SetActive(true); //주인공 대사창 띄우기
        detectiveText.DOText($"{questions[nowAnswerIndex]}", 1.5f);

        yield return sayDelay;

        suspectText.text = "";
        suspectSpeechBubble.SetActive(true); //범인 대사창 띄우기

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
}

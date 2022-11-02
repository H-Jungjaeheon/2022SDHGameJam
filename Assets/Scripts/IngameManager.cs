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
    [SerializeField]
    [Tooltip("인게임 - 기본적인 UI들을 모은 오브젝트")]
    private GameObject basicUisObj;

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
        StartCoroutine(StartAnim());
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    IEnumerator StartAnim()
    {
        WaitForSeconds sayDelay = new WaitForSeconds(2f);

        nowGameState = NowGameState.GameReady;

        suspectSpeechBubble.SetActive(true);
        suspectText.DOText("거 형사 양반...\n빨리빨리 끝냅시다.", 1.5f);

        yield return sayDelay;

        detectiveSpeechBubble.SetActive(true);
        detectiveText.DOText("이제 취조를 시작하겠습니다.\n솔직하게 답해주시길 바랍니다.", 1.5f);

        yield return sayDelay;

        suspectSpeechBubble.SetActive(false);
        detectiveSpeechBubble.SetActive(false);

        stageGuideImageRectTransform.DOAnchorPos(new Vector3(0,0,0), 1);

        yield return sayDelay;

        stageGuideImageRectTransform.DOAnchorPos(new Vector3(0,-645,0), 0.5f);

        yield return sayDelay;

        isTurnOn = true;
        qnaScrollViewRectTransform.DOAnchorPos(new Vector3(0, -60, 0), 0.25f);
        basicUisObj.SetActive(true);
        StartCoroutine(Timer());
        nowGameState = NowGameState.Gaming;
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
}

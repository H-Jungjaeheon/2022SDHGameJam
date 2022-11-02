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
    [Tooltip("�ΰ��� - �⺻���� UI���� ���� ������Ʈ")]
    private GameObject basicUisObj;

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
        suspectText.DOText("�� ���� ���...\n�������� �����ô�.", 1.5f);

        yield return sayDelay;

        detectiveSpeechBubble.SetActive(true);
        detectiveText.DOText("���� ������ �����ϰڽ��ϴ�.\n�����ϰ� �����ֽñ� �ٶ��ϴ�.", 1.5f);

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

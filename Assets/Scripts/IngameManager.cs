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
    [Tooltip("�������� �ȳ� �����׿� ���� �̹��� ��Ʈ Ʈ������")]
    private RectTransform stageGuideImageRectTransform;

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

    private NowGameState nowGameState;

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
        WaitForSeconds sayDelay = new WaitForSeconds(2.7f);

        nowGameState = NowGameState.GameReady;

        suspectText.DOText("�� ���� ���...\n�������� �����ô�.", 2);

        yield return sayDelay;

        detectiveText.DOText("���� ������ �����ϰڽ��ϴ�.\n�ִ��� �����ϰ� �����ֽñ� �ٶ��ϴ�.", 2);

        yield return sayDelay;

        suspectSpeechBubble.SetActive(false);
        detectiveSpeechBubble.SetActive(false);

        stageGuideImageRectTransform.DOAnchorPos(new Vector3(0,0,0), 5f);
    }
}

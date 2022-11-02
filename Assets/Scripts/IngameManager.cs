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
    [Tooltip("스테이지 안내 오프닝에 쓰일 이미지 렉트 트랜스폼")]
    private RectTransform stageGuideImageRectTransform;

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

        suspectText.DOText("거 형사 양반...\n빨리빨리 끝냅시다.", 2);

        yield return sayDelay;

        detectiveText.DOText("이제 취조를 시작하겠습니다.\n최대한 솔직하게 답해주시길 바랍니다.", 2);

        yield return sayDelay;

        suspectSpeechBubble.SetActive(false);
        detectiveSpeechBubble.SetActive(false);

        stageGuideImageRectTransform.DOAnchorPos(new Vector3(0,0,0), 5f);
    }
}

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum StageKind
{
    Tutorial,
    Stage1,
    Stage2,
    Stage3,
    Stage4,
    Stage5,
    StageLength
}

public class StageSelectManager : MonoBehaviour
{
    private string[,] listOfAnswersByStage = new string[6, 5];

    void Start()
    {
        for (int nowStageIndex = 0; nowStageIndex < (int)StageKind.StageLength; nowStageIndex++)
        {
            for (int nowAnswerIndex = 0; nowAnswerIndex < (int)StageKind.StageLength; nowAnswerIndex++)
            {
                switch (nowStageIndex)
                {
                    case (int)StageKind.Tutorial:
                        switch (nowAnswerIndex)
                        {
                            case 0:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "4명";
                                break;
                            case 1:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "";
                                break;
                            case 2:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "";
                                break;
                            case 3:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "";
                                break;
                            case 4:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "";
                                break;
                        }
                        break;
                    case (int)StageKind.Stage1:
                        switch (nowAnswerIndex)
                        {
                            case 0:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "공원";
                                break;
                            case 1:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "공사장";
                                break;
                            case 2:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "골목길";
                                break;
                            case 3:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "폐가";
                                break;
                            case 4:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "학교";
                                break;
                        }
                        break;
                    case (int)StageKind.Stage2:
                        switch (nowAnswerIndex)
                        {
                            case 0:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "사시미";
                                break;
                            case 1:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "콘파스";
                                break;
                            case 2:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "커터칼";
                                break;
                            case 3:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "면도칼";
                                break;
                            case 4:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "가위";
                                break;
                        }
                        break;
                    case (int)StageKind.Stage3:
                        switch (nowAnswerIndex)
                        {
                            case 0:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "야구배트";
                                break;
                            case 1:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "해머";
                                break;
                            case 2:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "각목";
                                break;
                            case 3:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "몽키스패너";
                                break;
                            case 4:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "삼단봉";
                                break;
                        }
                        break;
                    case (int)StageKind.Stage4:
                        switch (nowAnswerIndex)
                        {
                            case 0:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "주먹";
                                break;
                            case 1:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "독살";
                                break;
                            case 2:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "함정";
                                break;
                            case 3:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "덫";
                                break;
                            case 4:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "폭발물";
                                break;
                        }
                        break;
                    case (int)StageKind.Stage5:
                        switch (nowAnswerIndex)
                        {
                            case 0:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "오전 9시";
                                break;
                            case 1:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "오전 1시";
                                break;
                            case 2:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "오후 8시";
                                break;
                            case 3:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "오전 3시";
                                break;
                            case 4:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "오후 11시";
                                break;
                        }
                        break;
                }
            }
        }
    }

    public void PressStageButton(int nowPressedStageIndex)
    {
        var gmInstance = GameManager.Instance;
        StageKind nowPressedStageKind = (StageKind)nowPressedStageIndex;

        for (int nowArrayIndex = 0; nowArrayIndex < gmInstance.nowStageCorrectAnswerList.Length; nowArrayIndex++)
        {
            gmInstance.nowStageCorrectAnswerList[nowArrayIndex] = listOfAnswersByStage[(int)nowPressedStageKind, nowArrayIndex];
        }
        
        gmInstance.nowStageIndex = nowPressedStageIndex;

        switch (nowPressedStageIndex)
        {
            case (int)StageKind.Tutorial:
                gmInstance.nowFindThingType = "사람 수";
                break;
            case (int)StageKind.Stage1:
                gmInstance.nowFindThingType = "장소";
                break;
            case (int)StageKind.Stage2:
                gmInstance.nowFindThingType = "흉기";
                break;
            case (int)StageKind.Stage3:
                gmInstance.nowFindThingType = "둔기";
                break;
            case (int)StageKind.Stage4:
                gmInstance.nowFindThingType = "특수 무기";
                break;
            case (int)StageKind.Stage5:
                gmInstance.nowFindThingType = "사망 시간";
                break;
        }
        
        SceneManager.LoadScene("IngameTest");
    }
}

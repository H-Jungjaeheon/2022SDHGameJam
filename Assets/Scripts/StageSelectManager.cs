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
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "4��";
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
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "����";
                                break;
                            case 1:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "������";
                                break;
                            case 2:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "����";
                                break;
                            case 3:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "��";
                                break;
                            case 4:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "�б�";
                                break;
                        }
                        break;
                    case (int)StageKind.Stage2:
                        switch (nowAnswerIndex)
                        {
                            case 0:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "��ù�";
                                break;
                            case 1:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "���Ľ�";
                                break;
                            case 2:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "Ŀ��Į";
                                break;
                            case 3:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "�鵵Į";
                                break;
                            case 4:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "����";
                                break;
                        }
                        break;
                    case (int)StageKind.Stage3:
                        switch (nowAnswerIndex)
                        {
                            case 0:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "�߱���Ʈ";
                                break;
                            case 1:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "�ظ�";
                                break;
                            case 2:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "����";
                                break;
                            case 3:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "��Ű���г�";
                                break;
                            case 4:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "��ܺ�";
                                break;
                        }
                        break;
                    case (int)StageKind.Stage4:
                        switch (nowAnswerIndex)
                        {
                            case 0:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "�ָ�";
                                break;
                            case 1:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "����";
                                break;
                            case 2:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "����";
                                break;
                            case 3:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "��";
                                break;
                            case 4:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "���߹�";
                                break;
                        }
                        break;
                    case (int)StageKind.Stage5:
                        switch (nowAnswerIndex)
                        {
                            case 0:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "���� 9��";
                                break;
                            case 1:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "���� 1��";
                                break;
                            case 2:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "���� 8��";
                                break;
                            case 3:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "���� 3��";
                                break;
                            case 4:
                                listOfAnswersByStage[nowStageIndex, nowAnswerIndex] = "���� 11��";
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
                gmInstance.nowFindThingType = "��� ��";
                break;
            case (int)StageKind.Stage1:
                gmInstance.nowFindThingType = "���";
                break;
            case (int)StageKind.Stage2:
                gmInstance.nowFindThingType = "���";
                break;
            case (int)StageKind.Stage3:
                gmInstance.nowFindThingType = "�б�";
                break;
            case (int)StageKind.Stage4:
                gmInstance.nowFindThingType = "Ư�� ����";
                break;
            case (int)StageKind.Stage5:
                gmInstance.nowFindThingType = "��� �ð�";
                break;
        }
        
        SceneManager.LoadScene("IngameTest");
    }
}

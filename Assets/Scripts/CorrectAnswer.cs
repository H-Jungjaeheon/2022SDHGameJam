using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectAnswer : Singleton<CorrectAnswer>
{
    [Tooltip("정답이 공원일 때 질문별 맞는 답변")]
    public int[] CorrectAnswerWhenPark;

    [Tooltip("정답이 공사장일 때 질문별 맞는 답변")]
    public int[] CorrectAnswerWhenConstructionSite;

    [Tooltip("정답이 골목길일 때 질문별 맞는 답변")]
    public int[] CorrectAnswerWhenAlleys;

    [Tooltip("정답이 폐가일 때 질문별 맞는 답변")]
    public int[] CorrectAnswerWhenDesertedHouse;

    [Tooltip("정답이 학교일 때 질문별 맞는 답변")]
    public int[] CorrectAnswerWhenSchool;
}

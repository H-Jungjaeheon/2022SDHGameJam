using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectAnswer : Singleton<CorrectAnswer>
{
    [Tooltip("������ ������ �� ������ �´� �亯")]
    public int[] CorrectAnswerWhenPark;

    [Tooltip("������ �������� �� ������ �´� �亯")]
    public int[] CorrectAnswerWhenConstructionSite;

    [Tooltip("������ ������ �� ������ �´� �亯")]
    public int[] CorrectAnswerWhenAlleys;

    [Tooltip("������ ���� �� ������ �´� �亯")]
    public int[] CorrectAnswerWhenDesertedHouse;

    [Tooltip("������ �б��� �� ������ �´� �亯")]
    public int[] CorrectAnswerWhenSchool;
}

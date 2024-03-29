﻿using UnityEngine;

public static class GameConstant
{
    public static readonly float JUDGE_OFFSET_ENTRY = 60f;
    public static readonly float JUDGE_OFFSET_NORMAL = 45f;
    public static readonly float JUDGE_OFFSET_EXACT = 15f;
    public static readonly int JUDGE_SCORE_0 = 50;
    public static readonly int JUDGE_SCORE_1 = 100;
    public static readonly float JUDGE_MISS_LIFE_PENALTY = 0.1f;
    public static readonly float JUDGE_SUCCESS_LIFE_PRICE = 0.02f;

    public static readonly Color DUMMY_LIFE_COLOR_GREEN = new Color (196/255.0f, 1f, 78/255.0f, 1f); 
    public static readonly Color DUMMY_LIFE_COLOR_YELLOW = new Color (1f, 167/255.0f, 78/255.0f, 1f);
    public static readonly Color DUMMY_LIFE_COLOR_RED = new Color (183/255.0f, 0f, 0f, 1f);     
    public static readonly Color NOTE_COLOR_TINT = new Color (1f, 254/255.0f, 177/255.0f, 1f);
}

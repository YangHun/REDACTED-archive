using UnityEngine;

public static class GameConstant
{
    public static readonly float JUDGE_OFFSET_ENTRY = 110f;
    public static readonly float JUDGE_OFFSET_NORMAL = 100f;
    public static readonly float JUDGE_OFFSET_EXACT = 40f;
    public static readonly int JUDGE_SCORE_0 = 100;
    public static readonly int JUDGE_SCORE_1 = 50;
    public static readonly float JUDGE_MISS_LIFE_PENALTY = 0.1f;
    public static readonly float JUDGE_SUCCESS_LIFE_PRICE = 0.02f;

    public static readonly Color DUMMY_LIFE_COLOR_GREEN = new Color (196/255.0f, 1f, 78/255.0f, 1f); 
    public static readonly Color DUMMY_LIFE_COLOR_YELLOW = new Color (1f, 167/255.0f, 78/255.0f, 1f);
    public static readonly Color DUMMY_LIFE_COLOR_RED = new Color (183/255.0f, 0f, 0f, 1f);     
}

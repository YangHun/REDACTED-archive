using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : Singleton<GameUI>
{
    public TextMeshProUGUI score;
    public Slider life;
    public TextMeshProUGUI judge;

    private Coroutine judgeCoroutine;

    public void UpdateScoreText (int value) {
        score.text = string.Format("Score:{0}",value);
    }

    public void UpdateLifeBar (float value) {
        life.value = value;
        if (value > 0.5f) {
            life.fillRect.GetComponentInChildren<Image>().color = GameConstant.DUMMY_LIFE_COLOR_GREEN;
        }
        else if (value > 0.2f) {
            life.fillRect.GetComponentInChildren<Image>().color = GameConstant.DUMMY_LIFE_COLOR_YELLOW;
        }
        else {
            life.fillRect.GetComponentInChildren<Image>().color = GameConstant.DUMMY_LIFE_COLOR_RED;
        }

    }

    public void UpdateJudgeText (string text) {
        if (judgeCoroutine != null) StopCoroutine (judgeCoroutine);
        judge.text = text;
        judge.color = Color.white;
        judgeCoroutine = StartCoroutine (DelayTextFade(judge, 0.0f, 0.5f, 0.0f));
    }

    private IEnumerator DelayTextFade (TextMeshProUGUI text, float delay, float duration, float to) {
        float timer = 0.0f;
        while (timer < delay) {
            yield return null;
            timer += Time.fixedDeltaTime;
        }

        timer = 0.0f;
        Color32 start = text.color;
        Color32 end = start;
        end.a = (byte)to;

        while (timer <= duration) {
            text.color = Color32.Lerp(start, end, timer / duration);
            yield return null;
            timer += Time.fixedDeltaTime;
        }
        
        text.color = Color32.Lerp(start, end, 1);
    }
}

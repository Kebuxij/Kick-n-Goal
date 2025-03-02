using UnityEngine;
using System.Collections;
using TMPro;  // Подключаем TextMeshPro

public class ScoreDisplayFade : MonoBehaviour
{
    public TMP_Text team1ScoreText;  // Используем TMP_Text вместо Text
    public TMP_Text team2ScoreText;
    public float fadeDuration = 3f;

    public void DisplayScore(int team1Score, int team2Score)
    {
        team1ScoreText.text = team1Score.ToString();
        team2ScoreText.text = team2Score.ToString();

        SetAlpha(team1ScoreText, 1f);
        SetAlpha(team2ScoreText, 1f);

        StartCoroutine(FadeOut(team1ScoreText));
        StartCoroutine(FadeOut(team2ScoreText));
    }

    private IEnumerator FadeOut(TMP_Text text)
    {
        float elapsedTime = 0f;
        Color originalColor = text.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            SetAlpha(text, alpha);
            yield return null;
        }

        SetAlpha(text, 0f);
    }

    private void SetAlpha(TMP_Text text, float alpha)
    {
        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }
}

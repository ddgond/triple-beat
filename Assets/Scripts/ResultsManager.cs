using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultsManager : MonoBehaviour
{
    public ScrollingNumber score;
    public ScrollingNumber combo;
    public ScrollingRank rank;
    public ScrollingNumber miss;
    public ScrollingNumber good;
    public ScrollingNumber perfect;

    public float scrollDuration = 2f;
    public float preFullComboPause = 0.5f;
    public float fullComboDuration = 2f;

    public GameObject fadePrefab;
    public Transform fadeSpawn;

    public GameObject fullComboPrefab;
    public Transform fullComboSpawn;

    public TextMeshPro continueText;
    private Color continueTextColor;

    void Start()
    {
        continueTextColor = continueText.color;
        continueText.color = new Color(0, 0, 0, 0);

        Fade fadeIn = Instantiate(fadePrefab, fadeSpawn).GetComponent<Fade>();
        fadeIn.duration = 0.5f;
        fadeIn.fadeOut = false;
        LoadResults();
    }

    void LoadResults()
    {
        score.scrollDuration = scrollDuration;
        combo.scrollDuration = scrollDuration;
        rank.scrollDuration = scrollDuration;
        miss.scrollDuration = scrollDuration;
        good.scrollDuration = scrollDuration;
        perfect.scrollDuration = scrollDuration;

        score.SetValue(ResultsInfo.score);
        combo.SetValue(ResultsInfo.combo);
        rank.SetValue(RankMethods.ScoreToRank(ResultsInfo.score));
        miss.SetValue(ResultsInfo.misses);
        good.SetValue(ResultsInfo.goodHits);
        perfect.SetValue(ResultsInfo.perfectHits);

        if (ResultsInfo.misses == 0)
        {
            StartCoroutine(FullComboAnimation());
            StartCoroutine(WaitForWToGoHome(scrollDuration + preFullComboPause + fullComboDuration));
        }
        else
        {
            StartCoroutine(WaitForWToGoHome(scrollDuration));
        }

    }

    IEnumerator WaitForWToGoHome(float duration)
    {
        yield return new WaitForSeconds(duration);
        StartCoroutine(UnfadeContinueText());
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                float fadeDuration = 1f;
                Fade fadeOut = Instantiate(fadePrefab, fadeSpawn).GetComponent<Fade>();
                fadeOut.duration = fadeDuration;
                fadeOut.fadeOut = true;
                yield return new WaitForSeconds(fadeDuration);
                SceneManager.LoadScene("SongSelectionScene");
            }
            yield return null;
        }
    }

    IEnumerator FullComboAnimation() {
        yield return new WaitForSeconds(scrollDuration + preFullComboPause);
        FlyInAnimation fullCombo = Instantiate(fullComboPrefab, fullComboSpawn).GetComponent<FlyInAnimation>();
        fullCombo.duration = fullComboDuration;
    }

    IEnumerator UnfadeContinueText()
    {
        float duration = 0.5f;
        float startTime = Time.time;
        Color startColor = new Color(continueTextColor.r, continueTextColor.g, continueTextColor.b, 0);
        while (Time.time - startTime < duration)
        {
            continueText.color = Color.Lerp(startColor, continueTextColor, (Time.time - startTime) / duration);
            yield return null;
        }
        continueText.color = continueTextColor;
    }
}

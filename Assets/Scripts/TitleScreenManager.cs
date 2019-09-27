using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    public GameObject fadePrefab;
    public Transform fadeSpawn;

    void Start()
    {
        StartCoroutine(TitleScreenLoop());
        StartCoroutine(EditorPassword());
    }

    IEnumerator TitleScreenLoop()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                float fadeDuration = 1f;
                Fade fadeOut = Instantiate(fadePrefab, fadeSpawn.transform).GetComponent<Fade>();
                fadeOut.duration = fadeDuration;
                fadeOut.fadeOut = true;
                yield return new WaitForSeconds(fadeDuration);
                SceneManager.LoadScene("SongSelectionScene");
            }
            yield return null;
        }
    }

    IEnumerator EditorPassword()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Comma) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                yield return null;
                while (true)
                {
                    if (Input.GetKeyDown(KeyCode.Alpha3))
                    {
                        SceneManager.LoadScene("MapSelectionScene");
                    }
                    if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Alpha3))
                    {
                        break;
                    }
                    yield return null;
                }
            }
            yield return null;
        }
    }
}

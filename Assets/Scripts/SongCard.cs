using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class SongCard : MonoBehaviour
{
    public TextMeshPro titleTMP;
    public TextMeshPro artistTMP;
    public TextMeshPro easyTMP;
    public TextMeshPro regularTMP;
    public TextMeshPro expertTMP;
    public Material redCardMat;
    public Material greenCardMat;
    public Material blueCardMat;
    public Material grayCardMat;
    public MeshRenderer cardBackMeshRenderer;
    public MeshRenderer albumCoverMeshRenderer;
    public string selectedDifficulty = "easy";
    public SongInfo songInfo;

    private Texture2D coverImage;

    public void SetInfo(SongInfo newSongInfo)
    {
        this.songInfo = newSongInfo;
        SetTitle(newSongInfo.title);
        SetArtist(newSongInfo.artist);
        foreach (string difficulty in newSongInfo.difficulties)
        {
            if (difficulty.Equals("easy"))
            {
                EnableEasy();
            }
            if (difficulty.Equals("regular"))
            {
                EnableRegular();
            }
            if (difficulty.Equals("expert"))
            {
                EnableExpert();
            }
        }

        StartCoroutine(LoadAlbumArt());
    }

    private IEnumerator LoadAlbumArt()
    {
        string coverImagePath = "File://" + Path.Combine(Application.streamingAssetsPath, "CustomSongs", songInfo.folder, "cover.png");

        UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(coverImagePath);
        yield return unityWebRequest.SendWebRequest();
        Texture2D coverImage = DownloadHandlerTexture.GetContent(unityWebRequest);

        //WWW www = new WWW(coverImagePath);
        //yield return www;
        //Texture2D coverImage = new Texture2D(512, 512, TextureFormat.DXT5, false);
        //www.LoadImageIntoTexture(coverImage);
        albumCoverMeshRenderer.material.mainTexture = coverImage;
    }

    public void SetTitle(string title)
    {
        titleTMP.text = title;
    }

    public void SetArtist(string artist)
    {
        artistTMP.text = artist;
    }

    public void SelectDifficulty(string difficulty)
    {
        if (difficulty.Equals("easy"))
        {
            SelectEasy();
        }
        if (difficulty.Equals("regular"))
        {
            SelectRegular();
        }
        if (difficulty.Equals("expert"))
        {
            SelectExpert();
        }
    }

    public void DeselectDifficulty()
    {
        easyTMP.fontStyle = FontStyles.Normal;
        regularTMP.fontStyle = FontStyles.Normal;
        expertTMP.fontStyle = FontStyles.Normal;
        cardBackMeshRenderer.material = grayCardMat;
        titleTMP.color = grayCardMat.color;
        artistTMP.color = grayCardMat.color;
    }

    public void SelectEasy()
    {
        easyTMP.fontStyle = FontStyles.Underline;
        regularTMP.fontStyle = FontStyles.Normal;
        expertTMP.fontStyle = FontStyles.Normal;
        selectedDifficulty = "easy";
        cardBackMeshRenderer.material = blueCardMat;
        titleTMP.color = blueCardMat.color;
        artistTMP.color = blueCardMat.color;
    }

    public void SelectRegular()
    {
        easyTMP.fontStyle = FontStyles.Normal;
        regularTMP.fontStyle = FontStyles.Underline;
        expertTMP.fontStyle = FontStyles.Normal;
        selectedDifficulty = "regular";
        cardBackMeshRenderer.material = greenCardMat;
        titleTMP.color = greenCardMat.color;
        artistTMP.color = greenCardMat.color;
    }

    public void SelectExpert()
    {
        easyTMP.fontStyle = FontStyles.Normal;
        regularTMP.fontStyle = FontStyles.Normal;
        expertTMP.fontStyle = FontStyles.Underline;
        selectedDifficulty = "expert";
        cardBackMeshRenderer.material = redCardMat;
        titleTMP.color = redCardMat.color;
        artistTMP.color = redCardMat.color;
    }

    public void EnableEasy()
    {
        easyTMP.color = new Color(0.5019608f, 0.7254902f, 1);
    }

    public void EnableRegular()
    {
        regularTMP.color = new Color(0.8745098f, 1, 0.5019608f);
    }

    public void EnableExpert()
    {
        expertTMP.color = new Color(1, 0.5019608f, 0.5019608f);
    }

    public IEnumerator MoveToLeftCoroutine()
    {
        Vector3 initialPos = transform.position;
        Vector3 targetPos = new Vector3(-5f, 0, 0);
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = new Vector3(0.4f, 0.4f, 0.4f);
        for (int i = 0; i <= 10; i++)
        {
            transform.position = Vector3.Lerp(initialPos, targetPos, i / 10f);
            transform.localScale = Vector3.Lerp(initialScale, targetScale, i / 10f);
            yield return null;
        }
    }

    public IEnumerator MoveOffscreenLeftCoroutine()
    {
        Vector3 initialPos = transform.position;
        Vector3 targetPos = new Vector3(-13.3f, 0, 0);
        for (int i = 0; i <= 10; i++)
        {
            transform.position = Vector3.Lerp(initialPos, targetPos, i / 10f);
            yield return null;
        }
        Destroy(gameObject);
    }

    public IEnumerator MoveToRightCoroutine()
    {
        Vector3 initialPos = transform.position;
        Vector3 targetPos = new Vector3(5f, 0, 0);
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = new Vector3(0.4f, 0.4f, 0.4f);
        for (int i = 0; i <= 10; i++)
        {
            transform.position = Vector3.Lerp(initialPos, targetPos, i / 10f);
            transform.localScale = Vector3.Lerp(initialScale, targetScale, i / 10f);
            yield return null;
        }
    }

    public IEnumerator MoveOffscreenRightCoroutine()
    {
        Vector3 initialPos = transform.position;
        Vector3 targetPos = new Vector3(13.3f, 0, 0);
        for (int i = 0; i <= 10; i++)
        {
            transform.position = Vector3.Lerp(initialPos, targetPos, i / 10f);
            yield return null;
        }
        Destroy(gameObject);
    }

    public IEnumerator MoveToCenterCoroutine()
    {
        Vector3 initialPos = transform.position;
        Vector3 targetPos = new Vector3(0, 0, 0);
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = new Vector3(1, 1, 1);
        for (int i = 0; i <= 10; i++)
        {
            transform.position = Vector3.Lerp(initialPos, targetPos, i / 10f);
            transform.localScale = Vector3.Lerp(initialScale, targetScale, i / 10f);
            yield return null;
        }
    }
}

using UnityEngine;
using TMPro;

public class CurrentSongDisplay : MonoBehaviour
{
    private string infoPrefix = " <size=3.5>";
    private string infoSuffix = "</color></size>";
    private string infoInfix = " - <color=#334A66> ";

    void Start()
    {
        GetComponent<TextMeshPro>().text = infoPrefix + CurrentSongInfo.songDifficulty + infoInfix + CurrentSongInfo.songName + infoSuffix;
    }
}

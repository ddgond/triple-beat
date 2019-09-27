using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CurrentSongInfo
{
    public static string songDifficulty = "";
    public static string songName = "";
    public static string songFolder = "";
    public static float bpm;
    public static float spawnToHitTimeDelta;
    public static float pointsPerNote;
    public static List<NoteInfo> noteInfos = new List<NoteInfo>();
    public static SongInfo songInfo;

    public static void LoadNoteInfo(string folderName, string difficulty, SongInfo info)
    {
        songFolder = folderName;
        songInfo = info;
        songDifficulty = difficulty;
        noteInfos.Clear();
        if (songDifficulty.Equals(""))
        {
            songName = "";
            spawnToHitTimeDelta = 2;
            bpm = 0;
            return;
        }
        string noteInfoPath = Path.Combine(Application.streamingAssetsPath, "CustomSongs", folderName, songDifficulty) + ".beat";
        string noteInfoText = File.ReadAllText(noteInfoPath);
        string[] lines = noteInfoText.Split('\n');
        songName = lines[0].Substring(9);
        spawnToHitTimeDelta = float.Parse(lines[1].Substring(15));
        bpm = float.Parse(lines[2].Substring(4));
        for (int lineIndex = 3; lineIndex < lines.Length; lineIndex++)
        {
            string line = lines[lineIndex];
            if (line.Length == 0)
            {
                break;
            }
            int colonIndex = line.IndexOf(':');
            NoteColor noteColor = StringToNoteColor(line.Substring(0, colonIndex));
            float hitTime = float.Parse(line.Substring(colonIndex + 1));
            noteInfos.Add(new NoteInfo(noteColor, hitTime));
        }
        pointsPerNote = 1000000 / ((float)noteInfos.Count);
    }

    private static NoteColor StringToNoteColor(string noteColor)
    {
        if (noteColor.Equals("red"))
        {
            return NoteColor.Red;
        }
        if (noteColor.Equals("green"))
        {
            return NoteColor.Green;
        }
        if (noteColor.Equals("blue"))
        {
            return NoteColor.Blue;
        }
        return NoteColor.Green;
    }
}

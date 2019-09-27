using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace Mapper
{
    public class SongInfoManager : MonoBehaviour
    {
        public TMP_InputField folderInput;
        public TMP_InputField songInput;
        public TMP_InputField artistInput;
        public TMP_InputField mapperInput;
        public TMP_InputField bpmInput;
        public Toggle easyToggle;
        public Toggle regularToggle;
        public Toggle expertToggle;

        private SongInfo songInfo;
        private string prevFolderName;
        private float startOffset;

        void Start()
        {
            LoadSongInfo();
        }

        // Called by button
        public void ApplyChanges()
        {
            songInfo.folder = folderInput.text;
            songInfo.title = songInput.text;
            songInfo.artist = artistInput.text;
            songInfo.mapper = mapperInput.text;
            songInfo.bpm = float.Parse(bpmInput.text);
            songInfo.difficulties = DifficultiesAsArray();

            string oldFolderPath = Path.Combine(Application.streamingAssetsPath, "CustomSongs", prevFolderName);
            string newFolderPath = Path.Combine(Application.streamingAssetsPath, "CustomSongs", songInfo.folder);
            if (!Directory.Exists(newFolderPath))
            {
                Directory.CreateDirectory(newFolderPath);
                if (prevFolderName != "")
                {
                    IEnumerable<string> files = Directory.EnumerateFiles(oldFolderPath);

                    foreach (string currentFile in files)
                    {
                        string fileName = currentFile.Substring(oldFolderPath.Length + 1);
                        Directory.Move(currentFile, Path.Combine(newFolderPath, fileName));
                    }
                }
                prevFolderName = songInfo.folder;
            }
            else
            {
                folderInput.text = prevFolderName;
                songInfo.folder = prevFolderName;
            }

            if (songInfo.folder == "")
            {
                Debug.Log("Did not save, failed to create new folder");
                return;
            }

            GenerateDifficultyFiles(songInfo.difficulties);
            CurrentSongInfo.LoadNoteInfo(songInfo.folder, songInfo.difficulties[0], songInfo);

            string infoFileContents = "title:" + songInput.text;
            infoFileContents += "\n" + "artist:" + artistInput.text;
            infoFileContents += "\n" + "mapper:" + mapperInput.text;
            infoFileContents += "\n" + "difficulties:" + DifficultiesAsString();
            infoFileContents += "\n" + "bpm:" + bpmInput.text;
            infoFileContents += "\n" + "startOffset:" + songInfo.startOffset;

            string infoFilePath = Path.Combine(Application.streamingAssetsPath, "CustomSongs", songInfo.folder, "info.txt");
            using (StreamWriter sw = File.CreateText(infoFilePath))
            {
                sw.Write(infoFileContents);
            }
        }

        // Called by button
        public void EditMap(string difficulty)
        {
            CurrentSongInfo.LoadNoteInfo(songInfo.folder, difficulty, songInfo);
            SceneManager.LoadScene("MapperScene");
        }

        // Called by button
        public void ReturnToSongSelection()
        {
            SceneManager.LoadScene("MapSelectionScene");
        }

        // Called by button
        public void OpenFolder()
        {
            string folderPath = Path.Combine(Application.streamingAssetsPath, "CustomSongs", songInfo.folder);
            #if UNITY_EDITOR
            Unify.OpenInFileBrowser.Open(folderPath);
            #endif
        }

        private void GenerateDifficultyFiles(string[] difficulties)
        {
            foreach (string difficulty in difficulties)
            {
                string difficultyPath = Path.Combine(Application.streamingAssetsPath, "CustomSongs", songInfo.folder, difficulty + ".beat");
                if (!File.Exists(difficultyPath))
                {
                    string difficultyFileContents = "songName:" + songInfo.title;
                    difficultyFileContents += "\n" + "spawnToHitTime:" + CurrentSongInfo.spawnToHitTimeDelta;
                    difficultyFileContents += "\n" + "bpm:" + songInfo.bpm;

                    using (StreamWriter sw = File.CreateText(difficultyPath))
                    {
                        sw.Write(difficultyFileContents);
                    }
                }
            }
        }

        private string[] DifficultiesAsArray()
        {
            List<string> difficulties = new List<string>();
            if (easyToggle.isOn)
            {
                difficulties.Add("easy");
            }
            if (regularToggle.isOn)
            {
                difficulties.Add("regular");
            }
            if (expertToggle.isOn)
            {
                difficulties.Add("expert");
            }
            return difficulties.ToArray();
        }

        private string DifficultiesAsString()
        {
            bool firstDiff = true;
            string result = "";
            if (easyToggle.isOn)
            {
                firstDiff = false;
                result += "easy";
            }
            if (regularToggle.isOn)
            {
                if (!firstDiff)
                {
                    result += ",";
                }
                result += "regular";
            }
            if (expertToggle.isOn)
            {
                if (!firstDiff)
                {
                    result += ",";
                }
                result += "expert";
            }
            return result;
        }

        private void LoadSongInfo()
        {
            songInfo = CurrentSongInfo.songInfo;
            folderInput.text = songInfo.folder;
            songInput.text = songInfo.title;
            artistInput.text = songInfo.artist;
            mapperInput.text = songInfo.mapper;
            bpmInput.text = songInfo.bpm.ToString();
            easyToggle.isOn = ContainsDifficulty(songInfo, "easy");
            regularToggle.isOn = ContainsDifficulty(songInfo, "regular");
            expertToggle.isOn = ContainsDifficulty(songInfo, "expert");
            startOffset = songInfo.startOffset;

            prevFolderName = songInfo.folder;
        }

        private bool ContainsDifficulty(SongInfo info, string difficulty)
        {
            foreach (string diff in info.difficulties)
            {
                if (diff.Equals(difficulty))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
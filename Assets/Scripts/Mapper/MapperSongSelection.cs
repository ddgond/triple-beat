using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mapper
{
    public class MapperSongSelection : MonoBehaviour
    {
        public GameObject songCardPrefab;
        public List<SongInfo> songInfos = new List<SongInfo>();

        private SongCard activeCard;
        private SongCard leftCard;
        private SongCard rightCard;
        private int activeCardIndex = 0;
        private bool acceptingKeyInput = true;

        void Start()
        {
            LoadSongInfos();
            CreateSongInfoObjects();
        }

        private void Update()
        {
            if (acceptingKeyInput)
            {
                HandleKeyInput();
            }
        }

        private void HandleKeyInput()
        {
            // Right arrow input
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (activeCard.selectedDifficulty.Equals("easy"))
                {
                    if (activeCard.songInfo.difficulties.Length > 1)
                    {
                        activeCard.SelectDifficulty(activeCard.songInfo.difficulties[1]);
                    }
                    else
                    {
                        if (rightCard != null)
                        {
                            activeCard.DeselectDifficulty();
                            activeCardIndex++;
                            if (leftCard != null)
                            {
                                leftCard.StartCoroutine(leftCard.MoveOffscreenLeftCoroutine());
                            }
                            activeCard.StartCoroutine(activeCard.MoveToLeftCoroutine());
                            leftCard = activeCard;
                            rightCard.StartCoroutine(rightCard.MoveToCenterCoroutine());
                            activeCard = rightCard;
                            activeCard.SelectDifficulty(activeCard.songInfo.difficulties[0]);
                            rightCard = null;
                            if (activeCardIndex + 1 < songInfos.Count)
                            {
                                SongCard songCard = Instantiate(songCardPrefab).GetComponent<SongCard>();
                                songCard.transform.position = new Vector3(13.3f, 0, 0);
                                songCard.SetInfo(songInfos[activeCardIndex + 1]);
                                rightCard = songCard;
                                rightCard.StartCoroutine(rightCard.MoveToRightCoroutine());
                            }
                        }
                    }
                }
                else if (activeCard.selectedDifficulty.Equals("regular"))
                {
                    if (activeCard.songInfo.difficulties.Length > 1 && activeCard.songInfo.difficulties[1].Equals("expert") || activeCard.songInfo.difficulties.Length > 2 && activeCard.songInfo.difficulties[2].Equals("expert"))
                    {
                        activeCard.SelectExpert();
                    }
                    else
                    {
                        if (rightCard != null)
                        {
                            activeCard.DeselectDifficulty();
                            activeCardIndex++;
                            if (leftCard != null)
                            {
                                leftCard.StartCoroutine(leftCard.MoveOffscreenLeftCoroutine());
                            }
                            activeCard.StartCoroutine(activeCard.MoveToLeftCoroutine());
                            leftCard = activeCard;
                            rightCard.StartCoroutine(rightCard.MoveToCenterCoroutine());
                            activeCard = rightCard;
                            activeCard.SelectDifficulty(activeCard.songInfo.difficulties[0]);
                            rightCard = null;
                            if (activeCardIndex + 1 < songInfos.Count)
                            {
                                SongCard songCard = Instantiate(songCardPrefab).GetComponent<SongCard>();
                                songCard.transform.position = new Vector3(13.3f, 0, 0);
                                songCard.SetInfo(songInfos[activeCardIndex + 1]);
                                rightCard = songCard;
                                rightCard.StartCoroutine(rightCard.MoveToRightCoroutine());
                            }
                        }
                    }
                }
                else
                {
                    if (rightCard != null)
                    {
                        activeCard.DeselectDifficulty();
                        activeCardIndex++;
                        if (leftCard != null)
                        {
                            leftCard.StartCoroutine(leftCard.MoveOffscreenLeftCoroutine());
                        }
                        activeCard.StartCoroutine(activeCard.MoveToLeftCoroutine());
                        leftCard = activeCard;
                        rightCard.StartCoroutine(rightCard.MoveToCenterCoroutine());
                        activeCard = rightCard;
                        activeCard.SelectDifficulty(activeCard.songInfo.difficulties[0]);
                        rightCard = null;
                        if (activeCardIndex + 1 < songInfos.Count)
                        {
                            SongCard songCard = Instantiate(songCardPrefab).GetComponent<SongCard>();
                            songCard.transform.position = new Vector3(13.3f, 0, 0);
                            songCard.SetInfo(songInfos[activeCardIndex + 1]);
                            rightCard = songCard;
                            rightCard.StartCoroutine(rightCard.MoveToRightCoroutine());
                        }
                    }
                }
            }

            // Left Arrow input
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (activeCard.selectedDifficulty.Equals("expert"))
                {
                    if (activeCard.songInfo.difficulties.Length > 1)
                    {
                        activeCard.SelectDifficulty(activeCard.songInfo.difficulties[activeCard.songInfo.difficulties.Length - 2]);
                    }
                    else
                    {
                        if (leftCard != null)
                        {
                            activeCard.DeselectDifficulty();
                            activeCardIndex--;
                            if (rightCard != null)
                            {
                                rightCard.StartCoroutine(rightCard.MoveOffscreenRightCoroutine());
                            }
                            activeCard.StartCoroutine(activeCard.MoveToRightCoroutine());
                            rightCard = activeCard;
                            leftCard.StartCoroutine(leftCard.MoveToCenterCoroutine());
                            activeCard = leftCard;
                            activeCard.SelectDifficulty(activeCard.songInfo.difficulties[activeCard.songInfo.difficulties.Length - 1]);
                            leftCard = null;
                            if (activeCardIndex - 1 >= 0)
                            {
                                SongCard songCard = Instantiate(songCardPrefab).GetComponent<SongCard>();
                                songCard.transform.position = new Vector3(-13.3f, 0, 0);
                                songCard.SetInfo(songInfos[activeCardIndex - 1]);
                                leftCard = songCard;
                                leftCard.StartCoroutine(rightCard.MoveToLeftCoroutine());
                            }
                        }
                    }
                }
                else if (activeCard.selectedDifficulty.Equals("regular"))
                {
                    if (activeCard.songInfo.difficulties.Length > 1 && activeCard.songInfo.difficulties[0].Equals("easy"))
                    {
                        activeCard.SelectEasy();
                    }
                    else
                    {
                        if (leftCard != null)
                        {
                            activeCard.DeselectDifficulty();
                            activeCardIndex--;
                            if (rightCard != null)
                            {
                                rightCard.StartCoroutine(rightCard.MoveOffscreenRightCoroutine());
                            }
                            activeCard.StartCoroutine(activeCard.MoveToRightCoroutine());
                            rightCard = activeCard;
                            leftCard.StartCoroutine(leftCard.MoveToCenterCoroutine());
                            activeCard = leftCard;
                            activeCard.SelectDifficulty(activeCard.songInfo.difficulties[activeCard.songInfo.difficulties.Length - 1]);
                            leftCard = null;
                            if (activeCardIndex - 1 >= 0)
                            {
                                SongCard songCard = Instantiate(songCardPrefab).GetComponent<SongCard>();
                                songCard.transform.position = new Vector3(-13.3f, 0, 0);
                                songCard.SetInfo(songInfos[activeCardIndex - 1]);
                                leftCard = songCard;
                                leftCard.StartCoroutine(rightCard.MoveToLeftCoroutine());
                            }
                        }
                    }
                }
                else
                {
                    if (leftCard != null)
                    {
                        activeCard.DeselectDifficulty();
                        activeCardIndex--;
                        if (rightCard != null)
                        {
                            rightCard.StartCoroutine(rightCard.MoveOffscreenRightCoroutine());
                        }
                        activeCard.StartCoroutine(activeCard.MoveToRightCoroutine());
                        rightCard = activeCard;
                        leftCard.StartCoroutine(leftCard.MoveToCenterCoroutine());
                        activeCard = leftCard;
                        activeCard.SelectDifficulty(activeCard.songInfo.difficulties[activeCard.songInfo.difficulties.Length - 1]);
                        leftCard = null;
                        if (activeCardIndex - 1 >= 0)
                        {
                            SongCard songCard = Instantiate(songCardPrefab).GetComponent<SongCard>();
                            songCard.transform.position = new Vector3(-13.3f, 0, 0);
                            songCard.SetInfo(songInfos[activeCardIndex - 1]);
                            leftCard = songCard;
                            leftCard.StartCoroutine(leftCard.MoveToLeftCoroutine());
                        }
                    }
                }
            }

            // Spacebar input
            if (Input.GetKeyDown(KeyCode.Space))
            {
                acceptingKeyInput = false;
                LaunchSongInfoScene();
            }
        }

        public void NewSong()
        {
            CurrentSongInfo.LoadNoteInfo("", "", new SongInfo("", "", "", new string[0], "", 60, 0));
            SceneManager.LoadScene("SongInfoScene");
        }

        private void LaunchSongInfoScene()
        {
            CurrentSongInfo.LoadNoteInfo(activeCard.songInfo.folder, activeCard.selectedDifficulty, activeCard.songInfo);
            SceneManager.LoadScene("SongInfoScene");
        }

        private void LoadSongInfos()
        {
            songInfos.Clear();
            string customSongsFolderPath = Path.Combine(Application.streamingAssetsPath, "CustomSongs");
            DirectoryInfo customSongsFolderInfo = new DirectoryInfo(customSongsFolderPath);

            foreach (DirectoryInfo songFolderInfo in customSongsFolderInfo.GetDirectories())
            {
                foreach (FileInfo songFileInfo in songFolderInfo.GetFiles())
                {
                    if (songFileInfo.Name.Equals("info.txt"))
                    {
                        string songInfoText = File.ReadAllText(songFileInfo.FullName);
                        string[] lines = songInfoText.Split('\n');
                        string title = lines[0].Substring(6);
                        string artist = lines[1].Substring(7);
                        string mapper = lines[2].Substring(7);
                        string[] difficulties = lines[3].Substring(13).Split(',');
                        string folder = songFolderInfo.Name;
                        float bpm = float.Parse(lines[4].Substring(4));
                        float startOffset = float.Parse(lines[5].Substring(12));
                        songInfos.Add(new SongInfo(title, artist, mapper, difficulties, folder, bpm, startOffset));
                    }
                }
            }
        }

        private void CreateSongInfoObjects()
        {
            if (songInfos.Count == 0)
            {
                return;
            }

            SongCard songCard = Instantiate(songCardPrefab).GetComponent<SongCard>();
            songCard.SetInfo(songInfos[0]);
            songCard.SelectDifficulty(songInfos[0].difficulties[0]);
            activeCard = songCard;

            if (songInfos.Count > 1)
            {
                songCard = Instantiate(songCardPrefab).GetComponent<SongCard>();
                songCard.transform.position = new Vector3(5f, 0, 0);
                songCard.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                songCard.SetInfo(songInfos[1]);
                rightCard = songCard;
            }
        }
    }
}
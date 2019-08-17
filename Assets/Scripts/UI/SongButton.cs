using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongButton : MonoBehaviour
{
    private Button button;
    [SerializeField] private RectTransform detailPanel;
    private Button playButton;

    [SerializeField] public string songName;

    void Awake()
    {
        button = GetComponent<Button>();
        playButton = detailPanel.GetComponentInChildren<Button>();
        playButton.onClick.AddListener(() => { Song.currentSong = Song.LoadSong(songName); });
    }
}

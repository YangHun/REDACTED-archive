using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button backButton;
    
    public SongButton[] AllButtons { get; private set; }

    void Awake()
    {
        backButton.onClick.AddListener(() => { SceneManager.LoadScene("Title"); });
        AllButtons = FindObjectsOfType<SongButton>();
    }
}

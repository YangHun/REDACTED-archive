using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button retryButton;

    void Awake()
    {
        retryButton.onClick.AddListener(() => { SceneManager.LoadScene("SelectSong"); });
    }
}

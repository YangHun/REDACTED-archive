using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;

    void Awake()
    {
        startButton.onClick.AddListener(() => { SceneManager.LoadScene("SelectSong"); });
        creditsButton.onClick.AddListener(() => { SceneManager.LoadScene("Credits"); });
        exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}

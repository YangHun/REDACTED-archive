using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;

public class SongButton : MonoBehaviour
{
    [SerializeField] private Button selectButton;
    [SerializeField] private RectTransform detailPanel;
    private Button playButton;

    [SerializeField] public string songName;

    private float detailPanelX;
    
    [SerializeField] private bool isOpened = true;

    private static SongButton[] allButtons;

    void Awake()
    {
        if (allButtons == null)
        {
            allButtons = transform.parent.GetComponentsInChildren<SongButton>();
        }
        
        playButton = detailPanel.GetComponentInChildren<Button>();

        var detailPanelTrans = detailPanel.GetComponent<RectTransform>();
        detailPanelX = detailPanelTrans.anchoredPosition.x;
        
        selectButton.onClick.AddListener(() =>
        {
            ToggleDescription();
            foreach (var button in allButtons)
            {
                if (button.isOpened && button != this)
                {
                    button.ToggleDescription();
                    break;
                }
            }
        });
        playButton.onClick.AddListener(() =>
        {
            Song.currentSong = Song.LoadSong(songName);
            SceneManager.LoadScene("Dummy");
        });
        
        if (!isOpened)
        {
            detailPanelTrans.anchoredPosition = new Vector2(0.0f, detailPanelTrans.anchoredPosition.y);
            detailPanel.gameObject.SetActive(false);
        }
    }

    public void ToggleDescription()
    {
        if (isOpened)
        {
            var detailTrans = detailPanel.GetComponent<RectTransform>();
            detailTrans.DOKill();
            detailTrans.DOAnchorPosX(0.0f, 1.0f)
                .OnComplete(() => detailPanel.gameObject.SetActive(false)).Play();
        }
        else
        {
            detailPanel.gameObject.SetActive(true);
            var detailTrans = detailPanel.GetComponent<RectTransform>();
            detailTrans.DOKill();
            detailTrans.DOAnchorPosX(detailPanelX, 1.0f).Play();
        }

        isOpened = !isOpened;
    }
}

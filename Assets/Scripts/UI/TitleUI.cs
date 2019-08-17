using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;

    void Awake()
    {
        startButton.onClick.AddListener(() =>
        {
            
        });
        creditsButton.onClick.AddListener(() =>
        {
            
        });
        exitButton.onClick.AddListener(() =>
        {
            
        });
    }
}

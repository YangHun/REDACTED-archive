using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsUI : MonoBehaviour
{
    [SerializeField] private Button backButton;

    void Awake()
    {
        backButton.onClick.AddListener(() => { SceneManager.LoadScene("Title"); });
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    public event Action StartRequested;
    public event Action ExitRequested;

    [SerializeField] Button gameStartButton;
    [SerializeField] Button exitButton;


    void Awake()
    {
        gameStartButton.onClick.AddListener(() => StartRequested?.Invoke());
        exitButton.onClick.AddListener(() => ExitRequested?.Invoke());
    }
}

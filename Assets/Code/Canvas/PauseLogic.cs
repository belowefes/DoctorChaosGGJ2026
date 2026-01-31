using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseLogic : MonoBehaviour
{
    [SerializeField] private InputAction pauseInput;

    [SerializeField] private List<GameObject> turnOffAfterPause;
    [SerializeField] private List<GameObject> turnOnAfterPause;

    private bool _paused = false;

    public void TogglePause()
    {
        _paused = !_paused;
        foreach (var target in turnOffAfterPause)
        {
            target.SetActive(!_paused);
        }

        foreach (var target in turnOnAfterPause)
        {
            target.SetActive(_paused);
        }

        if (_paused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }


    void Start()
    {
        TogglePause();
    }

    void OnEnable()
    {
        pauseInput.Enable();
    }

    void OnDisable()
    {
        pauseInput.Disable();
    }

    private void FixedUpdate()
    {
        if (pauseInput.IsPressed())
        {
            TogglePause();
        }
    }
}
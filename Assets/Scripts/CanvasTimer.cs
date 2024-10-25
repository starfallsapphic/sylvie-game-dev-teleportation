using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CanvasTimer : MonoBehaviour
{
    private bool _timerActive;
    private float _currentTime;
    [SerializeField] private TMP_Text _text;

    // Start is called before the first frame update
    void Start()
    {
        _currentTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(_timerActive)
        {
            _currentTime += Time.deltaTime;
        }

        _text.text = GetTime();
    }

    public void StartTimer()
    {
        _timerActive = true;
    }

    public void StopTimer()
    {
        _timerActive = false;
    }

    public string GetTime()
    {
        TimeSpan time = TimeSpan.FromSeconds(_currentTime);
        return time.Minutes.ToString() + ":" + time.Seconds.ToString("D2") + ":" + time.Milliseconds.ToString();
    }
}

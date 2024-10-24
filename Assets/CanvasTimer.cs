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
        StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if(_timerActive)
        {
            _currentTime += Time.deltaTime;
        }

        TimeSpan time = TimeSpan.FromSeconds(_currentTime);

        _text.text = time.Minutes.ToString() + ":" + time.Seconds.ToString("D2") + ":" + time.Milliseconds.ToString();
    }

    public void StartTimer()
    {
        _timerActive = true;
    }

    public void StopTimer()
    {
        _timerActive = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI timerText;

    private float _timeLeft = 0f;

    private float StartTime { get; set; } = default;

    private string SaveKey { get; set; } = default;
    private string _Time { get; set; }
    //private bool FinishGame { get; set; } = default;

    private IEnumerator StartTimer()
    {
        do
        {
            _timeLeft += Time.deltaTime;
            UpdateTimeText();
            yield return null;
        } while (!GameController.isEndGame);
        PlayerPrefs.SetString($"{GameController.SaveKey}Time", _Time);
    }

    private void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        SaveKey = GameController.SaveKey;
        if (PlayerPrefs.HasKey($"{SaveKey}Timer"))
            StartTime = PlayerPrefs.GetFloat($"{SaveKey}Timer", _timeLeft);
        else
            PlayerPrefs.SetFloat($"{SaveKey}Timer", 0);
        _timeLeft = StartTime;

        StartCoroutine(StartTimer());
    }
    public void SaveTime()
    {
        PlayerPrefs.SetFloat($"{SaveKey}Timer", _timeLeft);
    }

    private void UpdateTimeText()
    {
        if (_timeLeft < 0)
            _timeLeft = 0;

        float minutes = Mathf.FloorToInt(_timeLeft / 60);
        float seconds = Mathf.FloorToInt(_timeLeft % 60);
        _Time = string.Format("{0:00}.{1:00}", minutes, seconds);
        timerText.text = _Time;
    }
}

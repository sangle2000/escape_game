using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CountDown : MonoBehaviour
{
    [SerializeField] private Text _count_down_text;
    [SerializeField] float _time_count_down = 150;

    private bool timerIsRunning = false;
    // Start is called before the first frame update
    void Start()
    {
        timerIsRunning = true;
        _count_down_text.text = "02:30";
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if (_time_count_down > 0)
            {
                _time_count_down -= Time.deltaTime;
                DisplayTime(_time_count_down);
            }
            else
            {
                _time_count_down = 0;

                DisplayTime(_time_count_down);

                SceneManager.LoadScene(2);

                timerIsRunning = false;
            }
        }
    }

    private void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        _count_down_text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}

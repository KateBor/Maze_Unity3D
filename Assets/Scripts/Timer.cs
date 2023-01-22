using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
  public Text time, bestTime;
  private int timeLimit = 300000; //5 минут в миллисекундах
  private TimeSpan ts;
  private string curr, best;
  private float startTime;
  private bool isWork;

  public GameObject restartButton, player, hand, textFinish, textTimeIsOver;
  private void Start()
  {
    Init();
  }

  public void Init()
  {
    isWork = true;
    if (restartButton.activeInHierarchy == true) {
      curr = "00:00:00";
      time.text = curr;
      startTime = Time.time * 1000;
      PlayerPrefs.SetInt("currentTime", 0);
      TimeSpan tm = TimeSpan.FromMilliseconds(PlayerPrefs.GetInt("bestTime"));
      best = tm.Minutes + ":" + tm.Seconds + ":" + tm.Milliseconds;
      bestTime.text = best;
    }
  }

  private void Update()
  {
    if (isWork)
    {
      if (!player.activeInHierarchy)
      {
        PlayerPrefs.SetInt("currentTime", (int)ts.TotalMilliseconds);

        if ((PlayerPrefs.GetInt("currentTime") - PlayerPrefs.GetInt("bestTime")) < 0 || PlayerPrefs.GetInt("bestTime") == 0)
        {
          PlayerPrefs.SetInt("bestTime", PlayerPrefs.GetInt("currentTime"));
          TimeSpan tm = TimeSpan.FromMilliseconds(PlayerPrefs.GetInt("bestTime"));
          best = tm.Minutes + ":" + tm.Seconds + ":" + tm.Milliseconds;
          bestTime.text = best;
        }
        Debug.Log("stop");
        isWork = false;
        return;
      }
      
      if ((PlayerPrefs.GetInt("currentTime") - timeLimit) >= 0)
      {
        Debug.Log("time limit is over");
        isWork = false;
        textTimeIsOver.SetActive(true);
        player.SetActive(false);
        player.GetComponent<ControlBall>().GetComponent<Rigidbody>().velocity = Vector3.zero;
        hand.SetActive(true);
        return;
      }

      float t = Time.time * 1000 - startTime;
      ts = TimeSpan.FromMilliseconds((int)t);
      PlayerPrefs.SetInt("currentTime", (int)t);
      curr = ts.Minutes + ":" + ts.Seconds + ":" + ts.Milliseconds;
      time.text = curr;
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    player.SetActive(false);
    player.GetComponent<ControlBall>().GetComponent<Rigidbody>().velocity = Vector3.zero;

    hand.SetActive(true);
    textFinish.SetActive(true);
  }
}

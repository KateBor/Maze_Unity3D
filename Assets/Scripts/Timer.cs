using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
  public Text time;
  private float startTime;
  private string bestMin = "0";
  private string bestSec = "0";
  public GameObject restartButton;
  private void Start()
  {
    if (restartButton.activeInHierarchy == true)
    {
      startTime = Time.time;
      time.text = $"<size=30> Best:</size> {bestMin}:{bestSec}\n<size=30> Now:</size> 0";
    }
  }

  private void Update()
  {
    float t = Time.time - startTime;

    string minutes = ((int)t / 60).ToString();
    string seconds = (t % 60).ToString();

    time.text = $"<size=30> Best:</size> {bestMin}:{bestSec}\n<size=30> Now:</size> 0";
  }
}

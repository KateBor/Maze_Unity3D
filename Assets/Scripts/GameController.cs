using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{


  private void Start()
  {
    
  }

  private void Update()
  {
    /*while (!IsFinish())
    {
      currentTime = Timer();
    }*/

  }

  /*public static bool IsFinish()
  {//не уверена что работает
    if (!BuildMaze.isFirstPlay)
      return (BuildMaze.finish.transform.position == BuildMaze.player.transform.position);
    //условие на совпадение координат
    else return true;
  }*/

  /*private Vector2 Timer()
  {
    secTime += 1;//переделать

    //time.text = "<size=30> Best:</size> " + PlayerPrefs.GetInt("score") + "\n<size=30> Now:</size> 0";
    return new Vector2(secTime, minTime);
  }*/
}

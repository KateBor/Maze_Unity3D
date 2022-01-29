using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasButton : MonoBehaviour
{
  public void RestartGame()
  {
    /*if (PlayerPrefs.GetString("music") != "No")
    {
      GetComponent<AudioSource>().Play();
    }*/
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }
}

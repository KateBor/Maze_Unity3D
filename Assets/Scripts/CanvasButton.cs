using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CanvasButton : MonoBehaviour
{
  public GameObject gameController;
  public void RestartGame()
  {
    /*if (PlayerPrefs.GetString("music") != "No")
    {
      GetComponent<AudioSource>().Play();
    }*/
    gameController.GetComponent<BuildMaze>().RerunMaze();
    //Destroy(gameController.GetComponent<BuildMaze>());
    //gameController.AddComponent<BuildMaze>();
    //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

  }
}

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class BuildMaze : MonoBehaviour
{
  private static float size = 1.5f;
  private int _wall = -1;
  private int _empty = 0;
  private int width_ = 21;
  private int height_ = 21;
  private int[,] Maze = new int[21, 21];
  private Vector3[] PossibleStart = new Vector3[4];

  public Text bestTime;
  public GameObject Wall, restartButton, text, hand, textFinish, textTimeIsOver;
  public GameObject AllWalls, Platform, MainCamera;

  public static bool isFinish = false;
  public GameObject finish, player;

  System.Random rnd = new System.Random();

  private void Start()
  {
    Init();
  }

  public void Init() {
    if (!PlayerPrefs.HasKey("bestTime"))
    {
      PlayerPrefs.SetInt("bestTime", 0);
    }
    TimeSpan tm = TimeSpan.FromMilliseconds(PlayerPrefs.GetInt("bestTime"));
    bestTime.text = tm.Minutes + ":" + tm.Seconds + ":" + tm.Milliseconds;

    Wall.transform.localScale = new Vector3(size, 0.8f, size);
    MainCamera.transform.localPosition = new Vector3(10 * size, 22, -7);
    Platform.transform.localScale = new Vector3(size * 21, 1f, size * 21);
    Platform.transform.localPosition = new Vector3(10 * size, (float)-0.5, 10 * size);

    for (int i = 0; i < height_; i++)
    {
      for (int j = 0; j < width_; j++)
      {
        Maze[i, j] = _empty;
      }
    }

    PossibleStart[0] = new Vector3(size, (float)0.8, size); //надо переделать чтоб не было захардкожено
    PossibleStart[1] = new Vector3(size, (float)0.8, 19 * size);
    PossibleStart[2] = new Vector3(19 * size, (float)0.8, size);
    PossibleStart[3] = new Vector3(19 * size, (float)0.8, 19 * size);
  }

  private bool IsPassage(int str, int elem, int lastElem)
  {
    int currentElem = elem;
    while (currentElem != lastElem)
    {
      if (Maze[str, currentElem] != _wall)
      {
        return true;
      }
      currentElem++;
    }
    return false;
  }
  private void AddWall(int str)
  {
    for (int i = 0; i < width_; i++)
    {
      Maze[str, i] = Maze[str - 1, i];
    }
    int elem = 1;
    int currentElem = elem;
    while (currentElem < width_)
    {
      if (Maze[str, currentElem] == _wall)
      {
        if (!IsPassage(str, elem, currentElem) && (elem != currentElem))
        {
          int i = rnd.Next() % (currentElem - elem);

          Maze[str, elem + i] = Maze[str - 1, elem + i];
        }
        elem = currentElem;
        while ((Maze[str, elem] == _wall) && (elem < width_ - 1))
        {
          elem++;
        }
        currentElem = elem;
      }
      else if ((rnd.Next() % 100 > 8) && ((currentElem) % 2) == 1) //изменить вероятность
      {
        Maze[str, currentElem] = _wall;
        Maze[str, currentElem + 1] = _wall;
        Maze[str, currentElem - 1] = _wall;
      }
      currentElem++;
    }
  }
  private void GenerateFirstStr()
  {
    //нечетные ячейки присваиваю множествам
    for (int i = 0; i < width_; i++)
    {
      if ((i % 2) == 1)
      {
        Maze[1, i] = i;
      }
      else
      {
        Maze[1, i] = _wall;
      }
    }
    // обработка
    for (int j = 1; j < width_ - 3; j += 2)
    {
      if (rnd.Next() % 100 > 25) //изменить вероятность
      {
        Merge(1, j);
      }
    }
    AddWall(2);
  }
  private void GenerateLastStr()
  {
    GenerateStr(height_ - 2);
    for (int i = 1; i < width_ - 1; i++)
    {
      if ((Maze[height_ - 2, i] != _wall) && (Maze[height_ - 2, i + 1] == _wall))
      {
        Merge(height_ - 2, i);
      }
    }
    for (int i = 0; i < width_; i++)
    {
      Maze[height_ - 1, i] = _wall;
    }
  }
  private void GenerateStr(int i)
  {
    for (int j = 0; j < width_; j++)
    {
      if (j % 2 == 0)
      {
        Maze[i, j] = _wall;
      }
      else
      {
        Maze[i, j] = Maze[i - 2, j];
      }
    }

    int elem = 1;
    while (elem != width_ - 1)
    {
      if ((Maze[i - 1, elem] == _wall) && (elem % 2) == 1)
      {
        Maze[i, elem] = elem;
      }
      elem++;
    }

    for (int j = 1; j < width_ - 3; j += 2)
    {
      if (rnd.Next() % 100 > 15) //изменить вероятность
      {
        Maze[i, j + 1] = _wall;
      }
      else
      {
        Merge(i, j);
      }
    }
  }
  private void Merge(int str, int firstElem)
  {
    int elem = firstElem;
    int secondElem = elem + 1;
    int type1 = Maze[str, elem];
    if (elem < width_ - 3)
    {
      while ((Maze[str, secondElem] == _wall) && (secondElem < width_ - 1))
      {
        secondElem++;
      }
      if ((Maze[str, secondElem] == _wall) || (Maze[str, secondElem] == type1))
      {
        return;
      }
      int type2 = Maze[str, secondElem];

      if (type1 > type2)
      {
        for (int i = 0; i < width_ - 2; i++)
        {
          if (Maze[str, i] == type1)
          {
            Maze[str, i] = type2;
          }
        }
        Maze[str, elem + 1] = type2;
      }
      else
      {
        Maze[str, elem + 1] = type1;
        elem++;

        for (; elem < width_ - 2; elem++)
        {
          if (Maze[str, elem + 1] == type2)
          {
            Maze[str, elem + 1] = type1;
          }
        }
      }
    }
  }
  private void GenerateMaze()
  {
    for (int i = 0; i < width_; i++)
    {
      Maze[0, i] = _wall;
    }
    GenerateFirstStr();

    for (int i = 3; i < height_ - 3; i += 2)
    {
      GenerateStr(i);
      AddWall(i + 1);
    }
    GenerateLastStr();

    for (int i = 1; i < height_ - 1; i++)
    {
      for (int j = 1; j < width_ - 1; j++)
      {
        if (Maze[i, j] != _wall)
        {
          Maze[i, j] = _empty;
        }
      }
    }
  }
  private void Build()
  {
    GenerateMaze();
    for (int i = 0; i < height_; i++)
    {
      for (int j = 0; j < width_; j++)
      {
        //создавать стены на нужных позициях
        if(Maze[i, j] == _wall)
        {
          GameObject NewWall = Instantiate(Wall,
            new Vector3((float)(i * size), 0.5f, (float)(j * size)), Quaternion.identity) as GameObject;
          NewWall.transform.SetParent(AllWalls.transform);
        }
      }
    }
    SetStartAndFinish();
  }
  private enum Turn
  { 
    Right,
    Down,
    Left,
    Up,
    None,
  }
  private Vector2[] deltha = new Vector2[]
  {
    new Vector2(-1, 0),
    new Vector2(0, -1),
    new Vector2(1, 0),
    new Vector2(0, 1)
  };

  /// <summary>
  /// Функция устанавливает начало пути в лабиринте и его конец.
  /// </summary>
  private void SetStartAndFinish()
  {
    player.transform.position = PossibleStart[rnd.Next() % 4];
    player.SetActive(true);

    var sx = (int)(player.transform.position.x / size);
    var sz = (int)(player.transform.position.z / size);

    var q = new Queue<Vector2>();
    var used = new bool[21, 21];

    //Enqueing start
    used[sx, sz] = true;
    var start = new Vector2(sx, sz);

    q.Enqueue(start);
    Vector2 lastPos = start;

    while(q.Count != 0)
    {
      var pos = q.Peek();
      q.Dequeue();

      for(var t = Turn.Right; t <= Turn.Up; t++)
      {
        int newX = (int)(pos.x + deltha[(int)t].x);
        int newY = (int)(pos.y + deltha[(int)t].y);
        if (!used[newX, newY] && Maze[newX, newY] != _wall)
        {
          used[newX, newY] = true;
          q.Enqueue(new Vector2(newX, newY));
        }
      }

      lastPos = pos;
    }

    finish.transform.position = new Vector3(lastPos.x * size, 1, lastPos.y * size);
    finish.SetActive(true);
  }

  private void Update()
  {
    if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) //для телефона надо сделать touch
    {
      RerunMaze();
    }
  }

  public void RerunMaze()
  {
    restartButton.SetActive(true);
    text.SetActive(false);
    hand.SetActive(false);
    textFinish.SetActive(false);
    textTimeIsOver.SetActive(false);
    foreach (Transform child in AllWalls.transform)
    {
      Destroy(child.gameObject);
    }
    Init();
    player.GetComponent<ControlBall>().GetComponent<Rigidbody>().velocity = Vector3.zero;
    Build();
    Debug.Log("rerun");
    finish.GetComponent<Timer>().Init();
  }
}

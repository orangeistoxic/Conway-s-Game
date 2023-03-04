using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private static int SCREEN_WIHTH = 192;                            //3072 pixels 
    private static int SCREEN_HEIGHT = 108;                           //1728 pixels 

    public float speed = 0.1f;

    private float timer=0;

    public bool simulationEnable=false;


    Cell[,] grid= new Cell[SCREEN_WIHTH, SCREEN_HEIGHT];

    // Start is called before the first frame update
    void Start()
    {
        PlaceCells();
    }

    // Update is called once per frame
    void Update()
    {
        if (simulationEnable)
        {
            if (timer > speed)
            {
                timer = 0;
                CountNeighbors();
                PopulationConcrol();
            }
            else
            {
                timer += Time.deltaTime;                                 //Time.deltatime 是上一幀所花的時間
            }
        }

        UserInput();

    }

    void UserInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            int x = Mathf.RoundToInt(mousePoint.x);
            int y = Mathf.RoundToInt(mousePoint.y);

            if (x >= 0 && y >= 0 && x < SCREEN_WIHTH && y < SCREEN_HEIGHT) //確認是否在範圍內
            {
                grid[x, y].SetAlive(!grid[x, y].IsAlive); //將點擊的Cell變成相反的狀態
            }
        }

        if (Input.GetKeyUp(KeyCode.P)) //暫停遊戲 
        {
            simulationEnable = false;
        }
        if (Input.GetKeyUp(KeyCode.B)) //繼續遊戲
        {
            simulationEnable = true;  
        }
    }

    void PopulationConcrol()
    {
        for(int y = 0; y < SCREEN_HEIGHT; y++)
        {
            for(int x = 0; x < SCREEN_WIHTH; x++)
            {
                //規則
                //活細胞身旁若有2~3個活細胞，下一代為活細胞
                //若沒有，則死亡
                //死細胞身旁若有正好3個細胞，下一代為活細胞
                //若沒有，則維持死亡
                if(grid[x, y].IsAlive)
                {
                    if(grid[x, y].NumNeighbors!=2 && grid[x, y].NumNeighbors != 3)
                    {
                        grid[x, y].SetAlive(false);
                    }
                }
                else
                {
                    if (grid[x, y].NumNeighbors == 3)
                    {
                        grid[x, y].SetAlive(true);
                    }
                }
            }
        }
    }
    void CountNeighbors()
    {
        for (int y = 0; y < SCREEN_HEIGHT; y++) 
        {
            for (int x = 0; x < SCREEN_WIHTH; x++) 
            {
                int numNeighbors = 0;
                if (y + 1 < SCREEN_HEIGHT)                       //北
                {
                    if (grid[x,y+1].IsAlive)
                    {
                        numNeighbors++;
                    }
                }
                if(x + 1 < SCREEN_WIHTH)                       //東
                {
                    if (grid[x + 1, y].IsAlive)
                    {
                        numNeighbors++;
                    }
                }
                if (y - 1 >= 0)                                //南
                {
                    if (grid[x, y - 1].IsAlive)
                    {
                        numNeighbors++;
                    }
                }
                if (x - 1 >= 0)                                //西
                {
                    if (grid[x - 1, y].IsAlive) 
                    {
                        numNeighbors++;
                    }
                }
                if (y + 1 < SCREEN_HEIGHT && x + 1 < SCREEN_WIHTH) //東北
                {
                    if (grid[x+1, y + 1].IsAlive)
                    {
                        numNeighbors++;
                    }
                }
                if (y + 1 < SCREEN_HEIGHT && x - 1 >= 0)       //西北
                {
                    if(grid[x - 1, y + 1].IsAlive)
                    {
                        numNeighbors++;
                    }
                }
                if (y - 1 >= 0 && x + 1 < SCREEN_WIHTH)         //東南
                {
                    if (grid[x + 1, y - 1].IsAlive)
                    {
                        numNeighbors++;
                    }
                }
                if (y - 1 >= 0 && x - 1 >= 0)                    //西南
                {
                    if (grid[x - 1, y - 1].IsAlive)
                    {
                        numNeighbors++;
                    }
                }
                grid[x,y].NumNeighbors=numNeighbors;
            }
        }
    }
    void PlaceCells()
    {
        for (int y = 0; y < SCREEN_HEIGHT; y++) 
        {
            for (int x = 0; x < SCREEN_WIHTH; x++) 
            {
                Cell cell = Instantiate(Resources.Load("Prefabs/Cell", typeof(Cell)),new Vector2(x,y),Quaternion.identity) as Cell;
                grid[x,y] = cell;
                grid[x, y].SetAlive(RandomAliveCell()); 
            }
        }
    }

    bool RandomAliveCell()
    {
        int rand=UnityEngine.Random.Range(0,100);
        if (rand > 75)
        {
            return true; 
        }
        return false;
    }
}

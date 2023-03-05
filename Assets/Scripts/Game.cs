using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;


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

    private void SavePattern()  //存檔                                     //待完成：可以自己取檔名
    {
        string path = "patterns";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        Pattern pattern = new Pattern();

        string patternString = null;
        
        for(int y=0; y<SCREEN_HEIGHT; y++)
        {
            for(int x=0; x<SCREEN_WIHTH; x++)
            {
                if (grid[x, y].IsAlive == false)
                {
                    patternString += "0";

                }
                else
                {
                    patternString += "1";
                }
            }
        }
        pattern.patternString = patternString;

        XmlSerializer serializer= new XmlSerializer(typeof(Pattern));
        StreamWriter writer = new StreamWriter(path + "/test.xml");
        serializer.Serialize(writer.BaseStream, pattern);
        writer.Close();

        Debug.Log(pattern.patternString);

    }
    void UserInput()                                                                             //輸入
    {
        if (Input.GetKey(KeyCode.LeftControl))                                                //按住左Ctrl可以持續拖曳生成活Cell
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                int x = Mathf.RoundToInt(mousePoint.x);
                int y = Mathf.RoundToInt(mousePoint.y);

                if (x >= 0 && y >= 0 && x < SCREEN_WIHTH && y < SCREEN_HEIGHT)                  //確認是否在範圍內
                {
                    grid[x, y].SetAlive(true); 
                }
            }
        }
        if (Input.GetMouseButtonDown(0))                                                       //一般點擊
        {
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            int x = Mathf.RoundToInt(mousePoint.x);
            int y = Mathf.RoundToInt(mousePoint.y);

            if (x >= 0 && y >= 0 && x < SCREEN_WIHTH && y < SCREEN_HEIGHT)                    //確認是否在範圍內
            {
                grid[x, y].SetAlive(!grid[x, y].IsAlive);                                    //將點擊的Cell變成相反的狀態
            }
        }

        if (Input.GetKeyUp(KeyCode.P))                                                       //暫停or繼續遊戲 
        {
            if (simulationEnable)
            {
                simulationEnable = false;
            }
            else
            {
                simulationEnable = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.S))                                                             //存檔按鍵
        {
            SavePattern();
        }

        if (Input.GetKeyUp(KeyCode.R))                                                                   //隨機決定所有細胞死活
        {
            for (int y = 0; y < SCREEN_HEIGHT; y++)
            {
                for (int x = 0; x < SCREEN_WIHTH; x++)
                {
                    
                    grid[x, y].SetAlive(RandomAliveCell());
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.T))                                                                //重置所有細胞
        {
            for (int y = 0; y < SCREEN_HEIGHT; y++)
            {
                for (int x = 0; x < SCREEN_WIHTH; x++)
                {

                    grid[x, y].SetAlive(false);
                }
            }
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
                grid[x, y].SetAlive(false); 
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

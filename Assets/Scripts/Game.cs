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
                timer += Time.deltaTime;                                 //Time.deltatime �O�W�@�V�Ҫ᪺�ɶ�
            }
        }

        UserInput();

    }

    private void SavePattern()  //�s��                                     //�ݧ����G�i�H�ۤv���ɦW
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
    void UserInput()                                                                             //��J
    {
        if (Input.GetKey(KeyCode.LeftControl))                                                //����Ctrl�i�H����즲�ͦ���Cell
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                int x = Mathf.RoundToInt(mousePoint.x);
                int y = Mathf.RoundToInt(mousePoint.y);

                if (x >= 0 && y >= 0 && x < SCREEN_WIHTH && y < SCREEN_HEIGHT)                  //�T�{�O�_�b�d��
                {
                    grid[x, y].SetAlive(true); 
                }
            }
        }
        if (Input.GetMouseButtonDown(0))                                                       //�@���I��
        {
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            int x = Mathf.RoundToInt(mousePoint.x);
            int y = Mathf.RoundToInt(mousePoint.y);

            if (x >= 0 && y >= 0 && x < SCREEN_WIHTH && y < SCREEN_HEIGHT)                    //�T�{�O�_�b�d��
            {
                grid[x, y].SetAlive(!grid[x, y].IsAlive);                                    //�N�I����Cell�ܦ��ۤϪ����A
            }
        }

        if (Input.GetKeyUp(KeyCode.P))                                                       //�Ȱ�or�~��C�� 
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

        if (Input.GetKeyDown(KeyCode.S))                                                             //�s�ɫ���
        {
            SavePattern();
        }

        if (Input.GetKeyUp(KeyCode.R))                                                                   //�H���M�w�Ҧ��ӭM����
        {
            for (int y = 0; y < SCREEN_HEIGHT; y++)
            {
                for (int x = 0; x < SCREEN_WIHTH; x++)
                {
                    
                    grid[x, y].SetAlive(RandomAliveCell());
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.T))                                                                //���m�Ҧ��ӭM
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
                //�W�h
                //���ӭM���ǭY��2~3�Ӭ��ӭM�A�U�@�N�����ӭM
                //�Y�S���A�h���`
                //���ӭM���ǭY�����n3�ӲӭM�A�U�@�N�����ӭM
                //�Y�S���A�h�������`
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
                if (y + 1 < SCREEN_HEIGHT)                       //�_
                {
                    if (grid[x,y+1].IsAlive)
                    {
                        numNeighbors++;
                    }
                }
                if(x + 1 < SCREEN_WIHTH)                       //�F
                {
                    if (grid[x + 1, y].IsAlive)
                    {
                        numNeighbors++;
                    }
                }
                if (y - 1 >= 0)                                //�n
                {
                    if (grid[x, y - 1].IsAlive)
                    {
                        numNeighbors++;
                    }
                }
                if (x - 1 >= 0)                                //��
                {
                    if (grid[x - 1, y].IsAlive) 
                    {
                        numNeighbors++;
                    }
                }
                if (y + 1 < SCREEN_HEIGHT && x + 1 < SCREEN_WIHTH) //�F�_
                {
                    if (grid[x+1, y + 1].IsAlive)
                    {
                        numNeighbors++;
                    }
                }
                if (y + 1 < SCREEN_HEIGHT && x - 1 >= 0)       //��_
                {
                    if(grid[x - 1, y + 1].IsAlive)
                    {
                        numNeighbors++;
                    }
                }
                if (y - 1 >= 0 && x + 1 < SCREEN_WIHTH)         //�F�n
                {
                    if (grid[x + 1, y - 1].IsAlive)
                    {
                        numNeighbors++;
                    }
                }
                if (y - 1 >= 0 && x - 1 >= 0)                    //��n
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

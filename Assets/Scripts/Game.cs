using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private static int SCREEN_WIHTH = 192;                            //3072 pixels 
    private static int SCREEN_HEIGHT = 108;                           //1728 pixels 

    public float speed = 0.1f;

    private float timer=0;



    Cell[,] grid= new Cell[SCREEN_WIHTH, SCREEN_HEIGHT];

    // Start is called before the first frame update
    void Start()
    {
        PlaceCells();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > speed)
        {
            timer = 0;
            CountNeighbors();
            PopulationConcrol();
        }
        else
        {
            timer+=Time.deltaTime;                                 //Time.deltatime �O�W�@�V�Ҫ᪺�ɶ�
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

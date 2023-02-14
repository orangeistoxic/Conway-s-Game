using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridOverlay : MonoBehaviour
{
    private Material LineMaterial;

    public bool ShowMain = true;
    public bool ShowSub = false;


    //此處未定義之 public values皆於Unity裡面手動定義 以下為建議預設值(單位皆為Unity裡面的Unit)
    public int GridSizeX; //192
    public int GridSizeY; //108

    public float StartX; //-0.5  
    public float StartY; //-0.5              //隔此0.5的用意為讓grids的交錯是在bits的邊邊 而不是中間
    public float StartZ; //0                 //雖然實際不需要 但使用的GL需要Z值
    
    public float SmallStep; //1              //Sub線所使用
    public float LargeStep; //10             //Main線所使用

    public Color MainClolor = new Color(0f, 0.9f, 0f, 1f); //亮綠色
    public Color SubClolor = new Color(0f, 0.5f, 0f, 1f);//也是綠色但是更深


    
    void CreatLineMaterial()
    {
        if (!LineMaterial)
        {
            var shader = Shader.Find("Hidden/Internal-Colored");
            LineMaterial =new Material(shader);

            LineMaterial.hideFlags = HideFlags.HideAndDontSave;                         //阻止電腦自動 清理緩存時把他處理掉

            //Turn on Alpha blending  (我也不知道在幹嘛)
            LineMaterial.SetInt("SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            LineMaterial.SetInt("DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

            //Turn off the Depth Writing (還是不知道在幹嘛)
            LineMaterial.SetInt("ZWrite", 0);

            //Turn off backface culling(still不知道)
            LineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        }
    }

    private void OnDisable()               //關掉程式時會呼叫此函數，來手動把LineMaterial刪除
    {
        DestroyImmediate(LineMaterial);
    } 

    private void OnPostRender()
    {
        CreatLineMaterial();
        LineMaterial.SetPass(0);

        GL.Begin(GL.LINES);


                              //繪製小網格
        if (ShowSub)
        {
            GL.Color(SubClolor);
            for(float y = 0f; y <= GridSizeY; y += SmallStep)
            {
                GL.Vertex3(StartX, StartY + y, StartZ);               //起點
                GL.Vertex3(StartX + GridSizeX, StartY + y, StartZ);   //終點
            }
            for(float x = 0f; x <= GridSizeX; x += SmallStep)
            {
                GL.Vertex3(StartX + x, StartY, StartZ);              //起點
                GL.Vertex3(StartX + x, StartY + GridSizeY, StartZ);  //終點
            }
        }


                               //繪製大網格
        if(ShowMain)
        {
            GL.Color(MainClolor);
            for (float y = 0f; y <= GridSizeY; y += LargeStep)
            {
                GL.Vertex3(StartX, StartY + y, StartZ);               //起點
                GL.Vertex3(StartX + GridSizeX, StartY + y, StartZ);   //終點
            }
            for (float x = 0f; x <= GridSizeX; x += LargeStep)
            { 
                GL.Vertex3(StartX + x, StartY, StartZ);               //起點
                GL.Vertex3(StartX + x, StartY + GridSizeY, StartZ);   //終點
            }
        }


        GL.End();
    }
}

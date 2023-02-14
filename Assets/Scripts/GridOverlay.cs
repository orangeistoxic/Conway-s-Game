using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridOverlay : MonoBehaviour
{
    private Material LineMaterial;

    public bool ShowMain = true;
    public bool ShowSub = false;


    //���B���w�q�� public values�ҩ�Unity�̭���ʩw�q �H�U����ĳ�w�]��(���Ҭ�Unity�̭���Unit)
    public int GridSizeX; //192
    public int GridSizeY; //108

    public float StartX; //-0.5  
    public float StartY; //-0.5              //�j��0.5���ηN����grids������O�bbits������ �Ӥ��O����
    public float StartZ; //0                 //���M��ڤ��ݭn ���ϥΪ�GL�ݭnZ��
    
    public float SmallStep; //1              //Sub�u�Ҩϥ�
    public float LargeStep; //10             //Main�u�Ҩϥ�

    public Color MainClolor = new Color(0f, 0.9f, 0f, 1f); //�G���
    public Color SubClolor = new Color(0f, 0.5f, 0f, 1f);//�]�O�����O��`


    
    void CreatLineMaterial()
    {
        if (!LineMaterial)
        {
            var shader = Shader.Find("Hidden/Internal-Colored");
            LineMaterial =new Material(shader);

            LineMaterial.hideFlags = HideFlags.HideAndDontSave;                         //����q���۰� �M�z�w�s�ɧ�L�B�z��

            //Turn on Alpha blending  (�ڤ]�����D�b�F��)
            LineMaterial.SetInt("SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            LineMaterial.SetInt("DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

            //Turn off the Depth Writing (�٬O�����D�b�F��)
            LineMaterial.SetInt("ZWrite", 0);

            //Turn off backface culling(still�����D)
            LineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        }
    }

    private void OnDisable()               //�����{���ɷ|�I�s����ơA�Ӥ�ʧ�LineMaterial�R��
    {
        DestroyImmediate(LineMaterial);
    } 

    private void OnPostRender()
    {
        CreatLineMaterial();
        LineMaterial.SetPass(0);

        GL.Begin(GL.LINES);


                              //ø�s�p����
        if (ShowSub)
        {
            GL.Color(SubClolor);
            for(float y = 0f; y <= GridSizeY; y += SmallStep)
            {
                GL.Vertex3(StartX, StartY + y, StartZ);               //�_�I
                GL.Vertex3(StartX + GridSizeX, StartY + y, StartZ);   //���I
            }
            for(float x = 0f; x <= GridSizeX; x += SmallStep)
            {
                GL.Vertex3(StartX + x, StartY, StartZ);              //�_�I
                GL.Vertex3(StartX + x, StartY + GridSizeY, StartZ);  //���I
            }
        }


                               //ø�s�j����
        if(ShowMain)
        {
            GL.Color(MainClolor);
            for (float y = 0f; y <= GridSizeY; y += LargeStep)
            {
                GL.Vertex3(StartX, StartY + y, StartZ);               //�_�I
                GL.Vertex3(StartX + GridSizeX, StartY + y, StartZ);   //���I
            }
            for (float x = 0f; x <= GridSizeX; x += LargeStep)
            { 
                GL.Vertex3(StartX + x, StartY, StartZ);               //�_�I
                GL.Vertex3(StartX + x, StartY + GridSizeY, StartZ);   //���I
            }
        }


        GL.End();
    }
}

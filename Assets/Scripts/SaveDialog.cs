using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SaveDialog : MonoBehaviour
{

    public InputField patternName;

    public void savePattern()
    {
        gameObject.SetActive(false);                                                 //將此gameObject隱形於遊戲中
    }    

    public void quitDialog()
    {
        gameObject.SetActive(false);                                                 //將此gameObject隱形於遊戲中
    }
}
 
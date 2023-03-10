using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class SaveDialog : MonoBehaviour
{

    public TMP_InputField patternName;
    public HUD hud;

    public void savePattern()
    {
        EventManager.TriggerEvent("SavePattern");

        hud.isActive = false;
        gameObject.SetActive(false);                                                 //將此gameObject隱形於遊戲中
    }    

    public void quitDialog()
    {
        hud.isActive=false;
        gameObject.SetActive(false);                                                 //將此gameObject隱形於遊戲中
    }
}
 
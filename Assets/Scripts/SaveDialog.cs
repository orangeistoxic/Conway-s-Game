using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SaveDialog : MonoBehaviour
{

    public InputField patternName;

    public void savePattern()
    {
        gameObject.SetActive(false);                                                 //�N��gameObject���Ω�C����
    }    

    public void quitDialog()
    {
        gameObject.SetActive(false);                                                 //�N��gameObject���Ω�C����
    }
}
 
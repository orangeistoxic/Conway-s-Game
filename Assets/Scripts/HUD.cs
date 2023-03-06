using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public SaveDialog saveDialog;
    // Start is called before the first frame update
    void Start()
    {
        saveDialog.gameObject.SetActive(false);
    }

    public void showSaveDialog()
    {
        saveDialog.gameObject.SetActive(true); 
    }
}

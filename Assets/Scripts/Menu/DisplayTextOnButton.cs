using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTextOnButton: MonoBehaviour
{
    public Text Textfield;
    public void DisplayText(string text)
    {
        Textfield.text = text;
    }
}

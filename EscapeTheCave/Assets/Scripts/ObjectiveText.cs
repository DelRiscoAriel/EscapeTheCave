using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveText : MonoBehaviour
{
    public Text display;
    public int collected = 0;

    // Update is called once per frame
    void Update()
    {
        display.text = ("Collect the Statues( " + collected + "/3)");

        if (collected == 3)
        {
            display.text = ("Go back to the Exit");
        }
    }
}

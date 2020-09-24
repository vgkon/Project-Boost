using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Color : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI mainText;
    int r=0, g=0, b=0;


    // Update is called once per frame
    void Update()
    {
        r += Random.Range(-10, 10);
        if (r < 0) 
        {
            r += 255;
        }
        else if (r > 255) 
        {
            r -= 255;
        }
        g += Random.Range(-10, 10);
        if (g < 0)
        {
            g += 255;
        }
        else if (g > 255)
        {
            g -= 255;
        }
        b += Random.Range(-10, 10);
        if (b < 0)
        {
            b += 255;
        }
        else if (b > 255)
        {
            b -= 255;
        }
        mainText.color += new UnityEngine.Color(r, g, b);
    }
}

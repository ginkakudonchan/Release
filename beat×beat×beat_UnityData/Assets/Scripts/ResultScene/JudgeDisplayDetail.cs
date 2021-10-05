using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JudgeDisplayDetail : MonoBehaviour
{
    // 0:missEarly, 1:goodEarly, 2:greatEarly, 3:perfect, 4:greatLate, 5:goodLate, 6:missLate
    public Text[] CountText;

    void Start()
    {
        for (int i = 0; i < CountText.Length; i++)
        {
            CountText[i].text = JudgeDisplay.count[i].ToString();
        }
    }

}

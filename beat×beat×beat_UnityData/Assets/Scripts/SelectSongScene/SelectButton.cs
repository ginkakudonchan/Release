using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectButton : MonoBehaviour
{
    public string _fileName;

    public void SaveFileName(string fileName)
    {
        _fileName = fileName;
    }
}

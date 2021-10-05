using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarLineScript_Tutorial : MonoBehaviour
{
    void Update()
    {
        MoveBarLine(TutorialController.notesSpeed);
    }

    void MoveBarLine(float notesSpeed)
    {
        this.transform.position += Vector3.down * notesSpeed * Time.deltaTime;
        if (this.transform.position.y < -2.0f)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void ChangeNotesSpeed(float differenceTime, float notesSpeed)
    {
        Vector3 pos = this.transform.position;
        pos.y = differenceTime * notesSpeed;
        this.transform.position = pos;
    }
}

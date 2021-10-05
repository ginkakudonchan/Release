using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesScript_Tutorial : MonoBehaviour
{
    void Update()
    {
        MoveNotes(TutorialController.notesSpeed);
    }

    void MoveNotes(float notesSpeed)
    {
        this.transform.position += Vector3.down * notesSpeed * Time.deltaTime;
        if (this.transform.position.y < -0.2f * notesSpeed)
        {
            TutorialController._combo = 0;
            TutorialController.nowNotesNum++;
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

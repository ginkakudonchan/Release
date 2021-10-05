using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarLineScript : MonoBehaviour
{
    private int bar = 0;

    void Update()
    {
        if (GameController._isPlaying)
        {
            MoveBarLine(GameController.notesSpeed);
        }
        else if (EditController._isPlaying)
        {
            MoveBarLine(EditController.notesSpeed);
        }

        // EditMode専用の処理
        if (!EditController._isPlaying)
        {
            MoveStopBarLine(EditController.notesSpeed, EditController.endBarLineNum);
        }
    }

    void MoveBarLine(float notesSpeed)
    {
        this.transform.position += Vector3.down * notesSpeed * Time.deltaTime;
        if (this.transform.position.y < -2.0f)
        {
            if (TitleScene.gameSceneFlag)
            {
                this.gameObject.SetActive(false);
            }
        }
    }

    void MoveStopBarLine(float notesSpeed, int endBarLineNum)
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && bar < endBarLineNum)
        {
            if (bar == 0)
            {
                this.transform.position += Vector3.down * EditController.startBarLineTiming * notesSpeed;
            }
            else
            {
                this.transform.position += Vector3.down * EditController.barTiming * notesSpeed;
            }
            bar++;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && bar < endBarLineNum)
        {
            if (bar == 0)
            {
                this.transform.position += Vector3.down * EditController.startBarLineTiming * notesSpeed;
                this.transform.position += Vector3.down * EditController.barTiming * notesSpeed * (10 - 1);
                bar += 10;
            }
            else if (endBarLineNum < bar + 10)
            {
                this.transform.position += Vector3.down * EditController.barTiming * notesSpeed * (endBarLineNum - bar);
                bar = endBarLineNum;
            }
            else
            {
                this.transform.position += Vector3.down * EditController.barTiming * notesSpeed * 10;
                bar += 10;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && 0 < bar)
        {
            if (bar == 1)
            {
                this.transform.position += Vector3.up * EditController.startBarLineTiming * notesSpeed;
            }
            else
            {
                this.transform.position += Vector3.up * EditController.barTiming * notesSpeed;
            }
            bar--;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && 0 < bar)
        {
            if (bar <= 10)
            {
                this.transform.position += Vector3.up * EditController.startBarLineTiming * notesSpeed;
                this.transform.position += Vector3.up * EditController.barTiming * notesSpeed * (bar - 1);
                bar = 0;
            }
            else
            {
                this.transform.position += Vector3.up * EditController.barTiming * notesSpeed * 10;
                bar -= 10;
            }
        }
    }

    public void RestartEdit(float barLineTiming, int bar, float startBarLineTiming, float barTiming, float notesSpeed)
    {
        Vector3 pos = this.transform.position;
        pos.y = barLineTiming * notesSpeed;
        if (0 < bar)
        {
            pos.y -= startBarLineTiming * notesSpeed;
            pos.y -= barTiming * notesSpeed * (bar - 1);
        }
        this.transform.position = pos;
    }

    public void ChangeNotesSpeed(float differenceTime, float notesSpeed)
    {
        Vector3 pos = this.transform.position;
        pos.y = differenceTime * notesSpeed;
        this.transform.position = pos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesScript : MonoBehaviour
{
    private int bar = 0;

    void Update()
    {
        if (GameController._isPlaying) MoveNotes(GameController.notesSpeed);
        else if (EditController._isPlaying) MoveNotes(EditController.notesSpeed);

        // EditMode専用の処理
        if (!EditController._isPlaying) MoveStopNotes(EditController.notesSpeed, EditController.endBarLineNum);
    }

    void MoveNotes(float notesSpeed)
    {
        this.transform.position += Vector3.down * notesSpeed * Time.deltaTime;
        // ノーツが消えるのはゲームシーンのみ、あれチュートリアルは？
        // ノーツが通り過ぎたら、missLateカウント
        if (TitleScene.gameSceneFlag && this.transform.position.y < -0.2f * notesSpeed)
        {
            // count;; 0:missEarly, 1:goodEarly, 2:greatEarly, 3:perfect, 4:greatLate, 5:goodLate, 6:missLate
            // missLateは必ず要素数の最後（JudgeDisplay.count.Length - 1）
            JudgeDisplay.count[JudgeDisplay.count.Length - 1]++;
            GameController._combo = 0;
            GameController._comboEffectLimit = 10;
            this.gameObject.SetActive(false);
        }
    }

    // EditMode専用の処理
    void MoveStopNotes(float notesSpeed, int endBarLineNum)
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

    public void RestartEdit(float timing, int bar, float startBarLineTiming, float barTiming, float notesSpeed)
    {
        Vector3 pos = this.transform.position;
        pos.y = timing * notesSpeed;
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

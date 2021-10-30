// ロングノーツ用の処理
// 慣れてきたらNotesScriptと結合させたい

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNotesScript : MonoBehaviour
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

        if (TitleScene.gameSceneFlag)
        {
            int num = 0;
            // ノーツのx座標から、numを算出
            // x == (-6.0f, -2.0f, 2.0f, 6.0f, 0.0f) ; num = (0, 1, 2, 3, 4) 
            // num = (this.transform.position.x + 6.0f) / 4.0fでnum = 4以外は求められる
            // num = 4のときにこれをやるとnumが小数になるから、これを利用すると良いかも
            if (this.transform.position.x == -6.0f) num = 0;
            else if (this.transform.position.x == -2.0f) num = 1;
            else if (this.transform.position.x == 2.0f) num = 2;
            else if (this.transform.position.x == 6.0f) num = 3;
            else if (this.transform.position.x == 0.0f) num = 4;

            // ノーツが通り過ぎたら、missLateカウント
            // ただし、キーが押されていない（GameController._longNotesFlag[num]がfalse）ときのみ
            if (!GameController.longNotesFlag[num] && this.transform.position.y < -0.3f * notesSpeed)
            {

                // count;; 0:missEarly, 1:goodEarly, 2:greatEarly, 3:perfect, 4:greatLate, 5:goodLate, 6:missLate
                // missLateは必ず要素数の最後（JudgeDisplay.count.Length - 1）
                JudgeDisplay.count[JudgeDisplay.count.Length - 1]++;
                GameController._combo = 0;
                GameController._comboEffectLimit = 10;
                this.gameObject.SetActive(false);
            }
        }
    }

    // EditMode専用の処理
    void MoveStopNotes(float notesSpeed, int endBarLineNum)
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && bar < endBarLineNum)
        {
            if (bar == 0) this.transform.position += Vector3.down * EditController.startBarLineTiming * notesSpeed;
            else this.transform.position += Vector3.down * EditController.barTiming * notesSpeed;
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
            if (bar == 1) this.transform.position += Vector3.up * EditController.startBarLineTiming * notesSpeed;
            else this.transform.position += Vector3.up * EditController.barTiming * notesSpeed;
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

    public void ChangeNotesSpeed(float differenceTime, float preNotesSpeed, float notesSpeed)
    {
        Vector3 pos = this.transform.position;
        pos.y = differenceTime * notesSpeed;
        this.transform.position = pos;

        // ロングノーツの場合はy座標位置以外にもノーツサイズを変更させる必要がある
        // ノーツスピードを変更した場合、ノーツサイズを変更
        if (preNotesSpeed != notesSpeed)
        {
            Vector3 dummy;
            dummy = this.transform.localScale;
            dummy.y = dummy.y / preNotesSpeed * notesSpeed;
            this.transform.localScale = dummy;
        }
    }
}

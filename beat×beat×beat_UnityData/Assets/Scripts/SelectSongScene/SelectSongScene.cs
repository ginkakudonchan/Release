using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectSongScene : MonoBehaviour
{
    private GameObject parentObject;
    public GameObject ButtonPrefab;

    private Button button;
    private GameObject[] ButtonEffect = new GameObject[3];

    private int posY;

    void Start()
    {
        Time.timeScale = 1f;

        // static変数の初期化
        GameController.songName = "";
        GameController.audioFilePath = "";
        GameController.notesFilePath = "";

        // 楽曲データのパスを検索
        string folderPath = Application.dataPath + "/Resources";
        string[] fs = System.IO.Directory.GetFiles(@folderPath, "*");
        posY = 0;
        // 全楽曲データに対してボタンを生成
        for (int i = 0; i < fs.GetLength(0); i++)
        {
            string fileName = System.IO.Path.GetFileNameWithoutExtension(fs[i]);
            if (fileName == "CSV" || fileName == "Config" || fileName == "unity default resources" || fileName == "unity_builtin_extra")
            {
                continue;
            }

            GameObject Button;
            Button = Instantiate(ButtonPrefab,
                new Vector3(0, posY, 0),
                Quaternion.identity);
            posY -= 120;

            SelectButton selectButton = Button.GetComponent<SelectButton>();
            selectButton.SaveFileName(fileName);

            // ボタンのテキスト
            Text buttonText = null;
            // ボタンのテキストコンポーネントをキャッシュ
            buttonText = Button.GetComponentInChildren<Text>();
            buttonText.text = JsonSongData.ReadSongNameJson(fileName);

            // Canvasの子としてプレハブを生成したいので親にCanvasを登録
            parentObject = GameObject.Find("Canvas");
            // 生成したインスタンスをparentobjectの子、つまりCanvasの子として登録
            Button.transform.SetParent(parentObject.transform, false);
        }

        // キャンバス上の一番上の子オブジェクトボタンを選択
        button = GameObject.Find("Canvas").GetComponentInChildren<Button>();
        // ボタンが選択された状態になる
        button.Select();

        // ボタンが一番上から一番下(逆も可)に行く時の処理、つまりナビゲーションの自動化
        // 子のボタンやスライダーなど操作可能なコンポーネントを取得する
        var selects = GameObject.Find("Canvas").GetComponentsInChildren<Selectable>();
        for (var i = 0; i < selects.Length; i++)
        {
            var nav = selects[i].navigation;
            nav.mode = Navigation.Mode.Explicit;
            nav.selectOnUp = selects[i == 0 ? selects.Length - 1 : i - 1];
            nav.selectOnDown = selects[(i + 1) % selects.Length];
            selects[i].navigation = nav;
        }
    }
}

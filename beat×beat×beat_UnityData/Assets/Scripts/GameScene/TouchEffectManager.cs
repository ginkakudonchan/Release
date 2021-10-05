using UnityEngine;
using System.Collections;

public class TouchEffectManager : SingletonMonoBehaviour<TouchEffectManager>
{

    private GameObject[] _touchEffects;
    public Material[] _materials;

    void Start()
    {
        _touchEffects = new GameObject[5];
        for (int i = 0; i < 5; i++)
        {
            _touchEffects[i] = this.transform.GetChild(i).gameObject;
        }
    }

    // 0:S_Critical, 1:CriticalNear, 2:Error, 3:Air
    public void PlayEffect(int num, int judgeNum)
    {
        StartCoroutine(TouchEffect(num, judgeNum));
    }

    IEnumerator TouchEffect(int num, int judgeNum)
    {
        // アクティブ状態ならそのまま戻る
        if (_touchEffects[num].activeInHierarchy)
        {
            yield break;
        }

        _touchEffects[num].GetComponent<Renderer>().material = _materials[judgeNum];
        _touchEffects[num].SetActive(true);
        yield return new WaitForSeconds(0.1f);
        _touchEffects[num].SetActive(false);
    }
}

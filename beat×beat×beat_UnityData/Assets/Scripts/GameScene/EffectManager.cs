using UnityEngine;
using System.Collections;

public class EffectManager : SingletonMonoBehaviour<EffectManager>
{

    public GameObject[] gameObjects;
    private ParticleSystem[] particles;

    public void PlayEffect(int num, int judgeNum)
    {
        if (0 <= num && num <= 4)
        {
            // 0:S_Critical, 1:CriticalNear, 2:Error
            particles = gameObjects[num].GetComponentsInChildren<ParticleSystem>();
            particles[judgeNum].Play(true);
        }
    }
}

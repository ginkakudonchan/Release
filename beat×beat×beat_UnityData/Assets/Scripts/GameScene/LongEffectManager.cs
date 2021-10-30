using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongEffectManager : SingletonMonoBehaviour<LongEffectManager>
{
    public GameObject[] gameObjects;
    private ParticleSystem[] particles;

    public void PlayEffect(int num, int judgeNum)
    {
        if (0 <= num && num <= 4)
        {
            // 0:Perfect, 1:Great_Good, 2:Miss
            gameObjects[num].SetActive(true);
            particles = gameObjects[num].GetComponentsInChildren<ParticleSystem>();
            particles[judgeNum].Play(true);
        }
    }

    public void StopEffect(int num)
    {
        if (0 <= num && num <= 4)
        {
            // 0:Perfect, 1:Great_Good, 2:Miss
            gameObjects[num].SetActive(false);
            /*
            particles = gameObjects[num].GetComponentsInChildren<ParticleSystem>();
            particles[judgeNum].Stop(true);
            */
        }
    }
}

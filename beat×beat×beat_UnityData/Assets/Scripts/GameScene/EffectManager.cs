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
            // 0:Perfect, 1:Great_Good, 2:Miss
            particles = gameObjects[num].GetComponentsInChildren<ParticleSystem>();
            particles[judgeNum].Play(true);
        }
    }
}

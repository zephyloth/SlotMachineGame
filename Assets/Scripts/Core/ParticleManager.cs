using System;
using System.Collections;
using UnityEngine;

public class ParticleManager : SingletonBehaviour<ParticleManager>
{
    [SerializeField]
    private CoinBurstController CoinBurstController;

    [SerializeField]
    private UIShineParticle UIShineParticle;

    private AudioManager AudioManager;

    private WaitForSeconds WaitBeforeCoinBurst = new WaitForSeconds(0.2f);
 
    protected override bool Init()
    {
        if (base.Init()) return true;

        AudioManager = AudioManager.Instance as AudioManager;
        return true;
    }
 
    //Plays coin particles and plays sound effect
    public IEnumerator BurstCoinParticles(SymbolData[] SymbolDataArray)
    {
        yield return WaitBeforeCoinBurst;
        CoinBurstController.BurstCoins(SymbolDataArray);
        UIShineParticle.Play();
        AudioManager.PlayCurrencyGatherSound();
    }

    //Increase value with a stylistic effect
    public static IEnumerator CollectEffect(int Target, int Source, float Duration, int Steps, Action<int> TickCallback)
    {
        if (TickCallback == null) yield break;

        float Interval = Duration / Steps;
        int StepDist = Mathf.Abs(Target - Source)/ Steps;
        var WaitInterval = new WaitForSecondsRealtime(Interval);

        while (Source < Target)
        {
            Source += StepDist;
            TickCallback.Invoke(Source);
            yield return WaitInterval;
        }

        //Ensure applying the target value
        Source = Target;
        TickCallback.Invoke(Source);
    }
}

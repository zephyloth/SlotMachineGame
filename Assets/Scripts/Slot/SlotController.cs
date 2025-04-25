using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class SlotController : MonoBehaviour
{
    [SerializeField]
    private ReelController[] Reels;
 
    [SerializeField]
    private UISettingsPopup UISettingsPopup;
 
    public int ReelsCount => Reels.Length;

    //Current spin index
    public int SpinIndex { get; private set; }

    //Current spin distance
    public int SpinDistance { get; private set; }
 
    private WaitForSeconds WaitForReelActivation;
    private WaitForSeconds WaitForDelayAfterSpin;

    private GameManager GameManager;
    private ProbabilityManager ProbabilityManager;
    private SaveManager SaveManager;
    private GameSettings GameSettings;
 
    private void Start()
    {
        GameManager = GameManager.Instance as GameManager;
        ProbabilityManager = ProbabilityManager.Instance as ProbabilityManager;
        SaveManager = SaveManager.Instance as SaveManager;
        GameSettings = GameManager.GameSettings;

        UISettingsPopup.SettingsApplied += OnSettingsApplied;

        //Obtain spin index from the save
        SpinIndex = SaveManager.SaveData.SpinIndex;
 
        WaitForReelActivation = new WaitForSeconds(SaveManager.SaveData.DynamicSettings.ReelActivationDelay);
        WaitForDelayAfterSpin = new WaitForSeconds(GameSettings.WaitDelayAfterSpin);
    }
 
    //Main mechanism for maintaining the reels
    public IEnumerator Spin(Action<SymbolTriplet> SpinResultCallback)
    {
        var DynamicSettings = SaveManager.SaveData.DynamicSettings;

        //Choose a Spin distance and index
        SpinDistance = Random.Range(DynamicSettings.MinSpinDistance, DynamicSettings.MaxSpinDistance);
        SpinIndex = ProbabilityManager.WrapIndex(SpinIndex + SpinDistance);

        //Get spin result
        SymbolTriplet SpinResult = ProbabilityManager.GetSpinResult(SpinIndex);

        for (int i = 0; i < Reels.Length; i++)
        {   
            if ( i < Reels.Length - 1)
            {
                StartCoroutine(Reels[i].Spin(GameSettings.StopDurationFast));

                //Wait for the other reel start
                yield return WaitForReelActivation;
            }
            else
            {
                //Choose medium or slower duration time if the first two are the same
                float SlowDuration;
                if (SpinResult.AreFirstTwoSame())
                    SlowDuration = Random.Range(0, 2) == 1 ? GameSettings.StopDurationSlow : GameSettings.StopDurationMedium;
                else
                    SlowDuration = GameSettings.StopDurationFast;

                StartCoroutine(Reels[i].Spin(SlowDuration));
            }
        }
        
        //Wait for reels to spin
        while (ReelController.ReelsSpinningCount > 0)
            yield return null;

        yield return WaitForDelayAfterSpin;
        
        //Return the result
        SpinResultCallback?.Invoke(SpinResult);
    }

    private void OnSettingsApplied()
    {
        WaitForReelActivation = new WaitForSeconds(SaveManager.SaveData.DynamicSettings.ReelActivationDelay);
    }
}

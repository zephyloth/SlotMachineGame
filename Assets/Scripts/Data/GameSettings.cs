using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "CaseProject/Game Settings")]
public class GameSettings : ScriptableObject
{
    [Header("Generic Settings")]
    //Min reel spin distance
    public int MinSpinDistance;
    //Max reel spin distance
    public int MaxSpinDistance;
    //Time delay for the next reel activation
    public float ReelActivationDelay;
    //Spin duration in seconds
    public float SpinDuration;
    //Time delay after spin operation done
    public float WaitDelayAfterSpin;

    [Header("Effect Settings")]
    //Blur factor associated with reel speed
    public float BlurMultiplier;

    [Header("Stopping Methods")]
    //Specific stopping durations
    public float StopDurationFast = 0.09f;
    public float StopDurationMedium = 1f;
    public float StopDurationSlow = 2.25f;
 
    //Symbol data associated with type
    [Space]
    public List<SerializableKeyValue<SymbolType, SymbolData>> SymbolSettings;

    //Probability table object
    [Header("Probability Settings")]
    public ProbabilityTable ProbabilityTable;
}

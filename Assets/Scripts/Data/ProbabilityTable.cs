using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProbabilityTable", menuName = "CaseProject/Probability Table")]
public class ProbabilityTable : ScriptableObject
{
    //Count of probability rows
    public int MaxTripletCount;

    //List of triplet percentages
    public List<TripletProbability> TripletProbabilities;
}
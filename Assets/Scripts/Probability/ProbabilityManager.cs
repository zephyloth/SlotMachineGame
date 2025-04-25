using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ProbabilityManager : SingletonBehaviour<ProbabilityManager>
{
    private GameManager GameManager;
    private GameSettings GameSettings;
    private ProbabilityTable ProbabilityTable;

    private SymbolTriplet[] SpinArray;
    public SymbolTriplet GetSpinResult(int Index) => SpinArray[WrapIndex(Index)];

    protected override bool Init()
    {
        if (base.Init()) return true;

        GameManager = GameManager.Instance as GameManager;
        GameSettings = GameManager.GameSettings;
        ProbabilityTable = GameSettings.ProbabilityTable;

        //Generates and fills spin array
        FillSpinArray();

        return true;
    }
 
    //Generate probability entry pool
    private List<ProbabilityEntry> GenerateProbabilityPool()
    {
        var ProbabilityPool = new List<ProbabilityEntry>();
        foreach (var Probability in ProbabilityTable.TripletProbabilities)
            ProbabilityPool.Add(new ProbabilityEntry(Probability.Triplet, ProbabilityTable.MaxTripletCount, Probability.Percentage));
        
        return ProbabilityPool;
    }

    private void FillSpinArray()
    {
        var ProbabilityPool = GenerateProbabilityPool();
        var ProbabilityArray = new ProbabilityEntry[ProbabilityTable.MaxTripletCount];
        SpinArray = new SymbolTriplet[ProbabilityTable.MaxTripletCount];

        for (int i = 0; i < ProbabilityTable.MaxTripletCount; i++)
        {
            int MinIndex = -1;
            float MinValue = float.MaxValue;

            //Find entry with smallest next
            for (int j = 0; j < ProbabilityPool.Count; j++)
            {
                if (ProbabilityPool[j].Remaining > 0 && ProbabilityPool[j].Next < MinValue)
                {                
                    MinIndex = j;
                    MinValue = ProbabilityPool[j].Next;
                }
            }

            //Place the chosen element
            var Selected = ProbabilityPool[MinIndex];
            SpinArray[i] = Selected.Triplet;

            //Decrease the remaining count
            Selected.Remaining--;

            //Point to next available gap
            Selected.Next += Selected.Step;
            ProbabilityPool[MinIndex] = Selected;
        }
    }

    //Counts triplets in every gap and outputs the test result
    public void DoProbabilityTest(ref StringBuilder Output)
    {
        int MaxTripletCount = ProbabilityTable.MaxTripletCount;
        int TestProbCount = 0;

        //Ensure triplet count
        for (int i = 0; i < ProbabilityTable.TripletProbabilities.Count; i++)
            TestProbCount += ProbabilityTable.TripletProbabilities[i].Percentage;

        if (TestProbCount == ProbabilityTable.MaxTripletCount)
        {
            Output.AppendLine($"There are total <color=green>{TestProbCount}</color> probabilities in the table.");
        }
        else
        {
            Output.AppendLine("<color=red>sum of percentages didn't match with MaxTripletCount.</color>");
            return;
        }

        //Range test of probabilities
        Output.AppendLine($"The range of the probabilities is given below:")
        .AppendLine();
 
        int FailedTests = 0;
        foreach (var Probability in ProbabilityTable.TripletProbabilities)
        {
            int i = 0;
            while (i < MaxTripletCount)
            {
                int SymbolCount = 0;

                //Probability step (gap)
                int Gap = Mathf.FloorToInt((float)MaxTripletCount / Probability.Percentage);
                
                int Start = i;
                int End = i + Gap;

                if(End > MaxTripletCount)
                    End = MaxTripletCount;
 
                string LocatedIndices = "";

                //Count symbols inside the range
                for (; i < End; i++)
                {
                    if (SpinArray[i] == Probability.Triplet)
                    {
                        SymbolCount++;

                        if(LocatedIndices.Length != 0)
                            LocatedIndices += "-";

                        LocatedIndices += $"{i}";
                    }
                }
                if (LocatedIndices.Length == 0)
                    LocatedIndices = "None";

                //Indicates that test is failed when symbol count is above the number 1
                bool FailedTest = SymbolCount > 1;
                if (FailedTest)
                    FailedTests++;
 
                //Print range, count and index information
                Output.AppendLine($"<color=yellow>{Probability.Triplet}</color>");
                Output.AppendLine($"In range [{Start} - {End - 1}]: {GenericUtils.ColorizeText(SymbolCount, FailedTest ? "red" : "green")} Element(s) found at {LocatedIndices}.");
            }
            Output.AppendLine();
        }

        //Print the final test result
        if (FailedTests > 0)
            Output.AppendLine().AppendLine($"<color=red>{FailedTests} symbol triplets couldn't pass the test.</color>");
        else
            Output.AppendLine().AppendLine($"<color=green>Test executed successfully.</color>");
    }

    public int WrapIndex(int Index)
    {      
        return ((Index % ProbabilityTable.MaxTripletCount) + ProbabilityTable.MaxTripletCount) % ProbabilityTable.MaxTripletCount;
    }

    public int GetForwardDistance(int From, int To)
    {
        return (To - From + ProbabilityTable.MaxTripletCount) % ProbabilityTable.MaxTripletCount;
    }
}

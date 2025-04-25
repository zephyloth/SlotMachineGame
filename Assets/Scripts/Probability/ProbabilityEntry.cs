public struct ProbabilityEntry 
{
    public SymbolTriplet Triplet;

    //Remaining triplet count
    public int Remaining;

    //Point to next gap
    public float Next;

    //Gap size
    public float Step;

    public ProbabilityEntry(SymbolTriplet Triplet, int ArraySize, int ElementCount)
    {
        this.Triplet = Triplet;
        Remaining = ElementCount;
        Step = (float)ArraySize / ElementCount;
        Next = 0;
    }
}

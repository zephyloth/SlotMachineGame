using System;

[Serializable]
public struct SymbolTriplet : IEquatable<SymbolTriplet>
{
    public SymbolType Type1;
    public SymbolType Type2;
    public SymbolType Type3;

    private const string IndexOutOfRangeInfo = "Index must be 0, 1, or 2";

    //Indexer for practical use
    public SymbolType this[int index]
    {
        get => index switch
        {
            0 => Type1,
            1 => Type2,
            2 => Type3,
            _ => throw new IndexOutOfRangeException(IndexOutOfRangeInfo)
        };
        set
        {
            switch (index)
            {
                case 0: Type1 = value; break;
                case 1: Type2 = value; break;
                case 2: Type3 = value; break;
                default: throw new IndexOutOfRangeException(IndexOutOfRangeInfo);
            }
        }
    }

    public bool Equals(SymbolTriplet Other)
    {
        return Type1 == Other.Type1 && Type2 == Other.Type2 && Type3 == Other.Type3;
    }

    public override bool Equals(object Obj)
    {
        return Obj is SymbolTriplet Other && Equals(Other);
    }
 
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 33 + Type1.GetHashCode();
            hash = hash * 33 + Type2.GetHashCode();
            hash = hash * 33 + Type3.GetHashCode();
            return hash;
        }
    }

    public override string ToString()
    {
        return $"[{Type1}, {Type2}, {Type3}]";
    }

    public static bool operator ==(SymbolTriplet a, SymbolTriplet b) => a.Equals(b);
    public static bool operator !=(SymbolTriplet a, SymbolTriplet b) => !a.Equals(b);

    public bool AreFirstTwoSame() => Type1 == Type2;
    public bool AreAllSame() => Type1 == Type2 && Type1 == Type3;
}

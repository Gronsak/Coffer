namespace Coffer.Models;
public class Currency
{
    public Currency() {}
    public Currency(int isoNum, string name, string symbol, int decimals, string isoName)
    {
        this.ISONum = isoNum;
        this.Name = name;
        this.Symbol = symbol;
        this.Decimals = decimals;
        this.ISOName = isoName;
    }
    public int ISONum { get; set; }
    public string Name { get; set; } = "";
    public string Symbol { get; set; } = "";
    public int Decimals { get; set; }
    public string ISOName { get; set; } = "";
    public override bool Equals(object? obj)
    {
        if(obj is null || obj is not Currency)
            return false;
        var other = (Currency)obj;

        if (ISONum != other.ISONum ||
            Name != other.Name ||
            Symbol != other.Symbol ||
            Decimals != other.Decimals ||
            ISOName != other.ISOName)
            return false;

        return true;
    }
    public static bool operator ==(Currency x, Currency y){ return x.Equals(y); }
    public static bool operator !=(Currency x, Currency y){ return !x.Equals(y); }
    public override int GetHashCode()
    {
        return ISONum.GetHashCode()
            ^ Name.GetHashCode()
            ^ Symbol.GetHashCode()
            ^ Decimals.GetHashCode()
            ^ ISOName.GetHashCode();
    }
}
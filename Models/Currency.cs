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
    public int Id { get; set; }
    public int ISONum { get; set; }
    public string Name { get; set; } = "";
    public string Symbol { get; set; } = "";
    public int Decimals { get; set; }
    public string ISOName { get; set; } = "";
}
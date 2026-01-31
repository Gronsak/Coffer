namespace Coffer.Models;
public class Currency (int isoNum, string name, string symbol, int decimals, string isoName)
{
    public int ISONum { get; set; } = isoNum;
    public string Name { get; set; } = name;
    public string Symbol { get; set; } = symbol;
    public int Decimals { get; set; } = decimals;
    public string ISOName { get; set; } = isoName;
}
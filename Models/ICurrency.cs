namespace Coffer.Models;
public interface ICurrency
{
    int ISONum { get; set; }
    string Name { get; set; }
    string Symbol { get; set; }
    int Decimals { get; set; }
    string ISOName { get; set; }
}
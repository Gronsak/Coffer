using Coffer.Data;

namespace Coffer.Models;
public class IOU
{
    public IOU() {}
    public IOU(AppUser owedByUser, AppUser owedToUser, decimal amount, Currency currency)
    {
        this.OwedByUser = owedByUser;
        this.OwedToUser = owedToUser;
        this.Amount = amount;
        this.Currency = currency;
    }
    public Guid Id { get; set; }
    public AppUser OwedByUser { get; set; } = new();
    public AppUser OwedToUser { get; set; } = new();
    public decimal Amount { get; set; }
    public Currency Currency { get; set; } = new();
    public decimal SettledAmount { get; set; } = 0;
    public bool Settled { get; set; } = false;
    public bool UpdateIOU(IOU iou)
    {
        if(iou.Id != this.Id || iou.OwedByUser != this.OwedByUser)
            return false;
        
        this.OwedToUser = iou.OwedToUser;
        this.Amount = iou.Amount;
        this.Currency = iou.Currency;
        this.SettledAmount = iou.SettledAmount;
        this.Settled = iou.Settled;
        
        return true;
    }
}
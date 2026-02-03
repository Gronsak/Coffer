using Coffer.Data;

namespace Coffer.Models;
public class Cost
{
    public Cost() {}
    public Cost(Currency currency, AppUser userAdding, AppUser userPayed)
    {
        this.Currency = currency;
        this.AddedBy = userAdding;
        this.PayedBy = userPayed;
    }
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public decimal Amount { get; set; }
    public Currency Currency { get; set; } = new();
    public string Description { get; set; } = "";
    public DateTime PayedOn { get; set; } = DateTime.Now;
    public DateTime Added { get; set; } = DateTime.Now;
    public DateTime Updated { get; set; } = DateTime.Now;
    public AppUser AddedBy { get; set; } = new();
    public AppUser PayedBy { get; set; } = new();
    public List<Tag> Tags { get; set; } = [];
}
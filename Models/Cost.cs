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
    public Guid Id { get; set; }
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
    public bool UpdateCost(Cost cost)
    {
        if(this.Id != cost.Id
        || string.IsNullOrWhiteSpace(cost.Name)
        || string.IsNullOrWhiteSpace(cost.Currency.ISOName) || cost.Currency.ISONum == 0 || string.IsNullOrWhiteSpace(cost.Currency.Name))
            return false;
        
        this.Name = cost.Name;
        this.Amount = cost.Amount;
        this.Currency = cost.Currency;
        this.Description = cost.Description;
        this.PayedOn = cost.PayedOn;
        this.Updated = DateTime.Now;
        this.PayedBy = cost.PayedBy;
        this.Tags = cost.Tags;
        return true;
    }
    public override bool Equals(object? obj)
    {
        if (obj is null || obj is not Cost)
            return false;
        
        var other = (Cost)obj;
        if (Id != other.Id ||
            Name != other.Name ||
            Amount != other.Amount ||
            Currency != other.Currency ||
            Description != other.Description ||
            PayedOn != other.PayedOn ||
            Added != other.Added ||
            Updated != other.Updated ||
            AddedBy != other.AddedBy ||
            PayedBy != other.PayedBy ||
            !Tags.SequenceEqual(other.Tags))
            return false;
        
        return true;
    }
    public static bool operator ==(Cost x, Cost y){ return x.Equals(y); }
    public static bool operator !=(Cost x, Cost y){ return !x.Equals(y); }
    public override int GetHashCode()
    {
        return Id.GetHashCode() 
        ^ Name.GetHashCode() 
        ^ Amount.GetHashCode() 
        ^ Currency.GetHashCode() 
        ^ Description.GetHashCode() 
        ^ PayedOn.GetHashCode() 
        ^ Added.GetHashCode() 
        ^ Updated.GetHashCode() 
        ^ AddedBy.GetHashCode() 
        ^ PayedBy.GetHashCode() 
        ^ Tags.GetHashCode();
    }
}
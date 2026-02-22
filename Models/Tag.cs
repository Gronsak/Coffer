namespace Coffer.Models;
public class Tag
{
    public Tag() {}
    public Tag(string name)
    {
        this.Name = name;
    }
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public List<Cost> Costs { get; } = [];
    public List<Share> IncludedShares { get; } = [];
    public List<Share> ExcludedShares { get; } = [];

    public override bool Equals(object? obj)
    {
        if (obj is null || obj is not Tag)
            return false;
        
        var other = (Tag)obj;
        if (Name != other.Name)
            return false;
        
        return true;
    }
    public static bool operator ==(Tag x, Tag y){ return x.Equals(y); }
    public static bool operator !=(Tag x, Tag y){ return !x.Equals(y); }
    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}
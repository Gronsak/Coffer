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
}
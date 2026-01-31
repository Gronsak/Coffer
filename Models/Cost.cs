using Coffer.Areas.Identity.Data;

namespace Coffer.Models;
public class Cost
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Sum { get; set; }
    public Currency Currency { get; set; }
    public string Description { get; set; }
    public DateOnly PayedOn { get; set; }
    public DateTime Added { get; set; }
    public DateTime Updated { get; set; }
    public AppUser AddedBy { get; set; }
    public AppUser PayedBy { get; set; }
    public List<Tag> Tag { get; set; }
}
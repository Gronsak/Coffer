using Coffer.Areas.Identity.Data;

namespace Coffer.Models;
public class Cost(Currency currency, AppUser userAdding)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public decimal Amount { get; set; }
    public Currency Currency { get; set; } = currency;
    public string Description { get; set; } = "";
    public DateTime PayedOn { get; set; }
    public DateTime Added { get; set; } = DateTime.Now;
    public DateTime Updated { get; set; } = DateTime.Now;
    public AppUser AddedBy { get; set; } = userAdding;
    public AppUser? PayedBy { get; set; }
    public List<Tag> Tag { get; set; } = new List<Tag>();
}
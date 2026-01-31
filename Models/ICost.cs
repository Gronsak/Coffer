using Coffer.Areas.Identity.Data;

namespace Coffer.Models;
public interface ICost
{
    Guid Id { get; set; }
    string Name { get; set; }
    decimal Sum { get; set; }
    ICurrency Currency { get; set; }
    string Description { get; set; }
    DateOnly PayedOn { get; set; }
    DateTime Added { get; set; }
    DateTime Updated { get; set; }
    AppUser AddedBy { get; set; }
    AppUser PayedBy { get; set; }
    List<ITag> Tag { get; set; }
}
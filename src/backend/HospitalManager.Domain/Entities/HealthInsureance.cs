namespace HospitalManager.Domain.Entities;
public class HealthInsureance : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public int DiscountPercentage { get; set; }
    public bool Active { get; set; } = true;
}

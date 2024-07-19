namespace HospitalManager.Domain.Entities;
public class Medicine : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Composition { get; set; } = string.Empty;
    public Guid PatientId { get; set; }
}

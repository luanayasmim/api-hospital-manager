namespace HospitalManager.Domain.Entities;
public class MedicalReport : EntityBase
{
    public string Description { get; set; } = string.Empty;
    public string Cid { get; set; } = string.Empty;
    public Guid ConsultationId { get; set; }
}

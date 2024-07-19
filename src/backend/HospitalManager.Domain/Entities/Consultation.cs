using HospitalManager.Domain.Enums;

namespace HospitalManager.Domain.Entities;
public class Consultation : EntityBase
{
    public ConsultationType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime SchedulingAt { get; set; }
    public DateTime? SchedulingReturnAt { get; set; }
    public Status Status { get; set; }
    public decimal Price { get; set; }
    public bool HasDiscount { get; set; }
    public decimal? PriceWithDiscount { get; set; }
    public Guid DoctorId { get; set; }
    public Guid PatientId { get; set; }
    public IEnumerable<MedicalReport> MedicalReports { get; set; } = [];
}

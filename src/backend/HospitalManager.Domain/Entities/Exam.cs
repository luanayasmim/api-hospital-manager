using HospitalManager.Domain.Enums;

namespace HospitalManager.Domain.Entities;
public class Exam : EntityBase
{
    public ExamType Type { get; set; }
    public string? Description { get; set; }
    public DateTime SchedulingAt { get; set; }
    public Status Status { get; set; }
    public decimal Price { get; set; }
    public bool HasDiscount { get; set; }
    public decimal? PriceWithDiscount { get; set; }
    public string? DocResultPath { get; set; }
    public Guid ResponsableDoctorId { get; set; }
}

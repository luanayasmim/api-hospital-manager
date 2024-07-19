using HospitalManager.Domain.Enums;

namespace HospitalManager.Domain.Entities;
public class Patient : EntityBase
{
    public string FullName { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string? ImageDocumentPath { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender? Gender { get; set; }
    public decimal? Weight { get; set; }
    public decimal? Height { get; set; }
    public bool Active { get; set; } = true;
    public bool HasHealthInsurance { get; set; }
    public string? ImageHealthInsurancePath { get; set; }
    public Guid? HealthInsureanceId { get; set; }
    public Guid UserId { get; set; }

    public IEnumerable<Consultation> Consultations { get; set; } = [];
    public IEnumerable<Exam> Exams { get; set; } = [];
    public IEnumerable<MedicalReport> Reports { get; set; } = [];
}

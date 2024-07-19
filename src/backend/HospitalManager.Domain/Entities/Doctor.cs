using HospitalManager.Domain.Enums;

namespace HospitalManager.Domain.Entities;
public class Doctor : EntityBase
{
    public string FullName { get; set; } = string.Empty;
    public string Crm { get; set; } = string.Empty;
    public string? ImageDocumentPath { get; set; }
    public DateTime HiredAt { get; set; }
    public bool Active { get; set; } = true;
    public AreaDoctor Area { get; set; }
    public SpecializationDoctor Specialization { get; set; }
    public Guid UserId { get; set; }

    public IEnumerable<Consultation>? Consultations { get; set; } = [];
    public IEnumerable<Exam>? Exams { get; set; } = [];
    public IEnumerable<MedicalReport>? Reports { get; set; } = [];
}

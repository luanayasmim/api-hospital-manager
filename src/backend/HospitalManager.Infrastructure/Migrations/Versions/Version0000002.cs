using FluentMigrator;

namespace HospitalManager.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLES_HOSPITAL, "Create all tables to save the hospital's information")]
public class Version0000002 : VersionBase
{
    public override void Up()
    {
        #region Tabela de Médico
        CreateTable("Doctors")
            .WithColumn("FullName").AsString(100).NotNullable()
            .WithColumn("Crm").AsString(13).NotNullable() //CRM/SP 123456
            .WithColumn("ImageDocumentPath").AsString().Nullable()
            .WithColumn("HiredAt").AsDateTime().NotNullable()
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("Area").AsInt32().NotNullable()
            .WithColumn("Specialization").AsInt32().NotNullable()
            // Na relação de 1-1 a chave estrangeira deve ficar na entidade que é a "dona" da outra.
            // Exemplo: um médico cadastrado possui um usuário para acessar o sistema
            .WithColumn("UserId").AsGuid().NotNullable().ForeignKey("FK_Doctor_User_Id", "Users", "Id");
        #endregion

        #region Tabela de Convênio 
        CreateTable("HealthInsureances")
            .WithColumn("Name").AsString().NotNullable()
            .WithColumn("DiscountPercentage").AsInt16().NotNullable()
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true);
        #endregion

        #region Tabela de Paciente
        CreateTable("Patients")
                .WithColumn("FullName").AsString().NotNullable()
                .WithColumn("Cpf").AsString().NotNullable()
                .WithColumn("ImageDocumentPath").AsString().Nullable()
                .WithColumn("BirthDate").AsDateTime().NotNullable()
                .WithColumn("Gender").AsInt16().Nullable()
                .WithColumn("Weight").AsDecimal().Nullable()
                .WithColumn("Height").AsDecimal().Nullable()
                .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn("HasHealthInsurance").AsBoolean().NotNullable()
                .WithColumn("ImageHealthInsurancePath").AsString().Nullable()
                .WithColumn("HealthInsureanceId").AsGuid().Nullable().ForeignKey("FK_Patient_HealthInsureance_Id", "HealthInsureances", "Id")
                .WithColumn("UserId").AsGuid().NotNullable().ForeignKey("FK_Patient_User_Id", "Users", "Id");
        #endregion

        #region Tabela de Remédios que o paciente toma
        CreateTable("Medicines")
            .WithColumn("Name").AsString().NotNullable()
            .WithColumn("Description").AsString().Nullable()
            .WithColumn("Type").AsString().Nullable()
            .WithColumn("Composition").AsString().Nullable()
            .WithColumn("PatientId").AsGuid().NotNullable().ForeignKey("FK_Medicine_Patient_Id", "Patients", "Id")
            .OnDelete(System.Data.Rule.Cascade);
        #endregion

        #region Tabela de Consultas
        CreateTable("Consultations")
            .WithColumn("Type").AsInt32().NotNullable()
            .WithColumn("Description").AsString().NotNullable()
            .WithColumn("SchedulingAt").AsDateTime().NotNullable()
            .WithColumn("SchedulingReturnAt").AsDateTime().Nullable()
            .WithColumn("Status").AsInt32().NotNullable()
            .WithColumn("Price").AsDecimal().NotNullable()
            .WithColumn("HasDiscount").AsBoolean().NotNullable()
            .WithColumn("PriceWithDiscount").AsDecimal().Nullable()
            .WithColumn("DoctorId").AsGuid().NotNullable().ForeignKey("FK_Consultation_Doctor_Id", "Doctors", "Id")
            .WithColumn("PatientId").AsGuid().NotNullable().ForeignKey("FK_Consultation_Patient_Id", "Patients", "Id");
        #endregion

        #region Tabela de Laudos Médicos
        CreateTable("MedicalReports")
            .WithColumn("Description").AsString().NotNullable()
            .WithColumn("Cid").AsString(3).Nullable()
            .WithColumn("ConsultationId").AsGuid().NotNullable().ForeignKey("FK_MedicalReports_Consultation_Id", "Consultations", "Id");
        #endregion

        #region Tebela de Exames
        CreateTable("Exams")
            .WithColumn("Type").AsInt32().NotNullable()
            .WithColumn("Description").AsString().Nullable()
            .WithColumn("SchedulingAt").AsDateTime().NotNullable()
            .WithColumn("Status").AsInt32().NotNullable()
            .WithColumn("Price").AsDecimal().NotNullable()
            .WithColumn("HasDiscount").AsBoolean().NotNullable()
            .WithColumn("PriceWithDiscount").AsDecimal().Nullable()
            .WithColumn("DocResultPath").AsString().Nullable()
            .WithColumn("ResponsableDoctorId").AsGuid().NotNullable().ForeignKey("FK_Exam_Doctor_Id", "Doctors", "Id")
            .WithColumn("PatientId").AsGuid().NotNullable().ForeignKey("FK_Exam_Patient_Id", "Patients", "Id");
        #endregion
    }
}

using FluentMigrator;

namespace HospitalManager.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_USER, "Create table to save the user's information")]
public class Version0000001 : VersionBase
{
    public override void Up()
    {
        #region Tabela de Usuários
        CreateTable("Users")
           .WithColumn("Name").AsString(255).NotNullable()
           .WithColumn("Email").AsString(255).NotNullable()
           .WithColumn("Password").AsString(2000).NotNullable()
           .WithColumn("Role").AsInt32().Nullable()
           .WithColumn("Active").AsBoolean().NotNullable()
           .WithColumn("ImageProfile").AsString()
           .WithColumn("UserIdentifier").AsGuid().NotNullable();
        #endregion
    }
}

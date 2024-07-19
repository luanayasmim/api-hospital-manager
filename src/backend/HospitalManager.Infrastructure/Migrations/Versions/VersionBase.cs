using FluentMigrator;
using FluentMigrator.Builders.Create.Table;

namespace HospitalManager.Infrastructure.Migrations.Versions;
public abstract class VersionBase : ForwardOnlyMigration
{
    protected ICreateTableColumnOptionOrWithColumnSyntax CreateTable(string table)
    {
        return Create.Table(table)
           .WithColumn("Id").AsGuid().PrimaryKey()
           .WithColumn("CreatedAt").AsDateTime().NotNullable();
    }
}
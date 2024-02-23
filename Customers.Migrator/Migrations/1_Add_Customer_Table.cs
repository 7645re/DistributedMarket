using FluentMigrator;

namespace Customers.Migrator.Migrations;

[Migration(1)]
public class AddCustomerTable : Migration 
{
    public override void Up()
    {
        Create
            .Table("Customer")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Name").AsString(50).NotNullable()
            .WithColumn("Email").AsString(50).NotNullable();
    }

    public override void Down()
    {
        Delete.Table("Customer");
    }
}
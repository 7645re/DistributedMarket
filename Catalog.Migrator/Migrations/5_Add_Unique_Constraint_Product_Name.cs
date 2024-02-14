using FluentMigrator;

namespace Catalog.Migrator.Migrations;

[Migration(5)]
public class AddUniqueConstraintProductName : Migration
{
    public override void Up()
    {
        Create.UniqueConstraint("UQ_Product_Name").OnTable("Product").Column("Name");
    }

    public override void Down()
    {
        Delete.UniqueConstraint("UQ_Product_Name").FromTable("Product");
    }
}
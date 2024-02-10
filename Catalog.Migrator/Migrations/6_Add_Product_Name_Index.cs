using FluentMigrator;

namespace Catalog.Migrator.Migrations;

[Migration(6)]
public class AddProductNameIndex : Migration
{
    public override void Up()
    {
        Create.Index("IX_Product_Name").OnTable("Product").OnColumn("Name");
    }

    public override void Down()
    {
        Delete.Index("IX_Product_Name").OnTable("Product");
    }
}
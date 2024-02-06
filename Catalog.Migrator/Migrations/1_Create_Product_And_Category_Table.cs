using FluentMigrator;

namespace Catalog.Migrator.Migrations;

[Migration(1)]
public class CreateProductAndCategoryTables : Migration
{
    public override void Up()
    {
        Create
            .Table("Product")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Name").AsString(20).NotNullable()
            .WithColumn("Price").AsDecimal().NotNullable()
            .WithColumn("Description").AsString(100).NotNullable();
        
        Create.Table("Category")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Name").AsString(20).NotNullable();
    }

    public override void Down()
    {
        Delete.Table("Product");
        Delete.Table("Category");
    }
}
using System.Data;
using FluentMigrator;

namespace Catalog.Migrator.Migrations;

[Migration(2)]
public class CreateProductCategoryTable : Migration
{
    public override void Up()
    {
        Create.Table("ProductCategory")
            .WithColumn("ProductId").AsInt32().ForeignKey("Product", "Id").OnDelete(Rule.Cascade).NotNullable()
            .WithColumn("CategoryId").AsInt32().ForeignKey("Category", "Id").NotNullable();
    }

    public override void Down()
    {
        Delete.Table("ProductCategory");
    }
}
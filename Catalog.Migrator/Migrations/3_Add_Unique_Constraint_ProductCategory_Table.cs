using FluentMigrator;

namespace Catalog.Migrator.Migrations;

[Migration(3)]
public class UniqueConstraintProductCategoryTable : Migration
{
    public override void Up()
    {
        Create.UniqueConstraint("UQ_ProductCategory").OnTable("ProductCategory").Columns("ProductId", "CategoryId");
    }

    public override void Down()
    {
        Delete.UniqueConstraint("UQ_ProductCategory").FromTable("ProductCategory");
    }
}
using FluentMigrator;

namespace Catalog.Migrator.Migrations;

[Migration(4)]
public class AddUniqueConstraintCategoryName : Migration 
{
    public override void Up()
    {
        Create.UniqueConstraint("UQ_Category_Name").OnTable("Category").Column("Name");
    }

    public override void Down()
    {
        Delete.UniqueConstraint("UQ_Category_Name").FromTable("Category");
    }
}
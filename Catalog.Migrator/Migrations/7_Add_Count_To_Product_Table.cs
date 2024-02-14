using FluentMigrator;

namespace Catalog.Migrator.Migrations;

[Migration(7)]
public class AddCountToProductTable : Migration {
    public override void Up()
    {
        Create.Column("Count").OnTable("Product").AsInt32().WithDefaultValue(0);
    }

    public override void Down()
    {
        Delete.Column("Count").FromTable("Product");
    }
}
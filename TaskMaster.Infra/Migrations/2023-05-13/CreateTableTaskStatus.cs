using FluentMigrator;
using System;

namespace TaskMaster.Infra.Migrations
{
    [Migration(1683997845)]
    public class CreateTableTaskStatus : Migration
    {
        private readonly string tableName = "task_status";

        private readonly string id = "id";
        private readonly string name = "name";
        private readonly string description = "description";
        private readonly string createdAt = "created_at";
        private readonly string updatedAt = "updated_at";

        public override void Up()
        {
            if (!Schema.Table(tableName).Exists())
            {
                Create.Table(tableName)
                    .WithColumn(id)
                        .AsInt32()
                        .PrimaryKey()
                        .Identity()
                    .WithColumn(name)
                        .AsString(50)
                        .NotNullable()
                    .WithColumn(description)
                        .AsString(255)
                        .Nullable()
                    .WithColumn(createdAt)
                        .AsDateTime()
                        .WithDefault(SystemMethods.CurrentDateTime)
                    .WithColumn(updatedAt)
                        .AsDateTime()
                        .Nullable()
                ;
            }
        }

        public override void Down()
        {
            if (Schema.Table(tableName).Exists())
            {
                Delete.Table(tableName);
            }
        }
    }
}

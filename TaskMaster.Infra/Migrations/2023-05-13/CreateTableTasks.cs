using FluentMigrator;
using System;
using System.Data;

namespace TaskMaster.Infra.Migrations
{
    [Migration(1683997854)]
    public class CreateTableTasks : Migration
    {
        private readonly string tableName = "tasks";
        private readonly string users = "users";
        private readonly string taskStatus = "task_status";

        private readonly string id = "id";
        private readonly string name = "name";
        private readonly string description = "description";
        private readonly string userId = "user_id";
        private readonly string statusId = "status_id";
        private readonly string createdAt = "created_at";
        private readonly string updatedAt = "updated_at";
        private readonly string deadline = "deadline";

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
                    .WithColumn(userId)
                        .AsInt32()
                        .Nullable()
                        .ForeignKey(users, "id").OnDelete(Rule.Cascade)
                    .WithColumn(statusId)
                        .AsInt32()
                        .NotNullable()
                        .ForeignKey(taskStatus, "id").OnDelete(Rule.Cascade)
                    .WithColumn(createdAt)
                        .AsDateTime()
                        .WithDefault(SystemMethods.CurrentDateTime)
                    .WithColumn(updatedAt)
                        .AsDateTime()
                        .Nullable()
                    .WithColumn(deadline)
                        .AsDateTime()
                        .NotNullable()
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

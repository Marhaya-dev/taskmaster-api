using FluentMigrator;
using System;

namespace TaskMaster.Infra.Migrations
{
    [Migration(1683996276)]
    public class CreateTableUsers : Migration
    {
        private readonly string tableName = "users";

        private readonly string refreshToken = "refresh_token";
        private readonly string refrestTokenExpiresAt = "refresh_token_expires_at";

        private readonly string id = "id";
        private readonly string name = "name";
        private readonly string email = "email";
        private readonly string password = "password";
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
                    .WithColumn(email)
                        .AsString(255)
                        .NotNullable()
                    .WithColumn(password)
                        .AsString(255)
                        .Nullable()
                    .WithColumn(refreshToken)
                        .AsString(255)
                        .Nullable()
                    .WithColumn(refrestTokenExpiresAt)
                        .AsDateTime()
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

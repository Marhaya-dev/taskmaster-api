using FluentMigrator;
using TaskMaster.Domain.Helpers;

namespace TaskMaster.Infra.Migrations._2023_05_13
{
    [Migration(1683998850)]
    public class InsertApplicationAdmin : Migration
    {
        private readonly string tableName = "users";

        public override void Up()
        {
            if (Schema.Table(tableName).Exists())
            {
                Insert.IntoTable(tableName)
                .Row(new
                {
                    name = "Administrador da Aplicação",
                    email = "admin@taskmaster.com",
                    password = PasswordHelper.GetHash("yxc78nvcfz6-9")
                });
            }
        }

        public override void Down()
        {
            if (Schema.Table(tableName).Exists())
            {
                Execute.Sql($@"DELETE FROM {tableName} WHERE email = 'admin@taskmaster.com';");
            }
        }
    }
}

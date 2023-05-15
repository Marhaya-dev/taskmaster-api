using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMaster.Domain.Helpers;

namespace TaskMaster.Infra.Migrations._2023_05_13
{
    [Migration(1683999418)]
    public class InsertDefaultTaskStatus : Migration
    {
        private readonly string tableName = "task_status";

        public override void Up()
        {
            if (Schema.Table(tableName).Exists())
            {
                Insert.IntoTable(tableName)
                .Row(new
                {
                    name = "Not Started",
                    description = "Indicates that the task has not yet started.",
                })
                .Row(new
                {
                    name = "In Progress",
                    description = "Indicates that the task is currently being worked on.",
                })
                .Row(new
                {
                    name = "Completed",
                    description = "Indicates that the task completed successfully.",
                })
                .Row(new
                {
                    name = "Pending",
                    description = "Indicates that the task is pending and awaiting further action before starting.",
                })
                ;
            }
        }

        public override void Down()
        {
            if (Schema.Table(tableName).Exists())
            {
                Delete.FromTable(tableName)
                .Row(new
                 {
                     name = "Not Started",
                     description = "Indicates that the task has not yet started.",
                 })
                .Row(new
                {
                    name = "In Progress",
                    description = "Indicates that the task is currently being worked on.",
                })
                .Row(new
                {
                    name = "Completed",
                    description = "Indicates that the task completed successfully.",
                })
                .Row(new
                {
                    name = "Pending",
                    description = "Indicates that the task is pending and awaiting further action before starting.",
                })
                ;
            }
        }
    }
}

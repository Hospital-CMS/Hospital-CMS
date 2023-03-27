namespace Hospital_CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class departmentdonors : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Departments", "DonorID", c => c.Int(nullable: false));
            CreateIndex("dbo.Departments", "DonorID");
            AddForeignKey("dbo.Departments", "DonorID", "dbo.Donors", "DonorID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Departments", "DonorID", "dbo.Donors");
            DropIndex("dbo.Departments", new[] { "DonorID" });
            DropColumn("dbo.Departments", "DonorID");
        }
    }
}

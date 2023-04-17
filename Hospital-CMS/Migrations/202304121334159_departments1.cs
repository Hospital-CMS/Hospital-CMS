namespace Hospital_CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class departments1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Departments", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.Departments", "DonorID", "dbo.Donors");
            DropIndex("dbo.Departments", new[] { "DonorID" });
            DropIndex("dbo.Departments", new[] { "DoctorId" });
            DropColumn("dbo.Departments", "DonorID");
            DropColumn("dbo.Departments", "DoctorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Departments", "DoctorId", c => c.Int(nullable: false));
            AddColumn("dbo.Departments", "DonorID", c => c.Int(nullable: false));
            CreateIndex("dbo.Departments", "DoctorId");
            CreateIndex("dbo.Departments", "DonorID");
            AddForeignKey("dbo.Departments", "DonorID", "dbo.Donors", "DonorID", cascadeDelete: true);
            AddForeignKey("dbo.Departments", "DoctorId", "dbo.Doctors", "DoctorId", cascadeDelete: true);
        }
    }
}

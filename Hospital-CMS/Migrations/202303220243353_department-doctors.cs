namespace Hospital_CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class departmentdoctors : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Departments", "DoctorId", c => c.Int(nullable: false));
            CreateIndex("dbo.Departments", "DoctorId");
            AddForeignKey("dbo.Departments", "DoctorId", "dbo.Doctors", "DoctorId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Departments", "DoctorId", "dbo.Doctors");
            DropIndex("dbo.Departments", new[] { "DoctorId" });
            DropColumn("dbo.Departments", "DoctorId");
        }
    }
}

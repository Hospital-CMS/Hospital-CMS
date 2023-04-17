namespace Hospital_CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DepartmentDonors : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DonorDepartments",
                c => new
                    {
                        Donor_DonorID = c.Int(nullable: false),
                        Department_DepartmentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Donor_DonorID, t.Department_DepartmentID })
                .ForeignKey("dbo.Donors", t => t.Donor_DonorID, cascadeDelete: true)
                .ForeignKey("dbo.Departments", t => t.Department_DepartmentID, cascadeDelete: true)
                .Index(t => t.Donor_DonorID)
                .Index(t => t.Department_DepartmentID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DonorDepartments", "Department_DepartmentID", "dbo.Departments");
            DropForeignKey("dbo.DonorDepartments", "Donor_DonorID", "dbo.Donors");
            DropIndex("dbo.DonorDepartments", new[] { "Department_DepartmentID" });
            DropIndex("dbo.DonorDepartments", new[] { "Donor_DonorID" });
            DropTable("dbo.DonorDepartments");
        }
    }
}

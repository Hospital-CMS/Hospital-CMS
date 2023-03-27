namespace Hospital_CMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class donors : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Donors",
                c => new
                    {
                        DonorID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Contact = c.Int(nullable: false),
                        Address = c.String(),
                        Value = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DonorID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Donors");
        }
    }
}

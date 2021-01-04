namespace InterviewSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserAppMaps", "AppId_Id", "dbo.Apps");
            DropIndex("dbo.UserAppMaps", new[] { "AppId_Id" });
            DropTable("dbo.Apps");
            DropTable("dbo.MainAppCredentials");
            DropTable("dbo.UserAppMaps");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserAppMaps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        AppId_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MainAppCredentials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Apps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationName = c.String(),
                        ApplicationURL = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.UserAppMaps", "AppId_Id");
            AddForeignKey("dbo.UserAppMaps", "AppId_Id", "dbo.Apps", "Id");
        }
    }
}

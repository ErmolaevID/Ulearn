using System.Data.Entity.Migrations;

namespace Database.Migrations
{
	public partial class AddUsersFullTextIndex : DbMigration
	{
		public override void Up()
		{
			Sql("ALTER TABLE dbo.AspNetUsers ADD Names AS ISNULL(UserName, '') + ' ' + ISNULL(FirstName, '') + ' ' + ISNULL(LastName, '')");
			Sql("CREATE FULLTEXT CATALOG AspNetUsersFullTextCatalog WITH ACCENT_SENSITIVITY = OFF", true);
			Sql("CREATE FULLTEXT INDEX ON dbo.AspNetUsers ([Names] LANGUAGE [Russian]) KEY INDEX [PK_dbo.AspNetUsers] ON (AspNetUsersFullTextCatalog, FILEGROUP[PRIMARY]) WITH (CHANGE_TRACKING = AUTO, STOPLIST = SYSTEM)", true);
			Sql("CREATE FUNCTION dbo.GetUsersByNamePrefix (@name NVARCHAR(4000)) RETURNS TABLE RETURN SELECT [Id] FROM dbo.AspNetUsers WHERE CONTAINS([Names], @name)");
		}

		public override void Down()
		{
		    DropColumn("dbo.AspNetUsers", "Names");
			Sql("DROP FUNCTION dbo.GetUsersByNamePrefix");
			Sql("DROP FULLTEXT INDEX ON dbo.AspNetUsers", true);
			Sql("DROP FULLTEXT CATALOG AspNetUsersFullTextCatalog", true);
		}
	}
}
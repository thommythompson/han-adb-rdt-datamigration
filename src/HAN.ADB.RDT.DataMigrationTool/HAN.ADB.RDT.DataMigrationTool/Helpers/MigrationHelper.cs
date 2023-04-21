using HAN.ADB.RDT.DataMigrationTool.DataAccess.MsSql;
using Microsoft.EntityFrameworkCore;

namespace HAN.ADB.RDT.DataMigrationTool.Helpers
{
	public class MigrationHelper : IMigrationHelper
	{
		public readonly SqlContext _sqlContext;

		public MigrationHelper(SqlContext sqlContext)
		{
			_sqlContext = sqlContext;
		}

		public async Task MigratePosts()
		{
			var posts = await _sqlContext.Posts
				.Include(e => e.Children)
				.Take(10)
				.ToListAsync();

			foreach(var post in posts)
			{
				Console.WriteLine($"Post with id {post.Id} has {post.Children.Count()} children:");

				foreach(var child in post.Children)
				{
                    Console.WriteLine($"- Id {child.Id}");
                }
			}
		}
	}
}
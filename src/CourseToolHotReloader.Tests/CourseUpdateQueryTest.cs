using System.Linq;
using CourseToolHotReloader.DirectoryWorkers;
using CourseToolHotReloader.Dtos;
using CourseToolHotReloader.UpdateQuery;
using FluentAssertions;
using NUnit.Framework;

namespace CourseToolHotReloader.Tests
{
	public class CourseUpdateQueryTest
	{
		private CourseUpdateQuery courseUpdateQuery;

		[SetUp]
		public void Setup()
		{
			courseUpdateQuery = new CourseUpdateQuery();
		}

		[Test]
		public void CreateTest()
		{
			var a = CreateSimpleCourseUpdate("a");

			courseUpdateQuery.RegisterCreate(a);

			courseUpdateQuery.GetAllDeletedFiles().Should().BeEmpty();
			courseUpdateQuery.GetAllCourseUpdate().Count.Should().Be(1);
		}
		
		[Test]
		public void CreateDeleteCreateTest()
		{
			var a = CreateSimpleCourseUpdate("a");

			courseUpdateQuery.RegisterCreate(a);
			courseUpdateQuery.RegisterDelete(a);
			courseUpdateQuery.RegisterCreate(a);
			courseUpdateQuery.RegisterDelete(a);
			courseUpdateQuery.RegisterCreate(a);

			courseUpdateQuery.GetAllDeletedFiles().Should().BeEmpty();
			courseUpdateQuery.GetAllCourseUpdate().Count.Should().Be(1);
		}

		[Test]
		public void CreateModifyDeleteTest()
		{
			var a = CreateSimpleCourseUpdate("a");

			courseUpdateQuery.RegisterCreate(a);
			courseUpdateQuery.RegisterUpdate(a);
			courseUpdateQuery.RegisterDelete(a);

			courseUpdateQuery.GetAllDeletedFiles().Should().BeEmpty();
			courseUpdateQuery.GetAllCourseUpdate().Should().BeEmpty();
		}

		[Test]
		public void SimpleTest()
		{
			var a = CreateSimpleCourseUpdate("a");
			var b = CreateSimpleCourseUpdate("b");
			var c = CreateSimpleCourseUpdate("c");

			courseUpdateQuery.RegisterCreate(a);
			courseUpdateQuery.RegisterUpdate(a);
			courseUpdateQuery.RegisterUpdate(b);
			courseUpdateQuery.RegisterUpdate(c);
			courseUpdateQuery.RegisterDelete(c);

			courseUpdateQuery.GetAllDeletedFiles().Single().Name.Should().Be("c");
			courseUpdateQuery.GetAllCourseUpdate().Count.Should().Be(2);
		}

		private static ICourseUpdate CreateSimpleCourseUpdate(string name)
		{
			return new CourseUpdate(name, name, name);
		}
	}
}
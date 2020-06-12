﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Database.Repos;
using JetBrains.Annotations;
using Serilog;
using uLearn;
using Ulearn.Core;
using Ulearn.Core.Courses;

namespace Database
{
	public class WebCourseManager : CourseManager, IWebCourseManager
	{
		private readonly ILogger logger;

		private readonly Dictionary<string, Guid> loadedCourseVersions = new Dictionary<string, Guid>();
		private readonly ConcurrentDictionary<string, DateTime> courseVersionFetchTime = new ConcurrentDictionary<string, DateTime>();
		private DateTime lastCoursesListFetchTime = DateTime.MinValue;
		private readonly TimeSpan fetchCourseVersionEvery = TimeSpan.FromSeconds(10);

		public WebCourseManager(ILogger logger)
			: base(GetCoursesDirectory())
		{
			this.logger = logger;
		}
		public WebCourseManager(ILogger logger,DirectoryInfo baseDirectory)
			: base(baseDirectory)
		{
			this.logger = logger;
		}

		private readonly object @lock = new object();

		[NotNull]
		public async Task<Course> GetCourseAsync(CoursesRepo coursesRepo, TempCoursesRepo tempCoursesRepo, string courseId)
		{
			Course course;
			try
			{
				course = base.GetCourse(courseId);
			}
			catch (KeyNotFoundException)
			{
				course = null;
			}

			if (IsCourseVersionWasUpdatedRecent(courseId))
				return course ?? throw new CourseNotFoundException(courseId);

			courseVersionFetchTime[courseId] = DateTime.Now;

			var publishedVersion = await coursesRepo.GetPublishedCourseVersionAsync(courseId).ConfigureAwait(false);

			if (publishedVersion == null)
				return course ?? throw new CourseNotFoundException(courseId);

			ReloadCourseIfLoadedAndPublishedVersionsAreDifferent(courseId, publishedVersion);
			return base.GetCourse(courseId);
		}

		public async Task<IEnumerable<Course>> GetCoursesAsync(ICoursesRepo coursesRepo, ITempCoursesRepo tempCoursesRepo)
		{
			if (lastCoursesListFetchTime > DateTime.Now.Subtract(fetchCourseVersionEvery))
				return base.GetCourses();

			var publishedCourseVersions = await coursesRepo.GetPublishedCourseVersionsAsync().ConfigureAwait(false);

			lastCoursesListFetchTime = DateTime.Now;
			foreach (var courseVersion in publishedCourseVersions)
			{
				try
				{
					ReloadCourseIfLoadedAndPublishedVersionsAreDifferent(courseVersion.CourseId, courseVersion);
				}
				catch (FileNotFoundException)
				{
					/* Sometimes zip-archive with course has been deleted already. It's strange but ok */
					logger.Warning("Это странно, что я не смог загрузить с диска курс, который, если верить базе данных, был опубликован. Но ничего, просто проигнорирую");
				}
			}
			LoadTempCoursesIfNotYet(tempCoursesRepo);

			return base.GetCourses();
		}

		private void LoadTempCoursesIfNotYet(ITempCoursesRepo tempCoursesRepo)
		{
			var tempCourses = tempCoursesRepo.GetTempCourses();
			tempCourses
				.Where(tempCourse => !HasCourse(tempCourse.CourseId))
				.ToList()
				.ForEach(course=>ReloadCourse(course.CourseId));
		}

		private bool IsCourseVersionWasUpdatedRecent(string courseId)
		{
			if (courseVersionFetchTime.TryGetValue(courseId, out var lastFetchTime))
				return lastFetchTime > DateTime.Now.Subtract(fetchCourseVersionEvery);
			return false;
		}

		public void UpdateCourseVersion(string courseId, Guid versionId)
		{
			lock (@lock)
			{
				loadedCourseVersions[courseId.ToLower()] = versionId;
			}
		}

		public new void ReloadCourse(string courseId)
		{
			base.ReloadCourse(courseId);
		}

		public new Course ReloadCourseFromDirectory(DirectoryInfo directory)
		{
			return base.ReloadCourseFromDirectory(directory);
		}

		private void ReloadCourseIfLoadedAndPublishedVersionsAreDifferent(string courseId, CourseVersion publishedVersion)
		{
			lock (@lock)
			{
				var isCourseLoaded = loadedCourseVersions.TryGetValue(courseId.ToLower(), out var loadedVersionId);
				if ((isCourseLoaded && loadedVersionId != publishedVersion.Id) || !isCourseLoaded)
				{
					var actual = isCourseLoaded ? loadedVersionId.ToString() : "<none>";
					logger.Information($"Загруженная версия курса {courseId} отличается от актуальной ({actual} != {publishedVersion.Id}). Обновляю курс.");
					ReloadCourse(courseId);
				}

				loadedCourseVersions[courseId.ToLower()] = publishedVersion.Id;
			}
		}
	}
}
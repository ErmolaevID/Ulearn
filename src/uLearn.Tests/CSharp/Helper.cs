﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic.FileIO;
using RunCsJob;
using test;
using uLearn.Model.Blocks;

namespace uLearn.CSharp
{
	public static class Helper
	{
		public static readonly string[] WrongAnswerNames =
		{
			$"{nameof(AnotherTask)}.WrongAnswer.88.cs",
			$"{nameof(MeaningOfLifeTask)}.WrongAnswer.27.cs",
			$"{nameof(MeaningOfLifeTask)}.WrongAnswer.Type.cs",
			$"{nameof(MeaningOfLifeTask)}.WrongAnswer.21.plus.21.cs"
		};

		public static readonly string[] SolutionNames =
		{
			$"{nameof(AnotherTask)}.Solution.cs",
			$"{nameof(MeaningOfLifeTask)}.Solution.cs"
		};

		public static string OrderedWrongAnswersAndSolutionNames => ToString(WrongAnswersAndSolutionNames.OrderBy(n => n));

		public static IEnumerable<string> WrongAnswersAndSolutionNames => WrongAnswerNames.Concat(SolutionNames);

		public static string WrongAnswerNamesToString() => ToString(WrongAnswerNames);

		public static string SolutionNamesToString() => ToString(SolutionNames);

		public static string ToString(IEnumerable<string> arr) => string.Join(", ", arr);

		public static void RecreateDirectory(string path)
		{
			if (FileSystem.DirectoryExists(path))
				FileSystem.DeleteDirectory(path, DeleteDirectoryOption.DeleteAllContents);
			FileSystem.CreateDirectory(path);
		}

		public static CourseValidator BuildValidator(ExerciseSlide slide, StringBuilder valOut)
		{
			var v = new CourseValidator(new List<Slide> { slide }, new SandboxRunnerSettings());
			v.Warning += msg => { valOut.Append(msg); };
			v.Error += msg => { valOut.Append(msg); };
			return v;
		}

		public static ExerciseSlide BuildSlide(ProjectExerciseBlock ex)
		{
			var unit = new Unit(new UnitSettings { Title = "UnitTitle" }, null);
			var slideInfo = new SlideInfo(unit, null, 0);
			return new ExerciseSlide(new List<SlideBlock> { ex }, slideInfo, "SlideTitle", Guid.Empty);
		}
	}
}
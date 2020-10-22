﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace uLearn.Web.Views.Course
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using Database.Extensions;
    using Database.Models;
    using uLearn;
    using Ulearn.Core;
    using Ulearn.Core.Courses.Slides.Blocks;
    using uLearn.Web;
    using uLearn.Web.Models;
    using uLearn.Web.Views.Course;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    public class AcceptedSolutionsHtml : System.Web.WebPages.HelperPage
    {

#line default
#line hidden
public static System.Web.WebPages.HelperResult AcceptedSolutions(AcceptedSolutionsPageModel model)
{
#line default
#line hidden
return new System.Web.WebPages.HelperResult(__razor_helper_writer => {
 

WriteLiteralTo(__razor_helper_writer, "\t<div");

WriteLiteralTo(__razor_helper_writer, " id=\"LikeSolutionUrl\"");

WriteLiteralTo(__razor_helper_writer, " data-url=\"");

           WriteTo(__razor_helper_writer,  model.LikeSolutionUrl);

WriteLiteralTo(__razor_helper_writer, "\"");

WriteLiteralTo(__razor_helper_writer, "></div>\r\n");

WriteLiteralTo(__razor_helper_writer, "\t<p>");

WriteTo(__razor_helper_writer, MvcHtmlString.Create(model.Slide.Exercise.CommentAfterExerciseIsSolved.RenderMarkdown(model.Slide.Info.SlideFile)));

WriteLiteralTo(__razor_helper_writer, "</p>\r\n");

	foreach (var solution in model.AcceptedSolutions)
	{
		var id = "solution_" + solution.Id;
		var code = new CodeBlock(solution.Code, model.Slide.Exercise.Language);

WriteLiteralTo(__razor_helper_writer, "\t\t<div");

WriteAttributeTo(__razor_helper_writer, "id", Tuple.Create(" id=\"", 679), Tuple.Create("\"", 687)
, Tuple.Create(Tuple.Create("", 684), Tuple.Create<System.Object, System.Int32>(id
, 684), false)
);

WriteLiteralTo(__razor_helper_writer, ">\r\n\t\t\t<button");

WriteAttributeTo(__razor_helper_writer, "class", Tuple.Create(" class=\"", 701), Tuple.Create("\"", 801)
, Tuple.Create(Tuple.Create("", 709), Tuple.Create("like-left-location", 709), true)
, Tuple.Create(Tuple.Create(" ", 727), Tuple.Create("btn", 728), true)
, Tuple.Create(Tuple.Create(" ", 731), Tuple.Create<System.Object, System.Int32>( solution.LikedAlready ? "btn-primary" : "btn-default"
, 732), false)
, Tuple.Create(Tuple.Create(" ", 789), Tuple.Create("like-button", 790), true)
);

WriteAttributeTo(__razor_helper_writer, "onclick", Tuple.Create(" onclick=\"", 802), Tuple.Create("\"", 838)
, Tuple.Create(Tuple.Create("", 812), Tuple.Create("likeSolution(", 812), true)
                                                                , Tuple.Create(Tuple.Create("", 825), Tuple.Create<System.Object, System.Int32>(solution.Id
, 825), false)
, Tuple.Create(Tuple.Create("", 837), Tuple.Create(")", 837), true)
);

WriteLiteralTo(__razor_helper_writer, ">\r\n\t\t\t\t<i");

WriteLiteralTo(__razor_helper_writer, " class=\"glyphicon glyphicon-heart\"");

WriteLiteralTo(__razor_helper_writer, "></i>\r\n\t\t\t\t<span");

WriteLiteralTo(__razor_helper_writer, " class=\"likes-counter\"");

WriteLiteralTo(__razor_helper_writer, ">");

              WriteTo(__razor_helper_writer, solution.UsersWhoLike.Count);

WriteLiteralTo(__razor_helper_writer, "</span>\r\n\t\t\t</button>\r\n\r\n");

			
             if (model.User.HasAccessFor(model.CourseId, CourseRole.Instructor))
			{

WriteLiteralTo(__razor_helper_writer, "\t\t\t\t<form");

WriteAttributeTo(__razor_helper_writer, "action", Tuple.Create(" action=\"", 1062), Tuple.Create("\"", 1101)
, Tuple.Create(Tuple.Create("", 1071), Tuple.Create<System.Object, System.Int32>( solution.RemoveSolutionUrl
, 1071), false)
);

WriteLiteralTo(__razor_helper_writer, " method=\"POST\"");

WriteLiteralTo(__razor_helper_writer, " novalidate=\"novalidate\"");

WriteLiteralTo(__razor_helper_writer, ">\r\n\t\t\t\t\t<button");

WriteLiteralTo(__razor_helper_writer, " class=\"btn btn-danger\"");

WriteLiteralTo(__razor_helper_writer, ">\r\n\t\t\t\t\t\t<i");

WriteLiteralTo(__razor_helper_writer, " class=\"glyphicon glyphicon-remove\"");

WriteLiteralTo(__razor_helper_writer, "></i>\r\n\t\t\t\t\t\tУдалить решение\r\n\t\t\t\t\t</button>\r\n\t\t\t\t</form>\r\n");

			}

WriteLiteralTo(__razor_helper_writer, "\r\n");

WriteLiteralTo(__razor_helper_writer, "\t\t\t");

WriteTo(__razor_helper_writer, SlideHtml.Block(code, null, null));

WriteLiteralTo(__razor_helper_writer, "\r\n\t\t</div>\r\n");

	}

});

#line default
#line hidden
}
#line default
#line hidden

    }
}
#pragma warning restore 1591

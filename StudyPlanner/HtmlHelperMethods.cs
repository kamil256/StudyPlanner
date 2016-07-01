using StudyPlanner.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudyPlanner
{
    public static class HtmlHelperMethods
    {
        public static MvcHtmlString MarkSearchedString(this HtmlHelper helper, string text, string search)
        {
            string marked = text;
            if (!String.IsNullOrEmpty(search))
            {
                int start = marked.ToLower().IndexOf(search.ToLower()), end;
                while (start != -1)
                {
                    end = start + search.Length;
                    marked = marked.Insert(end, "</mark>");
                    marked = marked.Insert(start, "<mark>");
                    start = marked.ToLower().IndexOf(search.ToLower(), end + "<mark></mark".Length);
                }
            }
            return MvcHtmlString.Create(marked);
        }

        public static MvcHtmlString MarkedAuthors(this HtmlHelper helper, List<Author> authors, string search)
        {
            string result = "";
            for (int i = 0; i < authors.Count; i++)
            {
                TagBuilder a = new System.Web.Mvc.TagBuilder("a");
                a.Attributes["href"] = "#";
                a.InnerHtml = authors[i].Name;
                result += a.ToString();
                if (i > 0)
                    result += ", ";
            }
            return MvcHtmlString.Create(result);
        }

        public static MvcHtmlString Pagination(this HtmlHelper helper, int PageNumber, int TotalPages)
        {
            TagBuilder ul = new TagBuilder("ul");
            for (int i = 1; i <= TotalPages; i++)
            {
                TagBuilder innerDiv = new TagBuilder("div");
                innerDiv.Attributes["onclick"] = $"ChangePage({i})";
                if (i == PageNumber)
                    innerDiv.Attributes["data-selected"] = "selected";
                innerDiv.SetInnerText(i.ToString());
                TagBuilder li = new TagBuilder("li");
                li.InnerHtml = innerDiv.ToString();
                ul.InnerHtml += li.ToString();
            }
            TagBuilder outerDiv = new TagBuilder("div");
            outerDiv.AddCssClass("pagination");
            outerDiv.InnerHtml = ul.ToString();
            return new MvcHtmlString(outerDiv.ToString());
                //MvcHtmlString("<ul><li><div onclick='ChangePage(1)'>1</div></li><li><div onclick='ChangePage(2)'>2</div></li><li><div onclick='ChangePage(3)'>3</div></li></ul>");
        }
    }
}
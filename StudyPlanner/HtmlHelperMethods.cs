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

        public static MvcHtmlString Pagination(this HtmlHelper helper, int currentPage, int totalItems, int itemsPerPage)
        {
            return new MvcHtmlString("<div class='pagination'><ul><li><a href='#'>1</a></li><li><a href='#' style='text-decoration: none'>2</a></li><li><a href='#'>3</a></li></ul></div>");
        }
    }
}
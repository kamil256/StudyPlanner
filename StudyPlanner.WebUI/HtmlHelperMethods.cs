using StudyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudyPlanner.WebUI
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

        public static MvcHtmlString Pagination(this HtmlHelper helper, int PageNumber, int TotalPages, Func<int, string> onClickFunction)
        {
            int left = PageNumber - 4;
            while (left < 1) left++;
            int right = PageNumber + 4;
            while (right > TotalPages) right--;
            TagBuilder ul = new TagBuilder("ul");

            if (left > 1)
                AddInnerDiv(ul, PageNumber, 1, onClickFunction);
            if (left > 2)
            {
                TagBuilder innerDiv = new TagBuilder("div");
                innerDiv.AddCssClass("disabled");
                innerDiv.SetInnerText("...");
                TagBuilder li = new TagBuilder("li");
                li.InnerHtml = innerDiv.ToString();
                ul.InnerHtml += li.ToString();
            }
            for (int i = left; i <= right; i++)
            {
                AddInnerDiv(ul, PageNumber, i, onClickFunction);
            }
            if (right < TotalPages - 1)
            {
                TagBuilder innerDiv = new TagBuilder("div");
                innerDiv.AddCssClass("disabled");
                innerDiv.SetInnerText("...");
                TagBuilder li = new TagBuilder("li");
                li.InnerHtml = innerDiv.ToString();
                ul.InnerHtml += li.ToString();
            }
            if (right < TotalPages)
                AddInnerDiv(ul, PageNumber, TotalPages, onClickFunction);

            TagBuilder outerDiv = new TagBuilder("div");
            outerDiv.AddCssClass("pagination");
            if (TotalPages < 2)
                outerDiv.AddCssClass("hide");
            outerDiv.InnerHtml = ul.ToString();

            return new MvcHtmlString(outerDiv.ToString());
        }

        private static void AddInnerDiv(TagBuilder ul, int PageNumber, int number, Func<int, string> onClickFunction)
        {
            // remove div and add line-height to li
            TagBuilder innerDiv = new TagBuilder("div");
            innerDiv.Attributes["onclick"] = onClickFunction(number);
            if (number == PageNumber)
                innerDiv.Attributes["data-selected"] = "selected";
            innerDiv.SetInnerText(number.ToString());
            TagBuilder li = new TagBuilder("li");
            li.InnerHtml = innerDiv.ToString();
            ul.InnerHtml += li.ToString();
        }
    }
}
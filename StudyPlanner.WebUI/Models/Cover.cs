using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace StudyPlanner.WebUI.Models
{
    public static class Cover
    {
        public static void Set(HttpPostedFileBase coverParam)
        {
            MyHttpPostedFileBase cover = new MyHttpPostedFileBase(coverParam);
            HttpContext.Current.Session["Cover"] = cover;
        }

        public static void Clear()
        {
            HttpContext.Current.Session["Cover"] = null;
        }

        public static bool IsSet()
        {
            return HttpContext.Current.Session["Cover"] != null;
        }

        public static MyHttpPostedFileBase GetHttpPostedFileBase()
        {
            return (MyHttpPostedFileBase)HttpContext.Current.Session["Cover"];
        }

        public static byte[] GetFile()
        {
            return GetHttpPostedFileBase().FileContents;
        }

        public static string GetContentType()
        {
            return GetHttpPostedFileBase().ContentType;
        }
    }

    public class MyHttpPostedFileBase : HttpPostedFileBase
    {
        public MyHttpPostedFileBase(byte[] fileContents, string contentType)
        {
            FileContents = fileContents;
            ContentType = contentType;
            FileName = "";
        }

        public MyHttpPostedFileBase(HttpPostedFileBase cover)
        {
            FileContents = new byte[cover.ContentLength];
            cover.InputStream.Read(FileContents, 0, FileContents.Length);
            ContentType = cover.ContentType;
            FileName = cover.FileName;
        }

        public byte[] FileContents { get; }
        public override int ContentLength { get { return FileContents.Length; } }
        public override string ContentType { get; }
        public override string FileName { get; }
        public override Stream InputStream { get { return new MemoryStream(FileContents); } }
    }
}
using CodeninModel.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace CodedenimWebApp.Service
{
    public class UploadedFileProcessor : IFilePathProcessor
    {
        public FilePath ProcessFilePath(HttpPostedFileBase file)
        {
            string _FileName = String.Empty;

            var fp = new FilePath();
            if (file.ContentLength > 0)
            {
                _FileName = Path.GetFileName(file.FileName);
                string path = HostingEnvironment.MapPath("~/MaterialUpload/") + _FileName;
                //string path = "~/MaterialUpload/" + _FileName;
                fp.FileLocation = path;
                var directory = new DirectoryInfo(HostingEnvironment.MapPath("~/MaterialUpload/"));
                if (directory.Exists == false)
                {
                    directory.Create();
                }
                file.SaveAs(path);
                fp.Path = _FileName;
            }
            return fp;


        }
    }
}
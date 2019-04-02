using CodeninModel.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace CodedenimWebApp.Service
{
    public class DeleteFile : IDeleteFile
    {
        public bool Delete(string fileName)
        {
           string path = HostingEnvironment.MapPath("~/MaterialUpload/") + fileName;
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
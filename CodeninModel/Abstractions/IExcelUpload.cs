﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CodeninModel.Abstractions
{
    public interface IExcelUpload
    {
        string ExcelUpload(HttpPostedFileBase upload);
    }
}

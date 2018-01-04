using System.Web;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.SessionState;
using System.IO;
using System;

using System.Linq;

using System.Text.RegularExpressions;
using System.Configuration;


namespace ykx
{
    /// <summary>
    /// Detail 的摘要说明
    /// </summary>
    public class Detail : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            //类别
            DataTable Category = SqlHelper.ExecuteDataTable("select *  from Category where IS_DELETE = 0");
            string Product_ID = context.Request["Product_ID"];
            DataTable Product = SqlHelper.ExecuteDataTable("select *from Product where Product_ID = "+ Product_ID);
            var Data = new { Category = Category.Rows, Product = Product.Rows};
            string html = CommonHelper.RenderHtml("productdetail.html", Data);
            context.Response.Write(html);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
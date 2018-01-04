using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace ykx
{
    /// <summary>
    /// surfaceFAQs 的摘要说明
    /// </summary>
    public class surfaceFAQs : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            DataTable Category = SqlHelper.ExecuteDataTable("select *  from Category where IS_DELETE = 0");
            
            string Action = context.Request["Action"];
            if (Action == "jieyong")
            {
                var Data = new { Category = Category.Rows };
                string html = CommonHelper.RenderHtml("contact.html", Data);
                context.Response.Write(html);
            }
            if (Action != null)
            {
                DataTable FAQs = SqlHelper.ExecuteDataTable("select * from V_FAQs where IS_DELETE = 0");
                //DataTable Category = SqlHelper.ExecuteDataTable("select *  from Category where IS_DELETE = 0");
                var Data = new { FAQs = FAQs.Rows, Category = Category.Rows };
                string html = CommonHelper.RenderHtml("faqs.html", Data);
                context.Response.Write(html);

            }
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
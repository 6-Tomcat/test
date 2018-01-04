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
    public class about : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            DataTable Category = SqlHelper.ExecuteDataTable("select *  from Category where IS_DELETE = 0");
           
            string Action = context.Request["Action"];
            if (Action != null)
            {
                DataTable Article = SqlHelper.ExecuteDataTable("select * from Article where IS_DELETE = 0");
                //DataTable Category = SqlHelper.ExecuteDataTable("select *  from Category where IS_DELETE = 0");
                var Data = new { Article = Article.Rows, Category = Category.Rows };
                string html = CommonHelper.RenderHtml("about.html", Data);
                context.Response.Write(html);
                /*
                string Shops_ID = context.Request["Shops_ID"];
                string Quantity = context.Request["Quantity"];
                string Total_Price = context.Request["Total_Price"];
                SqlHelper.ExecuteNonQuery("update Shops set Quantity=@Quantity,Total_Price=@Total_Price where Shops_ID=@Shops_ID",
                   new SqlParameter("@Quantity", Quantity),
                   new SqlParameter("@Shops_ID", Shops_ID),
                   new SqlParameter("@Total_Price", Total_Price)
                   );
                context.Response.Write("");
                 */
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
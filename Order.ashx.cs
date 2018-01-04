using System.Web;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.SessionState;

namespace ykx
{
    /// <summary>
    /// Order 的摘要说明
    /// </summary>
    public class Order : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            if (context.Session["User"] == null || context.Session["User"].ToString() == "")
            {
                context.Response.Redirect("/html/login.html");
            }
            string User_ID = context.Session["Id"].ToString();
            string Action = context.Request["Action"];
            DataTable Category = SqlHelper.ExecuteDataTable("select *  from Category where IS_DELETE = 0");
            if (Action  == "Search")
            {
                DataTable Orders = SqlHelper.ExecuteDataTable("select * from Orders where User_ID =" + User_ID + "and IS_DELETE =0");
                DataTable OrderD = SqlHelper.ExecuteDataTable("select * from V_O where User_ID =" + User_ID + "and IS_DELETE =0");
                var Data = new { Orders = Orders.Rows, OrderD = OrderD.Rows , Category = Category.Rows};
                string html = CommonHelper.RenderHtml("order.html", Data);
                context.Response.Write(html);
            }
            if (Action == "Detail")
            {
                string Order_ID = context.Request["Order_ID"];
                DataTable Orders = SqlHelper.ExecuteDataTable("select * from Orders where User_ID =" + User_ID + "and IS_DELETE =0"+ "and Order_ID="+ Order_ID);
                DataTable OrderD = SqlHelper.ExecuteDataTable("select * from V_O where User_ID =" + User_ID + "and IS_DELETE =0" + "and Order_ID=" + Order_ID);
                var Data = new { Orders = Orders.Rows, OrderD = OrderD.Rows, Category = Category.Rows };
                string html = CommonHelper.RenderHtml("orderDetail.html", Data);
                context.Response.Write(html);
            }
            if(Action == "Delete")
            {
                string Order_ID = context.Request["Order_ID"];
                SqlHelper.ExecuteNonQuery("update Orders set IS_DELETE=@IS_DELETE where Order_ID=@Order_ID",
                  new SqlParameter("@IS_DELETE", 1),
                  new SqlParameter("@Order_ID", Order_ID)
                  );
                context.Response.Redirect("http://localhost:54436/order.ashx?Action=Search");
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
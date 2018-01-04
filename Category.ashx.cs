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
    /// Student 的摘要说明
    /// </summary>
    public class Category : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            if (context.Session["Admin"] == null || context.Session["Admin"].ToString() == "")
            {
                context.Response.Redirect("/html/adminLogin.html");
            }
            string action = context.Request["Action"];
            string adminName = context.Session["Admin"].ToString();
            string adminId = "";
            DataTable dts =
            SqlHelper.ExecuteDataTable("select *  from Admin where Account = " + adminName);
            foreach (DataRow dr in dts.Rows)
            {
                adminId = dr["Admin_ID"].ToString();
            }
            //查询学生
            if (action == "Search")
            {
                string requestNum = context.Request["PageNumber"];
                string html = FenYe.FY("Category.ashx", "Category.html", "Category", requestNum, "Category_ID", adminId);
                context.Response.Write(html);
            }
            else if (action == "add")
            {
                string html = CommonHelper.RenderHtml("addCategory.html","");
                context.Response.Write(html);
            }
            else if (action == "Add")
            {
                
                string name = context.Request["Name"];
                int is_delete = 0;
                SqlHelper.ExecuteNonQuery("Insert into Category(Admin_ID,C_Name,IS_DELETE) values(@Admin_ID,@C_Name,@IS_DELETE)",
                         new SqlParameter("@Admin_ID", adminId)
                        , new SqlParameter("@C_Name", name)
                        , new SqlParameter("@IS_DELETE", is_delete)
                        );
                context.Response.Redirect("http://localhost:54436/Category.ashx?Action=Search");
            }
            else if (action == "Edit")
            {
                string id = context.Request["Id"];
                DataTable dt =
                SqlHelper.ExecuteDataTable("select *  from Category where Category_ID = " + id);
                string html = CommonHelper.RenderHtml("editCategory.html", dt.Rows);
                context.Response.Write(html);
            }
            else if (action == "edit")
            {
                string id = context.Request["Category_ID"];
                string name = context.Request["Name"];
                int is_delete = 0;
                SqlHelper.ExecuteNonQuery("update Category set C_Name=@C_Name,IS_DELETE=@IS_DELETE where Category_ID=@Category_ID"
                        , new SqlParameter("@C_Name", name)
                        , new SqlParameter("@IS_DELETE", is_delete)
                        , new SqlParameter("@Category_ID", id)
                        );
                context.Response.Redirect("http://localhost:54436/category.ashx?Action=Search");
            }
            else if (action == "Delete")
            {
                string id = context.Request["Id"];
                int is_delete = 1;
                SqlHelper.ExecuteNonQuery("update Category set IS_DELETE=@IS_DELETE where Category_ID=@Category_ID",
                         new SqlParameter("@IS_DELETE", is_delete)
                        , new SqlParameter("@Category_ID", id)
                        );
                context.Response.Redirect("http://localhost:54436/category.ashx?Action=Search");
            }
          
            else
            {
                context.Response.Redirect("http://localhost:54436/category.ashx?Action=Search");
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
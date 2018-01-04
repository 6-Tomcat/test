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
    /// BanJi 的摘要说明
    /// </summary>
    public class BanJi : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            if (context.Session["User"] == null || context.Session["User"].ToString() == "")
            {
                context.Response.Redirect("/html/login.html");
            }
            string action = context.Request["Action"];
            //查询学生
            if (action == "Search")
            {
                string requestNum = context.Request["PageNumber"];
                string html = FenYe.FY("BanJi.ashx", "bJ.html", "V_BJ", requestNum, "B_ID","");
                context.Response.Write(html);
            }
            else if (action == "add")
            {
                DataTable dt =
                SqlHelper.ExecuteDataTable("select *  from Teacher where IS_DELETE = 0");
                string html = CommonHelper.RenderHtml("addBJ.html", dt.Rows);
                context.Response.Write(html);
            }
            else if (action == "Add")
            {
                string name = context.Request["Name"];
                string t_id = context.Request["Teacher"];
                string num = context.Request["Num"];
                int is_delete = 0;
                SqlHelper.ExecuteNonQuery("Insert into BanJi(T_ID,B_NAME,B_NUM,IS_DELETE) values(@T_ID,@B_NAME,@B_NUM,@IS_DELETE)",
                        new SqlParameter("@T_ID", t_id)
                        , new SqlParameter("@B_NAME", name)
                        , new SqlParameter("@B_NUM", num)
                        , new SqlParameter("@IS_DELETE", is_delete)
                        );
                context.Response.Redirect("http://localhost:54436/banji.ashx?Action=Search");
            }
            else if (action == "Ajax_Add")
            {
                string json = context.Request["Json"];
                JObject obj = (JObject)JsonConvert.DeserializeObject(json);
                string name = obj["Name"].ToString();
                string t_id = obj["Teacher"].ToString();
                string num = obj["Num"].ToString();
                int is_delete = 0;
                SqlHelper.ExecuteNonQuery("Insert into BanJi(T_ID,B_NAME,B_NUM,IS_DELETE) values(@T_ID,@B_NAME,@B_NUM,@IS_DELETE)",
                         new SqlParameter("@T_ID", t_id)
                         , new SqlParameter("@B_NAME", name)
                         , new SqlParameter("@B_NUM", num)
                         , new SqlParameter("@IS_DELETE", is_delete)
                         );
                context.Response.Write("");
            }
            else if (action == "Edit")
            {
                string id = context.Request["Id"];
                DataTable dt =
                SqlHelper.ExecuteDataTable("select *  from V_BJ where B_ID = " + id);
                DataTable dt2 =
                SqlHelper.ExecuteDataTable("select *  from Teacher where IS_DELET = 0");
                string html = CommonHelper2.RenderHtml("editBJ.html", dt.Rows, dt2.Rows);
                context.Response.Write(html);
            }
            else if (action == "edit")
            {
                string id = context.Request["Id"];
                string name = context.Request["Name"];
                string t_id = context.Request["Teacher"];
                string num = context.Request["Num"];
                int is_delete = 0;
                SqlHelper.ExecuteNonQuery("update BanJi set T_ID=@T_ID,B_NAME=@B_NAME,B_NUM=@B_NUM,IS_DELETE=@IS_DELETE where B_ID=@B_ID",
                       new SqlParameter("@T_ID", t_id)
                        , new SqlParameter("@B_NAME", name)
                        , new SqlParameter("@B_NUM", num)
                        , new SqlParameter("@IS_DELETE", is_delete)
                         , new SqlParameter("@B_ID", id)
                        );
                context.Response.Redirect("http://localhost:54436/banji.ashx?Action=Search");
            }
            else if (action == "Delete")
            {
                string id = context.Request["Id"];
                int is_delete = 1;
                SqlHelper.ExecuteNonQuery("update BanJi set IS_DELETE=@IS_DELETE where B_ID=@B_ID",
                         new SqlParameter("@IS_DELETE", is_delete)
                        , new SqlParameter("@B_ID", id)
                        );
                context.Response.Redirect("http://localhost:54436/banji.ashx?Action=Search");
            }
            else if (action == "Ajax_Delete")
            {
                string id = context.Request["Id"];
                int is_delete = 1;
                SqlHelper.ExecuteNonQuery("update BanJi set IS_DELETE=@IS_DELETE where B_ID=@B_ID",
                          new SqlParameter("@IS_DELETE", is_delete)
                         , new SqlParameter("@B_ID", id)
                         );
                context.Response.Write("");
            }
            else
            {
                context.Response.Redirect("http://localhost:54436/banji.ashx?Action=Search");
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
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
    /// Teacher 的摘要说明
    /// </summary>
    public class Teacher : IHttpHandler, IRequiresSessionState
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
                string html = FenYe.FY("Teacher.ashx", "teacher.html", "Teacher", requestNum, "T_ID","");
                context.Response.Write(html);
            }
            else if (action == "Add")
            {
                string name = context.Request["Name"];
                string phone = context.Request["Phone"];
                string bir = context.Request["Bir"];
                string email = context.Request["Email"];
                int is_delete = 0;
                SqlHelper.ExecuteNonQuery("Insert into Teacher(T_NAME,T_PHONE,T_EMAIL,T_BR,IS_DELETE) values(@T_NAME,@T_PHONE,@T_EMAIL,@T_BR,@IS_DELETE)",
                        new SqlParameter("@T_NAME", name)
                        , new SqlParameter("@T_PHONE", phone)
                        , new SqlParameter("@T_EMAIL", email)
                        , new SqlParameter("@T_BR", bir)
                        , new SqlParameter("@IS_DELETE", is_delete)
                        );
                context.Response.Redirect("http://localhost:54436/teacher.ashx?Action=Search");
            }
            else if (action == "Ajax_Add")
            {
                string json = context.Request["Json"];
                JObject obj = (JObject)JsonConvert.DeserializeObject(json);
                string name = obj["Name"].ToString();
                string bir = obj["Bir"].ToString();
                string phone = obj["Phone"].ToString();
                string email = obj["Email"].ToString();
                int is_delete = 0;
                SqlHelper.ExecuteNonQuery("Insert into Teacher(T_NAME,T_PHONE,T_EMAIL,T_BR,IS_DELETE) values(@T_NAME,@T_PHONE,@T_EMAIL,@T_BR,@IS_DELETE)",
                         new SqlParameter("@T_NAME", name)
                         , new SqlParameter("@T_PHONE", phone)
                         , new SqlParameter("@T_EMAIL", email)
                         , new SqlParameter("@T_BR", bir)
                         , new SqlParameter("@IS_DELETE", is_delete)
                         );
                context.Response.Write("");
            }
            else if (action == "Edit")
            {
                string id = context.Request["Id"];
                DataTable dt =
                SqlHelper.ExecuteDataTable("select *  from Teacher where T_ID = " + id);
                string html = CommonHelper.RenderHtml("editTeacher.html", dt.Rows);
                context.Response.Write(html);
            }
            else if (action == "edit")
            {
                string id = context.Request["Id"];
                string name = context.Request["Name"];
                string phone = context.Request["Phone"];
                string bir = context.Request["Bir"];
                string email = context.Request["Email"];
                int is_delete = 0;
                SqlHelper.ExecuteNonQuery("update Teacher set T_NAME=@T_NAME,T_PHONE=@T_PHONE,T_EMAIL=@T_EMAIL,T_BR=@T_BR,IS_DELETE=@IS_DELETE where T_ID=@T_ID",
                         new SqlParameter("@T_NAME", name)
                        , new SqlParameter("@T_PHONE", phone)
                        , new SqlParameter("@T_EMAIL", email)
                        , new SqlParameter("@T_BR", bir)
                        , new SqlParameter("@IS_DELETE", is_delete)
                        , new SqlParameter("@T_ID", id)
                        );
                context.Response.Redirect("http://localhost:54436/teacher.ashx?Action=Search");
            }
            else if (action == "Delete")
            {
                string id = context.Request["Id"];
                int is_delete = 1;
                SqlHelper.ExecuteNonQuery("update Teacher set IS_DELETE=@IS_DELETE where T_ID=@T_ID",
                         new SqlParameter("@IS_DELETE", is_delete)
                        , new SqlParameter("@T_ID", id)
                        );
                context.Response.Redirect("http://localhost:54436/teacher.ashx?Action=Search");
            }
            else if (action == "Ajax_Delete")
            {
                string id = context.Request["Id"];
                int is_delete = 1;
                SqlHelper.ExecuteNonQuery("update Teacher set IS_DELETE=@IS_DELETE where T_ID=@T_ID",
                         new SqlParameter("@IS_DELETE", is_delete)
                        , new SqlParameter("@T_ID", id)
                        );
                context.Response.Write("");
            }
            else
            {
                context.Response.Redirect("http://localhost:54436/teacher.ashx?Action=Search");
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
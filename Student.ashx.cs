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
    public class Student : IHttpHandler, IRequiresSessionState
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
                string html = FenYe.FY("Student.ashx","student.html","V_S",requestNum,"S_ID","");
                context.Response.Write(html);
            }else if (action == "add")
            {
                DataTable dt =
                SqlHelper.ExecuteDataTable("select *  from BanJi where IS_DELETE = 0");
                string html = CommonHelper.RenderHtml("addStudent.html", dt.Rows);
                context.Response.Write(html);
            }
            else if (action == "Add")
            {
                string name = context.Request["Name"];
                string sex = context.Request["Sex"];
                string bir = context.Request["Bir"];
                string high = context.Request["High"];
                string tc = context.Request["TC"];
                string bj = context.Request["BJ"];
                int is_delete = 0;
                SqlHelper.ExecuteNonQuery("Insert into Student(S_NAME,B_ID,S_SEX,S_HIGH,IS_TC,IS_DELETE,S_BIR) values(@S_NAME,@B_ID,@S_SEX,@S_HIGH,@IS_TC,@IS_DELETE,@S_BIR)",
                        new SqlParameter("@S_NAME", name)
                        , new SqlParameter("@B_ID", bj)
                        , new SqlParameter("@S_SEX", sex)
                        , new SqlParameter("@S_HIGH", high)
                        , new SqlParameter("@IS_TC", tc)
                        , new SqlParameter("@IS_DELETE", is_delete)
                        , new SqlParameter("@S_BIR", bir)
                        );
                context.Response.Redirect("http://localhost:54436/student.ashx?Action=Search");
            }
            else if (action == "Ajax_Add")
            {
                string json = context.Request["Json"];
                JObject obj = (JObject)JsonConvert.DeserializeObject(json);
                 string name = obj["Name"].ToString();  
                 string sex = obj["Sex"].ToString();
                 string bir = obj["Bir"].ToString();
                 string high = obj["High"].ToString();
                 string tc = obj["TC"].ToString();
                 string bj = obj["BJ"].ToString();
                 int is_delete = 0;
                  SqlHelper.ExecuteNonQuery("Insert into Student(S_NAME,B_ID,S_SEX,S_HIGH,IS_TC,IS_DELETE,S_BIR) values(@S_NAME,@B_ID,@S_SEX,@S_HIGH,@IS_TC,@IS_DELETE,@S_BIR)",
                            new SqlParameter("@S_NAME", name)
                            , new SqlParameter("@B_ID", bj)
                            , new SqlParameter("@S_SEX", sex)
                            , new SqlParameter("@S_HIGH", high)
                            , new SqlParameter("@IS_TC", tc)
                            , new SqlParameter("@IS_DELETE", is_delete)
                            , new SqlParameter("@S_BIR", bir)
                            );
                context.Response.Write("");
            } else if (action == "Edit")
            {
                string id = context.Request["Id"];
                DataTable dt =
                SqlHelper.ExecuteDataTable("select *  from V_S where S_ID = "+ id);
                DataTable dt2 =
                SqlHelper.ExecuteDataTable("select *  from BanJi where IS_DELETE = 0");
                string html = CommonHelper2.RenderHtml("editStudent.html", dt.Rows, dt2.Rows);
                context.Response.Write(html);
            }else if (action == "edit")
            {
                string id = context.Request["Id"];
                string name = context.Request["Name"];
                string sex = context.Request["Sex"];
                string bir = context.Request["Bir"];
                string high = context.Request["High"];
                string tc = context.Request["TC"];
                string bj = context.Request["BJ"];
                int is_delete = 0;
                SqlHelper.ExecuteNonQuery("update Student set B_ID=@B_ID,S_NAME=@S_NAME,S_SEX=@S_SEX,S_HIGH=@S_HIGH,IS_TC=@IS_TC,IS_DELETE=@IS_DELETE,S_BIR=@S_BIR where S_ID=@S_ID",
                         new SqlParameter("@B_ID", bj)
                        , new SqlParameter("@S_NAME", name)
                        , new SqlParameter("@S_SEX", sex)
                        , new SqlParameter("@S_HIGH", high)
                        , new SqlParameter("@IS_TC", tc)
                        , new SqlParameter("@IS_DELETE", is_delete)
                        , new SqlParameter("@S_BIR", bir)
                        , new SqlParameter("@S_ID", id)
                        );
                context.Response.Redirect("http://localhost:54436/student.ashx?Action=Search");
            }else if(action == "Delete")
            {
                string id = context.Request["Id"];
                int is_delete = 1;
                SqlHelper.ExecuteNonQuery("update Student set IS_DELETE=@IS_DELETE where S_ID=@S_ID",                       
                         new SqlParameter("@IS_DELETE", is_delete)
                        , new SqlParameter("@S_ID", id)
                        );
                context.Response.Redirect("http://localhost:54436/student.ashx?Action=Search");
            }
            else if (action == "Ajax_Delete")
            {
                string id = context.Request["Id"];
                int is_delete = 1;
                SqlHelper.ExecuteNonQuery("update Student set IS_DELETE=@IS_DELETE where S_ID=@S_ID",
                         new SqlParameter("@IS_DELETE", is_delete)
                        , new SqlParameter("@S_ID", id)
                        );
                context.Response.Write("");
            }
            else
            {
                context.Response.Redirect("http://localhost:54436/student.ashx?Action=Search");
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
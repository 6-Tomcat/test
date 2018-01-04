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
    public class backAbout : IHttpHandler, IRequiresSessionState
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
                string html = FenYe.FY("backAbout.ashx", "searchAbout.html", "V_About", requestNum, "Article_ID", adminId);
                context.Response.Write(html);
            }
            else if (action == "add")
            {
                string html = CommonHelper.RenderHtml("addAbout.html", "");
                context.Response.Write(html);
            }
            else if (action == "Add")
            {
                
                string aid = context.Request["Admin_ID"];
                string title = context.Request["Title"];
                string content = context.Request["Content"];
                string judgeTime = context.Request["Time"];

                string time = null; 
                string is_delete = context.Request["IS_DELETE"];
                
                int judgeTimee = 1;
                judgeTimee = int.Parse(judgeTime);
                
                if (judgeTimee==1)
                {
                    System.DateTime currentTime = new System.DateTime();
                    currentTime = System.DateTime.Now;
                    time = currentTime.ToString("f"); 
                }
                SqlHelper.ExecuteNonQuery("Insert into Article(Admin_ID,Title,Content,Time,IS_DELETE) values(@Admin_ID,@Title,@Content,@Time,@IS_DELETE)",
                          new SqlParameter("@Admin_ID", aid)
                         , new SqlParameter("@Title", title)
                         , new SqlParameter("@Content", content)
                         , new SqlParameter("@Time", time)
                         , new SqlParameter("@IS_DELETE", is_delete)

                        );
                context.Response.Redirect("http://localhost:54436/backAbout.ashx?Action=Search");
            }
            else if (action == "Adda")
            {
                /*
                 id="User_ID" name="User_ID"
                 id="Admin_ID" name="Admin_ID
                 id="C_Name" name="C_Name"
                 id="C_Email" name="C_Email"
                 id="C_Subject" name="C_Subject"
                 id="Message" name="Message" 
                 */
                string uid = "1";//context.Request["User_ID"];
                string aid = adminId;//context.Request["Admin_ID"];
                string cname = context.Request["author"];
                string cemail = context.Request["email"];
                string csubject = context.Request["subject"];
                string message = context.Request["text"];

                int is_delete = 0;
                SqlHelper.ExecuteNonQuery("Insert into Contact(User_ID,Admin_ID,C_Name,C_Email,C_Subject,Message,IS_DELETE) values(@User_ID,@Admin_ID,@C_Name,@C_Email,@C_Subject,@Message,@IS_DELETE)",
                         new SqlParameter("@User_ID", uid)
                         , new SqlParameter("@Admin_ID", aid)
                         , new SqlParameter("@C_Name", cname)
                         , new SqlParameter("@C_Email", cemail)
                         , new SqlParameter("@C_Subject", csubject)
                         , new SqlParameter("@Message", message)
                        , new SqlParameter("@IS_DELETE", is_delete)
                        );
                context.Response.Redirect("http://localhost:54436/contact.html");
                //context.Response.Redirect("http://localhost:54436/Contact.ashx?Action=Search");
            }
            else if (action == "Edit")
            {
                string id = context.Request["Id"];
                DataTable dt =
                SqlHelper.ExecuteDataTable("select *  from Article where Article_ID = " + id);
                string html = CommonHelper.RenderHtml("editAbout.html", dt.Rows);
                context.Response.Write(html);
            }
            else if (action == "edit")
            {
                string id = context.Request["Article_ID"];
                string title = context.Request["Title"];
                string content = context.Request["Content"];
                string time = context.Request["Time"];
                string aid = adminId;
                int is_delete = 0;
                SqlHelper.ExecuteNonQuery("update Article set Admin_ID=@Admin_ID,Title=@Title,Content=@Content,Time=@Time,IS_DELETE=@IS_DELETE where Article_ID=@Article_ID"
                       , new SqlParameter("@Admin_ID", aid)
                       , new SqlParameter("@Title", title)
                       , new SqlParameter("@Content", content)
                       , new SqlParameter("@Time",time)
                       , new SqlParameter("@IS_DELETE", is_delete)
                       , new SqlParameter("@Article_ID", id)
                       );
                context.Response.Redirect("http://localhost:54436/backAbout.ashx?Action=Search");
            }
            else if (action == "Delete")
            {
                string id = context.Request["Id"];
                int is_delete = 1;
                SqlHelper.ExecuteNonQuery("update Article set IS_DELETE=@IS_DELETE where Article_ID=@Article_ID",
                         new SqlParameter("@IS_DELETE", is_delete)
                        , new SqlParameter("@Article_ID", id)
                        );
                context.Response.Redirect("http://localhost:54436/backAbout.ashx?Action=Search");
            }

            else
            {
                context.Response.Redirect("http://localhost:54436/backAbout.ashx?Action=Search");
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
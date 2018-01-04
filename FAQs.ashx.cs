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
    public class FAQs : IHttpHandler, IRequiresSessionState
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
                string html = FenYe.FY("FAQs.ashx", "searchFAQs.html", "V_F", requestNum, "FAQ_ID", adminId);
                context.Response.Write(html);
            }
            else if (action == "add")
            {
                string html = CommonHelper.RenderHtml("addFAQs.html", "");
                context.Response.Write(html);
            }
            else if (action == "Add")
            {
                
                string aid = context.Request["Admin_ID"];
                string title = context.Request["Title"];
                string content = context.Request["Content"];
                string is_delete = context.Request["IS_DELETE"];
                SqlHelper.ExecuteNonQuery("Insert into FAQs(Admin_ID,Title,Content,IS_DELETE) values(@Admin_ID,@Title,@Content,@IS_DELETE)",
                          new SqlParameter("@Admin_ID", aid)
                         , new SqlParameter("@Title", title)
                         , new SqlParameter("@Content", content)
                         , new SqlParameter("@IS_DELETE", is_delete)
                         
                        );
                context.Response.Redirect("http://localhost:54436/FAQs.ashx?Action=Search");
            }
            else if (action == "Adda")
            {
                string uid = "1";
                string aid = adminId;
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
                SqlHelper.ExecuteDataTable("select *  from FAQs where FAQ_ID = " + id);
                string html = CommonHelper.RenderHtml("editFAQs.html", dt.Rows);
                context.Response.Write(html);
            }
            else if (action == "edit")
            {
                string id = context.Request["FAQ_ID"];
                string title = context.Request["Title"];
                string content = context.Request["Content"];
                int is_delete = 0;
                string aid = adminId;
                SqlHelper.ExecuteNonQuery("update FAQs set Admin_ID=@Admin_ID,Title=@Title,Content=@Content,IS_DELETE=@IS_DELETE where FAQ_ID=@FAQ_ID"
                        , new SqlParameter("@Admin_ID", aid)
                        , new SqlParameter("@Title", title)
                        , new SqlParameter("@Content", content)
                        , new SqlParameter("@IS_DELETE", is_delete)
                        , new SqlParameter("@FAQ_ID", id)
                        );
                context.Response.Redirect("http://localhost:54436/FAQs.ashx?Action=Search");
            }
            else if (action == "Delete")
            {
                string id = context.Request["Id"];
                int is_delete = 1;
                SqlHelper.ExecuteNonQuery("update FAQs set IS_DELETE=@IS_DELETE where FAQ_ID=@FAQ_ID",
                         new SqlParameter("@IS_DELETE", is_delete)
                        , new SqlParameter("@FAQ_ID", id)
                        );
                context.Response.Redirect("http://localhost:54436/FAQs.ashx?Action=Search");
            }

            else
            {
                context.Response.Redirect("http://localhost:54436/FAQs.ashx?Action=Search");
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
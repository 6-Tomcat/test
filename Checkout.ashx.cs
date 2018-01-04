using System.Web;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.SessionState;
using System;

namespace ykx
{
    /// <summary>
    /// Chexkout 的摘要说明
    /// </summary>
    public class Chexkout : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            if (context.Session["User"] == null || context.Session["User"].ToString() == "")
            {
                context.Response.Redirect("/html/login.html");
            }
            string User_ID = context.Session["Id"].ToString();
            DataTable Category = SqlHelper.ExecuteDataTable("select *  from Category where IS_DELETE = 0");
            string total = "";
            string Action = context.Request["Action"];
            if (Action !=null && Action == "Save")
            {
                string Time = DateTime.Now.ToString();
                string Num = Time.Replace("/", "");
                Num = Num.Replace(":","");
                Num = Num.Replace(" ","");
                Num =  Num+ User_ID;
                string Card = context.Request["Card"];
                string Address = context.Request["Address"];
                string City = context.Request["City"];
                string Country = context.Request["Country"];
                string E_Mail = context.Request["E_Mail"];
                string Phone = context.Request["Phone"];
                string Total_Price = context.Request["Total_Price"];
                SqlHelper.ExecuteNonQuery("Insert into Orders(User_ID,Num,Time,Total_Price,Card,Address,City,Country,Phone,IS_DELETE,E_Mail)" +
                    " values(@User_ID,@Num,@Time,@Total_Price,@Card,@Address,@City,@Country,@Phone,@IS_DELETE,@E_Mail)",
                          new SqlParameter("@User_ID", User_ID)
                         , new SqlParameter("@Num", Num)
                         , new SqlParameter("@Time", Time)
                         , new SqlParameter("@Total_Price", Total_Price)
                         , new SqlParameter("@Card", Card)
                         , new SqlParameter("@Address", Address)
                         , new SqlParameter("@City", City)
                         , new SqlParameter("@Country", Country)
                         , new SqlParameter("@Phone", Phone)
                         , new SqlParameter("@IS_DELETE", "0")
                         , new SqlParameter("@E_Mail", E_Mail)
                         );
                DataTable Orders = SqlHelper.ExecuteDataTable("select * from Orders where Num = '"+ Num+"'");
                string Order_ID = "";
                foreach(DataRow da in Orders.Rows)
                {
                    Order_ID = da["Order_ID"].ToString();
                }
                DataTable Shops = SqlHelper.ExecuteDataTable("select *  from Shops where IS_DELETE = 0 and  User_ID ="+ User_ID);
                foreach(DataRow da in Shops.Rows)
                {
                    string Product_ID = da["Product_ID"].ToString();
                    string Quantity = da["Quantity"].ToString();
                    string Total_Price_S = da["Total_Price"].ToString();
                    SqlHelper.ExecuteNonQuery("Insert into OrderD(Product_ID,Order_ID,Quantity,Total_Price,IS_DELETE)" +
                    " values(@Product_ID,@Order_ID,@Quantity,@Total_Price,@IS_DELETE)",
                          new SqlParameter("@Product_ID", Product_ID)
                         , new SqlParameter("@Order_ID", Order_ID)
                         , new SqlParameter("@Quantity", Quantity)
                         , new SqlParameter("@Total_Price", Total_Price_S)
                         , new SqlParameter("@IS_DELETE", "0")
                         );
                }
                SqlHelper.ExecuteNonQuery("update Shops set IS_DELETE=@IS_DELETE where User_ID=@User_ID and IS_DELETE= @IS_DELETE1",
             new SqlParameter("@IS_DELETE", "2"),
             new SqlParameter("User_ID",User_ID),
             new SqlParameter("@IS_DELETE1", "0")
             );
              
                context.Response.Redirect("http://localhost:54436/Order.ashx?Action=Search");
            }
            else
            {
                total = context.Request["total"];
            }
            var Data = new { Category = Category.Rows, total = total };
            string html = CommonHelper.RenderHtml("checkout.html", Data);
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
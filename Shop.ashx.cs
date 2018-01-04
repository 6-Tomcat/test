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
    /// Shopp 的摘要说明
    /// </summary>
    public class Shopp : IHttpHandler, IRequiresSessionState
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
            if (Action != null && Action == "Save")
            {
                string Product_ID = context.Request["Product_ID"];
                string P_Price = context.Request["P_Price"];
                string Quantity = "";
            if (context.Request["Quantity"] == null || context.Request["Quantity"].ToString() == "")
            {
                Quantity = "1";
            }
            string Total_Price = (int.Parse(P_Price) * int.Parse(Quantity)).ToString();
            string IS_DELETE = "0";
            DataTable U_Shop = SqlHelper.ExecuteDataTable("select *  from Shops where User_ID ="+ User_ID+ " and Product_ID = "+ Product_ID+ "and IS_DELETE = 0");
                if (U_Shop.Rows.Count>=1)
                {
                    string u_shopId = "";
                    int Quantity_Add = 0;
                    foreach (DataRow da in U_Shop.Rows)
                    {
                        u_shopId = da["Shops_ID"].ToString();
                        Quantity_Add = int.Parse(da["Quantity"].ToString())+int.Parse(Quantity);
                    }
                    SqlHelper.ExecuteNonQuery("update Shops set Quantity=@Quantity where Shops_ID=@Shops_ID",
                    new SqlParameter("@Quantity", Quantity_Add),
                    new SqlParameter("@Shops_ID", u_shopId)
                    );
                }else
                {
                    SqlHelper.ExecuteNonQuery("Insert into Shops(User_ID,Product_ID,Quantity,Total_Price,IS_DELETE) " +
                "values(@User_ID,@Product_ID,@Quantity,@Total_Price,@IS_DELETE)",
                     new SqlParameter("@User_ID", User_ID)
                     , new SqlParameter("@Product_ID", Product_ID)
                     , new SqlParameter("@Quantity", Quantity)
                     , new SqlParameter("@Total_Price", Total_Price)
                     , new SqlParameter("@IS_DELETE", IS_DELETE)
                     );
                }
                
                context.Response.Redirect("http://localhost:54436/Shop.ashx");
            }
            if (Action != null && Action == "Delete")
            {
                string Shops_ID = context.Request["Shops_ID"];
                SqlHelper.ExecuteNonQuery("update Shops set IS_DELETE=@IS_DELETE where Shops_ID=@Shops_ID",
                    new SqlParameter("@IS_DELETE",1),
                    new SqlParameter("@Shops_ID", Shops_ID)
                    );
                context.Response.Redirect("http://localhost:54436/Shop.ashx");
            }
            if (Action != null && Action == "Ajax")
            {
                string Shops_ID = context.Request["Shops_ID"];
                string Quantity = context.Request["Quantity"];
                string Total_Price = context.Request["Total_Price"];
                SqlHelper.ExecuteNonQuery("update Shops set Quantity=@Quantity,Total_Price=@Total_Price where Shops_ID=@Shops_ID",
                   new SqlParameter("@Quantity", Quantity),
                   new SqlParameter("@Shops_ID", Shops_ID),
                   new SqlParameter("@Total_Price",Total_Price)
                   );
                context.Response.Write("");
            }else
            {
                DataTable Shops = SqlHelper.ExecuteDataTable("select * from V_Shop where User_ID = " + User_ID + " and" +
              " IS_DELETE = 0");
                DataTable Category = SqlHelper.ExecuteDataTable("select *  from Category where IS_DELETE = 0");
                var Data = new { Shops = Shops.Rows, Category = Category.Rows };
                string html = CommonHelper.RenderHtml("shoppingcart.html", Data);
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
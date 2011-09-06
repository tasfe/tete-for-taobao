using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;
using System.Collections.Generic;

public partial class top_market_ajax : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.tbsandbox.com/router/rest", "test", "test");

        //UserGetRequest request = new UserGetRequest();
        //request.Fields = "buyer_credit";
        //request.Nick = "golddonkey";

        //Taobao.Top.Api.Domain.User user = client.UserGet(request);

        //Response.Write(user.BuyerCredit.Score);





        //ProductsGetRequest request = new ProductsGetRequest();

        //request.Fields = "product_id,name,pic_url,shop_price";
        //request.Nick = "golddonkey";

        //PageList<Product> product = client.ProductsGet(request);

        //Response.Write(product.Content.Count);

        //test.DataSource = product.Content;
        //test.DataBind();



        ShopGetRequest request = new ShopGetRequest();
        request.Fields = "title";
        request.Nick = "golddonkey";

        Shop shop = client.ShopGet(request);

        Response.Write(shop.Title);
    }
}

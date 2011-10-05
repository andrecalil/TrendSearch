using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TrendSearch.Domain;

namespace TrendSearch.Web.App
{
    public partial class Results : BasePage
    {
        private Search aSearch;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session[SessionVariables.SearchEntity.ToString()] != null)
            {
                this.aSearch = (Search)this.Session[SessionVariables.SearchEntity.ToString()];
            }
            else
            {
                Response.Redirect("/Default.aspx");
            }

            this.DoSearch();
        }

        private void DoSearch()
        {
            this.gvResults.DataSource = this.aSearch.SearchAndReate();
            this.gvResults.DataBind();
        }

        protected void lbtExportSearch_Click(object sender, EventArgs e)
        {
            string mSerializedSearch = Serialization.Serialize(this.aSearch);

            Response.Clear();

            Response.ContentType = "text/xml";
            Response.AddHeader("content-disposition", "attachment; filename=trendsearch_settings.xml");

            Response.Write(mSerializedSearch);

            Response.End();
        }
    }
}
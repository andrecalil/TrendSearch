using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using TrendSearch.Domain;
using System.IO;
using TrendSearch.Domain.Sources;

namespace TrendSearch.Web.App
{
    public partial class Default : BasePage
    {
        private List<List<System.Web.UI.Control>> aDynamicControlsArray;

        protected override void OnInit(EventArgs e)
        {
            #region Recover the reference of the dynamic controls and rebind them to the view state

            if (this.Session[SessionVariables.DynamicControlsList.ToString()] != null)
            {
                this.aDynamicControlsArray = (List<List<System.Web.UI.Control>>)this.Session[SessionVariables.DynamicControlsList.ToString()];

                foreach (List<System.Web.UI.Control> mControlList in this.aDynamicControlsArray)
                {
                    foreach (Control mControl in mControlList)
                    {
                        this.phRssSource.Controls.Add(mControl);
                    }
                }
            }
            else
            {
                this.aDynamicControlsArray = new List<List<Control>>();
            }

            #endregion

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnUnload(EventArgs e)
        {
            this.Session[SessionVariables.DynamicControlsList.ToString()] = this.aDynamicControlsArray;

            base.OnUnload(e);
        }

        protected void btAddRssSource_Click(object sender, EventArgs e)
        {
            this.AddRssSourceRow();
        }

        /// <summary>
        /// Adds a new row (checkbox, textbox, textbox) to the RSS source table
        /// and to the page's dynamic control
        /// </summary>
        private void AddRssSourceRow()
        {
            int mDynamicControlsTotal = this.aDynamicControlsArray.Count;
            Literal mLiteralMarkup;
            List<Control> mControlsList = new List<Control>();

            #region <tr><td>

            mLiteralMarkup = new Literal()
            {
                Text = "<tr><td>"
            };

            this.phRssSource.Controls.Add(mLiteralMarkup);
            mControlsList.Add(mLiteralMarkup);

            #endregion

            #region <asp:CheckBox runat="server" ID="cbSourceYouTube" />

            CheckBox mCheckbox = new CheckBox()
            {
                ID = "cbSourceRss" + mDynamicControlsTotal,
                Checked = true,
                Enabled = false
            };

            this.phRssSource.Controls.Add(mCheckbox);
            mControlsList.Add(mCheckbox);

            #endregion

            #region </td><td>RSS</td><td>

            mLiteralMarkup = new Literal()
            {
                Text = "</td><td>RSS</td><td>"
            };

            this.phRssSource.Controls.Add(mLiteralMarkup);
            mControlsList.Add(mLiteralMarkup);

            #endregion
            
            #region <asp:TextBox runat="server" ID="tbRssURL" />

            TextBox mTextbox = new TextBox()
            {
                ID = "tbRssUrl" + mDynamicControlsTotal,
                CssClass = "textbox url",
                CausesValidation = true
            };

            this.phRssSource.Controls.Add(mTextbox);
            mControlsList.Add(mTextbox);

            RequiredFieldValidator mRequiredValidator = new RequiredFieldValidator()
            {
                ID = "rfvRssUrl" + mDynamicControlsTotal,
                Text = "*",
                ControlToValidate = mTextbox.ID
            };

            this.phRssSource.Controls.Add(mRequiredValidator);
            mControlsList.Add(mRequiredValidator);

            #endregion
            
            #region </td><td>Max posts search:

            mLiteralMarkup = new Literal()
            {
                Text = "</td><td>Max posts search:</td><td>"
            };

            this.phRssSource.Controls.Add(mLiteralMarkup);
            mControlsList.Add(mLiteralMarkup);

            #endregion
            
            #region <asp:TextBox runat="server" ID="tbMaxPosts" />

            mTextbox = new TextBox()
            {
                ID = "tbMaxPosts" + mDynamicControlsTotal,
                CssClass = "textbox number",
                CausesValidation = true
            };
            mTextbox.Attributes.Add("IsNumber", "true");

            this.phRssSource.Controls.Add(mTextbox);
            mControlsList.Add(mTextbox);

            mRequiredValidator = new RequiredFieldValidator()
            {
                ID = "rfvMaxPosts" + mDynamicControlsTotal,
                Text = "*",
                ControlToValidate = mTextbox.ID
            };

            this.phRssSource.Controls.Add(mRequiredValidator);
            mControlsList.Add(mRequiredValidator);

            CustomValidator mCustomValidator = new CustomValidator()
            {
                ID = "cvMaxPosts" + mDynamicControlsTotal,
                ControlToValidate = mTextbox.ID,
                ClientValidationFunction = "ValidateInput"
            };

            this.phRssSource.Controls.Add(mCustomValidator);
            mControlsList.Add(mCustomValidator);

            #endregion
            
            #region </td></tr>

            mLiteralMarkup = new Literal()
            {
                Text = "</td></tr>"
            };

            this.phRssSource.Controls.Add(mLiteralMarkup);
            mControlsList.Add(mLiteralMarkup);

            #endregion

            this.aDynamicControlsArray.Add(mControlsList);
        }

        protected void btDoSearch_Click(object sender, EventArgs e)
        {
            this.ProceedToResults(this.GetSearchFromWebForm());
        }

        protected void cbSourceTwitter_OnCheckedChanged(object sender, EventArgs e)
        {
            this.rfvMaxTweets.Enabled = this.cbSourceTwitter.Checked;
            this.cvMaxTweets.Enabled = this.cbSourceTwitter.Checked;
        }

        protected void cbSourceYouTube_OnCheckedChanged(object sender, EventArgs e)
        {
            this.rfvMaxVideos.Enabled = this.cbSourceYouTube.Checked;
            this.cvMaxVideos.Enabled = this.cbSourceYouTube.Checked;
        }

        protected void lbtUploadSearch_OnClick(object sender, EventArgs e)
        {
            if (this.fuSavedSearch.HasFile)
            {
                using (StreamReader mReader = new StreamReader(this.fuSavedSearch.PostedFile.InputStream))
                {
                    string mFileContent = mReader.ReadToEnd();

                    try
                    {
                        Search mRecoveredSearch = Serialization.Deserialize<Search>(mFileContent);
                        this.ProceedToResults(mRecoveredSearch);
                    }
                    catch
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "InvalidFile", "<script language='javascript' type='text/javascript'>ShowMessage('Invalid file');</script>");
                    }
                }
            }
        }

        /// <summary>
        /// Return the search object from the posted web form
        /// </summary>
        /// <returns></returns>
        private Search GetSearchFromWebForm()
        {
            Search mSearch = new Search()
            {
                SearchedDate = DateTime.Now,
                KeyWords = this.tbKeyWords.Text
            };

            if (this.cbSourceTwitter.Checked)
            {
                mSearch.Sources.Add(SourceFactory.GetSource(SourceType.Twitter, Convert.ToInt32(this.tbMaxTweets.Text)));
            }

            if(this.cbSourceYouTube.Checked)
            {
                mSearch.Sources.Add(SourceFactory.GetSource(SourceType.YouTube, Convert.ToInt32(this.tbMaxVideos.Text)));
            }

            foreach (List<Control> mDynamicControls in this.aDynamicControlsArray)
            {
                CheckBox mCheckRSS = (CheckBox)mDynamicControls[1];

                if (mCheckRSS.Checked)
                {
                    TextBox mRssUrl = (TextBox)mDynamicControls[3];
                    TextBox mRssMaxPosts = (TextBox)mDynamicControls[6];

                    RssSource mRssSource = (RssSource)SourceFactory.GetSource(SourceType.RSS, Convert.ToInt32(mRssMaxPosts.Text));
                    mRssSource.URL = mRssUrl.Text;

                    mSearch.Sources.Add(mRssSource);
                }
            }

            return mSearch;
        }

        private void ProceedToResults(Search pPostedSearch)
        {
            this.Session.Add(SessionVariables.SearchEntity.ToString(), pPostedSearch);
            
            Response.Redirect("./Results.aspx");
        }
    }
}
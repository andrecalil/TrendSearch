<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TrendSearch.Web.App.Default" EnableSessionState="True" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">
    function IsNumeric(sText) {
        var ValidChars = "0123456789";
        var IsNumber = true;
        var Char;
        
        for (i = 0; i < sText.length && IsNumber == true; i++) {
            Char = sText.charAt(i);
            if (ValidChars.indexOf(Char) == -1) {
                IsNumber = false;
            }
        }
        return IsNumber;
    }

    function ValidateInput(pSource, pClientsideArguments)
    {
        var mControl = document.getElementById(pSource.controltovalidate);

        if (mControl.value != '' && mControl.getAttribute('IsNumber')) {
            if (!IsNumeric(mControl.value))
            {
                ShowMessage('Only numbers, please');
                pClientsideArguments.IsValid = false;
                mControl.focus();
            }
            else {
                pClientsideArguments.IsValid = true;
            }
        }
    }

    function ShowMessage(pMessage)
    {
        var options = { id: 'messageBox',
            position: 'top',
            delay: 3000,
            speed: 500,
            size: 30,
            backgroundColor: '#CD141F',
            fontSize: '14px'
        };

        $.showMessage(pMessage, options);
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <p id="step1" class="step">Give us your keywords to search</p>
    <asp:TextBox runat="server" CssClass="textbox" CausesValidation="true" ID="tbKeyWords" />
    <asp:RequiredFieldValidator ID="rfvKeyWord" runat="server" ControlToValidate="tbKeyWords" EnableClientScript="true">
        <span class="errorMessage">*</span>
    </asp:RequiredFieldValidator>
    <asp:UpdatePanel ID="upSources" runat="server"> 
        <ContentTemplate>
            <p id="step2" class="step">Choose your search sources</p>
            <table id="sourceTable">
                <tr>
                    <td><asp:CheckBox runat="server" ID="cbSourceTwitter" OnCheckedChanged="cbSourceTwitter_OnCheckedChanged" AutoPostBack="True" /></td>
                    <td>Twitter</td>
                    <td/>
                    <td>Max tweets search:</td>
                    <td>
                        <asp:TextBox runat="server" CssClass="textbox number" AutoPostBack="true" CausesValidation="true" IsNumber="true" ID="tbMaxTweets" />
                        <asp:CustomValidator ID="cvMaxTweets" Enabled="false" runat="server" ControlToValidate="tbMaxTweets" ClientValidationFunction="ValidateInput" />
                        <asp:RequiredFieldValidator ID="rfvMaxTweets" Enabled="false" runat="server" ControlToValidate="tbMaxTweets" EnableClientScript="true">
                            <span class="errorMessage">*</span>
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td><asp:CheckBox runat="server" AutoPostBack="true" OnCheckedChanged="cbSourceYouTube_OnCheckedChanged" ID="cbSourceYouTube" /></td>
                    <td>YouTube</td>
                    <td/>
                    <td>Max videos search:</td>
                    <td>
                        <asp:TextBox runat="server" CssClass="textbox number" CausesValidation="true" IsNumber="true" ID="tbMaxVideos" />
                        <asp:CustomValidator ID="cvMaxVideos" runat="server" Enabled="false" ControlToValidate="tbMaxVideos" ClientValidationFunction="ValidateInput" />
                        <asp:RequiredFieldValidator ID="rfvMaxVideos" runat="server" Enabled="false" ControlToValidate="tbMaxVideos" EnableClientScript="true">
                            <span class="errorMessage">*</span>
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <asp:PlaceHolder runat="server" ID="phRssSource" />
            </table>
            <asp:Button runat="server" ID="btAddRssSource" Text="Add RSS source" CssClass="button addSource" onclick="btAddRssSource_Click" />
        </ContentTemplate>         
    </asp:UpdatePanel>
    <p id="step3" class="step">Way to go!</p>
    <asp:Button runat="server" ID="btDoSearch" Text="Search the trend!" CssClass="button search" onclick="btDoSearch_Click" />
    <br />
    <br />
    <asp:LinkButton ID="lbtUploadSearch" CausesValidation="false" runat="server" OnClick="lbtUploadSearch_OnClick" Text="Or, simply upload a previously saved search" />
    <br />
    <asp:FileUpload ID="fuSavedSearch" runat="server" />
</asp:Content>
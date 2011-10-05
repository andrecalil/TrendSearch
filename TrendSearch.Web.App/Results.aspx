<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Results.aspx.cs" Inherits="TrendSearch.Web.App.Results" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <asp:GridView
        runat="server"
        ID="gvResults"
        AutoGenerateColumns="False" 
        EnableModelValidation="True"
        ShowHeader="False">
        <Columns>
            <asp:ImageField DataImageUrlField="IconURL" />
            <asp:HyperLinkField DataNavigateUrlFields="URL" DataTextField="Title" />
        </Columns>
    </asp:GridView>
    <br />
    <asp:LinkButton ID="lbtExportSearch" runat="server" 
        Text="Save your search settings" onclick="lbtExportSearch_Click" />
</asp:Content>
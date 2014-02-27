<%@ Page Language="C#" AutoEventWireup="true" ViewStateMode="Disabled" CodeBehind="Default.aspx.cs" Inherits="Galleriet.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bildgalleriet</title>
    <link rel="stylesheet" href="Css/style.css">
</head>
<body>
    <form id="wrapper" runat="server">
        <h1>Bildgalleriet</h1>
        <div>
            <p id="theText">Här kan du ladda upp och spara alla dina bilder, sen kolla på dem när du vill!</p> 
            
            <%-- Repeater för tumnagelbilder --%>
            <div id="left">
                <asp:Repeater ID="ThumbRepeater" runat="server" ItemType="System.String" SelectMethod="ThumbImagesToRepeater">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" NavigateUrl='<%#:"Default.aspx?ImgUrl=" + Item.Substring(5) %>'>
                            <asp:Image runat="server" ImageUrl='<%#:"~/Pictures/Thumbs/" + Item %>' />
                        </asp:HyperLink>
                    </ItemTemplate>
                </asp:Repeater>

                <asp:FileUpload ID="UploadImage" runat="server" />
                <asp:Button ID="UploadButton" runat="server" Text="Ladda upp" OnClick="UploadButton_Click" />
            </div>

            <%-- Meddelanden --%>
            <div id="right">
                <asp:Label ID="SuccessLabel" runat="server" Visible="False" CssClass="greenBox"></asp:Label>
                <asp:Label ID="ErrorLabel" runat="server" Visible="False" CssClass="redBox"></asp:Label>


                <%-- Validering av bilder --%>
                <asp:ValidationSummary ID="ValSum" runat="server" CssClass="redBox" />
                
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                    runat="server"
                    ErrorMessage="Du måste välja en fil innan du laddar upp"
                    ControlToValidate="UploadImage"
                    Display="None" 
                    CssClass="red"></asp:RequiredFieldValidator>

                <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                    runat="server"
                    ErrorMessage="Endast bilder av typerna gif png och jpg får laddas upp"
                    Display="None"
                    ValidationExpression="^.*\.(gif|jpg|png|GIF|JPG|PNG)$"
                    ControlToValidate="UploadImage" 
                    CssClass="red"></asp:RegularExpressionValidator>

                <div id="Big">
                    <asp:Image ID="BigImage" runat="server" Visible="False" />
                </div>

            </div>
            <div id="footer">
                <p>Bildgalleriet</p>
            </div>
        </div>
    </form>
</body>
</html>
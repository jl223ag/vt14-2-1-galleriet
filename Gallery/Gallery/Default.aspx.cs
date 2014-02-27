using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Galleriet.Model;
using System.IO; // för att kunna se alla filer i en mapp

namespace Galleriet
{
    public partial class Default : System.Web.UI.Page
    {
        private Gallery Gall
        {
            get { return Session["Gallery"] as Gallery ?? (Gallery)(Session["Gallery"] = new Gallery()); } // om null instantiera nytt objekt
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var imgUrl = Request.QueryString["ImgUrl"];

            if (imgUrl != null)
            {
                BigImage.ImageUrl = String.Format("~/Pictures/{0}", imgUrl);
                BigImage.Visible = true;

                if (Request.QueryString["NewUpload"] != null) // skickar med denna vid uppladdning av ny bild
                {
                    SuccessLabel.Text = String.Format("Bilden \"{0}\" har sparats!", imgUrl);
                    SuccessLabel.Visible = true;
                }
            }
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                if (UploadImage.HasFile)
                {
                    try
                    {
                        string imgName;
                        imgName = Gall.SaveImage(UploadImage.FileContent, UploadImage.FileName);

                        Response.Redirect("Default.aspx?ImgUrl=" + imgName + "&NewUpload=Yes"); // kan man skicka med en bool på något sätt ist?
                    }
                    catch (Exception ex)
                    {
                        ErrorLabel.Text = ex.ToString();
                        ErrorLabel.Visible = true;
                    }
                }
            }
        }

        public IEnumerable<string> ThumbImagesToRepeater() // repeaterns SelectMethod.. bara att returnera stränglistan till den
        {
            var thumbImgs =  Gall.GetImageNames();
            return thumbImgs;
        }
    }
}
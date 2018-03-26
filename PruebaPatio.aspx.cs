using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PruebaPatio : System.Web.UI.Page
{
    string virtualPath = "~/img";
    private void Page_Load(object sender, System.EventArgs e)
    {
        if (!IsPostBack)
        {// First load
            ConfigureRadRotator(RotatorType.Carousel);//  By default, use the RotatorType.Carousel mode
        }

    }

    private void ConfigureRadRotator(RotatorType rotatorType)
    {
        RadRotator1.RotatorType = rotatorType;// Change rotator's type
        RadRotator1.Width = Unit.Percentage(100);
        RadRotator1.Height = Unit.Pixel(760);
        //RadRotator1.DataSource = GetFilesInFolder(virtualPath);// Set datasource
        RadRotator1.DataBind();
    }

    protected List<string> GetFilesInFolder(string folderVirtualPath)
    {
        string physicalPathToFolder = Server.MapPath(folderVirtualPath);// Get the physical path

        string filterExpression = string.Empty;
        if (IsIe6)
            filterExpression = "*.gif";
        else
            filterExpression = "*.png";

        string[] physicalPathsCollection = System.IO.Directory.GetFiles(physicalPathToFolder, filterExpression);// Get all child files of the given folder
        List<string> virtualPathsCollection = new List<string>();// Contains the result

        foreach (String path in physicalPathsCollection)
        {
            // The value of virtualPath will be similar to '~/PathToFolder/Image1.jpg
            string virtualPath = VirtualPathUtility.AppendTrailingSlash(folderVirtualPath) + System.IO.Path.GetFileName(path);
            virtualPathsCollection.Add(virtualPath);
        }
        return virtualPathsCollection;
    }

    private bool IsIe6
    {
        get
        {
            bool ie6 = Context.Request.Browser.IsBrowser("IE") && Context.Request.Browser.MajorVersion < 7;// Is IE 6
            return ie6;
        }
    }
}
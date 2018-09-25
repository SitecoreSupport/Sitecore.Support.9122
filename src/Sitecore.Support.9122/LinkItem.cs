namespace Sitecore.Support.XA.Foundation.Multisite.LinkManagers
{
  using Microsoft.Extensions.DependencyInjection;
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.DependencyInjection;
  using Sitecore.Links;
  using Sitecore.Sites;
  using Sitecore.Web;
  using Sitecore.XA.Foundation.Multisite;
  using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
  using Sitecore.Xml;
  using System.Xml;

  public class LinkItem : Sitecore.XA.Foundation.Multisite.LinkManagers.LinkItem
  {
    private string Url
    {
      get;
      set;
    }
    private string QueryString
    {
      get;
      set;
    }
    public override string TargetUrl
    {
      get
      {
        if (TargetItem != null)
        {
          if (IsMediaLink || TargetItem.Paths.IsMediaItem)
          {
            return MediaItemExtensions.GetMediaUrl(TargetItem, null) + (string.IsNullOrEmpty(this.QueryString) ? "" : ("?" + this.QueryString));
          }
          if (this.IsInternal)
          {
            SiteInfo siteInfo = ServiceLocator.ServiceProvider.GetService<ISiteInfoResolver>().GetSiteInfo(TargetItem);
            UrlOptions urlOptions = (UrlOptions)UrlOptions.DefaultOptions.Clone();
            if (siteInfo != null)
            {
              urlOptions.Site = new SiteContext(siteInfo);
            }
            return LinkManager.GetItemUrl(this.TargetItem, urlOptions) + (string.IsNullOrEmpty(this.QueryString) ? "" : ("?" + this.QueryString));
          }
        }
        return this.Url;
      }
    }

    public LinkItem() : base()
    {
      this.Url = "#";
    }

    public LinkItem(LinkField field) : base(field)
    {
      this.Url = ((field.LinkType == "anchor") ? "#" : "") + field.Url;
      this.QueryString = field.QueryString;
    }

    public LinkItem(FileField field) : base(field)
    { }

    public LinkItem(Item item) : base(item)
    { }

    public LinkItem(string xml) : base(xml)
    {
      if (!string.IsNullOrEmpty(xml))
      {
        XmlNode xmlNode = XmlUtil.GetXmlNode(xml);
        if (xmlNode != null)
        {
          if (xmlNode.Name == "link" && xmlNode.Attributes != null)
          {
            XmlAttribute xmlAttribute = xmlNode.Attributes["url"];
            if (xmlAttribute != null)
            {
              this.Url = xmlAttribute.Value;
            }
          }
        }
      }
    }
  }
}
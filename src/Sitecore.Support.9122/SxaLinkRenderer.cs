namespace Sitecore.Support.XA.Foundation.Multisite.LinkManagers
{
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.Links;

  public class SxaLinkRenderer : Sitecore.XA.Foundation.Multisite.LinkManagers.SxaLinkRenderer
  {
    public SxaLinkRenderer(Item item)
        : base(item)
    {
    }

    protected override string GetUrl(XmlField field)
    {
      if (field != null)
      {
        return new Sitecore.Support.XA.Foundation.Multisite.LinkManagers.LinkItem(field.Value).TargetUrl;
      }
      return LinkManager.GetItemUrl(base.Item, this.GetUrlOptions(base.Item));
    }
  }
}
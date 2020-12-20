using System.Configuration;

namespace ItemStockChecker
{
    public class UrlRetriever
    {
        public static UrlRetrieverSection _Config = ConfigurationManager.GetSection("UrlList") as UrlRetrieverSection;
        public static UrlElementCollection GetUrls()
        {
            return _Config.Urls;
        }
    }

    public class UrlRetrieverSection : ConfigurationSection
    {
        [ConfigurationProperty("urls")]
        public UrlElementCollection Urls
        {
            get { return (UrlElementCollection)this["urls"]; }
        }
    }

    [ConfigurationCollection(typeof(UrlElement))]
    public class UrlElementCollection : ConfigurationElementCollection
    {
        public UrlElement this[int index]
        {
            get { return (UrlElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                BaseAdd(index, value);
            }
        }
        protected override ConfigurationElement CreateNewElement()
        {
            return new UrlElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((UrlElement)element).Name;
        }
    }

    public class UrlElement : ConfigurationElement
    {
        public UrlElement() { }

        [ConfigurationProperty("name", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

    }

}

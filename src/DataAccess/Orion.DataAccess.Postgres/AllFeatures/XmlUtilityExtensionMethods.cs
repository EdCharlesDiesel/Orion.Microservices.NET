using System.Xml.Linq;

namespace Orion.DataAccess.Postgres.AllFeatures
{
    public static class XmlUtilityExtensionMethods
    {
        public static IEnumerable<XElement>? ElementsByLocalName(this XElement? parent, string name)
        {
            if (parent == null)
            {
                return null;
            }

            var result = (from temp in parent.Elements()
                where temp.Name.LocalName == name
                select temp);

            return result;
        }

        public static XElement ElementByLocalName(this XElement? parent, string name)
        {
            if (parent == null)
            {
                return null;
            }

            var result = (from temp in parent.Elements()
                where temp.Name.LocalName == name
                select temp).FirstOrDefault();

            return result;
        }

        public static XElement ElementByLocalNameAndAttributeValue(this XElement parent,
            string elementName,
            string attributeName,
            string attributeValue)
        {
            var matchingElementsByName = parent.ElementsByLocalName(elementName);

            var match = (from temp in matchingElementsByName
                         where
                         temp.HasAttributes &&
                         temp.AttributeValue(attributeName) == attributeValue
                         select temp).FirstOrDefault();

            return match;
        }

        /// <summary>
        /// Finds a child element starting from parent and returns the inner text value.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="childElement"></param>
        /// <returns></returns>
        public static string ElementValue(this XElement parent, string childElement)
        {
            var child = parent.ElementByLocalName(childElement);

            return child.Value;
        }

        public static void SetElementValueByLocalName(this XElement parent, string childElement, string value)
        {
            var child = parent.ElementByLocalName(childElement);

            child.Value = value;
        }

        public static string AttributeValue(this XElement? parent, string attributeName)
        {
            if (parent == null)
            {
                return String.Empty;
            }

            if (parent.HasAttributes == false)
            {
                return String.Empty;
            }

            if (parent.Attribute(attributeName) == null)
            {
                return String.Empty;
            }

            return parent.Attribute(attributeName)?.Value;
        }
    }
}

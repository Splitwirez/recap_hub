using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace ReCap.Hub
{
    public static class XmlUtils
    {
        public static bool TryGetAttributeValue(this XElement element, XName name, out string value)
        {
            var attr = element.Attribute(name);
            if (attr != null)
            {
                value = attr.Value;
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

        public static bool TryGetAttributeBool(this XElement element, XName name, out bool value)
        {
            value = false;
            if (element.TryGetAttributeValue(name, out string valStr))
                return bool.TryParse(valStr, out value);

            return false;
        }
        //=> element.TryGetAttributeValue(name, out string valStr) & bool.TryParse((valStr != null) ? valStr : false.ToString(), out value);

        public static bool TryGetAttributeEnum<TEnum>(this XElement element, XName name, bool ignoreCase, out TEnum value)
            where TEnum : struct
        {
            value = default(TEnum);
            if (element.TryGetAttributeValue(name, out string valStr))
                return Enum.TryParse<TEnum>(valStr, ignoreCase, out value);

            return false;
        }

        public static bool TryGetElementValue(this XElement element, XName name, out string value)
        {
            value = null;
            var subEl = element.Element(name);
            if (subEl != null)
            {
                var subElValue = subEl.Value;
                if (subElValue != null)
                    value = subElValue;
            }
            return value != null;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ReCap.Hub.Models
{
    public abstract class XmlPropertyBase : ModelBase
    {
        protected XName[] _tagPath;
        public XName[] TagPath
        {
            get => _tagPath;
        }

        protected abstract bool ReadFromXmlCore(XElement root);
        public bool ReadFromXml(XElement root, bool notify)
        {
            bool retVal = ReadFromXmlCore(root);
            if (retVal && notify)
                NotifyChanged();
            return retVal;
        }
        protected abstract void WriteToXmlCore(ref XElement root);
        public void WriteToXml(ref XElement root)
        {
            WriteToXmlCore(ref root);
        }

        public event EventHandler ValueChanged;
        public void NotifyChanged()
        {
            ValueChanged?.Invoke(this, new EventArgs());
        }

        public static XElement GetElementForTagPath(XElement root, XName[] tagPath, bool createPath)
        {
            XElement el = root;
            foreach (XName tagName in tagPath)
            {
                XElement nextEl = el.Element(tagName);
                if (nextEl == null)
                {
                    if (createPath)
                    {
                        nextEl = new XElement(tagName);
                        el.Add(nextEl);
                        //nextEl = newEl;
                    }
                    else
                    {
                        nextEl = null;
                        el = null;
                        break;
                    }
                }
                el = nextEl;
            }
            return el;
        }
    }

    public abstract class XmlElementsProperty<TSeq, TItem> : XmlPropertyBase where TSeq : ObservableCollection<TItem>, new()
    {
        readonly TSeq _sequence = new TSeq();
        public TSeq Sequence
        {
            get => _sequence;
        }

        
        protected override bool ReadFromXmlCore(XElement root)
        {
            Debug.WriteLine($"'{TagPath.Last().ToString()}' count before read: {Sequence.Count}");
            bool retVal = false;
            _suppressWrite = true;
            XElement el = XmlPropertyBase.GetElementForTagPath(root, TagPath, false);
            if (el == null)
            {
                retVal = false;
                goto ret;
            }
            var bkpSeq = Sequence.ToList();
            //Sequence.Clear();
            foreach (var child in bkpSeq)
            {
                Sequence.Remove(child);
            }
            //var newSeq = new List<TItem>();
            XName xName = el.Name;
            //Debug.WriteLine($"xName: '{xName}'");
            foreach (var child in el.Elements())
            {
                var item = _fromElement(child);
                if (item != null)
                {
                    Sequence.Add(item);
                    //_insertElement(Sequence, item);
                    Debug.WriteLine($"item added: '{item}'");
                }
            }
            /*Sequence.Clear();
            foreach (var item in newSeq)
            {
                Sequence.Add(item);
            }*/
            retVal = true;

            ret:
            _suppressWrite = false;
            Debug.WriteLine($"'{TagPath.Last().ToString()}' count after read: {Sequence.Count}");
            return retVal;
        }

        bool _suppressWrite = false;
        protected override void WriteToXmlCore(ref XElement root)
        {
            /*if (_suppressWrite)
                return;*/

            XElement el = XmlPropertyBase.GetElementForTagPath(root, TagPath, true);
            el.RemoveNodes();

            foreach (var item in Sequence)
            {
                XElement child = _toElement(item);
                el.Add(child);
                Debug.WriteLine($"element added: '{child}'");
            }
        }

        readonly Func<XElement, TItem> _fromElement;
        readonly Func<TItem, XElement> _toElement;
        readonly Action<TSeq, TItem> _insertElement = (seq, item) =>
        {
            seq.Add(item);
        };
        public XmlElementsProperty(Func<XElement, TItem> fromElement, Func<TItem, XElement> toElement, Action<TSeq, TItem> insertElement, params XName[] tagPath)
            : this(fromElement, toElement, tagPath)
        {
            _insertElement = insertElement;
        }
        public XmlElementsProperty(Func<XElement, TItem> fromElement, Func<TItem, XElement> toElement, params XName[] tagPath)
        {
            _fromElement = fromElement;
            _toElement = toElement;

            _tagPath = tagPath;
        }
    }

    public class XmlElementsProperty<T> : XmlElementsProperty<ObservableCollection<T>, T>
    {
        public XmlElementsProperty(Func<XElement, T> fromElement, Func<T, XElement> toElement, params XName[] tagPath)
            : base(fromElement, toElement, tagPath)
        {
        }

        public XmlElementsProperty(Func<XElement, T> fromElement, Func<T, XElement> toElement, Action<ObservableCollection<T>, T> insertElement, params XName[] tagPath)
            : this(fromElement, toElement, tagPath)
        {
        }
    }


    public abstract class XmlSingularPropertyBase : XmlPropertyBase
    {
        public XmlSingularPropertyBase(params XName[] tagPath)
        {
            _tagPath = tagPath;
        }

        string _rawValue = null;
        protected string RawValue
        {
            get => _rawValue;
            set => RASIC(ref _rawValue, value);
        }

        protected override bool ReadFromXmlCore(XElement root)
        {
            XElement el = XmlPropertyBase.GetElementForTagPath(root, TagPath, false);
            if (el == null)
                return false;
            
            RawValue = el.Value;
            return true;
        }

        protected override void WriteToXmlCore(ref XElement root)
        {
            XElement el = XmlPropertyBase.GetElementForTagPath(root, TagPath, true);

            el.Value = (RawValue != null)
                ? RawValue
                : string.Empty;
        }
    }

    public abstract class XmlSingularPropertyBase<TVal> : XmlSingularPropertyBase
    {
        public abstract TVal Value
        {
            get;
            set;
        }

        public XmlSingularPropertyBase(params XName[] tagPath)
            : base(tagPath)
        { }
        
        public XmlSingularPropertyBase(TVal defaultValue, params XName[] tagPath)
            : this(tagPath)
        {
            Value = defaultValue;
        }
    }

    public class XmlProperty<TVal> : XmlSingularPropertyBase<TVal>
    {
        Func<string, TVal> _fromString = null;
        Func<TVal, string> _toString = null;
        bool _got = false;
        public override TVal Value
        {
            get
            {
                if (!_got)
                    _got = true;

                string rawValue = RawValue;

                if (_fromString != null)
                    return _fromString(rawValue);
                else
                {

                    //TODO: https://stackoverflow.com/questions/26135340/dynamic-tryparse-for-all-data-types ?
                    /*var type = typeof(TVal);
                    return (TVal)(TypeDescriptor.GetConverter(type).ConvertFromString(rawValue));*/

                    dynamic dyType = typeof(TVal);
                    try
                    {
                        if (dyType.TryParse(rawValue, out TVal val))
                            return val;
                    }
                    catch
                    {
                        try
                        {
                            return dyType.Parse(rawValue);
                        }
                        catch
                        {
                        }
                    }
                }

                var type = typeof(TVal);
                return (TVal)(TypeDescriptor.GetConverter(type).ConvertFromString(rawValue));
            }
            set
            {
                TVal refValue = default(TVal);

                if (_toString != null)
                    RawValue = _toString(value);
                else
                    RawValue = value?.ToString();

                RASIC(ref refValue, value);
            }
        }

        public XmlProperty(Func<string, TVal> fromString, Func<TVal, string> toString, params XName[] tagPath)
            : base(tagPath)
        {
            _fromString = fromString;
            _toString = toString;
        }

        public XmlProperty(TVal defaultValue, Func<string, TVal> fromString, Func<TVal, string> toString, params XName[] tagPath)
            : this(fromString, toString, tagPath)
        {
            Value = defaultValue;
        }

        public XmlProperty(params XName[] tagPath)
            : this(null, null, tagPath)
        {

        }


        public XmlProperty(TVal defaultValue, params XName[] tagPath)
            : this(tagPath)
        {
            Value = defaultValue;
        }
    }


    public class XmlStringProperty : XmlSingularPropertyBase<string>
    {
        public XmlStringProperty(params XName[] tagPath)
            : base(tagPath)
        {
            /*if (!typeof(TVal).IsAssignableTo(typeof(string)))
                throw new InvalidDataException($"Type '{typeof(TVal).FullName}' is not assignable to '{typeof(string).FullName}', which is required.");*/
        }
        public XmlStringProperty(string defaultValue, params XName[] tagPath)
            : this(tagPath)
        {
            Value = defaultValue;
        }

        public override string Value
        {
            get => RawValue;
            set
            {
                string newValue = RawValue;
                RawValue = value;
                RASIC(ref newValue, value);
            }
        }
    }


    public class XmlNumericalBoolProperty : XmlProperty<bool>
    {
        static bool NumericalBoolFromString(string str)
        {
            if (int.TryParse(str, out int intVal))
                return intVal > 0;
            else
                throw new Exception();
        }

        public XmlNumericalBoolProperty(params XName[] tagPath)
            : base(NumericalBoolFromString, null, tagPath)
        {
            /*if (!typeof(TVal).IsAssignableTo(typeof(string)))
                throw new InvalidDataException($"Type '{typeof(TVal).FullName}' is not assignable to '{typeof(string).FullName}', which is required.");*/
        }

        public XmlNumericalBoolProperty(bool defaultValue, params XName[] tagPath)
            : this(tagPath)
        {
            Value = defaultValue;
        }

        /*public override bool Value
        {
            get => RawValue;
            set
            {
                string newValue = RawValue;
                RawValue = value;
                RASIC(ref newValue, value);
            }
        }*/
    }

    /*public class XmlIntProperty : XmlProperty<int>
    {
        public XmlIntProperty(params XName[] tagPath)
            : base(null, null, tagPath)
        {

        }
    }*/

    public interface IXmlElementModel
    {
        /*void RefreshFromXml(XElement element);
        void SaveToXml(ref XElement element);*/
    }

    public static class XmlPropExtensions
    {
        public static void XmlRASIC<TRet, TXmlProperty>(this RxObjectBase obj, TXmlProperty backingProperty, TRet newValue, [CallerMemberName] string propertyName = null) where TXmlProperty : XmlSingularPropertyBase<TRet>
        {
            //XmlPropertyBase
            var value = backingProperty.Value;
            bool changed = !(value.Equals(newValue));
            obj.RASIC(ref value, newValue, propertyName);
            backingProperty.NotifyChanged();
            Debug.WriteLine($"XmlRASIC propertyName = '{propertyName}', changed = {changed}");
        }
    }
}

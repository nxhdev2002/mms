using System;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace tmss.Common
{
    public class CommonXml
    {
        public static string SerialSapXml<T>(T dto, XmlQualifiedName ns = null, string dateFormat = "yyyyMMdd", IDictionary<string, string> replaceKeyValuePairs = null)
        {
            var xml = string.Empty;
            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    writer.WriteStartDocument();
                    var t = typeof(T);
                    var tAttr = (DisplayAttribute)t.GetCustomAttribute(typeof(DisplayAttribute));
                    ConvertToSapFormat(writer, t, tAttr?.Name ?? t.Name, null, dto, dateFormat, ns);
                    writer.WriteEndDocument();
                    writer.Flush();
                    //
                    xml = sww.ToString();
                    xml = xml.Replace("utf-16", "UTF-8", StringComparison.CurrentCultureIgnoreCase);
                    if (replaceKeyValuePairs != null && replaceKeyValuePairs.Count > 0)
                    {
                        foreach (var pair in replaceKeyValuePairs)
                        {
                            xml = xml.Replace(pair.Key, pair.Value, StringComparison.CurrentCultureIgnoreCase);
                        }
                    }
                }
            }
            return xml;
        }

        private static void ConvertToSapFormat(XmlWriter writer, Type t, string tagName, string tagGroupName, object dto, string dateFormat, XmlQualifiedName ns = null)
        {
            if (IsSimpleType(t))
            {
                writer.WriteStartElement(tagName);
                writer.WriteString(ConvertToStringValue(dto, dateFormat));
                writer.WriteEndElement();
            }
            else
            {
                if (typeof(IEnumerable).IsAssignableFrom(t) && t != typeof(string))
                {
                    var objT = t.GetGenericArguments()?.Length > 0 ? t.GetGenericArguments()[0] : t.GetElementType();
                    if (!IsSimpleType(objT) && !(typeof(IEnumerable).IsAssignableFrom(objT) && objT != typeof(string)))
                    {
                        if (dto != null)
                        {
                            if (!string.IsNullOrWhiteSpace(tagName))
                            {
                                writer.WriteStartElement(ns?.Name, tagName, ns?.Namespace);
                            }
                            //
                            foreach (var obj in (dto as IEnumerable))
                            {
                                ConvertToSapFormat(writer, objT, tagGroupName ?? objT.Name, tagGroupName, obj, dateFormat);
                            }
                            //
                            if (!string.IsNullOrWhiteSpace(tagName))
                            {
                                writer.WriteEndElement();
                            }
                        }
                    }
                }
                else
                {
                    writer.WriteStartElement(ns?.Name, tagName, ns?.Namespace);
                    foreach (var p in t.GetProperties())
                    {
                        var pT = p.PropertyType;
                        var pAttr = (DisplayAttribute)p.GetCustomAttribute(typeof(DisplayAttribute));
                        bool pAutoGenerateField = true;
                        try { pAutoGenerateField = pAttr == null || pAttr.AutoGenerateField; } catch { }
                        if (pAutoGenerateField)
                        {
                            var pTagName = string.Empty;
                            if (!IsSimpleType(pT) && typeof(IEnumerable).IsAssignableFrom(pT) && pT != typeof(string)) { pTagName = pAttr?.Name; }
                            else { pTagName = pAttr?.Name ?? p.Name; }
                            //
                            ConvertToSapFormat(writer, p.PropertyType, pTagName, pAttr?.GroupName, p.GetValue(dto), dateFormat);
                        }
                    }
                    writer.WriteEndElement();
                }
            }
        }

        private static bool IsSimpleType(Type type)
        {
            var otherSimpleTypes = new Type[] {
                typeof(string),
                typeof(decimal),
                typeof(DateTime),
                typeof(DateTimeOffset),
                typeof(TimeSpan),
                typeof(Guid)
            };
            //
            return
                type.IsPrimitive ||
                otherSimpleTypes.Contains(type) ||
                type.IsEnum ||
                Convert.GetTypeCode(type) != TypeCode.Object ||
                (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && IsSimpleType(type.GetGenericArguments()[0]));
        }

        private static string ConvertToStringValue(object value, string dateFormat)
        {
            string strValue = null;
            if (value != null)
            {
                var t = value.GetType();
                if (t == typeof(DateTime))
                {
                    strValue = ((DateTime)value).ToString(dateFormat);
                }
                else if (t == typeof(DateTimeOffset))
                {
                    strValue = ((DateTimeOffset)value).ToString(dateFormat);
                }
                else if (t == typeof(TimeSpan))
                {
                    strValue = ((TimeSpan)value).ToString(dateFormat);
                }
                else
                {
                    if (IsNumber(t))
                    {
                        var dotNumberFormat = (NumberFormatInfo)CultureInfo.InstalledUICulture.NumberFormat.Clone();
                        dotNumberFormat.NumberDecimalSeparator = ".";
                        strValue = string.Format(dotNumberFormat, "{0:0.##}", value);
                    }
                    else
                    {
                        strValue = value.ToString();
                    }
                }
            }
            return strValue;
        }

        private static bool IsNumber(Type type)
        {
            var numberTypes = new Type[] {
                typeof(sbyte),
                typeof(byte),
                typeof(short),
                typeof(ushort),
                typeof(int),
                typeof(uint),
                typeof(long),
                typeof(ulong),
                typeof(float),
                typeof(double),
                typeof(decimal),
                typeof(sbyte?),
                typeof(byte?),
                typeof(short?),
                typeof(ushort?),
                typeof(int?),
                typeof(uint?),
                typeof(long?),
                typeof(ulong?),
                typeof(float?),
                typeof(double?),
                typeof(decimal?)
            };
            return numberTypes.Contains(type);
        }

        public static T DeserialSapXml<T>(string xml, string dateFormat = "yyyyMMdd")
        {
            var t = typeof(T);
            var tAttr = (DisplayAttribute)t.GetCustomAttribute(typeof(DisplayAttribute));
            var dto = (T)Activator.CreateInstance(t);
            if (!string.IsNullOrWhiteSpace(xml))
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                var root = xmlDoc.DocumentElement;
                if (root.LocalName == (tAttr?.Name ?? t.Name))
                {
                    var xpath = string.Empty;
                    foreach (var p in t.GetProperties())
                    {
                        ConvertFromSapFormat(root, p, dto, xpath, dateFormat);
                    }
                }
            }
            return dto;
        }

        private static void ConvertFromSapFormat(XmlElement root, PropertyInfo p, object dto, string xpath, string dateFormat)
        {
            if (dto != null)
            {
                var t = p.PropertyType;
                var pAttr = (DisplayAttribute)p.GetCustomAttribute(typeof(DisplayAttribute));
                bool pAutoGenerateField = true;
                try { pAutoGenerateField = pAttr == null || pAttr.AutoGenerateField; } catch { }
                if (pAutoGenerateField)
                {
                    var curXpath = string.Concat(xpath, (string.IsNullOrEmpty(xpath) ? "" : "/"), pAttr?.Name ?? p.Name);
                    if (IsSimpleType(t))
                    {
                        var value = ConvertFromStringValue(t, root.SelectSingleNode(curXpath)?.InnerText, dateFormat);
                        p.SetValue(dto, value);
                    }
                    else
                    {
                        if (typeof(IEnumerable).IsAssignableFrom(t) && t != typeof(string))
                        {
                            var objT = t.GetGenericArguments()?.Length > 0 ? t.GetGenericArguments()[0] : t.GetElementType();
                            if (!IsSimpleType(objT) && !(typeof(IEnumerable).IsAssignableFrom(objT) && objT != typeof(string)))
                            {
                                if (!string.IsNullOrWhiteSpace(pAttr?.Name))
                                {
                                    curXpath = string.Concat(xpath, (string.IsNullOrEmpty(xpath) ? "" : "/"), pAttr?.Name ?? p.Name, "/", pAttr?.GroupName ?? objT.Name);
                                }
                                else
                                {
                                    curXpath = string.Concat(xpath, (string.IsNullOrEmpty(xpath) ? "" : "/"), pAttr?.GroupName ?? objT.Name);
                                }
                                var nodes = root.SelectNodes(curXpath);
                                if (nodes?.Count > 0)
                                {
                                    if (t.IsArray)
                                    {
                                        var arr = (Array)Activator.CreateInstance(t, new object[] { nodes.Count });
                                        p.SetValue(dto, arr);
                                        var i = 0;
                                        foreach (var node in nodes)
                                        {
                                            var obj = Activator.CreateInstance(objT);
                                            arr.SetValue(obj, i);
                                            foreach (var sp in objT.GetProperties())
                                            {
                                                ConvertFromSapFormat(root, sp, obj, string.Concat(curXpath, "[", i + 1, "]"), dateFormat);
                                            }
                                            i++;
                                        }
                                    }
                                    else
                                    {
                                        var list = (IList)Activator.CreateInstance(t);
                                        p.SetValue(dto, list);
                                        var i = 0;
                                        foreach (var node in nodes)
                                        {
                                            var obj = Activator.CreateInstance(objT);
                                            list.Add(obj);
                                            foreach (var sp in objT.GetProperties())
                                            {
                                                ConvertFromSapFormat(root, sp, obj, string.Concat(curXpath, "[", i + 1, "]"), dateFormat);
                                            }
                                            i++;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (root.SelectSingleNode(curXpath) != null)
                            {
                                var obj = Activator.CreateInstance(t);
                                p.SetValue(dto, obj);
                                foreach (var sp in t.GetProperties())
                                {
                                    ConvertFromSapFormat(root, sp, obj, curXpath, dateFormat);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static object ConvertFromStringValue(Type t, string value, string dateFormat)
        {
            object obj = value;
            if (obj != null)
            {
                if (t == typeof(DateTime))
                {
                    obj = DateTime.ParseExact(value, dateFormat, CultureInfo.CreateSpecificCulture("vi-VN"));
                }
                else if (t == typeof(DateTimeOffset))
                {
                    obj = DateTime.ParseExact(value, dateFormat, CultureInfo.CreateSpecificCulture("vi-VN"));
                }
                else if (t == typeof(TimeSpan))
                {
                    obj = DateTime.ParseExact(value, dateFormat, CultureInfo.CreateSpecificCulture("vi-VN"));
                }
                else if (IsNumber(t))
                {
                    var dotNumberFormat = (NumberFormatInfo)CultureInfo.InstalledUICulture.NumberFormat.Clone();
                    dotNumberFormat.NumberDecimalSeparator = ".";
                    //                
                    if (t == typeof(sbyte))
                    {
                        obj = sbyte.Parse(value, dotNumberFormat);
                    }
                    else if (t == typeof(byte))
                    {
                        obj = byte.Parse(value, dotNumberFormat);
                    }
                    else if (t == typeof(short))
                    {
                        obj = short.Parse(value, dotNumberFormat);
                    }
                    else if (t == typeof(ushort))
                    {
                        obj = ushort.Parse(value, dotNumberFormat);
                    }
                    else if (t == typeof(int))
                    {
                        obj = int.Parse(value, dotNumberFormat);
                    }
                    else if (t == typeof(uint))
                    {
                        obj = uint.Parse(value, dotNumberFormat);
                    }
                    else if (t == typeof(long))
                    {
                        obj = long.Parse(value, dotNumberFormat);
                    }
                    else if (t == typeof(ulong))
                    {
                        obj = ulong.Parse(value, dotNumberFormat);
                    }
                    else if (t == typeof(float))
                    {
                        obj = float.Parse(value, dotNumberFormat);
                    }
                    else if (t == typeof(double))
                    {
                        obj = double.Parse(value, dotNumberFormat);
                    }
                    else if (t == typeof(decimal))
                    {
                        obj = decimal.Parse(value, dotNumberFormat);
                    }
                }
            }
            return obj;
        }
    }
}

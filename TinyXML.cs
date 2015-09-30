using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

/* Beware, scary generic beasties lurk within.
 */

namespace TinyXML
{
    public interface IFileModel
    {
        XElement Serialize(string name = null);
    }

    public static class XmlParsingExtensions
    {
        public static int ParseInt(this XAttribute element)
        {
            int outVal;
            return Int32.TryParse(element.Value, out outVal) ? outVal : default(int);
        }

        public static int ParseInt(this XElement element)
        {
            int outVal;
            return Int32.TryParse(element.Value, out outVal) ? outVal : default(int);
        }

        public static int? TryParseInt(this XAttribute element)
        {
            int outVal;
            return Int32.TryParse(element.Value, out outVal) ? outVal : default(int?);
        }

        public static int? TryParseInt(this XElement element)
        {
            int outVal;
            return Int32.TryParse(element.Value, out outVal) ? outVal : default(int?);
        }

        public static long ParseLong(this XElement element)
        {
            long outVal;
            return Int64.TryParse(element.Value, out outVal) ? outVal : default(long);
        }

        public static long ParseLong(this XAttribute element)
        {
            long outVal;
            return Int64.TryParse(element.Value, out outVal) ? outVal : default(long);
        }

        public static ulong ParseULong(this XElement element)
        {
            ulong outVal;
            return UInt64.TryParse(element.Value, out outVal) ? outVal : default(ulong);
        }

        public static ulong ParseULong(this XAttribute element)
        {
            ulong outVal;
            return UInt64.TryParse(element.Value, out outVal) ? outVal : default(ulong);
        }

        public static float ParseFloat(this XAttribute element)
        {
            float outVal;
            return Single.TryParse(element.Value, out outVal) ? outVal : default(float);
        }

        public static float ParseFloat(this XElement element)
        {
            float outVal;
            return Single.TryParse(element.Value, out outVal) ? outVal : default(float);
        }

        public static decimal ParseDecimal(this XAttribute element)
        {
            decimal outVal;
            return Decimal.TryParse(element.Value, out outVal) ? outVal : default(decimal);
        }

        public static decimal ParseeDecimal(this XElement element)
        {
            decimal outVal;
            return Decimal.TryParse(element.Value, out outVal) ? outVal : default(decimal);
        }

        public static decimal? TryParseDecimal(this XAttribute element)
        {
            decimal outVal;
            return Decimal.TryParse(element.Value, out outVal) ? outVal : default(decimal?);
        }

        public static decimal? TryParseDecimal(this XElement element)
        {
            decimal outVal;
            return Decimal.TryParse(element.Value, out outVal) ? outVal : default(decimal?);
        }

        public static byte ParseByte(this XAttribute element)
        {
            byte outVal;
            return Byte.TryParse(element.Value, out outVal) ? outVal : default(byte);
        }

        public static byte ParseByte(this XElement element)
        {
            byte outVal;
            return Byte.TryParse(element.Value, out outVal) ? outVal : default(byte);
        }

        public static bool ParseBoolean(this XAttribute element)
        {
            bool outVal;
            return Boolean.TryParse(element.Value, out outVal) ? outVal : default(bool);
        }

        public static bool ParseBoolean(this XElement element)
        {
            bool outVal;
            return Boolean.TryParse(element.Value, out outVal) ? outVal : default(bool);
        }

        public static bool? TryParseBoolean(this XAttribute element)
        {
            bool outVal;
            return Boolean.TryParse(element.Value, out outVal) ? outVal : default(bool?);
        }

        public static bool? TryParseBoolean(this XElement element)
        {
            bool outVal;
            return Boolean.TryParse(element.Value, out outVal) ? outVal : default(bool?);
        }

        public static string TryParseString(this XElement element)
        {
            return String.IsNullOrWhiteSpace(element.Value) ? null : element.Value;
        }

        public static string TryParseString(this XAttribute element)
        {
            return String.IsNullOrWhiteSpace(element.Value) ? null : element.Value;
        }

        public static DateTime ParseDateTime(this XElement element)
        {
            DateTime outval;
            return DateTime.TryParse(element.Value, out outval) ? outval : DateTime.MinValue;
        }

        public static DateTime ParseDateTime(this XAttribute element)
        {
            DateTime outVal;
            return DateTime.TryParse(element.Value, out outVal) ? outVal : DateTime.MinValue;
        }

        public static DateTime? TryParseDateTime(this XElement element)
        {
            DateTime outVal;
            return DateTime.TryParse(element.Value, out outVal) ? outVal : default(DateTime?);
        }

        public static DateTime? TryParseDateTime(this XAttribute element)
        {
            DateTime outVal;
            return DateTime.TryParse(element.Value, out outVal) ? outVal : default(DateTime?);
        }

        public static Uri TryParseUri(this XElement element)
        {
            Uri outVal;
            return Uri.TryCreate(element.Value, UriKind.RelativeOrAbsolute, out outVal) ? outVal : default(Uri);
        }
    }

    public static class Namespace
    {
        public static XNamespace DefaultXName { get; set; }

        static Namespace()
        {
            DefaultXName = "";
        }
    }

    public static class DictionaryExtensions
    {
        public static T GetOr<T>(this IDictionary<string, T> context, string key, T defaultValue)
        {
            T tempValue;
            var success = context.TryGetValue(key, out tempValue);

            return success ? tempValue : defaultValue;
        }

        public static T GetOr<T>(this IDictionary<string, T> context, string key, Func<string, T> defaultFunc)
        {
            return context.GetOr(key, defaultFunc(key));
        }
    }

    public static class XMLExtensions
    {
        // Testing for elements/attributes

        /// <summary>
        /// Query if a element exists in the source object
        /// </summary>
        /// <param name="element">Source object to pull values from</param>
        /// <param name="elementName">Name of the element to search for</param>
        /// <returns>True if an element exists, false if it does not</returns>
        public static bool ElementExists(this XElement element, string elementName)
        {
            return element.Element(Namespace.DefaultXName + elementName) != null;
        }

        /// <summary>
        /// Return a truth value depending on if the named element exists
        /// </summary>
        /// <param name="element">Source object to pull values from</param>
        /// <param name="attributeName">Name of the attribute to search for</param>
        /// <returns>True if the attribute exists, false if it does not</returns>
        public static bool AttributeExists(this XElement element, string attributeName)
        {
            return element.Attribute(Namespace.DefaultXName + attributeName) != null;
        }

        // Failsafe Element/Attribute retrieval.

        /// <summary>
        /// Return the element named by the name of the property referenced or a default value.
        /// </summary>
        /// <typeparam name="T">Type of the source</typeparam>
        /// <typeparam name="TResult">Type of the property</typeparam>
        /// <param name="element">XElement to search in</param>
        /// <param name="source">Source object to pull values from</param>
        /// <param name="propertyName">A lambda pointing at the property</param>
        /// <param name="defaultValue">A default value to use if the element isn't found.</param>
        /// <returns>Either the element found, or a default-value element</returns>
        /// <example>
        ///     xelement.ElementOr(this, s => s.Property, default(int));
        /// </example>
        public static XElement ElementOr<T>(this XElement element, Expression<Func<T>> propertyName, T defaultValue)
        {
            var elementName = propertyName.GetProperty().Name;
            return element.Element(Namespace.DefaultXName + elementName) ?? new XElement(elementName, defaultValue);
        }

        /// <summary>
        /// Return the element named by the name of the property referenced or a default value, allows
        /// mapping the element into it's return form.
        /// </summary>
        /// <typeparam name="T">Type of the source</typeparam>
        /// <param name="element">XElement to search in</param>
        /// <param name="propertyName">A lambda pointing at the property</param>
        /// <param name="mapFunc">A lambda to map the XElement to its final form.</param>
        /// <returns>The mapped element</returns>
        /// <example>
        ///     int value = xelement.ElementOr(() => IntProperty, item => Int32.Parse(item));
        /// </example>
        /// <remarks>
        /// If the property is not found in the XElement, a default value will be returned, you should account
        /// for this in your mapping function.
        /// </remarks>
        public static T ElementOr<T>(this XElement element, Expression<Func<T>> propertyName, Func<XElement, T> mapFunc)
        {
            var elementName = propertyName.GetProperty().Name;
            return mapFunc(element.Element(Namespace.DefaultXName + elementName)
                           ?? new XElement(Namespace.DefaultXName + elementName, default(T)));
        }

        /// <summary>
        /// Return the element named by the name of the property referenced or a default value, allowing a mapping
        /// function for both a valid element and when the element isn't present.
        /// </summary>
        /// <typeparam name="T">Type of the source</typeparam>
        /// <param name="element">XElement to search in</param>
        /// <param name="propertyName">A lambda pointing at the property</param>
        /// <param name="mapFunc">A mapping function to invoke when there is a valid element</param>
        /// <param name="defaultMappingFunc">A mapping function to invoke when no element was found</param>
        /// <example>
        ///     int value = xelement.ElementOr(() => IntProperty, item => Int32.Parse(item), () => default(int));    
        /// </example>
        /// <remarks>If the property is not found in the XElement, the defaultMappingFunc will be invoked, otherwise mapFunc will be invoked</remarks>
        /// <returns>The mapped element</returns>
        public static T ElementOr<T>(this XElement element, Expression<Func<T>> propertyName, Func<XElement, T> mapFunc,
                Func<T> defaultMappingFunc)
        {
            var elementName = propertyName.GetProperty().Name;
            var foundElement = element.Element(Namespace.DefaultXName + elementName);
            return foundElement != null ? mapFunc(foundElement) : defaultMappingFunc();
        }

        /// <summary>
        /// Returns the named element from the XElement object, or a default value.
        /// </summary>
        /// <typeparam name="T">The type of the default value</typeparam>
        /// <param name="element">The XElement to search in</param>
        /// <param name="elementName">The name of the element to look for</param>
        /// <param name="defaultValue">A default value to supply if the element isn't found.</param>
        /// <returns>The found element, or a new element containing a default value with the correct name.</returns>
        public static XElement ElementOr<T>(this XElement element, string elementName, T defaultValue)
        {
            return element.Element(Namespace.DefaultXName + elementName) ?? new XElement(elementName, defaultValue);
        }

        /// <summary>
        /// Return the attribute named by the name of the property referenced or a specified default value. 
        /// </summary>
        /// <typeparam name="T">Type of the source</typeparam>
        /// <typeparam name="TResult">Type of the property</typeparam>
        /// <param name="element">XElement to search in</param>
        /// <param name="source">Source object to pull values from</param>
        /// <param name="propertyName">A lambda pointing at the property</param>
        /// <param name="defaultValue">A default value to use if the element is missing</param>
        /// <returns>An XAttribute with either the found value or a default value</returns>
        /// <example>
        ///     var result = xelement.AttributeOr(this, s => s.IntProperty, default(int));
        /// </example>
        public static XAttribute AttributeOr<T>(this XElement element, Expression<Func<T>> propertyName, T defaultValue)
        {
            var elementName = propertyName.GetProperty().Name;
            return element.Attribute(Namespace.DefaultXName + elementName) ?? new XAttribute(elementName, defaultValue);
        }

        /// <summary>
        /// Returns the attribute named by the name of the property referenced with the ability to map the XAttribute into it's final form.
        /// </summary>
        /// <typeparam name="T">Type of the source</typeparam>
        /// <typeparam name="TResult">Type of the property</typeparam>
        /// <param name="element">XElement to search in</param>
        /// <param name="source">Source object to pull values from</param>
        /// <param name="propertyName">A lambda pointing at the property</param>
        /// <param name="mapFunc">A lambda for mapping the XAttribute to the TResult, the attribute may have no value</param>
        /// <returns>An XAttribute with either the found value or no value.</returns>
        /// <example>
        ///     string result = xelement.AttributeOr(this, s => s.IntProperty, item => item.ToString("D4"));
        /// </example>
        public static T AttributeOr<T>(this XElement element, Expression<Func<T>> propertyName, Func<XAttribute, T> mapFunc)
        {
            var elementName = propertyName.GetProperty().Name;
            return mapFunc(element.Attribute(Namespace.DefaultXName + elementName)
                           ?? new XAttribute(Namespace.DefaultXName + elementName, String.Empty));
        }

        /// <summary>
        /// Returns the attribute named with a default value if not found in the XElement.
        /// </summary>
        /// <typeparam name="T">Type of the default value</typeparam>
        /// <param name="element">XElement to search in</param>
        /// <param name="elementName">The name of the element to look for</param>
        /// <param name="defaultValue">A default value to use if the element isn't found</param>
        /// <returns>An XAttribute with either the found value or a default value</returns>
        public static XAttribute AttributeOr<T>(this XElement element, XName elementName, T defaultValue)
        {
            return element.Attribute(elementName) ?? new XAttribute(elementName, defaultValue);
        }

        // Attachment of Elements/Attributes.

        /// <summary>
        /// Attach an attribute to the XElement from a property on a model.
        /// </summary>
        /// <typeparam name="T">The type of the source</typeparam>
        /// <param name="element">The XElement to attach to</param>
        /// <param name="propertyExpression">A lambda pointing to the property</param>
        /// <returns>An XElement with an attached attribute</returns>
        public static XElement AttachAttribute<T>(this XElement element, Expression<Func<T>> propertyExpression)
        {
            var value = propertyExpression.Compile().GetValue();

            Action<XElement> mutate = val => val.Add(new XAttribute(propertyExpression.GetProperty().Name, value));
            mutate(element);
            return element;
        }

        /// <summary>
        /// Attach an attribute to the XElement from a property on the model.
        /// If the passed string property is null, an empty string will be written instead.
        /// </summary>
        /// <param name="element">The XElement to attach to</param>
        /// <param name="propertyExpression">A lambda pointing to the property</param>
        /// <returns>An XElement with an attached attribute</returns>
        public static XElement AttachAttribute(this XElement element, Expression<Func<string>> propertyExpression)
        {
            var value = propertyExpression.Compile().GetValue();

            Action<XElement> mutate = val => val.Add(new XAttribute(propertyExpression.GetProperty().Name, value ?? String.Empty));
            mutate(element);
            return element;
        }

        /// <summary>
        /// Attach an attribute to the XElement from a property on a class if a predicate is met, allows a mapping function.
        /// </summary>
        /// <typeparam name="T">The type of the source</typeparam>        
        /// <typeparam name="TResult">The final type of the attribute</typeparam>
        /// <param name="element">The XElement to attach to</param>
        /// <param name="propertyName">A lambda pointing to the property</param>
        /// <param name="mapFunc">A mapping function to transform values</param>
        /// <param name="predicate">A predicate on if the attribute should be attached</param>
        /// <returns>An XElement with an attached attribute if the predicate is true.</returns>
        public static XElement AttachAttributeIf<T, TResult>(this XElement element, Expression<Func<T>> propertyName,
                Func<bool> predicate, Func<T, TResult> mapFunc)
        {
            return predicate() ? element.AttachAttribute(propertyName, mapFunc) : element;
        }

        public static XElement AttachAttributeIf<T>(this XElement element, Expression<Func<T>> propertyName, Func<bool> predicate)
        {
            return predicate() ? element.AttachAttribute(propertyName) : element;
        }

        /// <summary>
        /// Attach an attribute to the XElement from a property on a class, allowing a mapping function.
        /// </summary>
        /// <typeparam name="T">The type of source</typeparam>
        /// <typeparam name="U">The type of the property</typeparam>
        /// <typeparam name="TResult">The final type of the attribute</typeparam>
        /// <param name="element">The XElement to attach to</param>
        /// <param name="source">The source object to pull values from</param>
        /// <param name="propertyName">A lambda pointing to the property</param>
        /// <param name="mapFunc">A lambda with a mapping function</param>
        /// <returns>An XElement with an attached attribute</returns>
        /// <example>
        ///     new XElement("Element").AttachAttribute(this, s => s.IntProperty, item => item.ToString("D4"));
        /// </example>
        public static XElement AttachAttribute<T, TResult>(this XElement element, Expression<Func<T>> propertyName,
                Func<T, TResult> mapFunc)
        {
            var value = propertyName.Compile().GetValue();

            Action<XElement> mutate = val => val.Add(new XAttribute(propertyName.GetProperty().Name, mapFunc(value)));
            mutate(element);
            return element;
        }

        /// <summary>
        /// Attach an element to the given XElement using a property as the source and allowing a mapping function.
        /// </summary>
        /// <typeparam name="T">The type of source</typeparam>
        /// <typeparam name="U">The type of the property</typeparam>
        /// <typeparam name="TResult">The final type of the attribute</typeparam>
        /// <param name="element">The element to attach to</param>
        /// <param name="source">The source object to pull values from</param>
        /// <param name="propertyName">A lambda pointing to the property</param>
        /// <param name="mapFunc">A lambda with a mapping function</param>
        /// <returns>An XElement with an attached child element</returns>
        /// <example>
        ///     new XElement("Element").AttachElement(this, s => s.IntProperty, item => item.ToString("D4"));
        /// </example>
        public static XElement AttachElement<T, TResult>(this XElement element, Expression<Func<T>> propertyName,
                Func<T, TResult> mapFunc)
        {
            var value = propertyName.Compile().GetValue();
            Action<XElement> mutate = val => val.Add(new XElement(propertyName.GetProperty().Name, mapFunc(value)));
            mutate(element);
            return element;
        }

        /// <summary>
        /// Attach an XElement as a child to the given XElement using a property as the source.
        /// Automatically calls Serialize on the IFileModel
        /// </summary>
        /// <typeparam name="T">The type of the source</typeparam>
        /// <typeparam name="U">The type of the property</typeparam>
        /// <param name="element">The element to attach to</param>
        /// <param name="source">The source object to pull objects from</param>
        /// <param name="propertyName">A lambda pointing to the property</param>
        /// <returns>An XElement with the attached child element.</returns>
        public static XElement AttachModel(this XElement element, Expression<Func<IFileModel>> propertyName)
        {
            var value = propertyName.Compile().GetValue();
            if (value == null)
                return element;
            Action<XElement> mutate = val => val.Add(value.Serialize());
            mutate(element);
            return element;
        }

        public static XElement AttachModel(this XElement element, Expression<Func<IFileModel>> propertyName, string elementName)
        {
            var value = propertyName.Compile().GetValue();
            if (value == null)
                return element;
            Action<XElement> mutate = val => val.Add(value.Serialize(elementName));
            mutate(element);
            return element;
        }

        /// <summary>
        /// Attach an XElement as a child to the given XElement using a property as the source.
        /// </summary>
        /// <typeparam name="T">The type of the source</typeparam>
        /// <typeparam name="U">The type of the property</typeparam>
        /// <param name="element">The element to attach to</param>
        /// <param name="source">The source object to pull objects from</param>
        /// <param name="propertyName">A lambda pointing to the property</param>
        /// <returns>An XElement with the attached child element.</returns>
        public static XElement AttachElement<T>(this XElement element, Expression<Func<T>> propertyName)
        {
            var value = propertyName.Compile().GetValue();
            Action<XElement> mutate = val => val.Add(new XElement(propertyName.GetProperty().Name, value));
            mutate(element);
            return element;
        }

        /// <summary>
        /// Attach an XElement as a child to the given XElement using a property as the source and a custom name.
        /// </summary>
        /// <typeparam name="T">The type of the source</typeparam>
        /// <param name="element">The element to attach to</param>
        /// <param name="propertyName">A lambda pointing to the property</param>
        /// <param name="elementName">The name of the element</param>
        /// <returns>An XElement with the attached child element.</returns>
        public static XElement AttachElement<T>(this XElement element, Expression<Func<T>> propertyName, string elementName)
        {
            var value = propertyName.Compile().GetValue();
            Action<XElement> mutate = val => val.Add(new XElement(elementName, value));
            mutate(element);
            return element;
        }

        /// <summary>
        /// Select one of multiple predicate conditions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="conditions"></param>
        /// <exception cref="InvalidOperationException">More than one element predicate was true</exception>
        /// <returns></returns>
        public static T SingleElement<T>(this XElement source, params Tuple<bool, Func<XElement, T>>[] conditions)
        {
            var result = conditions.SingleOrDefault(s => s.Item1);

            return result != null ? result.Item2(source) : default(T);
        }

        /// <summary>
        /// Finds child elements matching <paramref name="name"/> and applies a mapping function to each value.
        /// </summary>
        /// <typeparam name="TSource">The final type to return</typeparam>
        /// <param name="element">XElement to work with</param>
        /// <param name="name">Name to search for</param>
        /// <param name="map">Map function to apply</param>
        /// <returns>Collection of mapped values</returns>
        public static IEnumerable<TSource> Map<TSource>(this XElement element, string name, Func<XElement, TSource> map)
        {
            return element.Elements(Namespace.DefaultXName + name).Select(map);
        }

        /// <summary>
        /// Write an XDocument to a string with an encoding declaration in the prolog.
        /// </summary>
        /// <param name="doc">XDocument to write to a string</param>
        /// <returns>A XML string of the XDocument</returns>
        public static string SerializeXDocumentWithDeclaration(this XDocument doc)
        {
            if (doc == null) throw new ArgumentNullException("doc");
            doc.Declaration = new XDeclaration("1.0", "utf-8", null);

            string result;

            using (var ms = new MemoryStream())
            {
                var settings = new XmlWriterSettings
                {
                    Encoding = Encoding.UTF8,
                    ConformanceLevel = ConformanceLevel.Document,
                    Indent = true
                };

                using (XmlWriter xw = XmlTextWriter.Create(ms, settings))
                {
                    doc.Save(xw);
                    xw.Flush();

                    var sr = new StreamReader(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    result = sr.ReadToEnd();
                }
            }

            return result;
        }
    }

    public static class XmlValidationExtensions
    {
        public static XmlSchemaSet ToSchemaSet(this IEnumerable<StreamReader> streams, ValidationEventHandler handler)
        {
            var schemaSet = new XmlSchemaSet();
            schemaSet.AddRange(streams.Select(s => XmlSchema.Read(s, handler)));
            return schemaSet;
        }

        public static XmlSchemaSet AddRange(this XmlSchemaSet context, IEnumerable<XmlSchema> schemas)
        {
            foreach (var schema in schemas)
            {
                context.Add(schema);
            }
            return context;
        }
    }

    public static class ExpressionExtensions
    {
        /// <summary>
        /// Decompose an Expression Tree into parts and return a PropertyInfo if the expression
        /// resolved to a property on a class.
        /// </summary>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="propertyExpression">Lambda pointing to the property</param>
        /// <returns>A PropertyInfo object describing the target</returns>
        public static PropertyInfo GetProperty<T>(this Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException("propertyExpression");

            var body = propertyExpression.Body as MemberExpression;

            if (body == null)
                throw new ArgumentException("Invalid Expression Body", "propertyExpression");

            var property = body.Member as PropertyInfo;

            if (property == null)
                throw new ArgumentException("Argument body is not a property", "propertyExpression");

            return property;
        }

        /// <summary>
        /// Get the value from an expression, allowing for types that could be null.
        /// </summary>
        /// <typeparam name="T">Type of the object to retrieve</typeparam>
        /// <param name="accessor">Lambda to retrieve the object</param>
        /// <param name="defaultValue">A default value to return instead of null</param>
        /// <returns>The objects value or the default value</returns>
        public static T GetValue<T>(this Func<T> accessor, T defaultValue = default(T))
        {
            var type = typeof(T);
            bool isNullable = !type.IsValueType || (Nullable.GetUnderlyingType(type) != null);
            T value;
            if (isNullable)
            {
                var val = accessor();
                value = val != null ? val : defaultValue;
            }
            else
            {
                value = accessor();
            }
            return value;
        }
    }
}

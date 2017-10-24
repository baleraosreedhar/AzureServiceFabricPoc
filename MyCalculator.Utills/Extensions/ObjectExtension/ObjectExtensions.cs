using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypeExtensions = MyUtils.Extensions.TypeExtension.TypeExtensions;

namespace MyUtils.Extensions.ObjectExtension
{
    /// <summary>
    /// An attribute that has a string Value {get; set;} property
    /// </summary>
    public interface IAttributeValue<TValue>
    {
        /// <summary>
        /// string value of the attribute
        /// </summary>
        TValue Value { get; set; }
    }

    /// <summary>
    /// object Extensions
    /// </summary>
    ////[CLSCompliant(true)]
    public static class ObjectExtensions
    {
        private static readonly ConcurrentDictionary<Type, FieldInfo[]> FieldMap;

        static ObjectExtensions()
        {
            FieldMap = new ConcurrentDictionary<Type, FieldInfo[]>();
        }

        private static FieldInfo[] GetFieldsFromType(Type type)
        {
            FieldInfo[] fields = null;
            if (FieldMap.TryGetValue(type, out fields))
                return fields;

            fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            FieldMap[type] = fields;
            return fields;
        }


        /// <summary>
        /// Gets the string value of an attribute that implements IAttributeValue
        /// Overload for int
        /// </summary>
        /// <typeparam name="TAttribute">Attribute to get the value</typeparam>
        /// <param name="item">Object containing the attribute</param>
        /// <param name="notFoundValue">Will use this string if no attribute is found</param>
        /// <returns>Value, or passed notFoundValue if not found</returns>
        public static int GetAttributeValue<TAttribute>(this object item, int notFoundValue) where TAttribute : Attribute, IAttributeValue<int>
        {
            return item.GetAttributeValue<TAttribute, int>(notFoundValue);
        }

        /// <summary>
        /// Gets the string value of an attribute that implements IAttributeValue
        /// Overload for string
        /// </summary>
        /// <typeparam name="TAttribute">Attribute to get the value</typeparam>
        /// <param name="item">Object containing the attribute</param>
        /// <param name="notFoundValue">Will use this string if no attribute is found</param>
        /// <returns>Value, or passed notFoundValue if not found</returns>
        public static string GetAttributeValue<TAttribute>(this object item, string notFoundValue) where TAttribute : Attribute, IAttributeValue<string>
        {
            return item.GetAttributeValue<TAttribute, string>(notFoundValue);
        }

        /// <summary>
        /// Gets the value of an attribute that implements IAttributeValue
        /// </summary>
        /// <typeparam name="TAttribute">Attribute to get the value</typeparam>
        /// <typeparam name="TValue">Type of the value to be returned</typeparam>
        /// <param name="item">Object containing the attribute</param>
        /// <param name="notFoundValue">Will use this string if no attribute is found</param>
        /// <returns>Value, or passed notFoundValue if not found</returns>
        public static TValue GetAttributeValue<TAttribute, TValue>(this object item, TValue notFoundValue) where TAttribute : Attribute, IAttributeValue<TValue>
        {
            TypeInfo itemType = item.GetType().GetTypeInfo();
            TValue returnValue = notFoundValue;

            foreach (object attribute in itemType.GetCustomAttributes(false))
            {
                if (attribute is TAttribute)
                {
                    returnValue = ((TAttribute)attribute).Value;
                    break;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Get list of properties decorated with the passed attribute
        /// </summary>
        /// <param name="item"></param>
        /// <param name="myAttribute"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertiesByAttribute(this object item, Type myAttribute)
        {
            TypeInfo itemType = item.GetType().GetTypeInfo();

            var returnValue = itemType.DeclaredProperties.Where(
                p => p.GetCustomAttributes(myAttribute, false).Any());

            return returnValue;
        }

        /// <summary>
        /// Safe Type Casting based on .NET default() method
        /// </summary>
        /// <typeparam name="TDestination">default(DestinationType)</typeparam>
        /// <param name="item">Item to default.</param>
        /// <returns>default(DestinationType)</returns>
        public static TDestination DefaultSafe<TDestination>(this object item)
        {
            var returnValue = TypeExtension.TypeExtensions.InvokeConstructorOrDefault<TDestination>();

            try
            {
                if (item != null)
                {
                    returnValue = (TDestination)item;
                }
            }
            catch
            {
                returnValue = TypeExtension.TypeExtensions.InvokeConstructorOrDefault<TDestination>();
            }

            return returnValue;
        }

        /// <summary>
        /// Safe type casting via (TDestination)item method.
        /// If cast fails, will attempt the slower Fill() of data via reflection
        /// </summary>
        /// <typeparam name="TDestination">Type to default, or create new()</typeparam>
        /// <param name="item">Item to cast</param>
        /// <returns>Cast result via (TDestination)item, or item.Fill(), or new TDestination().</returns>
        public static TDestination DirectCastSafe<TDestination>(this object item) where TDestination : new()
        {
            var returnValue = new TDestination();

            try
            {
                returnValue = item != null ? (TDestination)item : returnValue;
            }
            catch (InvalidCastException)
            {
                returnValue.Fill(item);
            }

            return returnValue;
        }

        /// <summary>
        /// Safe Type Casting based on TypeExtension.Default{Type} conventions.
        /// If cast fails, will attempt the slower Fill() of data via reflection
        /// </summary>
        /// <typeparam name="TDestination">Type to default, or create new()</typeparam>
        /// <param name="item">Item to cast</param>
        /// <returns>Defaulted type, or created new()</returns>
        public static TDestination DirectCastOrFill<TDestination>(this object item) where TDestination : new()
        {
            var returnValue = new TDestination();

            try
            {
                if (item != null)
                {
                    returnValue = (TDestination)item;
                }
            }
            catch
            {
                returnValue = new TDestination();
            }

            return returnValue;
        }

        /// <summary>
        /// Item to exception-safe cast to string
        /// </summary>
        /// <param name="item">Item to cast</param>
        /// <returns>Converted string, or ""</returns>
        public static string ToStringSafe(this object item)
        {
            var returnValue = TypeExtension.TypeExtensions.DefaultString;

            if (item == null == false)
            {
                returnValue = item.ToString();
            }

            return returnValue;
        }


        /// <summary>
        /// Fills this object with another object's data, by matching property names
        /// </summary>
        /// <typeparam name="T">Type of original object.</typeparam>
        /// <param name="item">Destination object to fill</param>
        /// <param name="sourceItem">Source object</param>
        public static void Fill<T>(this T item, object sourceItem)
        {
            var sourceType = sourceItem.GetType();

            foreach (PropertyInfo sourceProperty in sourceType.GetRuntimeProperties())
            {
                PropertyInfo destinationProperty = typeof(T).GetRuntimeProperty(sourceProperty.Name);
                if (destinationProperty != null && destinationProperty.CanWrite)
                {
                    // Copy data only for Primitive-ish types including Value types, Guid, String, etc.
                    Type destinationPropertyType = destinationProperty.PropertyType;
                    if (destinationPropertyType.GetTypeInfo().IsPrimitive || destinationPropertyType.GetTypeInfo().IsValueType
                        || (destinationPropertyType == typeof(string)) || (destinationPropertyType == typeof(Guid)))
                    {
                        destinationProperty.SetValue(item, sourceProperty.GetValue(sourceItem, null), null);
                    }
                }
            }
        }

        /// <summary>
        /// Initialize all Root properties of an object to TypeExtension.Default* conventions
        /// </summary>
        /// <typeparam name="TObjectType">Type of object to initialize</typeparam>
        /// <param name="item">Item to initialize</param>
        /// <returns>Initialized object to TypeExtension.Default* conventions</returns>
        public static TObjectType Initialize<TObjectType>(this Object item)
        {
            // Initialize
            var currentObjectType = item.GetType();

            // Loop through all new item's properties
            foreach (var currentProperty in currentObjectType.GetRuntimeProperties())
            {
                // Copy the data using reflection
                if (currentProperty.CanWrite)
                {
                    if (currentProperty.PropertyType == typeof(Int32) || currentProperty.PropertyType == typeof(int) || currentProperty.PropertyType == typeof(int?) || currentProperty.PropertyType == typeof(int?))
                    {
                        currentProperty.SetValue(item, TypeExtension.TypeExtensions.DefaultInt32, null);
                    }
                    else if (currentProperty.PropertyType == typeof(Int64) || currentProperty.PropertyType == typeof(long?) || currentProperty.PropertyType == typeof(long) || currentProperty.PropertyType == typeof(long?))
                    {
                        currentProperty.SetValue(item, TypeExtension.TypeExtensions.DefaultDouble, null);
                    }
                    else if ( currentProperty.PropertyType == typeof(double) || currentProperty.PropertyType == typeof(double?) )
                    {
                        currentProperty.SetValue(item, TypeExtension.TypeExtensions.DefaultDouble, null);
                    }
                    else if (currentProperty.PropertyType == typeof(decimal) || currentProperty.PropertyType == typeof(decimal?) )
                    {
                        currentProperty.SetValue(item, TypeExtension.TypeExtensions.DefaultDecimal, null);
                    }
                    else if ( currentProperty.PropertyType == typeof(string) ) 
                    {
                        currentProperty.SetValue(item, TypeExtension.TypeExtensions.DefaultString, null);
                    }
                    else if (currentProperty.PropertyType == typeof(Char) ||  currentProperty.PropertyType == typeof(char?) || currentProperty.PropertyType == typeof(char?))
                    {
                        currentProperty.SetValue(item, TypeExtension.TypeExtensions.DefaultChar, null);
                    }
                    else if (currentProperty.PropertyType == typeof(Guid) || currentProperty.PropertyType == typeof(Guid?))
                    {
                        currentProperty.SetValue(item, TypeExtension.TypeExtensions.DefaultGuid, null);
                    }
                    else if (currentProperty.PropertyType == typeof(Boolean) || currentProperty.PropertyType == typeof(bool) || currentProperty.PropertyType == typeof(bool?) || currentProperty.PropertyType == typeof(bool?))
                    {
                        currentProperty.SetValue(item, TypeExtension.TypeExtensions.DefaultBoolean, null);
                    }
                    else if (currentProperty.PropertyType == typeof(DateTime) || currentProperty.PropertyType == typeof(DateTime?))
                    {
                        currentProperty.SetValue(item, TypeExtension.TypeExtensions.DefaultDate, null);
                    }
                    else if (currentProperty.PropertyType == typeof(TimeSpan) || currentProperty.PropertyType == typeof(TimeSpan?))
                    {
                        currentProperty.SetValue(item, TypeExtensions.DefaultDate, null);
                    }
                    else if (currentProperty.GetValue(item, null) == null)
                    {
                        Type propType = currentProperty.PropertyType;
                        object newProp = Activator.CreateInstance(propType);
                        currentProperty.SetValue(item, newProp, null);
                    }
                }
            }

            return (TObjectType)item;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MyUtils.Extensions.IEnumerableExtension
{
    public static class IEnumerableExtensions
    {
        
        /// <summary>
        /// Indicates whether the specified collection is null or an empty collection.
        /// </summary>
        /// <param name="collection">The collection to test.</param>
        /// <returns>true if the collection is null or an empty collection; otherwise, false.</returns>
        public static bool IsNullOrEmpty(this IEnumerable collection)
        {
            if (collection == null)
            {
                return true;
            }
            return !collection.GetEnumerator().MoveNext();
        }
        /// <summary>
        /// Indicates whether the specified collection is null or an empty collection.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="collection">The collection to test.</param>
        /// <returns>true if the collection is null or an empty collection; otherwise, false.</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }

        /// <summary>
        /// Get an empty collection If <paramref name="collection"/> is null.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="collection">The collection to test.</param>
        /// <returns>The <paramref name="collection"/>,if <paramref name="collection"/>is null return an empty collection.</returns>
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> collection)
        {
            return collection ?? Enumerable.Empty<T>();
        }

        ///// <summary>
        ///// Convert the <see cref="IEnumerable{T}"/> to a DataTable.
        ///// </summary>
        ///// <typeparam name="T">The type of elements in the list.</typeparam>
        ///// <param name="collection">The collection to convert.</param>
        ///// <exception cref="ArgumentNullException"></exception>
        ///// <returns>The converted datatable.</returns>
        //public static DataTable ToDataTable<T>(this IEnumerable<T> collection)
        //{
        //    List<string> propertyNameList = new List<string>();
        //    DataTable dataTable = new DataTable("DataTable");
        //    if (collection.IsNullOrEmpty())
        //    {
        //        throw new ArgumentNullException("collection", "The collection to convert can not be null or empty.");
        //    }
        //    PropertyInfo[] propertys = collection.FirstOrDefault().GetType().GetProperties();
        //    foreach (PropertyInfo pi in propertys)
        //    {
        //        dataTable.Columns.Add(pi.Name, pi.PropertyType);
        //    }
        //    for (int i = 0; i < collection.Count(); i++)
        //    {
        //        ArrayList arrayList = new ArrayList();
        //        foreach (PropertyInfo pi in propertys)
        //        {
        //            if (propertyNameList.Count == 0)
        //            {
        //                object obj = pi.GetValue(collection.ToList()[i], null);
        //                arrayList.Add(obj);
        //            }
        //            else
        //            {
        //                if (propertyNameList.Contains(pi.Name))
        //                {
        //                    object obj = pi.GetValue(collection.ToList()[i], null);
        //                    arrayList.Add(obj);
        //                }
        //            }
        //        }
        //        object[] array = arrayList.ToArray();
        //        dataTable.LoadDataRow(array, true);
        //    }
        //    return dataTable;
        //}
    }
}

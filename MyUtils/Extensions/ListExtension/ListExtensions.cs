using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyUtils.Extensions.ObjectExtension;

namespace MyUtils.Extensions.ListExtension
{
    /// <summary>
    ///  /// List Extension
    /// </summary>
    ////[CLSCompliant(true)]
    public static class ListExtensions
    {
      
            /// <summary>
            /// Returns first item in a list, or empty constructed class
            /// </summary>
            /// <typeparam name="T">Type of the generic list</typeparam>
            /// <param name="item">List to get first item</param>
            /// <returns>First item, or new() constructed item</returns>
            public static T FirstOrDefaultSafe<T>(this List<T> item) where T : new()
            {
                return (item != null && item.FirstOrDefault() != null) ? item.FirstOrDefault() : new T();
            }

            /// <summary>
            /// Returns last item in a list, or empty constructed class
            /// </summary>
            /// <typeparam name="T">Type of the generic list</typeparam>
            /// <param name="item">List to get first item</param>
            /// <param name="defaultValue">Will return defaultValue if no items in collection</param>
            /// <returns>First item, or new() constructed item</returns>
            public static T FirstOrDefaultSafe<T>(this List<T> item, T defaultValue)
            {
                return (item != null && item.FirstOrDefault() != null) ? item.FirstOrDefault() : defaultValue;
            }

            /// <summary>
            /// Returns last item in a list, or empty constructed class
            /// </summary>
            /// <typeparam name="T">Type of the generic list</typeparam>
            /// <param name="item">List to get first item</param>
            /// <returns>First item, or new() constructed item</returns>
            public static T LastOrDefaultSafe<T>(this List<T> item) where T : new()
            {
                return (item != null && item.LastOrDefault() != null) ? item.LastOrDefault() : new T();
            }

            /// <summary>
            /// Returns first found item in a list, or empty constructed class.
            /// Exception-safe.
            /// </summary>
            /// <typeparam name="T">Type of generic list.</typeparam>
            /// <param name="item">Item to search.</param>
            /// <param name="index">Index position to search</param>
            /// <returns>Found item or constructed equivalent.</returns>
            public static T Item<T>(this List<T> item, int index) where T : new()
            {
                return item[index].DirectCastSafe<T>();
            }

            /// <summary>
            /// Exception safe Find()
            /// </summary>
            /// <typeparam name="T">Generic type of list</typeparam>
            /// <param name="item">Item to search.</param>
            /// <param name="query">Predicate query to search for data</param>
            /// <returns>Found item in list based on predicate</returns>
            public static T FindSafe<T>(this List<T> item, Predicate<T> query) where T : new()
            {
                return item.Find(query).DirectCastSafe<T>();
            }

            /// <summary>
            /// Adds list to current list
            /// </summary>
            /// <typeparam name="T">Type of lists</typeparam>
            /// <param name="item">Destination list</param>
            /// <param name="itemsToAdd">Source list</param>
            public static void AddRange<T>(this List<T> item, List<T> itemsToAdd)
            {
                foreach (T itemToAdd in itemsToAdd)
                {
                    item.Add(itemToAdd);
                }
            }

            /// <summary>
            /// Returns type of Generic.List
            /// </summary>
            /// <typeparam name="T">Type of list</typeparam>
            /// <param name="_">Item to determine type</param>
            /// <returns>Type of generic list</returns>
            public static Type GetListType<T>(this List<T> _)
            {
                return typeof(T);
            }

            /// <summary>
            /// Returns type of IEnumerable
            /// </summary>
            /// <typeparam name="T">Type of list</typeparam>
            /// <param name="_">Item to determine type</param>
            /// <returns>Type of generic list</returns>>
            public static Type GetEnumerableType<T>(this IEnumerable<T> _)
            {
                return typeof(T);
            }

            /// <summary>
            /// Fills this IEnumerable list with another IEnumerable list that has types with matching properties.
            /// </summary>
            /// <typeparam name="T">Type of original object.</typeparam>
            /// <param name="item">Destination object to fill</param>
            /// <param name="sourceList">Source object</param>
            public static void FillRange<T>(this List<T> item, IEnumerable sourceList) where T : new()
            {
                foreach (var sourceItem in sourceList)
                {
                    var newItem = new T();
                    newItem.Fill(sourceItem);
                    item.Add(newItem);
                }
            }
        

        /////// <summary>
        /////// Moves to.
        /////// </summary>
        /////// <typeparam name="T"></typeparam>
        /////// <param name="self">The self.</param>
        /////// <param name="destinationDirectory">The destination directory.</param>
        ////public static void MoveTo<T>(this List<T> self, string destinationDirectory)
        ////    where T : MediaFile
        ////{
        ////    var type = typeof(T);
        ////    var pi = type.GetTypeInfo().GetProperty("Source");
        ////    BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
        ////    var pi2 = typeof(MediaFile).GetTypeInfo().GetProperty("_Source", flag);
        ////    foreach (var x in self)
        ////    {
        ////        var src = (string)pi.GetValue(x);
        ////        var fname = destinationDirectory + System.IO.Path.GetFileName(src);
        ////        System.IO.File.Copy(src, fname);
        ////        System.IO.File.Delete(src);
        ////        pi2.SetValue(x, fname);
        ////    };
        ////}


        /////// <summary>
        /////// Copies to be.
        /////// </summary>
        /////// <typeparam name="T"></typeparam>
        /////// <param name="self">The self.</param>
        /////// <param name="destinationDirectory">The destination directory.</param>
        ////public static void CopyToBe<T>(this List<T> self, string destinationDirectory)
        ////    where T : MediaFile
        ////{
        ////    var type = typeof(T);
        ////    var pi = type.GetTypeInfo().GetProperty("Source");
        ////    BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
        ////    var pi2 = typeof(MediaFile).GetTypeInfo().GetProperty("_Source", flag);
        ////    foreach (var x in self)
        ////    {
        ////        var src = (string)pi.GetValue(x);
        ////        var fname = destinationDirectory + System.IO.Path.GetFileName(src);
        ////        System.IO.File.Copy(src, fname);
        ////        pi2.SetValue(x, fname);
        ////    }
        ////}


        /////// <summary>
        /////// Copies to.
        /////// </summary>
        /////// <typeparam name="T"></typeparam>
        /////// <param name="self">The self.</param>
        /////// <param name="destinationDirectory">The destination directory.</param>
        ////public static void CopyTo<T>(this List<T> self, string destinationDirectory)
        ////    where T : MediaFile
        ////{
        ////    var type = typeof(T);
        ////    var pi = type.GetTypeInfo().GetProperty("Source");
        ////    BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
        ////    var pi2 = typeof(MediaFile).GetTypeInfo().GetProperty("_Source", flag);
        ////    foreach (var x in self)
        ////    {
        ////        var src = (string)pi.GetValue(x);
        ////        var fname = destinationDirectory + System.IO.Path.GetFileName(src);
        ////        System.IO.File.Copy(src, fname);
        ////    }
        ////}
    }
}

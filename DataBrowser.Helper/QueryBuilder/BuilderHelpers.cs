using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Helper.QueryBuilder
{
    public static class BuilderHelpers
    {
        public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> sourceDictionary, Dictionary<TKey, TValue> collection)
        {
            foreach (var item in collection)
            {
                try
                {
                    sourceDictionary.Add(item.Key, item.Value);
                }
                catch (ArgumentException)
                {
                    throw new ArgumentException("The key already exists", item.Key.ToString());
                }
            }
        }
    }
}

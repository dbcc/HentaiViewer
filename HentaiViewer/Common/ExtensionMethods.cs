using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace HentaiViewer.Common {
    public static class ExtensionMethods {
        public static int Remove<T>(
            this ObservableCollection<T> coll, Func<T, bool> condition) {
            var itemsToRemove = coll.Where(condition).ToList();

            foreach (var itemToRemove in itemsToRemove) {
                coll.Remove(itemToRemove);
            }

            return itemsToRemove.Count;
        }

        public static void RemoveAll<T>(this ObservableCollection<T> collection,
            Func<T, bool> condition) {
            for (var i = collection.Count - 1; i >= 0; i--) {
                if (condition(collection[i])) {
                    collection.RemoveAt(i);
                }
            }
        }
    }
}
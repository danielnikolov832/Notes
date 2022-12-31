using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BindingIdeasLib;

internal static class ObservableCollectionStaticLib
{
    // Note : The CollectionChanged event this is subscribed to must be of a collection of type TOtherCollectionT
    public static void UpdateCollectionWhenOtherCollectionChanges<TOtherCollectionT, TThisCollectionT> (
        this ObservableCollection<TThisCollectionT> collectionToUpdate,
        Func<TOtherCollectionT, TThisCollectionT> conversionFunc,
        NotifyCollectionChangedEventArgs e,
        Func<TThisCollectionT, TOtherCollectionT, TThisCollectionT>? customReplaceAction = null)
    {
        if (e.Action == NotifyCollectionChangedAction.Reset)
        {
            collectionToUpdate.Clear();
        }
        else if (e.Action == NotifyCollectionChangedAction.Add)
        {
            TOtherCollectionT addedItem = (TOtherCollectionT)e.NewItems![ 0 ]!;

            collectionToUpdate.Insert(e.NewStartingIndex, conversionFunc(addedItem));
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            collectionToUpdate.RemoveAt(e.OldStartingIndex);
        }
        else if (e.Action == NotifyCollectionChangedAction.Replace)
        {
            ReplaceAction(collectionToUpdate, conversionFunc, e, customReplaceAction);
        }
        else
        {
            collectionToUpdate.Move(e.OldStartingIndex, e.NewStartingIndex);
        }
    }

    internal static void ReplaceAction<TOtherCollectionT, TThisCollectionT>(
        ObservableCollection<TThisCollectionT> collectionToUpdate,
        Func<TOtherCollectionT, TThisCollectionT> conversionFunc,
        NotifyCollectionChangedEventArgs e,
        Func<TThisCollectionT, TOtherCollectionT, TThisCollectionT>? customReplaceAction)
    {
        ReplaceAction(collectionToUpdate, conversionFunc, (TOtherCollectionT)e.NewItems![0]!, e.NewStartingIndex, customReplaceAction);
    }

    internal static void ReplaceAction<TOtherCollectionT, TThisCollectionT>(
        ObservableCollection<TThisCollectionT> collectionToUpdate,
        Func<TOtherCollectionT, TThisCollectionT> conversionFunc,
        TOtherCollectionT itemToReplaceWith,
        int index,
        Func<TThisCollectionT, TOtherCollectionT, TThisCollectionT>? customReplaceAction)
    {
        if (customReplaceAction is not null)
        {
            collectionToUpdate[index] = customReplaceAction.Invoke(collectionToUpdate[index], itemToReplaceWith);

            return;
        }

        collectionToUpdate[index] = conversionFunc(itemToReplaceWith);
    }
}

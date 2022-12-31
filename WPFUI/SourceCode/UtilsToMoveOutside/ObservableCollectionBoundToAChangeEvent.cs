using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace BindingIdeasLib;

public sealed class ObservableCollectionBoundToAChangeEvent<TOtherCollection, TThisCollection> : IReadOnlyList<TThisCollection>, INotifyCollectionChanged
{
    public ObservableCollectionBoundToAChangeEvent (
        Func<TOtherCollection, TThisCollection> conversionFunc,
        Func<TThisCollection, TOtherCollection, TThisCollection>? customReplaceAction = null)
    {
        _conversionFunc = conversionFunc;
        _customReplaceAction = customReplaceAction;
    }

    private readonly Func<TOtherCollection, TThisCollection> _conversionFunc;
    private readonly Func<TThisCollection, TOtherCollection, TThisCollection>? _customReplaceAction;

    private readonly ObservableCollection<TThisCollection> _underlyingObservableCollection = new();

    public void OnOtherCollectionCollectionChanged (object? sender, NotifyCollectionChangedEventArgs e)
    {
        _underlyingObservableCollection.UpdateCollectionWhenOtherCollectionChanges(_conversionFunc, e, _customReplaceAction);
    }

    #region IReadOnlyList members

    public int Count => _underlyingObservableCollection.Count;

    public TThisCollection this[ int index ] => _underlyingObservableCollection[ index ];

    public IEnumerator<TThisCollection> GetEnumerator ()
    {
        return _underlyingObservableCollection.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator ()
    {
        return _underlyingObservableCollection.GetEnumerator();
    }

    #endregion IReadOnlyList members

    #region INotifyCollectionChanged Members

    public event NotifyCollectionChangedEventHandler? CollectionChanged
    {
        add
        {
            ((INotifyCollectionChanged)_underlyingObservableCollection).CollectionChanged += value;
        }

        remove
        {
            ((INotifyCollectionChanged)_underlyingObservableCollection).CollectionChanged -= value;
        }
    }

    #endregion INotifyCollectionChanged Members
}
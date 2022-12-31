using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace BindingIdeasLib;

public sealed class ObservableCollectionBoundToAnotherObservableCollection<TOtherCollection, TThisCollection> : IReadOnlyList<TThisCollection>, INotifyCollectionChanged
{
    public ObservableCollectionBoundToAnotherObservableCollection (
       ObservableCollection<TOtherCollection>? observableCollectionToBindTo,
       Func<TOtherCollection, TThisCollection> conversionFunc,
       Func<TThisCollection, TOtherCollection, TThisCollection>? customReplaceAction = null)
    {
        getset_observableCollectionToBindTo = observableCollectionToBindTo;

        _conversionFunc = conversionFunc;

        _customReplaceAction = customReplaceAction;
    }

    private readonly Func<TOtherCollection, TThisCollection> _conversionFunc;
    private readonly Func<TThisCollection, TOtherCollection, TThisCollection>? _customReplaceAction;

    private ObservableCollection<TOtherCollection>? _observableCollectionToBindTo;

    private readonly ObservableCollection<TThisCollection> _underlyingObservableCollection = new();

    public ObservableCollection<TOtherCollection>? getset_observableCollectionToBindTo
    {
        get => _observableCollectionToBindTo;
        set
        {
            if (_observableCollectionToBindTo != null)
            {
                UnbindFromCollection(_observableCollectionToBindTo);
            }

            _observableCollectionToBindTo = value;

            if (_observableCollectionToBindTo != null)
            {
                BindToCollection(_observableCollectionToBindTo);
            }
        }
    }

    private void BindToCollection (ObservableCollection<TOtherCollection> observableCollectionToBindTo)
    {
        foreach (TOtherCollection otherCollectionItem in observableCollectionToBindTo)
        {
            _underlyingObservableCollection.Add(_conversionFunc(otherCollectionItem));
        }

        observableCollectionToBindTo.CollectionChanged += OnOtherCollectionCollectionChanged;
    }

    private void OnOtherCollectionCollectionChanged (object? sender, NotifyCollectionChangedEventArgs e)
    {
        _underlyingObservableCollection.UpdateCollectionWhenOtherCollectionChanges(_conversionFunc, e, _customReplaceAction);
    }

    private void UnbindFromCollection (ObservableCollection<TOtherCollection> collectionToUnbindFrom)
    {
        _underlyingObservableCollection.Clear();
        collectionToUnbindFrom.CollectionChanged -= OnOtherCollectionCollectionChanged;
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
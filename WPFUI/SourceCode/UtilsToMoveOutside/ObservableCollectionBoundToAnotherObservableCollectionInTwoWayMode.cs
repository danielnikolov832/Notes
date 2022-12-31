using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BindingIdeasLib;

// A class that can be bound to an ObservableCollection'1, and derivative getset_types, to allow automatic changes to one collection,
// when the other changes. Useful for data migrations (from Business to UI displayable objects and such), and is a small eventing 
// system of its own.
// A bit messy implementation, but the idea is for it to be easily usable.
public sealed class ObservableCollectionBoundToAnotherObservableCollectionInTwoWayMode<TOtherCollection, TThisCollection> : ObservableCollection<TThisCollection>
{
    public ObservableCollectionBoundToAnotherObservableCollectionInTwoWayMode(
        ObservableCollection<TOtherCollection>? observableCollectionToBindTo,
        Func<TOtherCollection, TThisCollection> conversionFunc,
        Func<TThisCollection, TOtherCollection> backwardsConversionFunc,
        Func<TThisCollection, TOtherCollection, TThisCollection>? customReplaceAction = null,
        Func<TOtherCollection, TThisCollection, TOtherCollection>? backwardsCustomReplaceAction = null)
    {
        _observableCollectionToBindTo = observableCollectionToBindTo;

        _conversionFunc = conversionFunc;
        _backwardsConversionFunc = backwardsConversionFunc;

        _customReplaceAction = customReplaceAction;
        _backwardsCustomReplaceAction = backwardsCustomReplaceAction;

        CollectionChanged += This_CollectionChanged;

        if (observableCollectionToBindTo is not null)
        {
            BindToCollection(observableCollectionToBindTo);
        }
    }

    private void This_CollectionChanged(object? @this, NotifyCollectionChangedEventArgs e)
    {
        OnThisCollectionChanged(@this, e);
    }

    private readonly Func<TOtherCollection, TThisCollection> _conversionFunc;
    private readonly Func<TThisCollection, TOtherCollection> _backwardsConversionFunc;

    private readonly Func<TThisCollection, TOtherCollection, TThisCollection>? _customReplaceAction;
    private readonly Func<TOtherCollection, TThisCollection, TOtherCollection>? _backwardsCustomReplaceAction;

    private ObservableCollection<TOtherCollection>? _observableCollectionToBindTo;
    public ObservableCollection<TOtherCollection>? getset_observableCollectionToBindTo
    {
        get => _observableCollectionToBindTo;
        set
        {
            if (_observableCollectionToBindTo is not null)
            {
                UnbindFromCollection(_observableCollectionToBindTo);
            }

            _observableCollectionToBindTo = value;

            if (_observableCollectionToBindTo is not null)
            {
                BindToCollection(_observableCollectionToBindTo);
            }
        }
    }

    private bool _isUnnecessaryForThisCollectionToChange = false;
    private bool _isUnnecessaryForOtherCollectionToChange = false;

    private void BindToCollection(ObservableCollection<TOtherCollection> observableCollectionToBindTo)
    {
        _isUnnecessaryForOtherCollectionToChange = true;

        foreach (TOtherCollection otherCollectionItem in observableCollectionToBindTo)
        {
            Add(_conversionFunc(otherCollectionItem));
        }

        observableCollectionToBindTo.CollectionChanged += OnOtherCollectionChanged;

        _isUnnecessaryForOtherCollectionToChange = false;
    }

    private void OnOtherCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (_isUnnecessaryForThisCollectionToChange) return;

        _isUnnecessaryForOtherCollectionToChange = true;

        OnCollectionChanged_UpdateOnOrTheOtherCollection(e, this,
            _conversionFunc, _customReplaceAction);

        _isUnnecessaryForOtherCollectionToChange = false;
    }

    private void OnThisCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (_isUnnecessaryForOtherCollectionToChange || _observableCollectionToBindTo is null) return;

        _isUnnecessaryForThisCollectionToChange = true;

        OnCollectionChanged_UpdateOnOrTheOtherCollection(e, _observableCollectionToBindTo,
            _backwardsConversionFunc, _backwardsCustomReplaceAction);

        _isUnnecessaryForThisCollectionToChange = false;
    }

    private static void OnCollectionChanged_UpdateOnOrTheOtherCollection<TToUpdate, TToGetFrom>(
        NotifyCollectionChangedEventArgs e,
        ObservableCollection<TToUpdate> observableCollectionToUpdate,
        Func<TToGetFrom, TToUpdate> conversionFuncForThisScenario,
        Func<TToUpdate, TToGetFrom, TToUpdate>? customReplaceFuncForThisScenario)
    {
        observableCollectionToUpdate.UpdateCollectionWhenOtherCollectionChanges(conversionFuncForThisScenario, e, customReplaceFuncForThisScenario);
    }

    private void UnbindFromCollection(ObservableCollection<TOtherCollection> collectionToUnbindFrom)
    {
        _isUnnecessaryForOtherCollectionToChange = true;

        collectionToUnbindFrom.CollectionChanged -= OnOtherCollectionChanged;

        Clear();
    }

    public void Replace(TOtherCollection itemToReplaceWith, int index)
    {
        ObservableCollectionStaticLib.ReplaceAction(this, _conversionFunc, itemToReplaceWith, index, _customReplaceAction);
    }
}
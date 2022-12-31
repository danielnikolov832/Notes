using BindingIdeasLib;
using BusinessLogicLibrary;
using BusinessLogicLibrary.Models;
using BusinessLogicLibrary.Services.NoteTypeGenerationService;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CustomMediatorForTalksBetweenVMsExp;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using System;
using WPFUI.Constants;
using WPFUI.Mediators;
using WPFUI.Mediators.Publications;
using WPFUI.VVM.NoteTreeView;

namespace WPFUI.VVM.NoteTypeCreatePopup;

public sealed class NoteTypeCreatePopupViewModel : ObservableObject
{
    public NoteTypeCreatePopupViewModel()
    {
        get_itemsSourceForPropsListView = new(
            observableCollectionToBindTo: _currentNote?.get_properties,
            conversionFunc: (valueContainer) => new(valueContainer),
            customReplaceAction: (oldItem, _) => oldItem);

        get_itemsSourceForNoteTreeView = new(
            observableCollectionToBindTo: _currentNote?.get_children,
            conversionFunc: (noteWrapper) => new(noteWrapper.getset_note));

        _selectedNoteChangedSubscriber = new(this, StaticMediators.get_currentPublisherForVMs);
    }

    private string _selectedTypeName = string.Empty;

    public string getset_selectedTypeName
    {
        get => _selectedTypeName;
        set
        {
            _selectedTypeName = value;
            OnPropertyChanged();
        }
    }

    private NoteWrapperWithSelectionData? _currentNote;

    public NoteWrapperWithSelectionData? getset_currentNote
    {
        get => _currentNote;
        set
        {
            _currentNote = value;

            get_itemsSourceForPropsListView.getset_observableCollectionToBindTo = _currentNote?.get_properties;
            get_itemsSourceForNoteTreeView.getset_observableCollectionToBindTo
                = _currentNote?.get_children;

            OnPropertyChanged();
        }
    }

    private NoteWrapperWithTypeCreationData? _selectedNoteInNoteTreeView;

    public NoteWrapperWithTypeCreationData? getset_selectedNoteInNoteTreeView
    {
        get => _selectedNoteInNoteTreeView;
        set
        {
            _selectedNoteInNoteTreeView = value;

            OnPropertyChanged();
        }
    }

    private ValueContainerWrapperWithEnumData? _selectedValueContainerInSideListView;

    public ValueContainerWrapperWithEnumData? getset_selectedValueContainerInSideListView
    {
        get => _selectedValueContainerInSideListView;
        set
        {
            _selectedValueContainerInSideListView = value;

            OnPropertyChanged();
        }
    }

    public ObservableCollectionBoundToAnotherObservableCollection<IValueContainerNonGeneric, ValueContainerWrapperWithEnumData> get_itemsSourceForPropsListView { get; init; }
    public ObservableCollectionBoundToAnotherObservableCollection<NoteWrapperWithSelectionData, NoteWrapperWithTypeCreationData> get_itemsSourceForNoteTreeView { get; init; }

    #region Commands

    private RelayCommand? _createTypeFromNoteCommand;
    private RelayCommand? _addNewItemToSideListCommand;
    private RelayCommand? _removeSelectedItemFromSideListCommand;

    public ICommand get_createTypeFromNoteCommand =>
    _createTypeFromNoteCommand ??= new RelayCommand(
        () =>
        {
            if (getset_selectedTypeName.Length == 0) return;

            NoteWrapperWithTypeCreationData dummyWrapper = new(new(string.Empty));

            foreach (ValueContainerWrapperWithEnumData valueWrapper in get_itemsSourceForPropsListView)
            {
                dummyWrapper.get_unboundPropertiesForTypeCreation.Add(valueWrapper);
            }

            foreach (NoteWrapperWithTypeCreationData wrapper in get_itemsSourceForNoteTreeView)
            {
                dummyWrapper.get_unboundChildrenForTypeCreation.Add(wrapper);
            }

            Note newNoteForType = RecursiveCreateNoteWithSelectedPropsAndChildrenOfWrapper(dummyWrapper);

            NoteTypeDataHolder.get_instance.TryAddNoteType(_selectedTypeName, newNoteForType);

            static Note RecursiveCreateNoteWithSelectedPropsAndChildrenOfWrapper(NoteWrapperWithTypeCreationData wrapper)
            {
                Note output = new(wrapper.getset_nameWrapperProperty);

                foreach(ValueContainerWrapperWithEnumData valueWrapper in wrapper.get_unboundPropertiesForTypeCreation)
                {
                    if (!valueWrapper.getset_isSelected) continue;

                    AcceptableDataEnum dataTypeEnum = valueWrapper.getset_enumData.get_dataEnum;

                    output.get_properties.Add(dataTypeEnum.CreateValueContainerWithDefaultValue(valueWrapper.getset_nameOfValueContainer));
                }

                foreach (NoteWrapperWithTypeCreationData item in wrapper.get_unboundChildrenForTypeCreation)
                {
                    if (item.get_isSelectedAsPartOfType)
                    {
                        output.get_children.Add(RecursiveCreateNoteWithSelectedPropsAndChildrenOfWrapper(item));
                    }
                }

                return output;
            }
        }
    );

    public static void RemoveAll<T>(IList<T> list, Predicate<T> predicate)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T item = list[i];

            if (predicate(item))
            {
                list.RemoveAt(i);
                i--;
            }
        }
    }

    public ICommand get_addNewItemToSideListCommand =>
    _addNewItemToSideListCommand ??= new RelayCommand(
        () =>
        {
            ValueContainerWrapperWithEnumData wrapperWithDefaultValues
                = new(TextConstants.defaultPropertyCreatedName, AcceptableDataEnum.Text);

            getset_selectedNoteInNoteTreeView!.get_unboundPropertiesForTypeCreation.Add(wrapperWithDefaultValues);
        },

        () => getset_selectedNoteInNoteTreeView is not null
    );

    public ICommand get_removeSelectedItemFromSideListCommand =>
    _removeSelectedItemFromSideListCommand ??= new RelayCommand(
        () =>
        {
            getset_selectedNoteInNoteTreeView!.get_unboundPropertiesForTypeCreation.Remove(_selectedValueContainerInSideListView!);
        },

        () => _selectedValueContainerInSideListView is not null
    );

    #endregion Commands

    #region Publisher setup

    private readonly SelectedNoteChangedSubscriber _selectedNoteChangedSubscriber;

    private sealed class SelectedNoteChangedSubscriber : SubscriberForVMBase<SelectedNoteChangedPublication, NoteTypeCreatePopupViewModel>
    {
        public SelectedNoteChangedSubscriber(NoteTypeCreatePopupViewModel viewModel, Publisher publisher) : base(viewModel, publisher)
        {
        }

        public override Task Handle(SelectedNoteChangedPublication request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return Task.FromCanceled(cancellationToken);

            if (!get_viewModelExists) return Task.CompletedTask;

            get_viewModelIfExists!.getset_currentNote = request.get_parameterObject;

            return Task.CompletedTask;
        }
    }

    #endregion Publisher setup
}
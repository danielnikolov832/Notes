using BindingIdeasLib;
using BusinessLogicLibrary;
using BusinessLogicLibrary.Models;
using BusinessLogicLibrary.Services.NoteTypeGenerationService;
using BusinessLogicLibrary.Services.NoteTypeGenerationService.Models;
using System.Collections.Generic;
using System.Windows.Input;
using WPFUI.Constants;
using WPFUI.VVM.MainWindow;
using WPFUI.Mediators;
using WPFUI.Mediators.Requests;
using System.Threading.Tasks;
using CustomMediatorForTalksBetweenVMsExp;
using System.Threading;
using WPFUI.Mediators.Publications;
using WPFUI.VVM.Core;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WPFUI.VVM.PropertyListView;

public sealed class PropertyListViewModel : ObservableObject
{
    public PropertyListViewModel()
    {
        _selectedNoteChangeSubscriber = new(this, StaticMediators.get_currentPublisherForVMs);
    }

    public List<AcceptableDataEnumDataCollection.IAcceptableDataValuesHolder_NonGeneric> get_listOfAllAcceptableValuesForDataType { get; init; }
        = new(AcceptableDataEnumDataCollection.GetAllAcceptableDataEnumValuesHolders());

    private PropertyListItem? _selectedItemInPropertyListView;

    public PropertyListItem? getset_selectedItemInPropertyListView
    {
        get => _selectedItemInPropertyListView;
        set
        {
            OnPropertyChanging();

            _selectedItemInPropertyListView = value;

            OnPropertyChanged();
        }
    }

    public ObservableCollectionBoundToAnotherObservableCollectionInTwoWayMode<IValueContainerNonGeneric, PropertyListItem> get_itemsSourceForListView { get; init; }
        = InitItemsSourceForListView();

    private static ObservableCollectionBoundToAnotherObservableCollectionInTwoWayMode<IValueContainerNonGeneric, PropertyListItem> InitItemsSourceForListView()
    {
        return new(
        observableCollectionToBindTo: null,
        conversionFunc: MakeWrapperForContainer,
        backwardsConversionFunc: GetContainerInWrapper,
        customReplaceAction: CustomReplace);

        static PropertyListItem MakeWrapperForContainer(IValueContainerNonGeneric valueContainer)
        {
            return new(valueContainer);
        }

        static IValueContainerNonGeneric GetContainerInWrapper(PropertyListItem item)
        {
            return item.get_valueContainerWrapper.get_valueContainerNonGeneric;
        }

        static PropertyListItem CustomReplace(PropertyListItem item, IValueContainerNonGeneric itemToReplaceWith)
        {
            item.MakeANewValueContainerWrapper(itemToReplaceWith);

            return item;
        }
    }

#pragma warning disable CA1822 // Mark members as static
    // reason : it is used in the view model, and UI needs to bind to its instance
    //public IReadOnlyList<NoteType> get_noteTypeComboboxItemsSource => NoteTypeDataHolder.get_instance.GetRootTypesList();
#pragma warning restore CA1822 // Mark members as static

    private NoteType? _selectedItemInTypeComboBox;
    public NoteType? get_selectedItemInTypeComboBox
    {
        get => _selectedItemInTypeComboBox;
        set
        {
            _selectedItemInTypeComboBox = value;
            OnPropertyChanged();
        }
    }

    private string? _noteTypeName;
    public string? getset_noteTypeName
    {
        get => _noteTypeName;
        set
        {
            _noteTypeName = value;
            OnPropertyChanged();
        }
    }

    private static INoteWrapperBase? GetSelectedNoteFromMediator()
    {
         return StaticMediators.get_currentMediatorForVMs.Send<INoteWrapperBase?, GetSelectedNoteRequest>(new GetSelectedNoteRequest());
    }

    #region Commands

    private RelayCommand? _switchValueContainerWrapperToAWrapperForNeededTypeCommand;
    private RelayCommand? _addNewPropertyToNoteCommand;
    private RelayCommand? _switchNoteToGivenTypeCommand;

    public ICommand get_switchValueContainerWrapperToAWrapperForNeededTypeCommand =>
    _switchValueContainerWrapperToAWrapperForNeededTypeCommand ??= new RelayCommand
        (
            () =>
            {
                AcceptableDataEnum enumForNeededContainer
                    = _selectedItemInPropertyListView!.getset_dataHolder.get_dataEnum;

                string nameOfContaier = _selectedItemInPropertyListView.getset_nameOfValueContainer;

                IValueContainerNonGeneric itemToReplaceWith = enumForNeededContainer.CreateValueContainerWithDefaultValue(nameOfContaier);

                int selectedItemIndex = get_itemsSourceForListView.IndexOf(_selectedItemInPropertyListView);

                get_itemsSourceForListView.Replace(itemToReplaceWith, selectedItemIndex);
            },

            () =>
            (_selectedItemInPropertyListView is not null
            && get_itemsSourceForListView.getset_observableCollectionToBindTo is not null)
        );

    public ICommand get_addNewPropertyToNoteCommand =>
    _addNewPropertyToNoteCommand ??= new RelayCommand
        (
            () =>
            {
                IValueContainerNonGeneric defaultValue
                    = AcceptableDataEnum.Text.CreateValueContainerWithDefaultValue(TextConstants.defaultPropertyCreatedName);

                get_itemsSourceForListView.Add(new PropertyListItem(defaultValue));
            },

            () => (get_itemsSourceForListView.getset_observableCollectionToBindTo is not null)
        );

    public ICommand get_switchNoteToGivenTypeCommand =>
    _switchNoteToGivenTypeCommand ??= new RelayCommand
        (
            () =>
            {
                Note noteFromType = NoteTypeGenerator.CreateNoteFromTypeData(get_selectedItemInTypeComboBox!);

                StaticMediators.get_currentPublisherForVMs.Publish(new SelectedNoteDataChangeRequestedPublication(noteFromType));
            },

            () => (get_selectedItemInTypeComboBox is not null
                && GetSelectedNoteFromMediator() is not null)
        );

    #endregion Commands

    #region Publisher Setup

#pragma warning disable IDE0052 // Remove unread private members
    private readonly SelectedNoteChangedSubscriber _selectedNoteChangeSubscriber;
#pragma warning restore IDE0052 // Remove unread private members

    internal class SelectedNoteChangedSubscriber : SubscriberForVMBase<SelectedNoteChangedPublication, PropertyListViewModel>
    {
        public SelectedNoteChangedSubscriber(PropertyListViewModel vm, Publisher publisher)
            : base(vm, publisher)
        {
        }

        public override Task Handle(SelectedNoteChangedPublication request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return Task.FromCanceled(cancellationToken);

            if (!get_viewModelExists) return Task.CompletedTask;

            get_viewModelIfExists!.get_itemsSourceForListView.getset_observableCollectionToBindTo
                = request.get_parameterObject?.get_properties;

            return Task.CompletedTask;
        }
    }

    #endregion Publisher Setup
}
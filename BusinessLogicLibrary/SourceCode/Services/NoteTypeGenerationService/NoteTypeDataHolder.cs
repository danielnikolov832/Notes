using BusinessLogicLibrary.Models;
using BusinessLogicLibrary.Services.NoteTypeGenerationService.DAL;
using BusinessLogicLibrary.Services.NoteTypeGenerationService.DAL.DAOs;
using BusinessLogicLibrary.Services.NoteTypeGenerationService.Models;
using Mapster;
using SqliteDALLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLibrary.Services.NoteTypeGenerationService;

public sealed class NoteTypeDataHolder
{
#region Singleton pattern

    private NoteTypeDataHolder()
    {
        DBInit dBInit = new();

        _dal = dBInit.DB;

        _types = _dal.GetTableWithPrimaryKey<NoteTypeDao>()!;
        _categories = _dal.GetTableWithPrimaryKey<TypeCategoryDao>()!;

        _rootCategoriesList = _categories.ReadAll().Where(cat => cat.get_parentCategoryForeignKey == 0).ToList();
        _rootTypesList = _types.ReadAll().Where(type => type.get_parentCategoryForeignKey == 0).ToList();

        InitFastReadConfig();

        void InitFastReadConfig()
        {
            _fastReadWithoutUnnessecaryInternalsConfig.Default.IgnoreNullValues(true);
            _fastReadWithoutUnnessecaryInternalsConfig.Default.IgnoreNullValues(true);

            _fastReadWithoutUnnessecaryInternalsConfig.NewConfig<NoteTypeDao, NoteType>()
                .ConstructUsing(() => new())
                .TwoWays()
                .PreserveReference(true);

            _fastReadWithoutUnnessecaryInternalsConfig.NewConfig<TypeCategoryDao, TypeCategory>()
                .ConstructUsing(() => new())
                .TwoWays()
                .PreserveReference(true);

            _fastReadWithoutUnnessecaryInternalsConfig.NewConfig<NoteDataDao, NoteData>()
                .ConstructUsing(() => new())
                .TwoWays()
                .PreserveReference(true);

            _fastReadWithoutUnnessecaryInternalsConfig.NewConfig<ValueContainerDataDao, ValueContainerData>()
                .ConstructUsing(() => new());

            Note note = new("dsdsd")
            {
                get_properties = new()
                {
                    new ValueContainer<string>("dsds", AcceptableDataEnum.Text),
                    new ValueContainer<string>("ssdsd", AcceptableDataEnum.Text),
                    new ValueContainer<string>("3333", AcceptableDataEnum.Text)
                },

                get_children = new()
                {
                    new("werwerwe"),
                    new("1212121")
                    {
                        get_properties = new()
                        {
                            new ValueContainer<TimeOnly>("45454", AcceptableDataEnum.Time)
                        }
                    }
                }
            };

            (bool _, TypeCategory category) = TryAddCategory(Random.Shared.Next().ToString());

            (bool _, NoteType noteType) = TryAddNoteType(Random.Shared.Next().ToString(), note, category);
            //(bool _, NoteType noteType2) = TryAddNoteType(Random.Shared.Next().ToString(), new(string.Empty));

            noteType.getset_name = "eeeeeeeeeeeeeeeeeeeeeeeeee";

            UpdateNoteType(noteType);

            TryRemoveNoteType(noteType);
        }
    }

    public static NoteTypeDataHolder get_instance { get; } = new();

#endregion Singleton pattern

    private readonly TypeAdapterConfig _fastReadWithoutUnnessecaryInternalsConfig = new();

    private readonly List<NoteTypeDao> _rootTypesList;
    public IReadOnlyList<NoteType> GetRootTypesList()
    {
        IEnumerable<NoteType> types =
            from NoteTypeDao dao in _rootTypesList
            select dao.Adapt<NoteType>(_fastReadWithoutUnnessecaryInternalsConfig);

        return types.ToList();
    }

    private readonly List<TypeCategoryDao> _rootCategoriesList;
    public IReadOnlyList<TypeCategory> GetRootCategoriesList()
    {
        IEnumerable<TypeCategory> categories =
            from TypeCategoryDao dao in _rootCategoriesList
            select dao.Adapt<TypeCategory>(_fastReadWithoutUnnessecaryInternalsConfig);

        return categories.ToList();
    }

#pragma warning disable S1450 // Private fields only used as local variables in methods should become local variables
    private readonly ISqliteDatabaseAccessLayer _dal;
#pragma warning restore S1450 // Private fields only used as local variables in methods should become local variables


    private readonly ISqliteRepositoryWithPrimaryKey<NoteTypeDao> _types;
    private readonly ISqliteRepositoryWithPrimaryKey<TypeCategoryDao> _categories;

    private static bool ContainsItemWithSameName<TCategoryChild>(string name, IEnumerable<TCategoryChild> list)
       where TCategoryChild : ICategoryChild
    {
        foreach (TCategoryChild categoryChild in list)
        {
            if (categoryChild.getset_name == name)
            {
                return true;
            }
        }

        return false;
    }

    private static bool TryAddItemInDatabase<TCategoryChild>(
        TCategoryChild item, IList<TCategoryChild> list,
        ISqliteRepositoryWithPrimaryKey<TCategoryChild> childRepo,
        bool addItemToList = false)
        where TCategoryChild : PrimaryKeyUser, ICategoryChild, new()
    {
        bool isItemUnique = !ContainsItemWithSameName(item.getset_name, list);

        if (isItemUnique)
        {
            childRepo.Insert(item);

            if (addItemToList)
            {
                list.Add(item);
            }
        }

        return isItemUnique;
    }

    private bool TryAddCategoryWithParentInDatabase(
        TypeCategoryDao item, TypeCategoryDao parent)
    {
        item.get_parentCategoryInverseProperty = parent;

        return TryAddItemInDatabase(item, parent.get_childCategotiesForDAL, _categories);
    }

    private bool TryAddTypeWithParentInDatabase(
        NoteTypeDao item, TypeCategoryDao parent)
    {
        item.get_parentCategoryInverseProperty = parent;

        return TryAddItemInDatabase(item, parent.get_childTypesForDAL, _types);
    }

    private static void UpdateItemInDatabase<TCategoryChild>(
       TCategoryChild item,
       ISqliteRepositoryWithPrimaryKey<TCategoryChild> childRepo)
       where TCategoryChild : PrimaryKeyUser, ICategoryChild, new()
    {
        childRepo.Update(item);
    }

    private static bool TryUpdateItemInDatabase<TCategoryChild>(
       TCategoryChild item, TCategoryChild newItem, IList<TCategoryChild> list,
       ISqliteRepositoryWithPrimaryKey<TCategoryChild> childRepo)
       where TCategoryChild : PrimaryKeyUser, ICategoryChild, new()
    {
        bool shouldItemBeUpdated = true;

        if (list is not null)
        {
            shouldItemBeUpdated = TryUpdateItemInCollection(item, list!, newItem);
        }

        if (shouldItemBeUpdated)
        {
            childRepo.Update(item, newItem);
        }

        return shouldItemBeUpdated;
    }

    private static bool TryUpdateItemInCollection<TCategoryChild>(TCategoryChild item, IList<TCategoryChild> list, TCategoryChild newItem)
        where TCategoryChild : PrimaryKeyUser, ICategoryChild, new()
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].getset_name == item.getset_name)
            {
                if (ContainsItemWithSameName(newItem.getset_name, list)) return false;

                list[i] = newItem;

                return true;
            }
        }

        return false;
    }

    private static bool TryRemoveItemInDatabase<TCategoryChild>(
       TCategoryChild item,
       ISqliteRepositoryWithPrimaryKey<TCategoryChild> childRepo,
       IList<TCategoryChild>? listToRemoveItemFrom = null)
       where TCategoryChild : PrimaryKeyUser, ICategoryChild, new()
    {
        bool shouldItemBeRemoved = true;

        if (listToRemoveItemFrom is not null)
        {
           shouldItemBeRemoved = listToRemoveItemFrom.Remove(item);
        }

        if (shouldItemBeRemoved)
        {
            childRepo.Delete(item);
        }

        return shouldItemBeRemoved;
    }

    public (bool succeeded, NoteType type) TryAddNoteType(string typeName, Note note, TypeCategory? parentCategoryOrRootIfNull = null)
    {
        NoteType typeFromNote = NoteTypeGenerator.CreateTypeDataFromNameAndNote(typeName, note);

        NoteTypeDao typeDao = typeFromNote.Adapt<NoteTypeDao>();

        if (parentCategoryOrRootIfNull is not null)
        {
            TypeCategoryDao parentDao = _categories.Read(parentCategoryOrRootIfNull.Id);

            bool isItemAddedToParent = TryAddTypeWithParentInDatabase(typeDao, parentDao);

            return (isItemAddedToParent, typeDao.Adapt<NoteType>(_fastReadWithoutUnnessecaryInternalsConfig));
        }

        bool isItemAddedToRoot = TryAddItemInDatabase(typeDao, _rootTypesList, _types);

        return (isItemAddedToRoot, typeDao.Adapt<NoteType>(_fastReadWithoutUnnessecaryInternalsConfig));
    }

    public void UpdateNoteType(NoteType type)
    {
        NoteTypeDao typeDao = type.Adapt<NoteTypeDao>(_fastReadWithoutUnnessecaryInternalsConfig);

        UpdateItemInDatabase(typeDao, _types);
    }

    public bool TryRemoveNoteType(NoteType type)
    {
        return TryRemoveNoteType(type.Id);
    }

    public bool TryRemoveNoteType(int typeId)
    {
        NoteTypeDao typeDao = _types.Read(typeId, cascade : false);

        //TypeCategoryDao parentCategory = _categories.Read(categoryDao.get_parentCategoryForeignKey, cascade : false);

        if (typeDao.get_parentCategoryForeignKey != 0)
        {
            return TryRemoveItemInDatabase(typeDao, _types);
        }

        return TryRemoveItemInDatabase(typeDao, _types, _rootTypesList);
    }

    public (bool succeeded, TypeCategory resultingCategory) TryAddCategory(string newCategoryName, TypeCategory? parentCategoryOrRootIfNull = null)
    {
        TypeCategory category = new(newCategoryName);

        TypeCategoryDao categoryDao = category.Adapt<TypeCategoryDao>();

        if (parentCategoryOrRootIfNull is not null)
        {
            TypeCategoryDao parentDao = _categories.Read(parentCategoryOrRootIfNull.Id);

            bool isAddedToParent = TryAddCategoryWithParentInDatabase(categoryDao, parentDao);

            return (isAddedToParent, SimplifiedNeedOnlyMapFromDAO(category, categoryDao));
        }

        bool isAddedToRoot = TryAddItemInDatabase(categoryDao, _rootCategoriesList, _categories);

        return (isAddedToRoot, SimplifiedNeedOnlyMapFromDAO(category, categoryDao));

        static TypeCategory SimplifiedNeedOnlyMapFromDAO(TypeCategory category, TypeCategoryDao categoryDao)
        {
            category.Id = categoryDao.Id;

            return category;
        }
    }

    public void UpdateCategory(TypeCategory category)
    {
        TypeCategoryDao categoryDao = category.Adapt<TypeCategoryDao>();

        UpdateItemInDatabase(categoryDao, _categories);
    }

    public bool TryRemoveCategory(TypeCategory category)
    {
        return TryRemoveCategory(category.Id);
    }

    public bool TryRemoveCategory(int categoryId)
    {
        TypeCategoryDao categoryDao = _categories.Read(categoryId, cascade: false);

        if (categoryDao.get_parentCategoryForeignKey != 0)
        {
            return TryRemoveItemInDatabase(categoryDao, _categories);
        }

        return TryRemoveItemInDatabase(categoryDao, _categories, _rootCategoriesList);
    }
}
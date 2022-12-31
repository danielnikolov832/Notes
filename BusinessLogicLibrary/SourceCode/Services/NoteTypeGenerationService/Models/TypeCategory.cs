using BusinessLogicLibrary.Services.NoteTypeGenerationService.DAL.DAOs;
using Mapster;
using SqliteDALLib;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLibrary.Services.NoteTypeGenerationService.Models;

public class TypeCategory : PrivatePrimaryKeyUserWithNoDBUse
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    internal TypeCategory()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    { }

    internal TypeCategory(string name)
    {
        getset_name = name;
    }

    public string getset_name { get; set; }

    [AdaptMember(nameof(TypeCategoryDao.get_childCategotiesForDAL))]
    private List<TypeCategory> get_childCategotiesForDAL { get; set; } = new();
    public IReadOnlyList<TypeCategory> get_childCategories => get_childCategotiesForDAL;

    [AdaptMember(nameof(TypeCategoryDao.get_childTypesForDAL))]
    private List<NoteType> get_childTypesForDAL { get; set; } = new();
    public IReadOnlyList<NoteType> get_types => get_childTypesForDAL;

    [AdaptMember(nameof(TypeCategoryDao.get_parentCategoryForeignKey))]
    private int get_parentCategoryForeignKey { get; set; }

    [AdaptMember(nameof(TypeCategoryDao.get_parentCategoryInverseProperty))]
    //[AdaptIgnore(side: MemberSide.Destination)]
    internal TypeCategory? get_parentCategoryInverseProperty { get; set; }
}
using SQLite;
using SqliteDALLib;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLibrary.Services.NoteTypeGenerationService.DAL.DAOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
internal class TypeCategoryDao : PrimaryKeyUser, ICategoryChild
{
    [MaxLength(250)]
    public string getset_name { get; set; }

    [OneToMany(inverseProperty: nameof(get_parentCategoryInverseProperty), CascadeOperations = CascadeOperation.All)]
    public List<TypeCategoryDao> get_childCategotiesForDAL { get; set; } = new();

    [OneToMany(CascadeOperations = CascadeOperation.All)]
    public List<NoteTypeDao> get_childTypesForDAL { get; set; } = new();

    [ForeignKey(typeof(TypeCategoryDao))]
    public int get_parentCategoryForeignKey { get; set; }

    [ManyToOne]
#pragma warning disable CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
    public TypeCategoryDao? get_parentCategoryInverseProperty { get; set; }
#pragma warning restore CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
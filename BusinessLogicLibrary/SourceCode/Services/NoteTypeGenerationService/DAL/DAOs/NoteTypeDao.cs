using BusinessLogicLibrary.Services.NoteTypeGenerationService.Models;
using SQLite;
using SqliteDALLib;
using SQLiteNetExtensions.Attributes;

namespace BusinessLogicLibrary.Services.NoteTypeGenerationService.DAL.DAOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
internal sealed class NoteTypeDao : PrimaryKeyUser, ICategoryChild
{
    [MaxLength(250)]
    public string getset_name { get; set; }

    [OneToOne(foreignKey: nameof(get_noteDataForDALForeignKey), CascadeOperations = CascadeOperation.All)]
    public NoteDataDao get_noteDataForDAL { get; set; }

    [ForeignKey(typeof(NoteDataDao))]
    public int get_noteDataForDALForeignKey { get; set; }

    [ForeignKey(typeof(TypeCategoryDao))]
    public int get_parentCategoryForeignKey { get; set; }

    [ManyToOne]
#pragma warning disable CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
    public TypeCategoryDao? get_parentCategoryInverseProperty { get; set; }
#pragma warning restore CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).

}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using SQLite;
using SqliteDALLib;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace BusinessLogicLibrary.Services.NoteTypeGenerationService.DAL.DAOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
internal sealed class NoteDataDao : PrimaryKeyUser
{
    [MaxLength(300)]
    public string get_nameOfNote { get; set; }

    [OneToMany(CascadeOperations = CascadeOperation.All)]
    public List<ValueContainerDataDao> get_notePropertyDataForDTO { get; set; } = new();

    [OneToMany(inverseProperty: nameof(get_parentNoteInverseProperty), CascadeOperations = CascadeOperation.All)]
    public List<NoteDataDao> get_noteChildrenDataForDTO { get; set; } = new();

    [ManyToOne]
    public NoteDataDao get_parentNoteInverseProperty { get; set; }

    [ForeignKey(typeof(NoteTypeDao))]
    public int get_parentTypeForeignKey { get; set; }

    [ForeignKey(typeof(NoteDataDao))]
    public int get_parentNoteForeignKey { get; set; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

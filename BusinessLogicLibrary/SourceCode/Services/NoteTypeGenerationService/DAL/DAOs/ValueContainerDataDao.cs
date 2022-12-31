using SQLite;
using SqliteDALLib;
using SQLiteNetExtensions.Attributes;

namespace BusinessLogicLibrary.Services.NoteTypeGenerationService.DAL.DAOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
internal sealed class ValueContainerDataDao : PrimaryKeyUser
{
    [MaxLength(250)]
    public string get_nameOfValueContainer { get; set; }
    public AcceptableDataEnum get_dataEnum { get; set; }

    [ForeignKey(typeof(NoteDataDao))]
    public int get_parentNoteForeignKey { get; set; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
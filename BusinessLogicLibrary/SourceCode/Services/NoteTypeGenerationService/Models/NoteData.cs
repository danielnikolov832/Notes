using BusinessLogicLibrary.Models;
using BusinessLogicLibrary.Services.NoteTypeGenerationService.DAL.DAOs;
using Mapster;
using SqliteDALLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLibrary.Services.NoteTypeGenerationService.Models;

public sealed class NoteData : PrivatePrimaryKeyUserWithNoDBUse
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    internal NoteData()
    { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    internal NoteData(Note note)
    {
        get_nameOfNote = note.getset_name;

        get_notePropertyDataForDTO.AddRange(
            from IValueContainerNonGeneric valueContainer in note.get_properties
            select new ValueContainerData(valueContainer));

        get_noteChildrenDataForDTO.AddRange(
            from Note noteChild in note.get_children
            select new NoteData(noteChild));
    }

    public string get_nameOfNote { get; set; }

    [AdaptMember(nameof(NoteDataDao.get_notePropertyDataForDTO))]
    private List<ValueContainerData> get_notePropertyDataForDTO { get; set; } = new();
    public IReadOnlyList<ValueContainerData> get_notePropertyData => get_notePropertyDataForDTO;

    [AdaptMember(nameof(NoteDataDao.get_noteChildrenDataForDTO))]
    private List<NoteData> get_noteChildrenDataForDTO { get; set; } = new();
    public IReadOnlyList<NoteData> get_noteChildrenData => get_noteChildrenDataForDTO;

    [AdaptMember(nameof(NoteDataDao.get_parentTypeForeignKey))]
    private int get_parentTypeForeignKey { get; set; }

    [AdaptMember(nameof(NoteDataDao.get_parentNoteForeignKey))]
    private int get_parentNoteForeignKey { get; set; }

    [AdaptMember(nameof(NoteDataDao.get_parentNoteInverseProperty))]
    //[AdaptIgnore(side: MemberSide.Destination)]
    internal NoteData? get_parentNoteInverseProperty { get; set; }
}

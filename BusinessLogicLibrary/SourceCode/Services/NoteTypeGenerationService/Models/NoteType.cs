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

public sealed class NoteType : PrivatePrimaryKeyUserWithNoDBUse
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    internal NoteType()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    { }

    internal NoteType(string name, NoteData noteData)
    {
        getset_name = name;
        get_noteDataForDAL = noteData;
    }

    public string getset_name { get; set; }

    [AdaptMember(nameof(NoteTypeDao.get_noteDataForDAL))]
    private NoteData get_noteDataForDAL { get; set; }

    public NoteData get_noteData => get_noteDataForDAL;

    [AdaptMember(nameof(NoteTypeDao.get_noteDataForDALForeignKey))]
    private int get_noteDataForDALForeignKey { get; set; }

    [AdaptMember(nameof(NoteTypeDao.get_parentCategoryForeignKey))]
    private int get_parentCatrgoryForeignKey { get; set; }

    [AdaptMember(nameof(NoteTypeDao.get_parentCategoryInverseProperty))]
    //[AdaptIgnore(side: MemberSide.Destination)]
    internal TypeCategory? get_parentCategoryInverseProperty { get; set; }
}

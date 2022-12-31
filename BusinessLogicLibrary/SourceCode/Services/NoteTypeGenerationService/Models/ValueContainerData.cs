using BusinessLogicLibrary.Models;
using BusinessLogicLibrary.Services.NoteTypeGenerationService.DAL.DAOs;
using Mapster;
using SqliteDALLib;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace BusinessLogicLibrary.Services.NoteTypeGenerationService.Models;

public sealed class ValueContainerData : PrivatePrimaryKeyUserWithNoDBUse
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    internal ValueContainerData()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    { }

    internal ValueContainerData(IValueContainerNonGeneric valueContainer)
    {
        get_nameOfValueContainer = valueContainer.getset_name;
        get_dataEnum = valueContainer.get_dataEnum;
    }

    public string get_nameOfValueContainer { get; set; }
    public AcceptableDataEnum get_dataEnum { get; set; }

    [AdaptMember(nameof(ValueContainerDataDao.get_parentNoteForeignKey))]
    private int get_parentNoteForeignKey { get; set; }
}
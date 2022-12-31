using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using BusinessLogicLibrary.Models;
using BusinessLogicLibrary.Services.NoteTypeGenerationService.Models;

namespace BusinessLogicLibrary.Services.NoteTypeGenerationService
{
    public static class NoteTypeGenerator
    {
        internal static NoteType CreateTypeDataFromNameAndNote(string typeName, Note note)
        {
            return new NoteType(typeName, new NoteData(note));
        }

        public static Note CreateNoteFromTypeData(NoteType typeData, string? nameOfNote = null)
        {
            return MapNoteFromNoteData(typeData.get_noteData, nameOfNote);
        }

        private static IValueContainerNonGeneric MapValueContainerFromData(ValueContainerData valueContainerData)
        {
            AcceptableDataEnum dataEnumInContainerData = valueContainerData.get_dataEnum;

            return dataEnumInContainerData.CreateValueContainerWithDefaultValue(valueContainerData.get_nameOfValueContainer);
        }

        private static Note MapNoteFromNoteData(NoteData noteData, string? optionalNameForNewNote = null)
        {
            string noteName = optionalNameForNewNote ?? noteData.get_nameOfNote;

            Note output = new(noteName);

            foreach (NoteData noteChildData in noteData.get_noteChildrenData)
            {
                output.get_children.Add(MapNoteFromNoteData(noteChildData));
            }

            foreach (ValueContainerData valueContainerData in noteData.get_notePropertyData)
            {
                output.get_properties.Add(MapValueContainerFromData(valueContainerData));
            }

            return output;
        }
    }
}

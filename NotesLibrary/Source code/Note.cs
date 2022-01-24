using System.Diagnostics.CodeAnalysis;

namespace NotesLibrary
{
    // The base form of a note, which is a holder of text information
    public class Note
    {
        private const string otherSubNotesWithTheSameNameString =
        "The argument 'name' is not valid, because there is a subNote with the same name";

        public string get_name { get; private set; }
        public string text;
        public Note? get_parentNote { get; init; }

        public HashSet<Note> get_subNotes { get; init; }
        private class NoteEqualityComparer : IEqualityComparer<Note>
        {
            public bool Equals(Note? x, Note? y)
            {
                if (x == null && y == null) return true;

                if (x == null || y == null) return false;

                bool output_areWithSameName = x.get_name == y.get_name;

                return output_areWithSameName;
            }

            public int GetHashCode([DisallowNullAttribute] Note obj)
            {
                return obj.get_name.GetHashCode(StringComparison.Ordinal);
            }
        }

        // Creates an object, with all of the same fields and properties 
        // as the copy object, but who is a different reference to it.
        private Note(Note copy)
        {
            get_name = copy.get_name;
            get_subNotes = copy.get_subNotes;
            text = copy.text;
            get_parentNote = copy.get_parentNote;
        }

        public Note(string name, string text = "", Note? parentNote = null)
        {
            get_name = name;
            this.text = text;
            get_parentNote = parentNote;
            
            get_subNotes = new HashSet<Note>(new NoteEqualityComparer());
        }

        public Note(string name, IEnumerable<Note> subNotes, string text = "", Note? parentNote = null)
        {
            get_name = name;
            this.text = text;
            get_parentNote = parentNote;

            get_subNotes = new HashSet<Note>(new NoteEqualityComparer());

            foreach (Note noteItem in subNotes)
            {
                AddSubNote_OrThrow(noteItem);
            }
        }

        public void SetName_OrThrow(string name)
        {
            bool isNoteValidWithNewName = IsNoteValidWithNewName(this, name);

            if (isNoteValidWithNewName)
            {
                this.get_name = name;
            }
            else
            {
                throw new ArgumentException(otherSubNotesWithTheSameNameString);
            }
        }

        private static bool IsNoteValidWithNewName(Note note, string newName)
        {   
            Note noteWithNewName = new Note(note);
            noteWithNewName.get_name = newName;

            bool output_isNoteValidForParentNote = IsSubNoteValidForParentNote(noteWithNewName);

            return output_isNoteValidForParentNote;
        }

        private static bool IsSubNoteValidForParentNote(Note subNote)
        {
            Note? parentNote = subNote.get_parentNote;

            if (parentNote == null) return true;

            return parentNote.get_subNotes.Contains(subNote);
        }

        public bool TrySetName(string name)
        {
            try
            {
                SetName_OrThrow(name);
            }
            catch(ArgumentException)
            {
                return false;
            }

            return true;
        }

        public void AddSubNote_OrThrow(Note subNote)
        {
            bool isSubNoteAdded = get_subNotes.Add(subNote);

            if (isSubNoteAdded == false)
            {
                throw new ArgumentException(otherSubNotesWithTheSameNameString);
            }
        }

        public bool TryAddSubNote(Note subNote)
        {
            try
            {
                AddSubNote_OrThrow(subNote);
            }
            catch(ArgumentException)
            {
                return false;
            }

            return true;
        }

        public Note? GetSubNoteWithName(string name)
        {
            foreach (Note noteItem in get_subNotes)
            {
                if (noteItem.get_name == name) return noteItem;
            }

            return null;
        }

        public bool TryRemoveSubNote(Note subNote)
        {
            bool output_isRemoved = get_subNotes.Remove(subNote);

            return output_isRemoved;
        }

        public bool TryRemoveSubNoteWithName(string name)
        {
            foreach (Note noteItem in get_subNotes)
            {
                if (noteItem.get_name == name)
                {
                    bool output_isRemoved = get_subNotes.Remove(noteItem);

                    return output_isRemoved;
                }
            }

            return false;
        }

        public void RemoveAllSubNotes()
        {
            RemoveAllSubNotesWhere( (Note obj) => { return true; } );
        }

        public void RemoveAllSubNotesWhere(Predicate<Note> match)
        {
            get_subNotes.RemoveWhere(match);
        }
    }
}
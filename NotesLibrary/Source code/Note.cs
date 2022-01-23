namespace NotesLibrary
{
    // The base form of a note, which is a holder of text information
    public class Note
    {
        private const string otherSubNotesWithTheSameNameString =
        "The argument 'name' is not valid, because there is a subNote with the same name";

        private string name;
        public string get_name { get => name; }

        public string text;

        private Note? parentNote {get; init;}

        public List<Note> get_subNotes { get; init; }

        public Note(string name, string text = "", Note? parentNote = null)
        {
            this.name = name;
            this.text = text;
            this.parentNote = parentNote;
            
            get_subNotes = new List<Note>();
        }

        public Note(string name, IEnumerable<Note> subNotes, string text = "", Note? parentNote = null)
        {
            this.name = name;
            this.text = text;
            this.parentNote = parentNote;

            this.get_subNotes = new List<Note>();

            foreach (Note noteItem in subNotes)
            {
                AddSubNote_OrThrow(noteItem);
            }
        }

        public void SetName_OrThrow(string name)
        {
            if (parentNote == null)
            {
                this.name = name;

                return;
            }
            
            bool isNoteNameValidForParentNote = parentNote.IsSubNoteNameValid(name);

            if (isNoteNameValidForParentNote)
            {
                this.name = name;
            }
            else
            {
                throw new ArgumentException(otherSubNotesWithTheSameNameString);
            }
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
            bool isSubNoteValid = IsSubNoteValid(subNote);

            if (isSubNoteValid)
            {
                get_subNotes.Add(subNote);
            }
            else
            {
                throw new ArgumentException(otherSubNotesWithTheSameNameString);
            }
        }

        public void AddSubNoteFromNameAndText_OrThrow(string name, string text = "")
        {
            Note subNote = new Note(name, text);

            AddSubNote_OrThrow(subNote);
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

        public bool TryAddSubNoteFromNameAndText(string name, string subtext = "")
        {
            Note subNote = new Note(name, subtext);

            bool output = TryAddSubNote(subNote);

            return output;
        }

        private bool IsSubNoteValid(Note subNote)
        {
            bool output = IsSubNoteNameValid(subNote.name);

            return output;
        }

        private bool IsSubNoteNameValid(string name)
        {
            foreach (Note noteItem in get_subNotes)
            {
                if (noteItem.name == name) return false;
            }

            return true;
        }

        public bool TryRemoveSubNote(Note subNote)
        {
            bool output = get_subNotes.Remove(subNote);

            return output;
        }

        public bool TryRemoveSubNoteWithName(string name)
        {
            foreach (Note noteItem in get_subNotes)
            {
                if (noteItem.name == name)
                {
                    bool output = get_subNotes.Remove(noteItem);

                    return output;
                }
            }

            return false;
        }
    }
}
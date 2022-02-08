namespace NotesLibrary.Notes
{
    public class NoteWithSubNotes<TNote> : Note where TNote : Note
    {           
        public NoteCollection<TNote> get_subNotes { get; init; }

        public NoteWithSubNotes(string name, string text = "") : base(name, text)
        {
            get_subNotes = new NoteCollection<TNote>();

            SetSubNotesEvents(get_subNotes);
        }

        public NoteWithSubNotes(string name, NoteCollection<TNote> subNotes, string text = "") : base(name, text)
        {
            get_subNotes = subNotes;

            SetSubNotesEvents(get_subNotes);

            foreach (TNote noteItem in subNotes)
            {
                SetParentNoteOfNoteToThis(noteItem);
            }
        }

        public NoteWithSubNotes(string name, Note parentNote, string text = "") : base(name, parentNote, text)
        {
            get_subNotes = new NoteCollection<TNote>();

            SetSubNotesEvents(get_subNotes);
        }

        public NoteWithSubNotes(string name, Note parentNote, NoteCollection<TNote> subNotes, string text = "") : base(name, parentNote, text)
        {
            get_subNotes = subNotes;

            SetSubNotesEvents(get_subNotes);

            foreach (TNote noteItem in subNotes)
            {
                SetParentNoteOfNoteToThis(noteItem);
            }
        }

        private void SetSubNotesEvents(NoteCollection<TNote> subNotes)
        {
            subNotes.OnAfterInsertAndBeforeBindToItem += NoteCollection_OnAfterInsertOrSetAndBeforeBindToItem;
            subNotes.OnAfterSetAndBeforeBindToItem += NoteCollection_OnAfterInsertOrSetAndBeforeBindToItem;
            subNotes.OnAfterRemoveAndUnbindFromItem += NoteCollection_OnAfterRemoveAndUnbindFromItem;
        }

        private void NoteCollection_OnAfterInsertOrSetAndBeforeBindToItem(object? sender, NoteCollection<TNote>.OnActionAndBindStateEventArgs e)
        {
            SetParentNoteOfNoteToThis(e.get_noteItem);
            
            NoteCollection_OnBeforeInsertOrSetItem(sender, e);
        }

        private void SetParentNoteOfNoteToThis(TNote note)
        {
            note.get_parentNote = this;
        }

        private void NoteCollection_OnAfterRemoveAndUnbindFromItem(object? sender, NoteCollection<TNote>.OnActionAndBindStateEventArgs e)
        {
            TNote subNote = e.get_noteItem;

            subNote.get_parentNote = null;

            NoteCollection_OnBeforeRemoveItem(sender, e);
        }

        protected virtual void NoteCollection_OnBeforeInsertOrSetItem(object? sender, NoteCollection<TNote>.OnActionAndBindStateEventArgs e)
        {

        }

        protected virtual void NoteCollection_OnBeforeRemoveItem(object? sender, NoteCollection<TNote>.OnActionAndBindStateEventArgs e)
        {

        }
    }
}
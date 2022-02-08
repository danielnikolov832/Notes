using NotesLibrary;

namespace ToDoListLibrary
{
    internal class LimitedTimeNoteCollection : NoteCollection<LimitedTimeNote>
    {
        private class LimitedTimeNoteEqualityComparer : NoteEqualityComparer
        {
            public override bool Equals(LimitedTimeNote? x, LimitedTimeNote? y)
            {
                return false;
            }
        }

        protected override NoteEqualityComparer none_comparer { get; init; } = new LimitedTimeNoteEqualityComparer();
    }
}
using NotesLibrary;
using NotesLibrary.Notes;

namespace ToDoListLibrary
{
    public class LimitedTimeNote : NoteWithSubNotes<Note>
    {
        // Do not modify directly, use getset_finalTime instead
        private DateTimeOffset _finalTime;
        public DateTimeOffset getset_finalTime
        {
            get => _finalTime;
            set => _finalTime = ValidateAndGetFinalTime_OrThrow(value);
        }

        public bool get_isFinalTimeLaterThanNow { get => IsDateTimeOffsetLaterThanNow(getset_finalTime); }

        public LimitedTimeNote(string name, DateTimeOffset finalTime, string text = "") : base(name, text)
        {
            getset_finalTime = finalTime;
        }

        public LimitedTimeNote(string name, NoteCollection<Note> subNotes, DateTimeOffset finalTime, string text = "") : base(name, subNotes, text)
        {
            getset_finalTime = finalTime;
        }

        private const string invalidFinalTimeArgumentString = "The argument 'finalTime' is invalid, because (finalTime =< DateTime.Now)";

        private DateTimeOffset ValidateAndGetFinalTime_OrThrow(DateTimeOffset finalTime)
        {
            if (IsDateTimeOffsetLaterThanNow(finalTime) == false)
            {
                throw new ArgumentException(invalidFinalTimeArgumentString);
            }

            return finalTime;
        }

        private bool IsDateTimeOffsetLaterThanNow(DateTimeOffset finalTime)
        {
            return finalTime > DateTimeOffset.Now;
        }
    }
}
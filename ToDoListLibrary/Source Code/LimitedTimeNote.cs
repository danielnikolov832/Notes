using NotesLibrary;
using NotesLibrary.Notes;

namespace ToDoListLibrary
{
    public class LimitedTimeNote : NoteWithSubNotes<Note>
    {
        private const string invalidFinalTimeArgumentString = "The argument 'finalTime' is invalid, because (finalTime =< DateTime.Now)";

        public DateTimeOffset get_finalTime { get; init; }
        public bool get_isFinalTimeLaterThanNow { get => IsFinalTimeLaterThanNow(get_finalTime); }

        public LimitedTimeNote(string name, DateTimeOffset finalTime, string text = "") : base(name, text)
        {
            get_finalTime = ValidateAndGetFinalTime_OrThrow(finalTime);
        }

        public LimitedTimeNote(string name, NoteCollection<Note> subNotes, DateTimeOffset finalTime, string text = "") : base(name, subNotes, text)
        {
            get_finalTime = ValidateAndGetFinalTime_OrThrow(finalTime);
        }

        private DateTimeOffset ValidateAndGetFinalTime_OrThrow(DateTimeOffset finalTime)
        {
            if (IsFinalTimeLaterThanNow(finalTime) == false)
            {
                throw new ArgumentException(invalidFinalTimeArgumentString);
            }

            return finalTime;
        }

        private bool IsFinalTimeLaterThanNow(DateTimeOffset finalTime)
        {
            return finalTime > DateTimeOffset.Now;
        }
    }
}
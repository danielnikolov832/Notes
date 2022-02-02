using NotesLibrary;

namespace ToDoListLibrary
{
    public class LimitedTimeNote : Note
    {
        private const string invalidFinalTimeArgumentString = "The argument 'finalTime is invalid, because (finalTime =< DateTime.Now)'";

        public DateTime get_finalTime {get; init;}

        public LimitedTimeNote(string name, DateTime finalTime, string text = "") : base(name, text)
        {
            if (IsValidFinalTime(finalTime))
            {
                get_finalTime = finalTime;
            }
            else
            {
                throw new ArgumentException(invalidFinalTimeArgumentString);
            }
        }

        public LimitedTimeNote(string name, IEnumerable<Note> subNotes, DateTime finalTime, string text = "") : base(name, subNotes, text)
        {
            get_finalTime = finalTime;
        }

        private bool IsValidFinalTime(DateTime finalTime)
        {
            return finalTime > DateTime.Now;
        }
    }
}
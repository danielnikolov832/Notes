using NotesLibrary;

namespace ToDoListLibrary
{
    public class TaskCalendar
    {
        private List<NoteCollection<LimitedTimeNote>> _toDoLists;
        public List<LimitedTimeNote> notesSortedByTime {get; init;}
        public Dictionary<DateOnly, List<LimitedTimeNote>> notesPairedWithDates {get; init;}

        public TaskCalendar(IEnumerable<NoteCollection<LimitedTimeNote>> noteCollections)
        {
            _toDoLists = noteCollections.ToList();

            List<LimitedTimeNote> joinedList = GetJoinedListFromCollections(_toDoLists);

            notesSortedByTime = GetNotesSortedByTimeFromNoteList(joinedList);

            notesPairedWithDates = GetNotesPairedWithDatesFromList(joinedList);
        }

        private Dictionary<DateOnly, List<LimitedTimeNote>> GetNotesPairedWithDatesFromList(List<LimitedTimeNote> noteList)
        {
            Dictionary<DateOnly, List<LimitedTimeNote>> output = new Dictionary<DateOnly, List<LimitedTimeNote>>();

            foreach (LimitedTimeNote noteItem in noteList)
            {
                DateTime finalTimeOfNoteItem = noteItem.get_finalTime;

                DateOnly dateFromNoteItemTime = new DateOnly(
                    finalTimeOfNoteItem.Year,
                    finalTimeOfNoteItem.Month,
                    finalTimeOfNoteItem.Day);

                bool isDayAlreadyWithNotes = output.ContainsKey(dateFromNoteItemTime);

                if (isDayAlreadyWithNotes)
                {
                    List<LimitedTimeNote> alreadyDefinedListForThisDate = output[dateFromNoteItemTime];

                    alreadyDefinedListForThisDate.Add(noteItem);
                }
                else
                {
                    output.Add(dateFromNoteItemTime, new List<LimitedTimeNote>() 
                    { 
                        noteItem
                    });
                }
            }

            foreach (KeyValuePair<DateOnly, List<LimitedTimeNote>> kvp in output)
            {
                List<LimitedTimeNote> valueFromKvp = output[kvp.Key];

                valueFromKvp = GetNotesSortedByTimeFromNoteList(valueFromKvp);
            }

            return output;
        }

        private List<LimitedTimeNote> GetNotesSortedByTimeFromNoteList(List<LimitedTimeNote> noteList)
        {
            noteList.Sort((LimitedTimeNote noteItem1, LimitedTimeNote noteItem2) => 
            {
                return noteItem1.get_finalTime.CompareTo(noteItem2.get_finalTime);
            });

            return noteList;
        }

        private List<LimitedTimeNote> GetJoinedListFromCollections(List<NoteCollection<LimitedTimeNote>> noteCollections)
        {
            List<LimitedTimeNote> output = new List<LimitedTimeNote>();

            foreach (NoteCollection<LimitedTimeNote> noteCollectionItem in noteCollections)
            {
                foreach (LimitedTimeNote limitedTimeNoteItem in noteCollectionItem)
                {
                    output.Add(limitedTimeNoteItem);
                }
            }

            return output;
        }
    }
}
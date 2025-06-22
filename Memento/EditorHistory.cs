namespace MementoWebApp.Memento
{

    public class EditorHistory
    {
               
        // Use List to store undo and redo states
        public List<ArticleMemento> UndoStack { get; set; } = new List<ArticleMemento>();
        public List<ArticleMemento> RedoStack { get; set; } = new List<ArticleMemento>();

        // Add methods to manipulate the stacks

        public void SaveState(ArticleMemento memento)
        {
            UndoStack.Add(memento);
            RedoStack.Clear();
        }

        public ArticleMemento Undo(ArticleEditor editor)
        {
            if (UndoStack.Count == 0)
                return null;

            // Get the last saved state
            var lastIndex = UndoStack.Count - 1;
            var memento = UndoStack[lastIndex];
            UndoStack.RemoveAt(lastIndex);

            // Save current state to redo stack
            RedoStack.Add(editor.CreateMemento());

            return memento;
        }

        public ArticleMemento Redo(ArticleEditor editor)
        {
            if (RedoStack.Count == 0)
                return null;

            var lastIndex = RedoStack.Count - 1;
            var memento = RedoStack[lastIndex];
            RedoStack.RemoveAt(lastIndex);

            UndoStack.Add(editor.CreateMemento());

            return memento;
        }

        public int GetUndoCount() => UndoStack.Count;
        public int GetRedoCount() => RedoStack.Count;

       
    }
}

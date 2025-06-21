namespace MementoWebApp.Memento
{
    public class EditorHistory
    {
         public Stack<ArticleMemento> _undoStack { get; set; } = new Stack<ArticleMemento>();
         public Stack<ArticleMemento> _redoStack { get; set; } = new Stack<ArticleMemento>();


         public void SaveState(ArticleMemento memento)
         {
             _undoStack.Push(memento);
             _redoStack.Clear(); // Reset redo on new action
         }

         public ArticleMemento Undo(ArticleEditor editor)
         {
             if (_undoStack.Count == 0) return null;

             var current = editor.CreateMemento();
             var previous = _undoStack.Pop();
            
             _redoStack.Push(current);

            
             return previous;
         }

         public ArticleMemento Redo(ArticleEditor editor)
         {
             if (_redoStack.Count == 0) return null;

             var current = editor.CreateMemento();
            var next = _redoStack.Pop();
            _undoStack.Push(current);

             
             return next;
         }

         public int GetUndoCount() => _undoStack.Count;
         public int GetRedoCount() => _redoStack.Count;

        public void PrintStacks()
        {
            Console.WriteLine("----- Stack Status -----");

            Console.WriteLine("Undo stack (oldest to newest):");
            foreach (var m in _undoStack.Reverse())
            {
                Console.WriteLine(" - " + m.Title);
            }

            Console.WriteLine("Redo stack (top to bottom):");
            foreach (var m in _redoStack)
            {
                Console.WriteLine(" - " + m.Title);
            }

            Console.WriteLine("-------------------------");
        }

        /*
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

       */
    }
}

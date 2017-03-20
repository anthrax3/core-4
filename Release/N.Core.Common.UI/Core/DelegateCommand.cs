using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace N.Core.Common.UI.Core
{
   public class DelegateCommand<T> : ICommand
   {
      #region Properties

      readonly Action<T> _Execute = null;
      readonly Predicate<T> _CanExecute = null;
      public string Label { get; set; }
      #endregion
      //-----------------------------------------------------------------------------------------------------

      #region Constructors

      public DelegateCommand(Action<T> execute) : this(execute, null) { }

      public DelegateCommand(Action<T> execute, Predicate<T> canExecute) : this(execute, canExecute, "") { }

      public DelegateCommand(Action<T> execute, Predicate<T> canExecute, string label)
      {
         _Execute = execute;
         _CanExecute = canExecute;

         Label = label;
      }
      #endregion
      //-----------------------------------------------------------------------------------------------------

      #region Methods

      public void Execute(object parameter)
      {
         _Execute((T)parameter);
      }

      public bool CanExecute(object parameter)
      {
         return _CanExecute == null ? true : _CanExecute((T)parameter);
      }
      #endregion
      //-----------------------------------------------------------------------------------------------------

      #region Events

      public event EventHandler CanExecuteChanged
      {
         add
         {
            if (_CanExecute != null)
               CommandManager.RequerySuggested += value;
         }
         remove
         {
            if (_CanExecute != null)
               CommandManager.RequerySuggested -= value;
         }
      }
      #endregion
   }
}

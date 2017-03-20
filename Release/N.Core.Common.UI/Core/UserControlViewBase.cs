﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace N.Core.Common.UI.Core
{
   public class UserControlViewBase : UserControl
   {
      #region Properties

      public static readonly DependencyProperty ViewLoadedProperty =
         DependencyProperty.Register("ViewLoaded", typeof(object), typeof(UserControlViewBase),
         new PropertyMetadata(null));
      #endregion

      //-------------------------------------------------------------------------------------------------
      #region Constructors

      public UserControlViewBase()
      {
         // Programmatically bind the viewmodel's ViewLoaded property to the view's ViewLoaded property.
         BindingOperations.SetBinding(this, ViewLoadedProperty, new Binding("ViewLoaded"));

         DataContextChanged += OnDataContextChanged;
      }
      #endregion

      //-------------------------------------------------------------------------------------------------
      #region Methods

      protected virtual void OnUnwireViewModelEvents(ViewModelBase viewModel) { }
      protected virtual void OnWireViewModelEvents(ViewModelBase viewModel) { }

      void OnDataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
      {
         if (e.NewValue == null)
         {
            if (e.OldValue != null)
            {
               // view going out of scope and view-model disconnected (but still around in the parent)
               // unwire events to allow view to dispose

               OnUnwireViewModelEvents(e.OldValue as ViewModelBase);
            }
         }
         else
         {
            OnWireViewModelEvents(e.NewValue as ViewModelBase);
         }
      }
      #endregion
   }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace P.Core.Common.Core
{
    /// <summary>
    /// This is the abstract base class for any object that provides property change notifications.  
    /// </summary>
    public abstract class NotificationObject : INotifyPropertyChanged
    {
        #region Properties
        private event PropertyChangedEventHandler _PropertyChangedEvent;
        protected List<PropertyChangedEventHandler> _PropertyChangedSubscribers = new List<PropertyChangedEventHandler>();
        #endregion
        //---------------------------------------------------------------------------------------------------

        #region Members
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            this.VerifyPropertyName(propertyName);

            // set reference to avoid invoking a nulled handler after the null check in multi-threaded applications
            PropertyChangedEventHandler handler = this._PropertyChangedEvent;

            if (handler != null) // null check
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        //---------------------------------------------------------------------------------------------------

        #region Debugging Aides

        /// <summary>
        /// Warns the developer if this object does not have
        /// a public property with the specified name. This 
        /// method does not exist in a Release build.
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // If you raise PropertyChanged and do not specify a property name,
            // all properties on the object are considered to be changed by the binding system.
            if (String.IsNullOrEmpty(propertyName))
                return;

            //PropertyInfo[] properties = () => {
                
            //    PropertyInfo[] props;
            //    Type objType = null;
            //    while(objType != typeof(object)) 
            //    {
            //        objType = this;
            //        props = objType.get
            //    }
            //}
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            foreach (var prop in this.GetType().GetRuntimeProperties())
            {
                if (prop.Name == propertyName)
                {
                    return;
                }
            }
            string msg = "Invalid property name: " + propertyName;

            if (this.ThrowOnInvalidPropertyName)
                throw new ArgumentException(msg);
            else
                Debug.Assert(false, msg);
        }

        /// <summary>
        /// Returns whether an exception is thrown, or if a Debug.Fail() is used
        /// when an invalid property name is passed to the VerifyPropertyName method.
        /// The default value is false, but subclasses used by unit tests might 
        /// override this property's getter to return true.
        /// </summary>
        [NotNavigable]
        protected virtual bool ThrowOnInvalidPropertyName { get; private set; }

        #endregion // Debugging Aides
        //---------------------------------------------------------------------------------------------------

        #region Members.INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                if (!_PropertyChangedSubscribers.Contains(value))
                {
                    _PropertyChangedEvent += value;
                    _PropertyChangedSubscribers.Add(value);
                }
            }
            remove
            {
                _PropertyChangedEvent -= value;
                _PropertyChangedSubscribers.Remove(value);
            }
        }
        #endregion
    }
}

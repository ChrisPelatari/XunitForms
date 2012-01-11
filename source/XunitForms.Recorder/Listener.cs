#region Copyright (c) 2003-2005, Luke T. Maxon

/********************************************************************************************************************
'
' Copyright (c) 2003-2005, Luke T. Maxon
' All rights reserved.
' 
' Redistribution and use in source and binary forms, with or without modification, are permitted provided
' that the following conditions are met:
' 
' * Redistributions of source code must retain the above copyright notice, this list of conditions and the
' 	following disclaimer.
' 
' * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and
' 	the following disclaimer in the documentation and/or other materials provided with the distribution.
' 
' * Neither the name of the author nor the names of its contributors may be used to endorse or 
' 	promote products derived from this software without specific prior written permission.
' 
' THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED
' WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
' PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
' ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
' LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
' INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
' OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN
' IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
'
'*******************************************************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace Xunit.Extensions.Forms.Recorder
{
    public delegate void EventHappened(Type testerType, object control, Action action);

    ///<summary>
    /// A <see cref="Listener"/> is used by a <see cref="Recorder"/>
    /// to manage events happening on a target control.
    ///</summary>
    public class Listener
    {
        private string actionInProcess;
        private object controlInProcess;
        private List<Form> formsIAmListeningTo = new List<Form>();
        private bool inProcess = false;
        private SupportedEventsRegistry registry;
        private Type testerTypeInProcess;

        ///<summary>
        /// Constructs a new <see cref="Listener"/>.
        ///</summary>
        public Listener()
        {
            registry = new SupportedEventsRegistry(this);
        }

        public event EventHappened Event;

        public void ListenTo(Control control)
        {
            if (control is Form)
            {
                formsIAmListeningTo.Add((Form) control);
            }

            // Todo: This is problably not the correct way of solving the 
            // problem but it solves the problem with ToolStripComboBox,
            // ToolStripTextBox and ToolStripProgressBar
            // We here test if the item parent is a Toolbar,context menu or menu. If so and is 
            // not a ToolStripItem we do no add an eventlistener.
            /*
      if (!(control is ToolStripItem) && (control.Parent != null) && (control.Parent.GetType() == typeof(ToolStrip) ||
        control.Parent.GetType() == typeof(MenuStrip) || control.Parent.GetType() == typeof(ContextMenuStrip)))
      {
        return;
      }
      */

            if (!string.IsNullOrEmpty(control.Name))
            {
                AddEventListeners(control);
            }
            ListenTo(control.ContextMenu);

            if (control is ToolStrip)
            {
                ToolStrip toolstrip = (ToolStrip) control;
                if (control.ContextMenuStrip != null)
                {
                    ListenTo(control.ContextMenuStrip);
                }

                foreach (ToolStripItem item in toolstrip.Items)
                {
                    if (item is ToolStripControlHost)
                    {
                        ToolStripControlHost host = (ToolStripControlHost) item;
                        ListenTo(host.Control);
                    }

                    AddEventListeners(item);

                    if (item is ToolStripDropDownItem)
                    {
                        ToolStripDropDownItem dropdown = (ToolStripDropDownItem) item;
                        ListenTo(dropdown.DropDownItems);
                    }
                }
            }

            if (!string.IsNullOrEmpty(control.Name))
            {
                AddPropertyAssertListeners(control);
            }
            control.ControlAdded += new ControlEventHandler(ControlAdded);


            foreach (Control c in control.Controls)
            {
                ListenTo(c);
            }
        }

        private void ListenTo(ToolStripItemCollection collection)
        {
            if (collection != null)
            {
                foreach (ToolStripItem item in collection)
                {
                    AddEventListeners(item);
                    if (item is ToolStripControlHost)
                    {
                        ToolStripControlHost host = (ToolStripControlHost) item;
                        ListenTo(host.Control);
                    }
                    if (item is ToolStripDropDownItem)
                    {
                        ToolStripDropDownItem dropdown = (ToolStripDropDownItem) item;
                        ListenTo(dropdown.DropDownItems);
                    }
                }
            }
        }

        private void ListenTo(Menu menu)
        {
            if (menu == null)
            {
                return;
            }
            AddEventListeners(menu);
            foreach (MenuItem item in menu.MenuItems)
            {
                ListenTo(item);
            }
        }

        private void AddPropertyAssertListeners(Control control)
        {
            PropertyInfo[] properties = control.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in properties)
            {
                AddAssertMenuItem(propertyInfo, control);
            }
        }

        private void AddAssertMenuItem(PropertyInfo propertyInfo, Control control)
        {
            if (propertyInfo.PropertyType.IsPrimitive || propertyInfo.PropertyType.Equals(typeof (string)))
            {
                string propertyName = propertyInfo.Name;
                EventHandler recorder = registry.PropertyAssertHandler(control.GetType());
                if (recorder != null)
                {
                    AddAssertMenuItem(control, propertyName, recorder);
                }
            }
        }

        private void AddEventListeners(object control)
        {
            EventInfo[] events = control.GetType().GetEvents();

            foreach (EventInfo eventInfo in events)
            {
                string eventName = eventInfo.Name;
                MulticastDelegate recorder = registry.EventHandler(control.GetType(), eventName);
                if (recorder != null)
                {
                    eventInfo.AddEventHandler(control, recorder);
                    AddEventHandlerAtStartOfChain(eventInfo, control, recorder);
                }
            }
        }

        private void AddEventHandlerAtStartOfChain(EventInfo eventInfo, object control, MulticastDelegate recorder)
        {
            //this will not work for all events as they are not implemented consistently
            //TODO: there will be more special cases here eventually.
            PropertyInfo eventsProp =
                control.GetType().GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance);
            EventHandlerList handlers = (EventHandlerList) eventsProp.GetValue(control, null);

            Recorder rec = (Recorder) recorder.Target;
            FieldInfo eventKey = rec.EventKey(eventInfo.Name);

            if (eventKey == null)
            {
                return;
            } //actually this should probably allow the exception
            //most of the time.. this could hide a problem!
            //TODO: investigate.

            object key = eventKey.GetValue(control);

            handlers[key] = Delegate.Combine(recorder, handlers[key]);
        }

        private void AddAssertMenuItem(Control control, string name, EventHandler handler)
        {
            ContextMenu menu = control.ContextMenu;
            if (menu == null)
            {
                menu = new ContextMenu();
                control.ContextMenu = menu;
            }
            menu.MenuItems.Add(new MenuItem(name, handler));
        }

        public void FireEvent(Type testerType, object control, string name, params object[] args)
        {
            if (!InProcess(testerType, control, name))
            {
                OnEvent(testerType, control, new EventAction(name, args));
            }
            CheckForNewForms();
        }

        public void FireEvent(Type testerType, object control, EventAction action)
        {
            if (!InProcess(testerType, control, action.MethodName))
            {
                OnEvent(testerType, control, action);
            }
            CheckForNewForms();
        }

        public void FireEvent(Type testerType, object control, PropertyAssertAction action)
        {
            OnEvent(testerType, control, action);
            CheckForNewForms();
        }

        private bool InProcess(Type testerType, object control, string action)
        {
            if (inProcess)
            {
                if ((testerType == testerTypeInProcess) && (control == controlInProcess) && (action == actionInProcess))
                {
                    inProcess = false;
                }
                return true;
            }
            else
            {
                inProcess = true;
                testerTypeInProcess = testerType;
                controlInProcess = control;
                actionInProcess = action;
                return false;
            }
        }

        protected void OnEvent(Type testerType, object control, Action action)
        {
            if (Event != null)
            {
                Event(testerType, control, action);
            }
        }

        private void ControlAdded(object sender, ControlEventArgs e)
        {
            ListenTo(e.Control);
        }

        private void CheckForNewForms()
        {
            List<Form> forms = new FormFinder().FindAll();
            foreach (Form form in forms)
            {
                if (!(form is AppForm))
                {
                    if (!formsIAmListeningTo.Contains(form))
                    {
                        ListenTo(form);
                    }
                }
            }
        }
    }
}
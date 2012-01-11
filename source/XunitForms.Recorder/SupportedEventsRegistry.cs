#region Copyright (c) 2003-2004, Luke T. Maxon

/********************************************************************************************************************
'
' Copyright (c) 2003-2004, Luke T. Maxon
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
using System.Reflection;
using System.Windows.Forms;

namespace Xunit.Extensions.Forms.Recorder
{
    using EventMap = System.Collections.Generic.Dictionary<string, Delegate>;

    internal class SupportedEventsRegistry
    {
        private const string PropertyAssert = "PropertyAssert";
        private static Dictionary<Type, Type> mapArgsToHandler;

        //table contains:
        //  key = type of a control
        //  value = hashtable where
        //           key = name of event
        //           value = delegate method listening to that event.
        private Dictionary<Type, EventMap> eventTable = new Dictionary<Type, EventMap>();

        static SupportedEventsRegistry()
        {
            mapArgsToHandler = new Dictionary<Type, Type>();

            mapArgsToHandler[typeof (TreeViewEventArgs)] = typeof (TreeViewEventHandler);
            mapArgsToHandler[typeof (LinkLabelLinkClickedEventArgs)] = typeof (LinkLabelLinkClickedEventHandler);
        }

        public SupportedEventsRegistry(Listener listener)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (Censor.Contains(assembly.FullName.Substring(0, assembly.FullName.IndexOf(","))))
                    continue;

                try
                {
                    foreach (Type type in assembly.GetTypes())
                    {
                        if (type.IsSubclassOf(typeof (Recorder)) && !type.IsAbstract)
                        {
                            AddRecorderToRegistry(type, listener);
                        }
                    }
                }
                catch (ReflectionTypeLoadException)
                {
                    // Skip assemblies we can't load from.
                    // This lets tests run under JetBrains UnitRunner.
                }
            }
        }

        public MulticastDelegate EventHandler(Type type, string eventName)
        {
            EventMap events;
            Type thisType = type;
            bool found;
            do
            {
                found = eventTable.TryGetValue(thisType, out events);
                thisType = thisType.BaseType;
            } while (!found && thisType != typeof (object));

            if (events == null)
                return null;

            Delegate output;
            if (events.TryGetValue(eventName, out output))
                return (MulticastDelegate) output;
            else
                return null;
        }

        public EventHandler PropertyAssertHandler(Type type)
        {
            return (EventHandler) EventHandler(type, PropertyAssert);
        }

        private void AddRecorderToRegistry(Type type, Listener listener)
        {
            Recorder recorder = (Recorder) Activator.CreateInstance(type, new object[] {listener});

            EventMap events = new EventMap();

            AddEventHandlersToRegistry(recorder, type, events);
            AddPropertyAssertHandlersToRegistry(recorder, events);

            eventTable.Add(recorder.RecorderType, events);
        }

        private static void AddEventHandlersToRegistry(Recorder recorder, Type type, EventMap events)
        {
            ICollection<string> eventNames = Types.GetEventNames(recorder.RecorderType);
            foreach (MethodInfo info in type.GetMethods())
            {
                if (!eventNames.Contains(info.Name))
                    continue;

                Delegate handler = Delegate.CreateDelegate(EventHandlerType(info), recorder, info.Name, false);
                events.Add(info.Name, handler);
            }
        }

        private static void AddPropertyAssertHandlersToRegistry(Recorder recorder, EventMap events)
        {
            if (Types.HasPublicProperties(recorder.TesterType))
            {
                Delegate handler = Delegate.CreateDelegate(typeof (EventHandler), recorder, PropertyAssert, false);
                events.Add(PropertyAssert, handler);
            }
        }

        private static Type EventHandlerType(MethodInfo info)
        {
            ParameterInfo[] parms = info.GetParameters();

            Type handlerType;
            if (parms.Length == 2 && mapArgsToHandler.TryGetValue(parms[1].ParameterType, out handlerType))
                return handlerType;
            else
                return typeof (EventHandler);
        }
    }
}
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
using System.Reflection;
using System.Windows.Forms;

namespace Xunit.Extensions.Forms.Recorder
{
    ///<summary>
    /// This is the abstract base class for all event recorder classes
    /// that don't depend on a concrete object type.
    ///</summary>
    /// <remarks>
    /// This class is inherited by <see cref="ControlRecorder"/>
    /// and <see cref="ToolStripRecorder"/> which provide
    /// control-specific services.
    /// </remarks>
    public abstract class Recorder : IRecorder
    {
        private Listener listener;

        /// <summary>
        /// Constructs a new <see cref="Recorder"/>.
        /// </summary>
        /// <param name="listener">The <see cref="Listener"/> to use with this <see cref="Recorder"/>.</param>
        protected Recorder(Listener listener)
        {
            this.listener = listener;
        }

        #region IRecorder Members

        /// <summary>
        /// Gets the type of object being recorded.
        /// </summary>
        public abstract Type RecorderType { get; }

        /// <summary>
        /// Gets the type of the <see cref="ControlTester"/>
        /// being used.
        /// </summary>
        public abstract Type TesterType { get; }

        ///<summary>
        /// Gets the <see cref="Listener"/> associated with this <see cref="Recorder"/>.
        ///</summary>
        public Listener Listener
        {
            get { return listener; }
        }

        /// <summary>
        /// Returns the "Event Key" for the given named event.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The <see cref="Control"/> base class defines a large number of events for use in user code, 
        /// which are inherited by all derived classes. Instead of members for each possible event handler,
        /// a single hash table is used to store event handlers. This hash table is keyed on 
        /// private static readonly objects whose names correspond to event handlers.
        /// </para>
        /// <para>
        /// The <see cref="Listener"/> class retreives these objects in order to manually combine event handler delegates
        /// at run time.
        /// </para>
        /// </remarks>
        /// <param name="name">The name of the event.</param>
        /// <returns>A <see cref="FieldInfo"/> object representing the event key for the named event.</returns>
        public virtual FieldInfo EventKey(string name)
        {
            return EventKey(name, RecorderType);
        }

        #endregion

        protected virtual FieldInfo EventKey(string name, Type type)
        {
            FieldInfo key = type.GetField("Event" + name, BindingFlags.Static | BindingFlags.NonPublic);
            if (key != null)
            {
                return key;
            }

            key = type.GetField("EVENT_" + name.ToUpper(), BindingFlags.Static | BindingFlags.NonPublic);
            if (key != null)
            {
                return key;
            }

            if (name == "TextChanged")
            {
                key = type.GetField("EventText", BindingFlags.Static | BindingFlags.NonPublic);
            }
            if (key != null)
            {
                return key;
            }

            if (type == typeof (object))
            {
                return null;
            }

            return EventKey(name, type.BaseType);
        }
    }
}
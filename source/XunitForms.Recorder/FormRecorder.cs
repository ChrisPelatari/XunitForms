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
using System.Windows.Forms;

namespace Xunit.Extensions.Forms.Recorder
{
    /// <summary>
    /// A <see cref="ControlRecorder"/> class for <see cref="Form"/>s.
    /// </summary>
    public class FormRecorder : ControlRecorder
    {
        /// <summary>
        /// Constructes a new <see cref="FormRecorder"/> with the given listener.
        /// </summary>
        public FormRecorder(Listener listener) : base(listener)
        {
        }

        /// <summary>
        /// The type of control being recorded, <see cref="Form"/>.
        /// </summary>
        public override Type RecorderType
        {
            get { return typeof (Form); }
        }

        /// <summary>
        /// The tester type for this recorder, <see cref="FormTester"/>.
        /// </summary>
        public override Type TesterType
        {
            get { return typeof (FormTester); }
        }

        /// <summary>
        /// Fires the "Close" event for a form.
        /// </summary>
        public void Closed(object sender, EventArgs args)
        {
            Listener.FireEvent(TesterType, sender, "Close");
        }
    }
}
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
    public class ListBoxRecorder : ControlRecorder
    {
        public ListBoxRecorder(Listener listener) : base(listener)
        {
        }

        public override Type RecorderType
        {
            get { return typeof (ListBox); }
        }

        public override Type TesterType
        {
            get { return typeof (ListBoxTester); }
        }

        public void SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox list = (ListBox) sender;
            if (list.SelectionMode == SelectionMode.One)
            {
                FireSingleSelection(list);
            }
            else if (list.SelectionMode == SelectionMode.MultiSimple || list.SelectionMode == SelectionMode.MultiExtended)
            {
                FireMultipleSelection(list);
            }
        }

        private void FireSingleSelection(ListBox list)
        {
            EventAction action = new EventAction("Select", list.SelectedIndex);
            action.Comment = list.Text;
            Listener.FireEvent(TesterType, list, action);
        }

        private void FireMultipleSelection(ListBox list)
        {
            //HACK: SelectedItem does not return last item selected or indicate on/off so 
            //unless we have a smarter plan just issue clear and setselected
            CompositeAction actions = new CompositeAction("MultipleSelections");
            actions.Add("ClearSelected");

            foreach (int index in list.SelectedIndices)
            {
                EventAction action = new EventAction("SetSelected", index, true);
                action.Comment = list.Items[index].ToString();
                actions.Add(action);
            }

            Listener.FireEvent(TesterType, list, actions);
        }
    }
}
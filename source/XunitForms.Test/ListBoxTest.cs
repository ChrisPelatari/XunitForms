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
using System.Collections.Specialized;
using System.Windows.Forms;
using Xunit;
using Should.Fluent;

namespace Xunit.Extensions.Forms.TestApplications
{
    
    public class ListBoxTest : XunitFormTest
    {
        public ListBoxTest()
        {
            new ListBoxTestForm().Show();
        }

        private string[] rainbowArray = new string[] {"Red", "Orange", "Yellow", "Green", "Blue", "Indigo", "Violet"};

        [Fact]
        public void ListBoxMultiSelection()
        {
            ListBoxTester myListBox = new ListBoxTester("myListBox");

            string[] alternateColors = new string[] {"Red", "Yellow", "Blue", "Violet"};
            StringCollection alternates = new StringCollection();
            alternates.AddRange(alternateColors);

            myListBox.ClearSelected();

            foreach (string color in alternates)
            {
                myListBox.SetSelected(color, true);
            }

            myListBox.Properties.SelectedItems.Count.Should().Equal(4);

            foreach (object selectedItem in myListBox.Properties.SelectedItems)
            {
                alternates.Contains(Convert.ToString(selectedItem)).Should().Equal(true);
            }
        }

        [Fact]
        public void ListBoxPropertyAsserts()
        {
            ListBoxTester myListBox = new ListBoxTester("myListBox");
            myListBox.Properties.Visible.Should().Equal(true);
            myListBox.Properties.SelectedItem.Should().Be.Null();
            myListBox.Properties.SelectionMode.Should().Equal(SelectionMode.MultiExtended);
        }

        [Fact]
        public void ListBoxSelection()
        {
            LabelTester myLabel = new LabelTester("myLabel");
            ListBoxTester myListBox = new ListBoxTester("myListBox");

            myListBox.ClearSelected();

            myListBox.Select(rainbowArray[0]);
            myLabel.Text.Should().Equal(rainbowArray[0]);
        }

        [Fact]
        public void ListBoxSelectionBad()
        {
			Assert.Throws<FormsTestAssertionException>(() => { new ListBoxTester("myListBox").Select("NotFound"); });
        }

        [Fact]
        public void SelectDoesNotExist()
        {
            ListBoxTester myListBox = new ListBoxTester("myListBox");
            Assert.Throws<ArgumentOutOfRangeException>(() => myListBox.SetSelected("blah", true));
        }
    }
}
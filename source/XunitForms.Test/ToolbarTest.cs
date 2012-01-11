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

//Contributed by: Ian Cooper

using Xunit;

namespace Xunit.Extensions.Forms.TestApplications
{
    
    public class ToolbarTest : XunitFormTest
    {
        private LabelTester labelToolbarSelection;
        private ToolBarTester toolbarTest;


        public override void Setup()
        {
            new ToolbarTestForm().Show();
            labelToolbarSelection = new LabelTester("labelToolbarSelection");
            toolbarTest = new ToolBarTester("toolBarTest");
        }

        [Fact]
        public void ClickToolbarButton()
        {
            ToolBarButtonTester button = toolbarTest.GetButton("Open");
            button.Click();
            Assert.Equal("Open", labelToolbarSelection.Text);

            button = toolbarTest.GetButton("Previous");
            button.Click();
            Assert.Equal("Previous", labelToolbarSelection.Text);
        }

        [Fact]
        public void DropDownButtonTest()
        {
            ToolBarButtonTester button = toolbarTest.GetButton("Color");
            button.ClickDropDownMenuItem("Red");
            Assert.Equal("Red", labelToolbarSelection.Text);

            button = toolbarTest.GetButton("Color");
            button.ClickDropDownMenuItem("Violet");
            Assert.Equal("Violet", labelToolbarSelection.Text);
        }

        [Fact]
        public void GetToolbar()
        {
            Assert.True(toolbarTest.Properties.Visible);
        }

        [Fact]
        public void ToggleButtonTest()
        {
            ToolBarButtonTester button = toolbarTest.GetButton(7);
            button.Push();
            Assert.True(button.Pushed);

            button.PartialPush();
            Assert.True(button.PartialPushed);
            button.PartialPush();
            Assert.False(button.PartialPushed);
        }
    }
}
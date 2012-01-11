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
using Xunit;
using Should.Fluent;

namespace Xunit.Extensions.Forms.TestApplications
{
    
    public class ButtonTest : XunitFormTest
    {
        private LabelTester label;

        private ButtonTester button;

        public ButtonTest()
        {
            new ButtonTestForm().Show();
            button = new ButtonTester("myButton");
            label = new LabelTester("myLabel");
        }

        [Fact]
        public void ButtonClick()
        {
            label.Text.Should().Equal("0");
            button.Click();
            label.Text.Should().Equal("1");
        }

        [Fact]
        public void ButtonText()
        {
            button.Text.Should().Equal("button1");
        }

        [Fact]
        public void Click_ThrowsException_IfNotEnabled()
        {
            button.Properties.Enabled = false;
            Assert.Throws<ControlNotEnabledException>(() => button.Click());
        }

        [Fact]
        public void Click_ThrowsException_IfNotVisible()
        {
            button.Properties.Visible = false;
            Assert.Throws<ControlNotVisibleException>(() => button.Click());
        }

        [Fact]
        public void FireEvent()
        {
            label.Text.Should().Equal("0");
            button.FireEvent("Click");
            label.Text.Should().Equal("1");
        }

        [Fact]
        public void FireEventWithArg()
        {
            label.Text.Should().Equal("0");
            button.FireEvent("Click", new EventArgs());
            label.Text.Should().Equal("1");
        }
    }
}
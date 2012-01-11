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

using Xunit;

namespace Xunit.Extensions.Forms.TestApplications
{
    
    public class RadioButtonTest : XunitFormTest
    {
        public override void Setup()
        {
            new RadioButtonTestForm().Show();
        }

        [Fact]
        public void SelectOption()
        {
            RadioButtonTester rbRed = new RadioButtonTester("rbRed");
            LabelTester lblSelectedColor = new LabelTester("lblSelectedColor");
            RadioButtonTester rbOrange = new RadioButtonTester("rbOrange");
            RadioButtonTester rbGreen = new RadioButtonTester("rbGreen");
            RadioButtonTester rbYellow = new RadioButtonTester("rbYellow");
            RadioButtonTester rbBlue = new RadioButtonTester("rbBlue");
            RadioButtonTester rbIndigo = new RadioButtonTester("rbIndigo");
            RadioButtonTester rbViolet = new RadioButtonTester("rbViolet");

            rbRed.Click();
            Assert.Equal("Red", lblSelectedColor.Properties.Text);
            Assert.Equal(true, rbRed.Properties.Checked);
            Assert.Equal(false, rbOrange.Properties.Checked);
            Assert.Equal("Red", rbRed.Text);
            Assert.Equal("Red", rbRed.Properties.Text);

            rbOrange.Click();
            Assert.Equal("Orange", lblSelectedColor.Properties.Text);
            Assert.Equal(false, rbRed.Properties.Checked);
            Assert.Equal(true, rbOrange.Properties.Checked);
            Assert.Equal("Orange", rbOrange.Text);
            Assert.Equal("Orange", rbOrange.Properties.Text);
        }
    }
}
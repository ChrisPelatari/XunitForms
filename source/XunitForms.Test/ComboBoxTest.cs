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
    
    public class ComboBoxTest : XunitFormTest
    {
        private ComboBoxTester box;

        public override void Setup()
        {
            new ComboBoxTestForm().Show();
            box = new ComboBoxTester("myComboBox");
        }

        [Fact]
        public void MethodInvokeOnControlTester()
        {
            object foundIndex = box.Invoke("FindStringExact", "three");
            Assert.Equal(2, foundIndex);
        }

        [Fact]
        public void Select()
        {
            box.Select(2);
            Assert.Equal("three", box.Text);
        }

        [Fact]
        public void SelectByBadText()
        {
            Assert.Throws<FormsTestAssertionException>(() => box.Select("not-in-the-box"));
        }

        [Fact]
        public void SelectByText()
        {
            box.Select("three");
            Assert.Equal("three", box.Text);
        }

        [Fact]
        public void TextBox()
        {
            Assert.Equal("None", box.Text);
            box.Enter("Text");
            Assert.Equal("Text", box.Text);
        }
    }
}
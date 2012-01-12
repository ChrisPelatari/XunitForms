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

using System.Windows.Forms;
using Xunit;
using Should.Fluent;

namespace Xunit.Extensions.Forms.TestApplications
{
    
    public class ModalDialogsTest : XunitFormTest
    {
        public void MessageBoxOkHandler(string name, System.IntPtr hWnd)
        {
            MessageBoxTester messageBox = new MessageBoxTester(hWnd);
            messageBox.Text.Should().Equal("test string");
            messageBox.Title.Should().Equal("caption");
            messageBox.ClickOk();
        }

        public void MessageBoxCancelHandler(string name, System.IntPtr hWnd)
        {
            MessageBoxTester messageBox = new MessageBoxTester(hWnd);
            messageBox.Text.Should().Equal("test string");
            messageBox.Title.Should().Equal("caption");
            messageBox.ClickCancel();
        }

        public void SimpleOKHandler(string name, System.IntPtr hWnd)
        {
            MessageBoxTester messageBox = new MessageBoxTester(hWnd);
            messageBox.Text.Should().Equal("Just An OK Button");
            messageBox.Title.Should().Equal("JustOK");
            messageBox.SendCommand(MessageBoxTester.Command.OK);
        }

        public void OKAndCancelHandler(string name, System.IntPtr hWnd)
        {
            MessageBoxTester messageBox = new MessageBoxTester(hWnd);
            messageBox.SendCommand(MessageBoxTester.Command.Cancel);
        }

        [Fact]
        public void NoModalFound()
        {
			Assert.Throws<ControlNotVisibleException>(() => { string text = new MessageBoxTester("NotFound").Text; });
        }

        [Fact]
        public void TestMessageBoxCancel()
        {
            DialogBoxHandler = MessageBoxCancelHandler;
            MessageBox.Show("test string", "caption", MessageBoxButtons.OKCancel);
        }

        [Fact]
        public void TestMessageBoxOK()
        {
            DialogBoxHandler = MessageBoxOkHandler;
            MessageBox.Show("test string", "caption");
        }

        [Fact]
        public void TestOKCancelMessageBox()
        {
            DialogBoxHandler = OKAndCancelHandler;
            MessageBox.Show("Both OK and Cancel buttons", "OKAndCancel", MessageBoxButtons.OKCancel).Should().Equal(DialogResult.Cancel);
        }

        [Fact]
        public void TestSimpleMessageBox()
        {
            DialogBoxHandler = SimpleOKHandler;
            MessageBox.Show("Just An OK Button", "JustOK", MessageBoxButtons.OK).Should().Equal(DialogResult.OK);
        }

        [Fact]
        public void UnexpectedModalIsClosedAndFails()
        {
            MessageBox.Show("I didn't expect this!", "blah");
            Assert.Throws<FormsTestAssertionException>(() => Verify());
        }

        [Fact]
        public void UnexpectedModalIsClosedAndFailsNoTitle()
        {
			MessageBox.Show("I didn't expect this!"); // no title specified
			Assert.Throws<FormsTestAssertionException>(() => Verify());
        }
    }
}
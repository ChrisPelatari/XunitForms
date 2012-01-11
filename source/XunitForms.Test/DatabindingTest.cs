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
using Should.Fluent;

namespace Xunit.Extensions.Forms.TestApplications
{
    
    public class DatabindingTest : XunitFormTest
    {
        public void falsehandler(string name, System.IntPtr hWnd, System.Windows.Forms.Form form)
        {
            MessageBoxTester mb = new MessageBoxTester(hWnd);
            mb.Text.Should().Equal("False");
            mb.ClickOk();
        }

        public void truehandler(string name, System.IntPtr hWnd, System.Windows.Forms.Form form)
        {
            MessageBoxTester mb = new MessageBoxTester(hWnd);
            mb.Text.Should().Equal("True");
            mb.ClickOk();
        }

        public void oldhandler(string name, System.IntPtr hWnd, System.Windows.Forms.Form form)
        {
            MessageBoxTester mb = new MessageBoxTester(hWnd);
            mb.Text.Should().Equal("Old");
            mb.ClickOk();
        }

        public void newhandler(string name, System.IntPtr hWnd, System.Windows.Forms.Form form)
        {
            MessageBoxTester mb = new MessageBoxTester(hWnd);
            mb.Text.Should().Equal("New");
            mb.ClickOk();
        }

        [Fact]
        public void CheckBoxDataSetBinding()
        {
            CheckBoxDataSetBindingTestForm f;
            f = new CheckBoxDataSetBindingTestForm();
            f.Show();
            ModalFormHandler = falsehandler;
            new ButtonTester("btnView").Click();

            new CheckBoxTester("myCheckBox").Check();

            ModalFormHandler = truehandler;
            new ButtonTester("btnView").Click();
            f.Close();
        }

        [Fact]
        public void DataSetBindingWithGenericPropertySetter()
        {
            TextBoxDataSetBindingTestForm f;
            f = new TextBoxDataSetBindingTestForm();
            f.Show();
            ModalFormHandler = oldhandler;
            new ButtonTester("btnView").Click();

            new TextBoxTester("myTextBox")["Text"] = "New";

            ModalFormHandler = newhandler;
            new ButtonTester("btnView").Click();
            f.Close();
        }

        [Fact]
        public void TextBoxDataSetBinding()
        {
            TextBoxDataSetBindingTestForm f;
            f = new TextBoxDataSetBindingTestForm();
            f.Show();
            ModalFormHandler = oldhandler;

            new ButtonTester("btnView").Click();

            new TextBoxTester("myTextBox").Enter("New");
            ModalFormHandler = newhandler;
            new ButtonTester("btnView").Click();
            f.Close();
        }
    }
}
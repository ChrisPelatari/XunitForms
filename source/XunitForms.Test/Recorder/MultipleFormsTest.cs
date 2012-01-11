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

using Xunit.Extensions.Forms.TestApplications;
using Xunit;
using System.ComponentModel;

namespace Xunit.Extensions.Forms.Recorder.Test
{
    
    [Category("Recorder")]
    public class MultipleFormsTest : XunitFormTest
    {
        [Fact]
        public void EventCausesAnother()
        {
            MultiForm form = new MultiForm();
            form.Show();
            TestWriter writer = new TestWriter(form);
            ButtonTester button = new ButtonTester("btnClose");
            button.Click();
			Assert.Throws<NoSuchControlException>(() => button.Click());

            Assert.Equal(
                @"[Fact]
public void Test()
{

	ButtonTester btnClose = new ButtonTester(""btnClose"");

	btnClose.Click();

}",
                writer.Test);
        }

        [Fact]
        public void FormClose()
        {
            MultiForm form = new MultiForm();
            form.Show();
            TestWriter writer = new TestWriter(form);
            ButtonTester button = new ButtonTester("myButton");
            button.Click();
            FormTester form0 = new FormTester("Form-0");
            form0.Close();

            Assert.Equal(
                @"[Fact]
public void Test()
{

	ButtonTester myButton = new ButtonTester(""myButton"");
	FormTester Form-0 = new FormTester(""Form-0"");

	myButton.Click();
	Form-0.Close();

}",
                writer.Test);
        }

        [Fact]
        public void MultipleForms()
        {
            MultiForm form = new MultiForm();
            form.Show();
            TestWriter writer = new TestWriter(form);
            Assert.Equal("", writer.Test);

            ButtonTester button = new ButtonTester("myButton");
            button.Click();
            ButtonTester button2 = new ButtonTester("myButton", "Form-0");
            button2.Click();

            Assert.Equal(
                @"[Fact]
public void Test()
{

	ButtonTester myButton = new ButtonTester(""myButton"");
	ButtonTester Form-0_myButton = new ButtonTester(""myButton"", ""Form-0"");

	myButton.Click();
	Form-0_myButton.Click();

}",
                writer.Test);
        }

        [Fact]
        public void NamesShouldAdapt()
        {
            MultiForm form = new MultiForm();
            form.Show();
            TestWriter writer = new TestWriter(form);
            Assert.Equal("", writer.Test);

            ButtonTester nothingButton = new ButtonTester("nothingButton");
            nothingButton.Click();

            //------------------------------------------------------

            Assert.Equal(
                @"[Fact]
public void Test()
{

	ButtonTester nothingButton = new ButtonTester(""nothingButton"");

	nothingButton.Click();

}",
                writer.Test);

            //------------------------------------------------------

            ButtonTester myButton = new ButtonTester("myButton");
            myButton.Click();

            //------------------------------------------------------

            Assert.Equal(
                @"[Fact]
public void Test()
{

	ButtonTester nothingButton = new ButtonTester(""nothingButton"");
	ButtonTester myButton = new ButtonTester(""myButton"");

	nothingButton.Click();
	myButton.Click();

}",
                writer.Test);

            //------------------------------------------------------

            ButtonTester nothingButton2 = new ButtonTester("nothingButton", "Form-0");
            nothingButton2.Click();
            ButtonTester nothingButton3 = new ButtonTester("nothingButton", "Form");
            nothingButton3.Click();

            //------------------------------------------------------

            Assert.Equal(
                @"[Fact]
public void Test()
{

	ButtonTester Form_nothingButton = new ButtonTester(""nothingButton"", ""Form"");
	ButtonTester myButton = new ButtonTester(""myButton"");
	ButtonTester Form-0_nothingButton = new ButtonTester(""nothingButton"", ""Form-0"");

	Form_nothingButton.Click();
	myButton.Click();
	Form-0_nothingButton.Click();
	Form_nothingButton.Click();

}",
                writer.Test);
            //------------------------------------------------------
        }
    }
}
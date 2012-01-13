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
using Xunit;
using System.ComponentModel;
using Should.Fluent;

namespace Xunit.Extensions.Forms.TestApplications
{
	
	[Category("DisplayHidden")]
	public class SimpleAPIKeyboardTest : XunitFormTest
	{
		public override bool DisplayHidden
		{
			get { return true; }
		}

		[Fact]
		public void PressEnterClicksButton()
		{
			Form form = new ButtonTestForm();
			form.Show();
			LabelTester label = new LabelTester("myLabel", form);
			ButtonTester button = new ButtonTester("myButton", form);

			label.Text.Should().Equal("0");

			Keyboard.UseOn(button);
			Keyboard.Click(Key.RETURN);

			label.Text.Should().Equal("1");
		}

		[Fact]
		public void TextBox()
		{
			Form form = new TextBoxTestForm();
			form.Show();

			TextBoxTester box = new TextBoxTester("myTextBox", form);
			box.Text.Should().Equal("default");

			Keyboard.UseOn(box);

			Keyboard.Click(Key.A);
			Keyboard.Click(Key.B);
			Keyboard.Click("+(c)");
			Keyboard.Click("C");

			box.Text.Should().Equal("abCC");
		}

		[Fact]
		public void TypeShiftAB()
		{
			new TextBoxTestForm().Show();
			TextBoxTester box = new TextBoxTester("myTextBox");
			box.Text.Should().Equal("default");

			Keyboard.UseOn(box);

			Keyboard.Type("+ab");

			box.Text.Should().Equal("Ab");
		}

		[Fact]
		public void TypeSpecialKey()
		{
			new TextBoxTestForm().Show();
			TextBoxTester box = new TextBoxTester("myTextBox");
			box.Text.Should().Equal("default");

			Keyboard.UseOn(box);

			Keyboard.Type("abc1def ghi");

			box.Text.Should().Equal("abc1def ghi");
		}

		[Fact]
		public void ReplaceOneWithDIGIT_1WhenNotInBraces()
		{
			new TextBoxTestForm().Show();
			TextBoxTester box = new TextBoxTester("myTextBox");
			box.Text.Should().Equal("default");

			Keyboard.UseOn(box);

			Keyboard.Type("1231");

			box.Text.Should().Equal("1231");
		}

		[Fact]
		public void ToUpper()
		{
			new TextBoxTestForm().Show();
			TextBoxTester box = new TextBoxTester("myTextBox");
			box.Text.Should().Equal("default");

			Keyboard.UseOn(box);

			Keyboard.Type("a");

			box.Text.Should().Equal("a");
		}

		[Fact]
		public void TypeShiftAGroup()
		{
			new TextBoxTestForm().Show();
			TextBoxTester box = new TextBoxTester("myTextBox");
			box.Text.Should().Equal("default");

			Keyboard.UseOn(box);

			Keyboard.Type("q+(ABC)d");

			box.Text.Should().Equal("qABCd");
		}

		[Fact(Skip="This test leaves keyboard controller in a shift state affecting following test.")]
		public void UnbalancedGroupDelimitersThrowsException()
		{
			new TextBoxTestForm().Show();
			TextBoxTester box = new TextBoxTester("myTextBox");
			box.Text.Should().Equal("default");

			Assert.Throws<ArgumentException>(() => {

				Keyboard.UseOn(box);

				Keyboard.Click(Key.A);
				Keyboard.Press(Key.SHIFT);
			});
		}

		[Fact]
		public void KeyDefinitions_ShiftAndRelease()
		{
			new TextBoxTestForm().Show();
			TextBoxTester box = new TextBoxTester("myTextBox");

			Keyboard.UseOn(box);

			Keyboard.Click(Key.A);
			Keyboard.Click(Key.B);
			Keyboard.Press(Key.SHIFT + Key.C + Key.SHIFT_RELEASE);

			box.Text.Should().Equal("abC");
		}
	}
}
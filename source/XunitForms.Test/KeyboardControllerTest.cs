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
' LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES, LOSS OF USE, DATA, OR PROFITS, OR BUSINESS
' INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
' OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN
' IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
'
'*******************************************************************************************************************/

#endregion

using Xunit.Extensions.Forms.TestApplications;
using Xunit;
using System;
using Should.Fluent;


namespace Xunit.Extensions.Forms.UnitTests
{
	/// <summary>
	/// Test existing KeyboardController functionality.
	/// </summary>
	
    public class KeyboardControllerTest : IDisposable
	{
		private KeyboardController keyboardController;
		private ControlTester textBoxTester;
		private TextBoxTestForm textBoxForm;

		public KeyboardControllerTest()
		{
			textBoxForm = new TextBoxTestForm();
			textBoxForm.Show();

			textBoxTester = new ControlTester("myTextBox");
            textBoxTester.Text.Should().Equal("default");

			keyboardController = new KeyboardController(textBoxTester);	
		}

        public void Dispose() {
            keyboardController.Dispose();
            textBoxForm.Close();
        }

		[Fact]
		public void Type_WithSingleCharacterShift()
		{
			keyboardController.Type("a+bc");
			textBoxTester.Text.Should().Equal("aBc");
		}

		[Fact]
		public void Type_WithGroupedCharacterShift()
		{
			keyboardController.Type("a+(bc)d");
			textBoxTester.Text.Should().Equal("aBCd");
		}

		[Fact]
		public void Type_BACKSPACE()
		{
			keyboardController.Type("123{BACKSPACE}4");
			textBoxTester.Text.Should().Equal("124");
		}

		[Fact]
		public void Type_EscapesFormatCharacters()
		{
			keyboardController.Type("a{+}{(}bc{)}de{%}{[}{]} {{}123{}}");
			textBoxTester.Text.Should().Equal("a+(bc)de%[] {123}");
		}

		[Fact]
		public void Type_PreversesCharacterCase()
		{
			keyboardController.Type("aBcDE");
			textBoxTester.Text.Should().Equal("aBcDE");
		}

		[Fact]
		public void Type_RepeatCharacters()
		{
			keyboardController.Type("-{o 4}-");
			textBoxTester.Text.Should().Equal("-oooo-");
		}

		[Fact]
		public void Type_LEFT()
		{
			keyboardController.Type("12{LEFT}34");
			textBoxTester.Text.Should().Equal("1342");
		}

		[Fact]
		public void Type_RIGHT()
		{
			keyboardController.Type("123{LEFT}{LEFT}{RIGHT}45");
			textBoxTester.Text.Should().Equal("12453");
		}

		[Fact]
		public void Type_HOME()
		{
			keyboardController.Type("123{HOME}5");
			textBoxTester.Text.Should().Equal("5123");
		}
	}
}
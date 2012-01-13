#region Copyright (c) 2003-2007, Luke T. Maxon

/********************************************************************************************************************
'
' Copyright (c) 2003-2007, Luke T. Maxon
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

using Xunit.Extensions.Forms.Win32Interop;
using Xunit;
using Xunit.Extensions.Forms.SendKey;
using Should.Fluent;

namespace Xunit.Extensions.Forms.UnitTests
{
	
	public class SendKeysParserTest
	{
		[Fact]
		public void GroupExtraction()
		{
			ISendKeysParser parser = new SendKeysParser("111+(aaa)22+^(bbb)33{{}4%(a)");

			parser.Groups.Length.Should().Equal(8);

			int groupIndex = 0;
			AssertGroup(parser.Groups[groupIndex++], "", "111");
			AssertGroup(parser.Groups[groupIndex++], "+", "aaa");
			AssertGroup(parser.Groups[groupIndex++], "", "22");
			AssertGroup(parser.Groups[groupIndex++], "+^", "bbb");
			AssertGroup(parser.Groups[groupIndex++], "", "33");
			AssertGroup(parser.Groups[groupIndex++], "", "{");
			AssertGroup(parser.Groups[groupIndex++], "", "4");
			AssertGroup(parser.Groups[groupIndex], "%", "a");
		}

		[Fact]
		public void ShiftModifier()
		{
			ISendKeysParser parser = new SendKeysParser("a+bc");

			parser.Groups.Length.Should().Equal(3);

			int groupIndex = 0;
			AssertGroup(parser.Groups[groupIndex++], "", "a");
			AssertGroup(parser.Groups[groupIndex++], "+", "b");
			AssertGroup(parser.Groups[groupIndex], "", "c");
		}

		[Fact]
		public void ControlModifier()
		{
			ISendKeysParser parser = new SendKeysParser("a^bc");

			parser.Groups.Length.Should().Equal(3);

			int groupIndex = 0;
			AssertGroup(parser.Groups[groupIndex++], "", "a");
			AssertGroup(parser.Groups[groupIndex++], "^", "b");
			AssertGroup(parser.Groups[groupIndex], "", "c");
		}

		[Fact]
		public void AltModifier()
		{
			ISendKeysParser parser = new SendKeysParser("a%bc");

			parser.Groups.Length.Should().Equal(3);

			int groupIndex = 0;
			AssertGroup(parser.Groups[groupIndex++], "", "a");
			AssertGroup(parser.Groups[groupIndex++], "%", "b");
			AssertGroup(parser.Groups[groupIndex], "", "c");
		}

		[Fact]
		public void Key_BACKSPACE()
		{
			AssertKeywordIsParsedAs("{BACKSPACE}", VirtualKeyCodes.BACK);
			AssertKeywordIsParsedAs("{BS}", VirtualKeyCodes.BACK);
			AssertKeywordIsParsedAs("{BKSP}", VirtualKeyCodes.BACK);
		}

		[Fact(Skip="Required but not implemented functionalty")]
		public void Key_BREAK()
		{
			// Need to confirm correct key code
			AssertKeywordIsParsedAs("{BREAK}", VirtualKeyCodes.None);
		}

		[Fact(Skip="Required but not implemented functionalty")]
		public void Key_CAPSLOCK()
		{
			// Need to confirm correct key code
			AssertKeywordIsParsedAs("{CAPSLOCK}", VirtualKeyCodes.CAPITAL);
			AssertKeywordIsParsedAs("{CAP}", VirtualKeyCodes.CAPITAL);
		}

		[Fact]
		public void Key_DELETE()
		{
			AssertKeywordIsParsedAs("{DELETE}", VirtualKeyCodes.DELETE);
			AssertKeywordIsParsedAs("{DEL}", VirtualKeyCodes.DELETE);
		}

		[Fact]
		public void Key_DOWN()
		{
			AssertKeywordIsParsedAs("{DOWN}", VirtualKeyCodes.DOWN);
		}

		[Fact]
		public void Key_END()
		{
			AssertKeywordIsParsedAs("{END}", VirtualKeyCodes.END);
		}

		[Fact]
		public void Key_ENTER()
		{
			AssertKeywordIsParsedAs("{ENTER}", VirtualKeyCodes.RETURN);
		}

		[Fact]
		public void Key_ESC()
		{
			AssertKeywordIsParsedAs("{ESC}", VirtualKeyCodes.ESCAPE);
		}

		[Fact]
		public void Key_HELP()
		{
			AssertKeywordIsParsedAs("{HELP}", VirtualKeyCodes.HELP);
		}

		[Fact]
		public void Key_HOME()
		{
			AssertKeywordIsParsedAs("{HOME}", VirtualKeyCodes.HOME);
		}

		[Fact]
		public void Key_INSERT()
		{
			AssertKeywordIsParsedAs("{INSERT}", VirtualKeyCodes.INSERT);
			AssertKeywordIsParsedAs("{INS}", VirtualKeyCodes.INSERT);
		}

		[Fact(Skip="Required but not implemented functionalty")]
		public void Key_LEFT()
		{
			AssertKeywordIsParsedAs("{LEFT}", VirtualKeyCodes.LEFT);
		}

		[Fact(Skip="Required but not implemented functionalty")]
		public void Key_NUMLOCK()
		{
			AssertKeywordIsParsedAs("{NUMLOCK}", VirtualKeyCodes.NUMLOCK);
		}

		[Fact(Skip="Required but not implemented functionalty")]
		public void Key_PGDN()
		{
			// Need to find the virtual key code
			AssertKeywordIsParsedAs("{PGDN}", VirtualKeyCodes.None);
		}

		[Fact(Skip="Required but not implemented functionalty")]
		public void Key_PGUP()
		{
			// Need to find the virtual key code
			AssertKeywordIsParsedAs("{PGUP}", VirtualKeyCodes.None);
		}

		[Fact(Skip="Required but not implemented functionalty")]
		public void Key_PRTSC()
		{
			// Need to find the virtual key code
			AssertKeywordIsParsedAs("{PRTSC}", VirtualKeyCodes.None);
		}

		[Fact(Skip="Required but not implemented functionalty")]
		public void Key_RIGHT()
		{
			AssertKeywordIsParsedAs("{RIGHT}", VirtualKeyCodes.RIGHT);
		}

		[Fact(Skip="Required but not implemented functionalty")]
		public void Key_SCROLLLOCK()
		{
			// Need to find the virtual key code
			AssertKeywordIsParsedAs("{SCROLLLOCK}", VirtualKeyCodes.None);
		}

		[Fact(Skip="Required but not implemented functionalty")]
		public void Key_SPACE()
		{
			AssertKeywordIsParsedAs("{SPACE}", VirtualKeyCodes.SPACE);
		}

		[Fact(Skip="Required but not implemented functionalty")]
		public void Key_TAB()
		{
			AssertKeywordIsParsedAs("{TAB}", VirtualKeyCodes.TAB);
		}

		[Fact(Skip="Required but not implemented functionalty")]
		public void Key_UP()
		{
			// Need to find the virtual key code
			AssertKeywordIsParsedAs("{UP}", VirtualKeyCodes.UP);
		}

		private static void AssertKeywordIsParsedAs(string keyword, VirtualKeyCodes expectedKey)
		{
			ISendKeysParser parser = new SendKeysParser(keyword);

			parser.Groups.Length.Should().Equal(1);

            parser.Groups[0].ModifierCharacters.Should().Be.Empty();
			parser.Groups[0].EscapedKey.Should().Equal(expectedKey);
            parser.Groups[0].Body.Should().Be.Empty();
		}

		[Fact]
		public void Key_FunctionKeys()
		{
			ISendKeysParser parser = new SendKeysParser("{F1}{F2}{F3}{F4}{F5}{F6}{F7}{F8}{F9}{F10}{F11}{F12}{F13}{F14}{F15}{F16}");

			parser.Groups.Length.Should().Equal(16);

			int groupIndex = 0;
			parser.Groups[groupIndex++].EscapedKey.Should().Equal(VirtualKeyCodes.F1);
			parser.Groups[groupIndex++].EscapedKey.Should().Equal(VirtualKeyCodes.F2);
			parser.Groups[groupIndex++].EscapedKey.Should().Equal(VirtualKeyCodes.F3);
			parser.Groups[groupIndex++].EscapedKey.Should().Equal(VirtualKeyCodes.F4);
			parser.Groups[groupIndex++].EscapedKey.Should().Equal(VirtualKeyCodes.F5);
			parser.Groups[groupIndex++].EscapedKey.Should().Equal(VirtualKeyCodes.F6);
			parser.Groups[groupIndex++].EscapedKey.Should().Equal(VirtualKeyCodes.F7);
			parser.Groups[groupIndex++].EscapedKey.Should().Equal(VirtualKeyCodes.F8);
			parser.Groups[groupIndex++].EscapedKey.Should().Equal(VirtualKeyCodes.F9);
			parser.Groups[groupIndex++].EscapedKey.Should().Equal(VirtualKeyCodes.F10);
			parser.Groups[groupIndex++].EscapedKey.Should().Equal(VirtualKeyCodes.F11);
			parser.Groups[groupIndex++].EscapedKey.Should().Equal(VirtualKeyCodes.F12);
			parser.Groups[groupIndex++].EscapedKey.Should().Equal(VirtualKeyCodes.F13);
			parser.Groups[groupIndex++].EscapedKey.Should().Equal(VirtualKeyCodes.F14);
			parser.Groups[groupIndex++].EscapedKey.Should().Equal(VirtualKeyCodes.F15);
			parser.Groups[groupIndex].EscapedKey.Should().Equal(VirtualKeyCodes.F16);

			foreach(SendKeysParserGroup group in parser.Groups)
			{
				group.ModifierCharacters.Should().Be.Empty();
				group.Body.Should().Be.Empty();
			}
		}

		private static void AssertGroup(ISendKeysParserGroup group, string modifierCharacters, string bodyText)
		{
			group.ModifierCharacters.Should().Equal(modifierCharacters);
			group.Body.Should().Equal(bodyText);
		}
	}
}
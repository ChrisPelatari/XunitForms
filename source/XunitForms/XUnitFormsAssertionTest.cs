using Xunit;

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

namespace Xunit.Extensions.Forms
{
    /// <summary>
    /// One of three base classes for your XunitForms tests.  This one can be
    /// used by people who want "built-in" Assertion functionality and prefer
    /// the older style "Assertion" syntax.
    /// </summary>
    
    public class XunitFormsAssertionTest : XunitFormTest
    {
        public void Assert(bool condition, string message)
        {
            Xunit.Assert.True(condition, message);
        }

        public void Assert(bool condition)
        {
            Xunit.Assert.True(condition);
        }

        public void AssertEquals(object expected, object actual)
        {
            Xunit.Assert.Equal(expected, actual);
        }

        public void AssertEquals(int expected, int actual)
        {
            Xunit.Assert.Equal(expected, actual);
        }

        public void AssertNotNull(object anObject)
        {
            Xunit.Assert.NotNull(anObject);
        }

        public void AssertSame(object expected, object actual)
        {
            Xunit.Assert.Same(expected, actual);
        }
    }
}
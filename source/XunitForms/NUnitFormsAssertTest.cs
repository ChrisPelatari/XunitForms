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

namespace Xunit.Extensions.Forms
{
    /// <summary>
    /// One of three base classes for your XunitForms tests.  This one can be
    /// used by people who want "built-in" Assert functionality and prefer
    /// the newer style "Assert" syntax.
    /// </summary>
    
    public class XunitFormsAssertTest : XunitFormTest
    {
        public void Equal(object expected, object actual)
        {
            Assert.Equal(expected, actual);
        }

        public void Equal(int expected, int actual)
        {
            Assert.Equal(expected, actual);
        }

        public void Equal(decimal expected, decimal actual)
        {
            Assert.Equal(expected, actual);
        }

        public void AreSame(object expected, object actual)
        {
            Assert.Same(expected, actual);
        }

        public void True(bool condition)
        {
            Assert.True(condition);
        }

        public void True(bool condition, string message)
        {
            Assert.True(condition, message);
        }

        public void False(bool condition)
        {
            Assert.False(condition);
        }

        public void False(bool condition, string message)
        {
            Assert.False(condition, message);
        }

        public void Null(object anObject)
        {
            Assert.Null(anObject);
        }

        public void NotNull(object anObject)
        {
            Assert.NotNull(anObject);
        }
    }
}
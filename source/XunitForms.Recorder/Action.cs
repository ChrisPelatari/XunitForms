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

namespace Xunit.Extensions.Forms.Recorder
{
    ///<summary>
    /// The abstract base class for all recordable Actions.
    ///</summary>
    public abstract class Action
    {
        /// <summary>
        /// The reference to the associated definition.
        /// </summary>
        private Definition definition;

        /// <summary>
        /// Sets or gets the <see cref="Definition"/> associated with this <c>Action</c>.
        /// </summary>
        /// <value>
        /// The associated <see cref="Definition"/>.
        /// </value>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if no effective value is given.
        /// </exception>
        public Definition Definition
        {
            get { return definition; }
            set
            {
                if (value != null)
                {
                    definition = value;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Control
        {
            get { return RemoveSpaces(Definition.Name); }
        }

        /// <summary>
        /// Removes all spaces from a string.
        /// </summary>
        /// <param name="name">
        /// Remove all spaces from this string.
        /// </param>
        /// <returns>
        /// <list type="bullet">
        /// <item><paramref name="name"/> without spaces.</item>
        /// <item>if <paramref name="name"/> is not effective, returns an empty string</item>
        /// </list>
        /// </returns>
        private static string RemoveSpaces(string name)
        {
            if (name != null)
            {
                return name.Replace(" ", "");
            }
            else
            {
                return String.Empty;
            }
        }
    }
}
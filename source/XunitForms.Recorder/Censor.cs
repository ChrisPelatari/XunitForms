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

using System.Collections.Generic;

namespace Xunit.Extensions.Forms.Recorder
{
    /// <summary>
    /// This class can be used to prevent <see cref="Recorder"/>-derived
    /// classes from automatically being added to a <see cref="SupportedEventsRegistry"/>.
    /// </summary>
    public static class Censor
    {
        //can suppress the effect of specific recorder assemblies during testing.
        //this is easier (and more hackish) than loading / unloading app domains.

        private static IList<string> CensoredRecorders = new List<string>();

        ///<summary>
        /// Adds the named class to the list of censored <see cref="Recorder"/>s.
        ///</summary>
        public static void Add(string recorderName)
        {
            CensoredRecorders.Add(recorderName);
        }

        /// <summary>
        /// Removes the named class from the list of censored <see cref="Recorder"/>s.
        /// </summary>
        public static void Remove(string recorderName)
        {
            CensoredRecorders.Remove(recorderName);
        }

        ///<summary>
        /// Returns true if the <see cref="Censor"/> is blocking
        /// the given <see cref="Recorder"/> name.
        ///</summary>
        public static bool Contains(string recorderName)
        {
            return CensoredRecorders.Contains(recorderName);
        }
    }
}
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
'   following disclaimer.
' 
' * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and
'   the following disclaimer in the documentation and/or other materials provided with the distribution.
' 
' * Neither the name of the author nor the names of its contributors may be used to endorse or 
'   promote products derived from this software without specific prior written permission.
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

//* Contributed by Levi Khatskevitch */

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Xunit;

namespace Xunit.Extensions.Forms.TestApplications
{
    /// <summary>
    /// Simulates Windows Service envinronment by running the entire test
    /// fixture in a service WindowStation. If these tests pass we can show
    /// modal forms from test cases running inside the Cruise Control .NET
    /// Windows Service as well.
    /// </summary>
    /// <remarks>
    /// While running these tests Xunit GUI won't repaint itself (e.i. update
    /// the progress bar and etc.). This is a normal side effect of temporary
    /// assigning a service (hidden) WindowStation to the process.
    /// </remarks>
    
    [Category("UsesServiceWindowStation")]
    public class ModalFormsInServiceTest : ModalFormsTest
    {
        private const int GENERIC_ALL = 0x10000000;

        [DllImport("user32.dll")]
        private static extern IntPtr GetProcessWindowStation();

        [DllImport("user32.dll")]
        private static extern bool SetProcessWindowStation(IntPtr handle);

        [DllImport("user32.dll")]
        private static extern IntPtr CreateWindowStation(string name, int flags, int access, IntPtr security);

        [DllImport("user32.dll")]
        private static extern bool CloseWindowStation(IntPtr handle);

        private IntPtr originalWinStation;

        private IntPtr serviceWinStation;

        [Fact(Skip="does not work when run from a service")]
        public void PreInit()
        {
            originalWinStation = GetProcessWindowStation();
            if (originalWinStation == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            serviceWinStation = CreateWindowStation(null, 0, GENERIC_ALL, IntPtr.Zero);
            if (serviceWinStation == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            if (!SetProcessWindowStation(serviceWinStation))
            {
                throw new Win32Exception();
            }
        }

        [Fact(Skip="does not work when run from a service")]
        public void PostVerify()
        {
            if (originalWinStation == IntPtr.Zero)
            {
                return;
            }
            SetProcessWindowStation(originalWinStation);

            CloseWindowStation(originalWinStation);
            CloseWindowStation(serviceWinStation);
        }
    }
}
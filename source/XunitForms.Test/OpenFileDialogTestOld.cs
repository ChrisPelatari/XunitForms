#region Copyright (c) 2006-2007, Luke T. Maxon (Authored by Anders Lillrank)

/********************************************************************************************************************
'
' Copyright (c) 2006-2007, Luke T. Maxon
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
using System.IO;
using Xunit;
using Should.Fluent;

namespace Xunit.Extensions.Forms.TestApplications
{
    ///<summary>
    /// Test Fixture for the OpenFileDialogTester class.
    ///</summary>
    
    public class OpenFileDialogTestOld : XunitFormTest
    {
        ///<summary>
        /// Sets up this test by showing a new OpenFileDialogTestForm form.
        ///</summary>
        public OpenFileDialogTestOld()
        {
            new OpenFileDialogTestForm().Show();
        }

        ///<summary>
        /// Modal handler to click the open button.
        ///</summary>
        public void OpenFileHandler()
        {
            OpenFileDialogTester dlg_tester = new OpenFileDialogTester("Open");
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XunitForms.dll");
            dlg_tester.OpenFile(fileName);
        }

        ///<summary>
        /// Modal handler to click the cancel button.
        ///</summary>
        public void CancelFileHandler()
        {
            OpenFileDialogTester dlg_tester = new OpenFileDialogTester("Open");
            dlg_tester.ClickCancel();
        }

        ///<summary>
        /// Tests cancelling.
        ///</summary>
        [Fact, STAThread]
        public void CancelTest()
        {
            LabelTester label1 = new LabelTester("lblFileName");
            ExpectFileDialog("CancelFileHandler");
            ButtonTester open_btn = new ButtonTester("btOpenFile");
            open_btn.Click();
            label1.Text.Should().Equal("cancel pressed");
        }

        ///<summary>
        /// Tests opening a file.
        ///</summary>
        [Fact, STAThread]
        public void OpenTest()
        {
            LabelTester label1 = new LabelTester("lblFileName");
            ExpectFileDialog("OpenFileHandler");
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XunitForms.dll");
            ButtonTester open_btn = new ButtonTester("btOpenFile");
            open_btn.Click();

            label1.Text.ToLower().Should().Equal(fileName.ToLower());
        }

        [Fact(Skip="This test is used for debug confidence, not to prove the functionality of the OpenFileDialogTester."), STAThread]
        public void OpenTest_ConfidenceOnManyOpens()
        {
            for (int count = 0; count < 1000; count++)
            {
                //PROBLEM
                // This test runs most of the time. 
                // Every now and then it will leave an open file dialog on display as though the handler has not been run.
                // When this happens, switching focus away from it then back to it allows the handler to run, 
                // but fails the test when checking the number of invocations.

                //SOLUTION
                /* The 'SetDlgItemText' function in the 'FileDialogTester.cs' file did not set the text 
                 * in the dialog box occassionaly. The solution was to set the text twice on the dialog box.
                 * The code below was run upto 1000 times to check that the solution works. 
                 */

                LabelTester label1 = new LabelTester("lblFileName");
                ExpectFileDialog("OpenFileHandler");
                string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XunitForms.dll");
                ButtonTester open_btn = new ButtonTester("btOpenFile");
                open_btn.Click();

                label1.Text.ToLowerInvariant().Should().Equal(fileName.ToLowerInvariant());
            }
        }
    }
}
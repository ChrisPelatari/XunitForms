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
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Xunit.Extensions.Forms;

namespace Xunit.Extensions.Forms.Recorder
{
    /// <summary>
    /// The main form for the Recorder application.
    /// </summary>
    public partial class AppForm : Form
    {
        private EventHandler handler = null;
        private TestWriter writer = null;

        ///<summary>
        /// Constructs a new <see cref="AppForm"/>.
        ///</summary>
        public AppForm()
        {
            InitializeComponent();
        }

        private void Load_Click(object sender, EventArgs e)
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"D:\opensource\Xunit2.0\bin";
            ofd.Filter = "Dll files (*.dll)|*.dll|Exe files (*.exe)|*.exe|All files (*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.Multiselect = true;
            ofd.ReadOnlyChecked = true;
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string fileName in ofd.FileNames)
                {
                    Assembly.LoadFrom(new FileInfo(fileName).ToString());

                    combo.SelectedIndexChanged -= new EventHandler(combo_SelectedIndexChanged);

                    combo.Items.Clear();
                    foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        foreach (Type t in a.GetTypes())
                        {
                            if (t.IsSubclassOf(typeof (Form)) && ! t.FullName.StartsWith("System."))
                            {
                                combo.Items.Add(t.FullName);
                            }
                        }
                    }
                    combo.SelectedIndex = 0;
                    combo.SelectedIndexChanged += new EventHandler(combo_SelectedIndexChanged);
                }
            }
        }

        public void UpdateTests(object sender, EventArgs args)
        {
            textBox.Text = writer.Test;
            textBox.Select(textBox.TextLength, 0);
            textBox.ScrollToCaret();
        }

        private void combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (handler == null)
            {
                handler = new EventHandler(UpdateTests);
            }
            if (writer != null)
            {
                writer.TestChanged -= handler;
            }

            Type type = Type.GetType(combo.SelectedItem.ToString());
            if (type == null)
            {
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    type = assembly.GetType(combo.SelectedItem.ToString());
                    if (type != null)
                    {
                        break;
                    }
                }
            }

            form = new FormFactory().New(type);

            writer = new TestWriter(form);
            writer.TestChanged += handler;

            form.Show();
        }

        private void New_Click(object sender, EventArgs e)
        {
            textBox.Text = "";
        }

        private void AppForm_Load(object sender, EventArgs e)
        {
            //New_Click(null, null);
            //Load_Click(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (form != null)
            {
                try
                {
                    ImageFormatHandler handlers = new ImageFormatHandler();
                    ScreenCapture capture = new ScreenCapture(handlers);
                    pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
                    pictureBox1.Image = capture.Capture(form, @"XunitFormsCapture\");
                    CompareControlCaptureAction action = new CompareControlCaptureAction(capture.LastCapture, null);
                    writer.AddAction(action);
                    textBox.Text = writer.Test;
                }
                catch (ObjectDisposedException)
                {
                    MessageBox.Show("Please re-open your form.");
                }
            }
        }
    }
}
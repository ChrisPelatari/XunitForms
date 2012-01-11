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
    /// This class represents a C# variable initialization.
    ///</summary>
    public class Definition
    {
        private object control;
        private string formName;
        private string name;
        private Type testerType;

        ///<summary>
        /// Constructs a new <see cref="Definition"/>.
        ///</summary>
        public Definition(object control, string name, Type testerType, string formName)
        {
            this.control = control;
            this.name = name;
            this.testerType = testerType;
            this.formName = formName;
        }

        ///<summary>
        /// The control being defined.
        ///</summary>
        public object Control
        {
            get { return control; }
        }

        ///<summary>
        /// The control name to use.
        ///</summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// The variable name of an instance.
        /// </summary>
        /// <example>
        /// <i>toolStripSplitButton1</i> Is the variable name of an instance:
        /// <code>
        /// ToolStripSplitButtonTester toolStripSplitButton1 = new ToolStripSplitButtonTester("toolStripSplitButton1");
        /// </code>
        /// </example>
        public string VarName
        {
            get
            {
                if (FormName == null)
                    return Name.Replace('.', '_');
                else
                    return (FormName + "_" + Name).Replace('.', '_');
            }
        }

        ///<summary>
        /// The parent form name.
        ///</summary>
        public string FormName
        {
            get { return formName; }
            set { formName = value; }
        }

        ///<summary>
        /// Gets the <see cref="TesterType"/> associated with this <see cref="Definition"/>.
        ///</summary>
        public Type TesterType
        {
            get { return testerType; }
        }

        ///<summary>
        /// 
        ///</summary>
        ///<returns>C# code to initialize the defined variable.</returns>
        public override string ToString()
        {
            if (FormName == null)
            {
                return string.Format(
                    "{0} {1} = new {0}(\"{2}\");",
                    TesterType.Name,
                    Strings.SafeRemoveSpaces(VarName),
                    Name);
            }
            else
            {
                return string.Format(
                    "{0} {1} = new {0}(\"{2}\", \"{3}\");",
                    TesterType.Name,
                    Strings.SafeRemoveSpaces(VarName),
                    Name,
                    FormName);
            }
        }
    }
}
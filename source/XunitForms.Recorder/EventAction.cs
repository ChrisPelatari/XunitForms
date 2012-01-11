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
using System.CodeDom;
using System.IO;
using System.Text;
using Microsoft.CSharp;

namespace Xunit.Extensions.Forms.Recorder
{
    ///<summary>
    /// Recordable action for an Event.
    ///</summary>
    public class EventAction : Action
    {
        private object[] args;

        private string comment;
        private string methodName;

        ///<summary>
        /// Constructs a new EventAction.
        ///</summary>
        public EventAction(string methodName, params object[] args)
        {
            this.methodName = methodName;

            if (args.Length > 0 && args[0] is Array)
            {
                this.args = (object[]) args[0];
            }
            else
            {
                this.args = args;
            }

            comment = string.Empty;
        }

        public string MethodName
        {
            get { return methodName; }
        }

        public object[] Args
        {
            get { return args; }
        }

        public string Comment
        {
            set { comment = " //" + value; }
        }

        private string NoSpace(string name)
        {
            return name.Replace(" ", "");
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}({2});{3}", NoSpace(Definition.VarName), methodName, GetArgs(args), comment);
        }

        private static string GetArgs(object[] newArgs)
        {
            StringBuilder sb = new StringBuilder();
            bool first = true;
            foreach (object arg in newArgs)
            {
                if (!first)
                {
                    sb.Append(", ");
                }
                first = false;
                sb.Append(FormatArgument(arg));
            }
            return sb.ToString();
        }

        private static string FormatArgument(object arg)
        {
            using(StringWriter sw = new StringWriter())
            {
                CSharpCodeProvider provider = new CSharpCodeProvider();
                provider.GenerateCodeFromExpression(new CodePrimitiveExpression(arg), sw, null);
                return sw.ToString();
            }
        }
    }
}
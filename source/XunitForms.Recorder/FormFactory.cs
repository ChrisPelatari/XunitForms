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

//This class used to be in a separate project so that people could reuse it.
//However it is not generic enough yet, and something like PicoContainer.NET
//would be better anyway.  If you WERE using it in a previous release, let me
//know if it would be better separated out again.  Otherwise I am assuming
//nobody cares and the code is simple enough to write (or borrow) yourself.
using System;
using System.Collections;
using System.Reflection;
using System.Windows.Forms;

namespace Xunit.Extensions.Forms.Recorder
{
    /// <summary>
    /// This class can create forms of any type and satisfies any
    /// constructor specified dependencies.
    /// </summary>
    /// <remarks>
    /// Consider it for internal purposes only by the recorder
    /// application. The recorder should eventually be able to
    /// generate tests that use mock objects to satisfy the
    /// dependencies of a Form. Expectations should be generated by
    /// the recorder application. Until then it is just used to
    /// instantiate simple forms. So test this.
    /// </remarks>
    public class FormFactory
    {
        /// <summary>
        /// Create a form of the specified type.
        /// </summary>
        /// <param name="type">The type of form to create</param>
        /// <returns>An instance of the form</returns>
        public Form New(Type type)
        {
            if (!typeof (Form).IsAssignableFrom(type))
            {
                throw new Exception("Your type is not a form!  -->" + type);
            }

            return (Form) NewType(type);
        }

        private object NewObject(Type type)
        {
            ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            ConstructorInfo constructor = constructors[0];
            
            foreach(ConstructorInfo ctor in constructors)
            {
                if(ctor.GetParameters().Length < constructor.GetParameters().Length)
                {
                    constructor = ctor;
                }
            }

            ParameterInfo[] parameters = constructor.GetParameters();

            object[] arguments = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                arguments[i] = NewType(parameters[i].ParameterType);
            }

            return constructor.Invoke(parameters);
        }

        private object NewInterface(Type type)
        {
            ArrayList candidates = new ArrayList();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type t in assembly.GetTypes())
                {
                    if (type.IsAssignableFrom(t) && !type.Equals(t))
                    {
                        candidates.Add(t);
                    }
                }
            }
            if (candidates.Count != 1)
            {
                throw new Exception("Implementing type not found or ambiguous");
            }
            return NewType((Type) candidates[0]);
        }

        private object NewType(Type type)
        {
            if (type.IsInterface)
            {
                return NewInterface(type);
            }
            else
            {
                return NewObject(type);
            }
        }
    }
}
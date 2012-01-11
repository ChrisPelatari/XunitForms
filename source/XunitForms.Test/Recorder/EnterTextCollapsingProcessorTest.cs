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
using Xunit;
using System.ComponentModel;

namespace Xunit.Extensions.Forms.Recorder.Test
{
    
    [Category("Recorder")]
    public class EnterTextCollapsingProcessorTest
    {
        #region Setup/Teardown

		public EnterTextCollapsingProcessorTest()
        {
            processor = new EnterTextCollapsingProcessor();
            list = new List<Action>();
        }

        #endregion

        private IList<Action> list = null;

        private string control1 = "control1";

        private string control2 = "control2";

        private string enter = "Enter";

        private string notenter = "NotEnter";

        private EnterTextCollapsingProcessor processor = null;

        public void Add(string control, string method, string arg)
        {
            EventAction action = new EventAction(method, arg);
            action.Definition = new Definition(null, control, null, null);
            list.Add(action);
        }

        public void Process()
        {
            list = new List<Action>(processor.Process(list));
        }

        public EventAction Action(int i)
        {
            return list[i] as EventAction;
        }

        [Fact]
        public void CanCollapse()
        {
            Add(control1, enter, "test");
            Add(control1, enter, "test2");
            Process();
            Assert.Equal(1, list.Count);
            Assert.Equal("test2", Action(0).Args[0]);
        }

        [Fact]
        public void CanCollapseMoreComplex()
        {
            Add(control1, enter, "test");
            Add(control1, enter, "test2");
            Add(control1, enter, "test3");
            Add(control1, notenter, "test4");
            Add(control2, enter, "test5");
            Add(control2, enter, "test6");
            Add(control1, enter, "test7");

            Process();
            Assert.Equal(4, list.Count);
            Assert.Equal("test3", Action(0).Args[0]);
            Assert.Equal("test4", Action(1).Args[0]);
            Assert.Equal("test6", Action(2).Args[0]);
            Assert.Equal("test7", Action(3).Args[0]);
        }

        [Fact]
        public void DontCollapseDifferentControls()
        {
            Add(control1, enter, "test");
            Add(control2, enter, "test2");
            Process();
            Assert.Equal(2, list.Count);
        }

        [Fact]
        public void DontCollapseDifferentMethods()
        {
            Add(control1, enter, "test");
            Add(control1, notenter, "test2");
            Process();
            Assert.Equal(2, list.Count);
        }
    }
}
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
using System.Collections.Generic;
using System.Windows.Forms;
using Xunit;

namespace Xunit.Extensions.Forms.TestApplications
{
    
    public class FormFinderTest : XunitFormTest
    {
        private FormFinder finder;

        public override void Setup()
        {
            finder = new FormFinder();
        }

        private Form ShowNewForm(string name)
        {
            Form form = new Form();
            form.Name = name;
            form.Show();
            return form;
        }

        [Fact]
        public void FindAll()
        {
            Form one = ShowNewForm("form");
            Form two = ShowNewForm("form2");
            Form three = ShowNewForm("form3");
            List<Form> found = finder.FindAll();
            Assert.Equal(3, found.Count);
            Assert.True(found.Contains(one));
            Assert.True(found.Contains(two));
            Assert.True(found.Contains(three));
        }

        [Fact]
        public void FinderWithBadObjectHasNoName()
        {
            Assert.Throws<Exception>(delegate{ new Finder<Control>().Name("a");});
        }

        [Fact]
        public void FindOneForm()
        {
            Form form = ShowNewForm("form");
            Form found = finder.Find("form");
            Assert.Same(form, found);
        }

        [Fact]
        public void FindOneFormOutOfTwo()
        {
            Form one = ShowNewForm("form");
            Form two = ShowNewForm("form2");
            Assert.Equal(2, finder.FindAll().Count);

            Form found = finder.Find("form");
            Assert.Same(one, found);
            found = finder.Find("form2");
            Assert.Same(two, found);
        }

        [Fact]
        public void FindOneFormWhenThereAreNone()
        {
            Assert.Throws<NoSuchControlException>(() => finder.Find("form"));
        }

        [Fact]
        public void FindOneFormWhenThereAreTwo()
        {
            ShowNewForm("form");
            ShowNewForm("form");
            Assert.Throws<AmbiguousNameException>(() => finder.Find("form"));
        }

        [Fact]
        public void FindTwoFormsWhenThereAreTwo()
        {
            Form one = ShowNewForm("form");
            Form two = ShowNewForm("form");
            List<Form> found = finder.FindAll("form");
            Assert.Equal(2, found.Count);
            Assert.True(found.Contains(one));
            Assert.True(found.Contains(two));
        }
    }
}
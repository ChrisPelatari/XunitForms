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

using Xunit;

namespace Xunit.Extensions.Forms.TestApplications
{
    
    public class ContextMenuTest : XunitFormTest
    {
        private LabelTester label;

        public override void Setup()
        {
            new ContextMenuTestForm().Show();
            label = new LabelTester("myCounterLabel");
        }

        [Fact]
        public void AmbiguousNameBecauseInSubMenusButNotQualified()
        {
            MenuItemTester myMenuItem = new MenuItemTester("Not Ambiguous");
            Assert.Throws<AmbiguousNameException>(() => myMenuItem.Click());
        }

        [Fact]
        public void AmbiguousNameBecauseInTwoMenus()
        {
            MenuItemTester myMenuItem = new MenuItemTester("Test 2.Not Ambiguous");
            Assert.Throws<AmbiguousNameException>(() => myMenuItem.Click());
        }

        [Fact]
        public void AmbiguousNameBecauseWeUseTextNotNameForMenuItems()
        {
            MenuItemTester myMenuItem = new MenuItemTester("Ambiguous");
            Assert.Throws<AmbiguousNameException>(() => myMenuItem.Click());
        }

        [Fact]
        public void ContextMenuClick()
        {
            MenuItemTester myMenuItem = new MenuItemTester("Click To Count");
            myMenuItem.Click();
            Assert.Equal("1", label.Text);
        }

        [Fact]
        public void DontNeedToSpecifyWhichForm()
        {
            MenuItemTester myMenuItem = new MenuItemTester("myCounterLabel.ContextMenu.Test 2.Not Ambiguous");
            myMenuItem.Click();
        }

        [Fact]
        public void GeneratedTest()
        {
            MenuItemTester ClickToCount = new MenuItemTester("Click To Count");
            ClickToCount.Click();
            Assert.Equal("1", label.Properties.Text);
        }

        [Fact]
        public void NoSuchControlFinder()
        {
            MenuItemTester myMenuItem = new MenuItemTester("junkData");
            Assert.Throws<NoSuchControlException>(() => myMenuItem.Click());
        }

        [Fact]
        public void NotAmbiguousNameBecauseInSubMenus()
        {
            MenuItemTester myMenuItem = new MenuItemTester("Test 1.Not Ambiguous");
            myMenuItem.Click();
        }

        [Fact]
        public void NotAmbiguousNameBecauseInTwoMenusButQualified()
        {
            //source control property of the menu item is not actually set on the menu item in the tester
            //so handlers that rely on this are currently broken.  I am not sure whether anyone will notice
            //this.
            MenuItemTester myMenuItem = new MenuItemTester("myCounterLabel.ContextMenu.Test 2.Not Ambiguous");
            myMenuItem.Click();
        }
    }
}
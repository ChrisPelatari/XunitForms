using System.Windows.Forms;
using Xunit.Extensions.Forms.TestApplications;
using Xunit;

namespace Xunit.Extensions.Forms.Test
{
    
    public class DialogWithNoHandlerTest : XunitFormTest
    {
        private ButtonTester acceptButton;
        private ButtonTester rejectButton;


        [Fact]
        public void TestAcceptButton()
        {
            ModalFormHandler =
                delegate(string name, System.IntPtr hWnd, System.Windows.Forms.Form fform)
                {
                    acceptButton = new ButtonTester("button1");
                    acceptButton.Click();
                };
            DialogWithNoHandlersForm form = new DialogWithNoHandlersForm();
            DialogResult result = form.ShowDialog();
            Assert.Equal(DialogResult.OK, result);
            Assert.False(form.Visible, "Form was still visible.");
            form.Close();
        }

        [Fact]
        public void TestRejectButton()
        {
            ModalFormHandler =
                delegate(string name, System.IntPtr hWnd, System.Windows.Forms.Form fform)
                    {
                        rejectButton = new ButtonTester("button2");
                        rejectButton.Click();
                    };

            DialogWithNoHandlersForm form = new DialogWithNoHandlersForm();
            DialogResult result = form.ShowDialog();
            Assert.Equal(DialogResult.Cancel, result);
            Assert.False(form.Visible, "Form was still visible.");
            form.Close();
        }
    }
}
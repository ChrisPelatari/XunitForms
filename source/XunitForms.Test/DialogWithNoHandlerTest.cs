using System.Windows.Forms;
using Xunit.Extensions.Forms.TestApplications;
using Xunit;
using Should.Fluent;

namespace Xunit.Extensions.Forms.Test
{
    
    public class DialogWithNoHandlerTest : XunitFormTest
    {
        private ButtonTester acceptButton;
        private ButtonTester rejectButton;


        [Fact]
        public void TestAcceptButton()
        {
            ModalFormHandler = (name, hWnd, fform) => {
                    acceptButton = new ButtonTester("button1");
                    acceptButton.Click();
                };
            DialogWithNoHandlersForm form = new DialogWithNoHandlersForm();
            DialogResult result = form.ShowDialog();
            result.Should().Equal(DialogResult.OK);
            form.Visible.Should().Equal(false);
            form.Close();
        }

        [Fact]
        public void TestRejectButton()
        {
            ModalFormHandler = (name, hWnd, fform) => {
                        rejectButton = new ButtonTester("button2");
                        rejectButton.Click();
                    };

            DialogWithNoHandlersForm form = new DialogWithNoHandlersForm();
            DialogResult result = form.ShowDialog();
            result.Should().Equal(DialogResult.Cancel);
            form.Visible.Should().Equal(false);
            form.Close();
        }
    }
}
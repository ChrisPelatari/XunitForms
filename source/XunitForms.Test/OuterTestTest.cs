using System.Windows.Forms;
using Xunit;
using System;

namespace Xunit.Extensions.Forms.TestApplications
{
    public class OuterTestTest
    {
        [Fact, STAThread]
        public void RunTwiceWithoutFailure()
        {
            using (OuterTest nuf = new OuterTest())
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.CheckFileExists = false;

                    nuf.ModalFormHandler = (name, hWnd, form) => {
                        new OpenFileDialogTester(hWnd).ClickCancel();
                    };

                    dlg.ShowDialog();
                }
            }
            using (OuterTest nuf = new OuterTest())
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.CheckFileExists = false;

                    nuf.ModalFormHandler = (name, hWnd, form) => {
                        new OpenFileDialogTester(hWnd).ClickCancel();
                    };

                    dlg.ShowDialog();
                }
            }
        }

        // Put this here, because ExpectedException does not work when the exn is thrown in teardown
        [Fact]
        public void DanglingWindowMessage()
        {
            using (OuterTest nuf = new OuterTest())
            {
                Form f = new Form();
                f.Show();
                System.Threading.EventWaitHandle w = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.AutoReset);
                System.Threading.ThreadPool.QueueUserWorkItem((object o) => {
                    f.BeginInvoke(new MethodInvoker(() => {
                        MessageBox.Show("", "Blah");
                    }));
                    w.Set();
                });
				w.WaitOne();
				Assert.Throws<FormsTestAssertionException>(() => nuf.Verify());
            }
        }

    }
}

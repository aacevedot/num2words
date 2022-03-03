using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace WpfClient.Test
{
    public class MainWindowTests
    {
        [Fact]
        public void Test_Window()
        {
            var showMonitor = new ManualResetEventSlim(false);
            var closeMonitor = new ManualResetEventSlim(false);

            var th = new Thread(new ThreadStart(delegate
            {
                var mw = new MainWindow();
                mw.Show();

                showMonitor.Set();
                closeMonitor.Wait();
            }))
            {
                ApartmentState = ApartmentState.STA
            };

            th.Start();

            showMonitor.Wait();
            Task.Delay(1000).Wait();
            
            // TODO: Testing...

            closeMonitor.Set();
        }
    }
}
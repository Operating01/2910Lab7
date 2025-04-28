using System.IO;
using BlazorApp1.Components.Services;
using BlazorApp1.Models;
using BlazorApp1.Services;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace Lab6
{
    [TestClass]
    [DoNotParallelize]
    public sealed class DeleteTest
    {
        private LibraryService _service;
        private string u;

        [TestInitialize]
        public void Setup()
        {
            _service = new LibraryService();
        }

        [DataRow(1)]
        [DataRow(100)]
        [TestMethod]
        public async Task TestDeleteUser(int id)
        {
            var random = new Random();
            int randNum = random.Next(1000, 9999);
            string tempfilepath = ".\\Data\\Users" + randNum.ToString() + DateTime.Now.Ticks.ToString() + ".csv";
            File.Copy(".\\Data\\TUsers.csv", tempfilepath, overwrite: true);
            try
            {
                await _service.ReadUsers(tempfilepath);
                await _service.DeleteUser(id, tempfilepath);
                Console.WriteLine("Got past acting");
                Assert.IsNull(_service.Users.FirstOrDefault(u => u.Id == id));
            }
            finally
            {
                File.Delete(tempfilepath);
            }
        }

        [DataRow(1)]
        [DataRow(100)]
        [TestMethod]
        public async Task TestDeleteBook(int id)
        {
            var random = new Random();
            int randNum = random.Next(1000, 9999);
            string tempfilepath = ".\\Data\\Books" + randNum.ToString() + DateTime.Now.Ticks.ToString() + ".csv";
            File.Copy(".\\Data\\TBooks.csv", tempfilepath, overwrite: true);
            try
            {
                await _service.ReadBooks(tempfilepath);
                await _service.DeleteBook(id, tempfilepath);
                Console.WriteLine("Got past acting");
                Assert.IsNull(_service.Books.FirstOrDefault(b => b.Id == id));
            }
            finally
            {
                File.Delete(tempfilepath);
            }
        }

        [DataRow(-1)]
        [DataRow(9999)]
        [TestMethod]
        public async Task TestDeleteBookERR(int id)
        {
            string err = null;
            var random = new Random();
            int randNum = random.Next(1000, 9999);
            string tempfilepath = ".\\Data\\Books" + randNum.ToString() + DateTime.Now.Ticks.ToString() + ".csv";
            File.Copy(".\\Data\\TBooks.csv", tempfilepath, overwrite: true);
            try
            {
                try
                {
                    await _service.ReadBooks(tempfilepath);
                    await _service.DeleteBook(id, tempfilepath);
                }
                catch (NullReferenceException excep)
                {
                    err = excep.Message;
                }
                Assert.IsNotNull(err);
            }
            finally
            {
                File.Delete(tempfilepath);
            }
        }

        [DataRow(-1)]
        [DataRow(9999)]
        [TestMethod]
        public async Task TestDeleteUserERR(int id)
        {
            string err = null;
            var random = new Random();
            int randNum = random.Next(1000, 9999);
            string tempfilepath = ".\\Data\\Users" + randNum.ToString() + DateTime.Now.Ticks.ToString() + ".csv";
            File.Copy(".\\Data\\TUsers.csv", tempfilepath, overwrite: true);
            try
            {
                try
                {
                    await _service.ReadUsers(tempfilepath);
                    await _service.DeleteUser(id, tempfilepath);
                }
                catch (NullReferenceException excep)
                {
                    err = excep.Message;
                }
                Assert.IsNotNull(err);
            }
            finally
            {
                File.Delete(tempfilepath);
            }
        }

    }
}

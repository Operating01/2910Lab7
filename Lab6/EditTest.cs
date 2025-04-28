using System.IO;
using BlazorApp1.Components.Services;
using BlazorApp1.Models;
using BlazorApp1.Services;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace Lab6
{
    [TestClass]
    [DoNotParallelize]
    public sealed class EditTest
    {
        private LibraryService _service;
        private string u;

        [TestInitialize]
        public void Setup()
        {
            _service = new LibraryService();
            StreamReader userstrd = new StreamReader(".\\Data\\TUsers.csv");
            //string u = userstrd.;
            
        }

        [TestMethod]
        [DataRow(1, "Jeremy Fritz", "bears@email.com")]
        [DataRow(10, "Jeremy Fritz lol", "bears@email.com")]
        public async Task TestEditUser(int id, string name, string email)
        {
            var random = new Random();
            int randNum = random.Next(1000, 9999);
            string tempfilepath = ".\\Data\\Users" + randNum.ToString() + DateTime.Now.Ticks.ToString() + ".csv";
            File.Copy(".\\Data\\TUsers.csv", tempfilepath, overwrite: true);
            try
            {
            // Arrange 
                        BlazorApp1.Models.User user = new BlazorApp1.Models.User
                        {
                            Name = name,
                            Id = id,
                            Email = email
                        };
                        Console.WriteLine("Got to reading");
                        await _service.ReadUsers(tempfilepath);
                        // Used to reset the data
                        List<BlazorApp1.Models.User> Users2 = _service.Users;

                        //  Act
                        Console.WriteLine("Got to Writing");
                        await _service.EditUser(user, tempfilepath);

                        // Assert
                        Console.WriteLine("Got to Asserting");
                        Assert.AreEqual(_service.Users[id - 1].Name, name);
            }
            finally
            {
                Console.WriteLine("DONE");
            }
        }

        [TestMethod]
        // [DataRow(1, "This is so a new book", "Jeremy Fritz", "3845-2")]
        [DataRow(10, "This is so another new book", "Jeremy Fritz lol", "3555-9")]
        public async Task TestEditBook(int id, string title, string author, string isbn)
        {
            var random = new Random();
            int randNum = random.Next(1000, 9999);
            string tempfilepath = ".\\Data\\Books" + randNum.ToString() + DateTime.Now.Ticks.ToString() + ".csv";
            File.Copy(".\\Data\\TBooks.csv", tempfilepath, overwrite: true);
            try
            {
                // Arrange 
                Book book = new Book
                {
                    Id = id,
                    Title = title,
                    Author = author,
                    ISBN = isbn
                };
                Console.WriteLine("Got to reading");
                await _service.ReadBooks(tempfilepath);
                // Used to reset the data
                List<Book> Books2 = _service.Books;
                int count = Directory.GetFiles(".//Data").Count();

                // Act
                Console.WriteLine("Got to Writing");
                await _service.EditBook(book, tempfilepath);

                // Assert
                Console.WriteLine("Got to Asserting");
                Console.WriteLine(_service.Books.Count);
                Assert.AreEqual(_service.Books[id - 1].Title, title);
            } finally
            {
                File.Delete(tempfilepath);
            }
            
        }
    }
}

using System;
using BlazorApp1.Components.Services;
using BlazorApp1.Models;
using BlazorApp1.Services;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
namespace Lab6
{
    [TestClass]
    [DoNotParallelize]
    public sealed class CreationTests
    {
        private LibraryService _service;
        private string path;

        [TestInitialize]
        public void Setup()
        {
            _service = new LibraryService();
            path = Directory.GetCurrentDirectory();

        }

        // USER CREATION TESTS
        [TestMethod]
        [DataRow(999, "John", "fish@email.com")]
        [DataRow(1000, "Jeremy Fritz", "bears@email.com")]
        public async Task TestUserCreate(int id, string name, string email)
        {
            var random = new Random();
            int randNum = random.Next(1000, 9999);
            string tempfilepath = path + "//Data//Users" + randNum.ToString() + DateTime.Now.Ticks.ToString() + ".csv";
            File.Copy($"{path}//Data//TUsers.csv", tempfilepath, overwrite: true);
            BlazorApp1.Models.User user = new BlazorApp1.Models.User
            {
                Id = id,
                Name = name,
                Email = email
            };
            await _service.ReadUsers(tempfilepath);
            await _service.AddUser(user, tempfilepath);
            List<BlazorApp1.Models.User> Users = _service.Users;
            Assert.AreEqual(Users[Users.Count - 1], user);
            File.Delete(tempfilepath);
        }

        [TestMethod]
        [DataRow(999, "John", "")]
        [DataRow(0, "John", "fish@email.com")]
        public async Task TestBadFieldUser(int id, string name, string email)
        {
            var random = new Random();
            int randNum = random.Next(1000, 9999);
            string tempfilepath = path + "//Data//Users" + randNum.ToString() + DateTime.Now.Ticks.ToString() + ".csv";
            File.Copy($"{path}//Data//TUsers.csv", tempfilepath, overwrite: true);
            // Arrange
            string excep = null;
            BlazorApp1.Models.User user = new BlazorApp1.Models.User
            {
                Id = id,
                Name = name,
                Email = email
            };
            await _service.ReadUsers(tempfilepath);

            // Act
            try
            {
                await _service.AddUser(user, tempfilepath);
            }
            catch (BadField bf)
            {
                excep = bf.Message;
            }

            // Assert
            Assert.IsNotNull(excep);
            File.Delete(tempfilepath);
        }

        // BOOK CREATION TESTS
        [TestMethod]
        [DataRow(999, "John", "John Fish", "99-0")]
        [DataRow(1000, "Jeremy Fritz", "Types of Bears", "100-1")]
        public async Task TestBookCreation(int id, string title, string author, string isbn)
        {
            var random = new Random();
            int randNum = random.Next(1000, 9999);
            string tempfilepath = path + "//Data//Books" + randNum.ToString() + DateTime.Now.Ticks.ToString() + ".csv";
            File.Copy($"{path}//Data//TBooks.csv", tempfilepath, overwrite: true);
            Book book = new Book
            {
                Id = id,
                Title = title,
                Author = author,
                ISBN = isbn
            };
            await _service.ReadBooks(tempfilepath);
            await _service.AddBook(book, tempfilepath);
            List<Book> Books = _service.Books;
            Assert.AreEqual(Books[Books.Count - 1], book);
            File.Delete(tempfilepath);
        }

        [TestMethod]
        [DataRow(0, "John", "John Fish", "99-0")]
        [DataRow(1000, "", "Types of Bears", "100-1")]
        public async Task TestBadFieldBook(int id, string title, string author, string isbn)
        {
            var random = new Random();
            int randNum = random.Next(1000, 9999);
            string tempfilepath = path + "//Data//Books" + randNum.ToString() + DateTime.Now.Ticks.ToString() + ".csv";
            File.Copy($"{path}//Data//TBooks.csv", tempfilepath, overwrite: true);
            string excep = null;
            Book book = new Book
            {
                Id = id,
                Title = title,
                Author = author,
                ISBN = isbn
            };
            await _service.ReadBooks(tempfilepath);
            // Act
            try
            {
                await _service.AddBook(book, tempfilepath);
            }
            catch (BadField bf)
            {
                excep = bf.Message;
            }

            // Assert
            Assert.IsNotNull(excep);
            File.Delete(tempfilepath);
        }
    }
}

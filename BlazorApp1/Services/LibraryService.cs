using System.Globalization;
using System.IO;
using BlazorApp1.Components.Pages;
using BlazorApp1.Models;
using BlazorApp1.Services;

namespace BlazorApp1.Components.Services
{
    public class BookAlreadyBorrowed : Exception
    {
        public BookAlreadyBorrowed(string message) : base(message) { }
    }
    public class BadField : Exception
    {
        public BadField(string message) : base(message) { }
    }
    public class LibraryService : ILibraryService
    {
        
        private List<User> users = new(); 
        public List<User> Users => users;

        private List<Book> books = new(); 
        public List<Book> Books => books;

        private Dictionary<User, List<Book>> borrowedBooks = new();
        public Dictionary<User, List<Book>> BorrowedBooks => borrowedBooks;

        // USERS
        public async Task ReadUsers(string path)
        {
            Users.Clear();
            Console.WriteLine("This is the start");
            try
            {
                using var reader = new StreamReader(path);
                string line;

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var fields = line.Split(',');

                    if (fields.Length >= 3)
                    {
                        var user = new User
                        {
                            Id = int.Parse(fields[0].Trim()),
                            Name = fields[1].Trim(),
                            Email = fields[2].Trim()
                        };
                        Users.Add(user);
                    }
                }
                Console.WriteLine("This is the end");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public async Task DeleteUser(int id, string path) {
            var userToRemove = Users.FirstOrDefault(u => u.Id == id);
            if (userToRemove == null)
            {
                throw new NullReferenceException("The user could not be found!");
            }
            else if (userToRemove != null)
            {
                users.Remove(userToRemove);
                using var writer = new StreamWriter(path, append: false);
                string userstring = "";
                foreach (User user2 in users)
                {
                    userstring += user2.Id + "," + user2.Name + "," + user2.Email + "\n";
                }
                await writer.WriteLineAsync(userstring);
            }
            
        }

        public async Task EditUser(User user, string path) {
            var newuser = Users.FirstOrDefault(u => u.Id == user.Id);

            if (newuser == null)
            {
                throw new NullReferenceException("This user does not exist!");
            }
            else if (user.Id == 0 || string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Email))
            {
                throw new BadField("You are missing a field in your entry!");
            }
            else {
                newuser.Name = user.Name;
                newuser.Email = user.Email;
                string userstring = "";
                foreach (User user2 in users)
                {
                    userstring += user2.Id + "," + user2.Name + "," + user2.Email + "\n";
                }
                using var writer = new StreamWriter(path, append: false);
                await writer.WriteLineAsync(userstring);
                await writer.DisposeAsync();
            }
            
        }

        public async Task AddUser(User user, string path) {
            if (user.Id == 0 || string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Email))
            {
                throw new BadField("You are missing a field in your entry!");                                                                                            
            }
            Users.Add(user);
            using var writer = new StreamWriter(path, append: false);
            string userstring = "";
            foreach(User user2 in Users){
                userstring += user2.Id + "," + user2.Name + "," + user2.Email + "\n";
            }
            await writer.WriteLineAsync(userstring);
        }

        // BOOKS
        public async Task ReadBooks(string path)
        {
            Books.Clear();
            int index;
            try
            {
                using var reader = new StreamReader(path);
                string line;

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var fields = line.Split(',');

                    if (fields.Length >= 3 && fields[1].StartsWith("\""))
                    {
                        index = fields.Length - 1;
                        var book1 = new Book
                        {
                            Id = int.Parse(fields[0].Trim()),
                            Title = string.Join(",", fields.Skip(1).Take(index - 2)).Trim(),
                            Author = fields[index-1].Trim(),
                            ISBN = fields[index].Trim()
                        };
                        Books.Add(book1);
                        }

                    else if (fields.Length >= 3)
                    { 
                            var book2 = new Book
                            {
                                Id = int.Parse(fields[0].Trim()),
                                Title = fields[1].Trim(),
                                Author = fields[2].Trim(),
                                ISBN = fields[3].Trim(),
                            };
                            Books.Add(book2);

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public async Task AddBook(Book book, string path) {
            if (book.Id == 0 || string.IsNullOrEmpty(book.Author) || string.IsNullOrEmpty(book.ISBN) || string.IsNullOrEmpty(book.Title))
            {
                throw new BadField("A field in this entry was filled out incorrectly!");
            }
            Books.Add(book);
            using var writer = new StreamWriter(path, append: false);
            foreach (Book book2 in Books)
            {
                
            }
        }

        public async Task EditBook(Book book, string path)
        {
            var existingBook = Books.FirstOrDefault(b => b.Id == book.Id);

            if (existingBook == null)
            {
                throw new NullReferenceException("This book does not exist!");
            }
            else if (book.Id == 0 || string.IsNullOrEmpty(book.Author) || string.IsNullOrEmpty(book.ISBN) || string.IsNullOrEmpty(book.Title))
            {
                throw new BadField("A field in this entry was filled out incorrectly!");
            }
            else
            {
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.ISBN = book.ISBN;
                string bookstring = "";
                foreach (Book book2 in Books)
                {
                    bookstring += $"{book2.Id},{book2.Title},{book2.Author}, {book2.ISBN} \n";
                }
                using var writer = new StreamWriter(path, append: false);
                await writer.WriteLineAsync(bookstring);
                await writer.DisposeAsync();
            }
            
        }

        public async Task DeleteBook(int id, string path)
        {
            var bookToRemove = Books.FirstOrDefault(u => u.Id == id);
            Console.WriteLine(id);
            if (String.IsNullOrEmpty(bookToRemove.Author))
            {
                throw new NullReferenceException("The book could not be found!");
            }
            else if (bookToRemove != null)
            {
                Books.Remove(bookToRemove);
                using var writer = new StreamWriter(path, append: false);
                foreach (Book book2 in Books)
                {
                    await writer.WriteLineAsync($"{book2.Id},{book2.Title},{book2.Author},{book2.ISBN}");
                }
            }
        }

        // BORROWING

        public async Task BorrowBook(int userId, int bookId)
        {
            foreach (Book book in Books)
            {
                Console.WriteLine(book.Id);
            }
            Console.WriteLine(userId + "," + bookId);
            User dicUser = await Task.Run(() => Users.FirstOrDefault(u => u.Id == userId));
            if (dicUser == null)
            {
                throw new ArgumentNullException("This user does not exist!");
            }
            if (BorrowedBooks.ContainsKey(dicUser))
            {
                List<Book> userBooks = BorrowedBooks[dicUser];
                Book dicBook = Books.FirstOrDefault(b => b.Id == bookId);
                if (dicBook == null)
                {
                    throw new ArgumentNullException("This book does not exist!");
                }
                Console.WriteLine(dicBook);
                if (BorrowedBooks.Values.Any(bookList => bookList.Contains(dicBook)))
                {
                    throw new BookAlreadyBorrowed("This book already is borrowed!");
                }
                userBooks.Add(dicBook);
                BorrowedBooks[dicUser] = userBooks;
            }
            else
            {
                Book dicBook = Books.FirstOrDefault(b => b.Id == bookId);
                if (dicBook == null)
                {
                    throw new ArgumentNullException("This book does not exist!");
                }
                if (BorrowedBooks.Values.Any(bookList => bookList.Contains(dicBook)))
                {
                    throw new BookAlreadyBorrowed("This book already is borrowed!");
                }
                List<Book> userBooks = new List<Book>() { dicBook };
                BorrowedBooks.Add(dicUser, userBooks);
            }
        }

        public async Task ReturnBook(int userId, int bookId)
        {
            User dicUser = await Task.Run(() => Users.FirstOrDefault(u => u.Id == userId));
            Book dicBook = await Task.Run(() => Books.FirstOrDefault(b => b.Id == bookId));
            Console.WriteLine(borrowedBooks[dicUser].Count);
            BorrowedBooks[dicUser].Remove(dicBook);
            if (borrowedBooks[dicUser].Count == 0)
            {
                borrowedBooks.Remove(dicUser);
            }
        }

    }
}

using BlazorApp1.Models;

namespace BlazorApp1.Services
{

    public interface ILibraryService
    {
        List<Book> Books { get; }
        List<User> Users { get; }
        Dictionary<User, List<Book>> BorrowedBooks { get; }
        // Users 
        Task ReadUsers(string path);
        Task AddUser(User user, string path);
        Task DeleteUser(int id, string path);
        Task EditUser(User user, string path);

        // Books
        Task ReadBooks(string path);
        Task AddBook(Book book, string path);
        Task EditBook(Book book, string path);
        Task DeleteBook(int id, string path);

        // BORROWING

        Task BorrowBook(int userId, int bookId);
        Task ReturnBook(int userId, int bookId);
    }
}
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Booxtore.Domain.Models
{
    public class CartItem
    {
        public int BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string CoverImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total => Price * Quantity;
        
        [JsonIgnore]
        public virtual Book? Book { get; set; }
    }

    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal TotalAmount => Items.Sum(x => x.Total);
        public int TotalItems => Items.Sum(x => x.Quantity);
        
        public void AddItem(Book book, int quantity = 1)
        {
            var existingItem = Items.FirstOrDefault(x => x.BookId == book.BookId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                Items.Add(new CartItem
                {
                    BookId = book.BookId,
                    Title = book.Title,
                    CoverImageUrl = book.CoverImageUrl ?? "",
                    Price = (decimal)book.Price,
                    Quantity = quantity,
                    Book = book
                });
            }
        }

        public void RemoveItem(int bookId)
        {
            Items.RemoveAll(x => x.BookId == bookId);
        }

        public void UpdateQuantity(int bookId, int quantity)
        {
            var item = Items.FirstOrDefault(x => x.BookId == bookId);
            if (item != null)
            {
                if (quantity <= 0)
                {
                    RemoveItem(bookId);
                }
                else
                {
                    item.Quantity = quantity;
                }
            }
        }

        public void Clear()
        {
            Items.Clear();
        }
    }
}

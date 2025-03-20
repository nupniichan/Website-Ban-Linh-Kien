namespace Admin_WBLK.Models.Prototypes
{
    /// <summary>
    /// Interface định nghĩa phương thức Clone cho mẫu Prototype
    /// </summary>
    public interface IProductPrototype
    {
        /// <summary>
        /// Tạo một bản sao của đối tượng
        /// </summary>
        object Clone();
    }
} 
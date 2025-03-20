using System;

namespace Website_Ban_Linh_Kien.Models.Factories
{
    // Interface cho các ID Generator
    public interface IIdGenerator
    {
        string GenerateId(string lastId);
    }

    // Concrete implementation cho Account ID Generator
    public class AccountIdGenerator : IIdGenerator
    {
        public string GenerateId(string lastId)
        {
            if (string.IsNullOrEmpty(lastId))
            {
                return "TK00001";
            }

            int lastNumber = int.Parse(lastId.Substring(2));
            return $"TK{(lastNumber + 1).ToString("D5")}";
        }
    }

    // Concrete implementation cho Customer ID Generator
    public class CustomerIdGenerator : IIdGenerator
    {
        public string GenerateId(string lastId)
        {
            if (string.IsNullOrEmpty(lastId))
            {
                return "KH000001";
            }

            int lastNumber = int.Parse(lastId.Substring(2));
            return $"KH{(lastNumber + 1).ToString("D6")}";
        }
    }

    // Factory class để tạo các ID Generator
    public class IdGeneratorFactory
    {
        public static IIdGenerator CreateAccountIdGenerator()
        {
            return new AccountIdGenerator();
        }

        public static IIdGenerator CreateCustomerIdGenerator()
        {
            return new CustomerIdGenerator();
        }
    }
} 
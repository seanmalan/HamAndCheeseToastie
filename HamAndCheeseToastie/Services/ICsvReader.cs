using HamAndCheeseToastie.Models;

namespace HamAndCheeseToastie.Services
{
    public interface ICsvReader
    {
        List<Product> ImportCsv(StreamReader reader);
    }

}

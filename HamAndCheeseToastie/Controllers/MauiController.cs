using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HamAndCheeseToastie.Models;
using HamAndCheeseToastie.Database;
using Microsoft.EntityFrameworkCore;
using HamAndCheeseToastie.DTOs;

namespace HamAndCheeseToastie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MauiController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<MauiController> _logger;

        public MauiController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet("products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllProductsMaui(int offset = 0, int limit = 30)
        {
            if (limit <= 0 || offset < 0)
            {
                return BadRequest("Offset and limit must be non-negative, and limit must be greater than 0.");
            }

            var productsQuery = _context.Products
                .Include(p => p.Category)
                .Select(p => new ProductDto
                {
                    ID = p.ID,
                    Name = p.Name,
                    BrandName = p.BrandName,
                    Weight = p.Weight,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name,
                    CurrentStockLevel = p.CurrentStockLevel,
                    MinimumStockLevel = p.MinimumStockLevel,
                    Price = p.Price,
                    WholesalePrice = p.WholesalePrice,
                    EAN13Barcode = p.EAN13Barcode,
                    ImagePath = p.ImagePath
                });

            var totalProducts = await productsQuery.CountAsync();
            var products = await productsQuery
                .Skip(offset)
                .Take(limit)
                .ToListAsync();

            var result = new
            {
                TotalCount = totalProducts,
                Products = products
            };

            return Ok(result);
        }


        [HttpGet("product/search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchProducts(
            [FromQuery] string query,
            [FromQuery] string category = null)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query parameter is required.");
            }

            // Build the queryable collection of products
            var productsQuery = _context.Products
                .Include(p => p.Category)
                .AsQueryable();

            // Apply search query filter
            productsQuery = productsQuery.Where(p =>
                p.Name.Contains(query) ||
                p.BrandName.Contains(query) ||
                p.EAN13Barcode.Contains(query));

            // Apply category filter if specified
            if (!string.IsNullOrEmpty(category))
            {
                productsQuery = productsQuery.Where(p => p.Category.Name == category);
            }

            var products = await productsQuery
                .Select(p => new ProductDto
                {
                    ID = p.ID,
                    Name = p.Name,
                    BrandName = p.BrandName,
                    Weight = p.Weight,
                    CategoryId = p.CategoryId, // Ensure this matches the property correctly
                    CategoryName = p.Category.Name, // CategoryName mapped to the Category's Name property
                    CurrentStockLevel = p.CurrentStockLevel,
                    MinimumStockLevel = p.MinimumStockLevel,
                    Price = p.Price,
                    WholesalePrice = p.WholesalePrice,
                    EAN13Barcode = p.EAN13Barcode,
                    ImagePath = p.ImagePath
                })
                .ToListAsync();

            if (!products.Any())
            {
                return NotFound("No products found matching the criteria.");
            }

            return Ok(products);
        }


        [HttpGet("Product/barcode/{barcode}")]
        public async Task<IActionResult> GetProductByBarcode(string barcode)
        {
            try
            {
                var product = await _context.Products
                    .Select(p => new ProductDto
                    {
                        ID = p.ID,
                        Name = p.Name,
                        BrandName = p.BrandName,
                        Weight = p.Weight,
                        CategoryId = p.CategoryId, // Ensure this matches the property correctly
                        CategoryName = p.Category.Name, // CategoryName mapped to the Category's Name property
                        CurrentStockLevel = p.CurrentStockLevel,
                        MinimumStockLevel = p.MinimumStockLevel,
                        Price = p.Price,
                        WholesalePrice = p.WholesalePrice,
                        EAN13Barcode = p.EAN13Barcode,
                        ImagePath = p.ImagePath
                    })
                    .FirstOrDefaultAsync(p => p.EAN13Barcode == barcode);

                if (product == null)
                {
                    return NotFound(new { Message = "Product not found." });
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, new { Message = $"Server error: {ex.Message}" });
            }
        }



        // GET: api/Maui/Customer
        /// <summary>
        /// Retrieves a simplified list of customers for Maui clients.
        /// </summary>
        /// <response code="200">Returns the list of customers formatted for Maui clients.</response>
        [HttpGet("Customers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCustomerMaui()
        {
            var customers = await _context.Customer
                .Select(c => new MauiCustomerDto
                {
                    Id = c.CustomerId,
                    CustomerId = c.CustomerId,
                    Barcode = c.Barcode,
                    CustomerName = c.FirstName,
                    Surname = c.LastName,
                    Email = c.Email,
                    Phone = c.PhoneNumber,
                    IsMember = c.IsLoyaltyMember
                })
                .ToListAsync();

            return Ok(customers);
        }


        //Searching for a customer by name
        [HttpGet("Customer/Search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchCustomers(
            [FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Query parameter is required.");
            }

            // Build the queryable collection of Customers
            var customerQuery = _context.Customer.AsQueryable();

            // Apply search query filter
            customerQuery = customerQuery.Where(c =>
                c.FirstName.Contains(name) ||
                c.LastName.Contains(name) ||
                c.Email.Contains(name));

            var customer = await customerQuery
                .Select(c => new MauiCustomerDto
                {
                    Id = c.CustomerId,
                    CustomerId = c.CustomerId,
                    Barcode = c.Barcode,
                    CustomerName = c.FirstName,
                    Surname = c.LastName,
                    Email = c.Email,
                    Phone = c.PhoneNumber,
                    IsMember = c.IsLoyaltyMember
                })
                .ToListAsync();

            if (!customer.Any())
            {
                return NotFound("No Customers found matching the criteria.");
            }

            return Ok(customer);
        }

        // POST: api/Maui/Customer
        /// <summary>
        /// Adds a new customer to the database.
        /// </summary>
        /// <response code="201">Returns the created customer.</response>
        /// <response code="400">If the customer data is invalid.</response>
        [HttpPost("Customers")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostCustomerMaui([FromBody] MauiCustomerDto customerDto)
        {
            if (customerDto == null)
            {
                return BadRequest("Customer data is required.");
            }

            try
            {
                // Create a new Customer instance from the customerDto
                var customer = new Customer
                {
                    // Do not set CustomerId manually, let the database generate it
                    Barcode = customerDto.Barcode,
                    FirstName = customerDto.CustomerName,
                    LastName = customerDto.Surname,
                    Email = customerDto.Email,
                    PhoneNumber = customerDto.Phone,
                    IsLoyaltyMember = customerDto.IsMember
                };

                // Add the customer to the context and save changes
                await _context.Customer.AddAsync(customer);
                await _context.SaveChangesAsync();

                // Return the created customer
                return CreatedAtAction(nameof(PostCustomerMaui), new { id = customer.CustomerId }, customer);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error occurred while adding customer: {ex.Message}");
            }
        }




        /// <summary>
        /// Retrieves filtered transactions based on date range and count.
        /// </summary>
        /// <param name="dateFrom">Start date of the transaction period.</param>
        /// <param name="dateTo">End date of the transaction period.</param>
        /// <param name="count">Number of transactions to retrieve.</param>
        /// <response code="200">Returns the filtered list of transactions.</response>
        [HttpGet("Transactions")]
        public async Task<IActionResult> GetTransactions(
        [FromQuery] string dateFrom = null,
        [FromQuery] string dateTo = null,
        [FromQuery] int? count = null)
        {
            if (!DateTime.TryParse(dateFrom, out var fromDate))
            {
                fromDate = DateTime.UtcNow.AddDays(-30);
            }

            if (!DateTime.TryParse(dateTo, out var toDate))
            {
                toDate = DateTime.UtcNow;
            }

            if (fromDate > toDate)
            {
                return BadRequest("Invalid date range: 'dateFrom' must be earlier than 'dateTo'.");
            }

            var transactionCount = count ?? 10;

            // Get the New Zealand time zone
            var nzTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific/Auckland");

            // Query and convert to New Zealand time zone
            var transactions = await _context.Transaction
                .Where(t => t.TransactionDate >= fromDate && t.TransactionDate <= toDate)
                .OrderByDescending(t => t.TransactionDate)
                .Take(transactionCount)
                .Select(t => new
                {
                    t.TransactionId,
                    TransactionDate = TimeZoneInfo.ConvertTimeFromUtc(t.TransactionDate, nzTimeZone), // Convert here
                    t.TotalAmount,
                    t.Discount,
                    t.PaymentMethod,
                    t.TaxAmount,
                    t.UserId,
                    t.CustomerId
                })
                .ToListAsync();

            if (!transactions.Any())
            {
                return NotFound("No transactions found for the given criteria.");
            }

            Console.Write(transactions);

            return Ok(transactions);
        }


        // GET: api/Transaction/5
        [HttpGet("Transaction/{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await _context.Transaction.FindAsync(id);

            if (transaction == null)
            {
                return NotFound(new { Message = $"Transaction with ID {id} not found." });
            }

            return Ok(transaction);
        }
        


        [HttpGet("Transactions/{transactionId}/Items")]
        public async Task<IActionResult> GetTransactionItems(int transactionId)
        {
            var items = await _context.TransactionItem
                .Where(i => i.TransactionId == transactionId)
                .ToListAsync();

            if (!items.Any())
            {
                return NotFound("No items found for this transaction.");
            }

            return Ok(items);
        }




        /// <summary>
        /// Retrieves a transaction by ID along with its associated transaction items.
        /// </summary>
        /// <param name="id">The ID of the transaction.</param>
        /// <response code="200">Returns the transaction with its items.</response>
        /// <response code="404">If the transaction is not found.</response>
        [HttpGet("Transactions/{id}/Items")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTransactionWithItems(int id)
        {
            // First get the transaction
            var transaction = await _context.Transaction
                .FirstOrDefaultAsync(t => t.TransactionId == id);

            if (transaction == null)
            {
                return NotFound(new { Message = $"Transaction with ID {id} not found." });
            }

            // Then get the transaction items with their associated products
            var items = await _context.TransactionItem
                .Where(ti => ti.TransactionId == id)
                .Join(
                    _context.Products,
                    ti => ti.ProductId,
                    p => p.ID,
                    (ti, p) => new
                    {
                        ti.Id,
                        ti.ProductId,
                        ProductName = p.Name,
                        ti.Quantity,
                        ti.UnitPrice,
                        ti.TotalPrice
                    })
                .ToListAsync();

            var result = new
            {
                TransactionId = transaction.TransactionId,
                TransactionDate = transaction.TransactionDate,
                TotalAmount = transaction.TotalAmount,
                PaymentMethod = transaction.PaymentMethod.ToString(),
                Discount = transaction.Discount,
                TaxAmount = transaction.TaxAmount,
                UserId = transaction.UserId,
                CustomerId = transaction.CustomerId,
                Items = items
            };

            return Ok(result);
        }


        /// <summary>
        /// Creates a new transaction
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/Maui/Transaction
        ///     {
        ///         "transactionDate": "2024-11-28T21:11:27.114Z",
        ///         "totalAmount": 13.50,
        ///         "discount": 0,
        ///         "taxAmount": 0,
        ///         "userId": 1,
        ///         "customerId": 2,
        ///         "paymentMethod": "DebitCard"
        ///     }
        /// </remarks>
        [HttpPost("Transactions")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TransactionDto>> PostTransaction([FromBody] TransactionCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { Message = "Invalid model state" });
                }

                if (createDto.UserId <= 0)
                {
                    return BadRequest(new { Message = "Invalid or missing UserId" });
                }

                if (createDto.CustomerId <= 0)
                {
                    return BadRequest(new { Message = "Invalid or missing CustomerId" });
                }

                // Validate that the customer exists
                var customerExists = await _context.Customer.AnyAsync(c => c.CustomerId == createDto.CustomerId);
                if (!customerExists)
                {
                    return BadRequest(new { Message = $"Customer with ID {createDto.CustomerId} not found" });
                }

                var transaction = new Transaction
                {
                    TransactionDate = createDto.TransactionDate,
                    TotalAmount = createDto.TotalAmount,
                    Discount = createDto.Discount,
                    PaymentMethod = Enum.Parse<PaymentMethod>(createDto.PaymentMethod),
                    TaxAmount = createDto.TaxAmount,
                    UserId = createDto.UserId,
                    CustomerId = createDto.CustomerId
                };

                _context.Transaction.Add(transaction);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetTransaction),
                    new { id = transaction.TransactionId },
                    new { TransactionId = transaction.TransactionId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Error creating transaction",
                    Error = ex.Message
                });
            }
        }



        [HttpPost("Transactions/{transactionId}/Items")]
        public async Task<ActionResult> PostTransactionItems(int transactionId, [FromBody] List<TransactionItemDto> items)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Start a transaction to ensure both operations complete or neither does
                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    // First, add the transaction items
                    var transactionItems = items.Select(item => new TransactionItem
                    {
                        TransactionId = transactionId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.TotalPrice
                    }).ToList();

                    await _context.TransactionItem.AddRangeAsync(transactionItems);

                    // Now update the product stock levels
                    foreach (var item in items)
                    {
                        var product = await _context.Products
                            .FirstOrDefaultAsync(p => p.ID == item.ProductId);

                        if (product != null)
                        {
                            product.CurrentStockLevel -= item.Quantity;
                            // Ensure stock level doesn't go negative
                            if (product.CurrentStockLevel < 0)
                            {
                                product.CurrentStockLevel = 0;
                            }

                            _context.Products.Update(product);
                        }
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Ok(new { Message = "Transaction items added and stock levels updated successfully" });
                }
                catch (Exception)
                {
                    // If anything goes wrong, roll back both operations
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Error processing transaction: {ex.Message}" });
            }
        }



        // GET: api/Maui/Users
        [HttpGet("Users")]
        public async Task<IActionResult> GetAllUsers()
        {
            _logger.LogInformation("Fetching all users in MAUI");
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }




    }
}

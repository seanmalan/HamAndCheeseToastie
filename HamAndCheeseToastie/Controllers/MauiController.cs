﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HamAndCheeseToastie.Models;
using HamAndCheeseToastie.Database;
using Microsoft.EntityFrameworkCore;
using HamAndCheeseToastie.DTOs;

namespace HamAndCheeseToastie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class MauiController : ControllerBase
    {
        #region Fields
        private readonly DatabaseContext _context;
        private readonly ILogger<MauiController> _logger;
        #endregion

        #region Constructor
        public MauiController(DatabaseContext context, ILogger<MauiController> logger)
        {
            _context = context;
            _logger = logger;
        }
        #endregion

        #region Product Endpoints
        /// <summary>
        /// Retrieves a paginated list of products
        /// </summary>
        /// <param name="offset">Number of records to skip</param>
        /// <param name="limit">Maximum number of records to return</param>
        /// <returns>List of products with total count</returns>
        /// <response code="200">Returns the list of products</response>
        /// <response code="400">If the offset or limit parameters are invalid</response>
        /// <response code="404">If no products are found</response>
        [HttpGet("products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Searches for products based on query and optional category
        /// </summary>
        /// <param name="query">Search term for product name, brand, or barcode</param>
        /// <param name="category">Optional category to filter by</param>
        /// <returns>List of matching products</returns>
        /// <response code="200">Returns the matching products</response>
        /// <response code="400">If the search query is empty</response>
        /// <response code="404">If no matching products are found</response>
        [HttpGet("product/search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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



        /// <summary>
        /// Gets a product by its barcode
        /// </summary>
        /// <param name="barcode">The product's barcode</param>
        /// <returns>Product details if found</returns>
        /// <response code="200">Returns the requested product</response>
        /// <response code="404">If the product is not found</response>
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
        #endregion



        #region Customer Endpoints
        /// <summary>
        /// Gets all customers
        /// </summary>
        /// <returns>List of all customers</returns>
        /// <response code="200">Returns the list of customers</response>
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


        /// <summary>
        /// Searches for customers by name or email
        /// </summary>
        /// <param name="name">Search term for customer name or email</param>
        /// <returns>List of matching customers</returns>
        /// <response code="200">Returns matching customers</response>
        /// <response code="404">If no customers are found</response>
        [HttpGet("Customer/Search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SearchCustomers([FromQuery] string name)
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

        /// <summary>
        /// Creates a new customer
        /// </summary>
        /// <param name="customerDto">The customer data</param>
        /// <returns>The created customer</returns>
        /// <response code="201">Customer created successfully</response>
        /// <response code="400">If the customer data is invalid</response>
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
        #endregion





        #region Transaction Endpoints
        /// <summary>
        /// Gets filtered transactions
        /// </summary>
        /// <param name="dateFrom">Start date for filtering</param>
        /// <param name="dateTo">End date for filtering</param>
        /// <param name="count">Maximum number of transactions to return</param>
        /// <returns>List of filtered transactions</returns>
        /// <response code="200">Returns the filtered transactions</response>
        /// <response code="404">If no transactions are found</response>
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
                    TransactionDate = TimeZoneInfo.ConvertTimeFromUtc(t.TransactionDate, nzTimeZone),
                    t.TotalAmount,
                    t.Discount,
                    t.PaymentMethod,
                    t.TaxAmount,
                    t.UserId,
                    t.CustomerId,
                    Customer = new
                    {
                        t.Customer.FirstName,
                        t.Customer.LastName,
                        FullName = t.Customer.FirstName + " " + t.Customer.LastName
                    }
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
        public async Task<ActionResult> PostTransactionItems(int transactionId, [FromBody] List<MauiTransactionItemsDto> items)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    // Modified to only use the fields we're sending
                    var transactionItems = items.Select(item => new TransactionItem
                    {
                        TransactionId = transactionId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.TotalPrice
                    }).ToList();

                    await _context.TransactionItem.AddRangeAsync(transactionItems);

                    foreach (var item in items)
                    {
                        var product = await _context.Products
                            .FirstOrDefaultAsync(p => p.ID == item.ProductId);

                        if (product != null)
                        {
                            product.CurrentStockLevel -= item.Quantity;
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
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Error processing transaction: {ex.Message}" });
            }
        }
        #endregion





        #region User Endpoints
        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns>List of users</returns>
        /// <response code="200">Returns the list of users</response>
        [HttpGet("Users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                var userDtos = users.Select(user =>
                {
                    var names = user.username.Split(' ');
                    return new UserListDto
                    {
                        Id = user.id,
                        FirstName = names.FirstOrDefault() ?? string.Empty,
                        LastName = names.Length > 1 ? string.Join(" ", names.Skip(1)) : string.Empty,
                        Email = user.email,
                        Role = user.Role,
                        Passcode = user.Passcode,
                        last_login_at = user.last_login_at  // Add this
                    };
                });
                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching users");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets a specific user by ID
        /// </summary>
        /// <param name="userId">The user ID to search for</param>
        /// <returns>The requested user</returns>
        /// <response code="200">Returns the requested user</response>
        /// <response code="404">If the user is not found</response>
        [HttpGet("Users/{userId}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var names = user.username.Split(' ');
            var userDto = new UserListDto
            {
                Id = user.id,
                FirstName = names.FirstOrDefault() ?? string.Empty,
                LastName = names.Length > 1 ? string.Join(" ", names.Skip(1)) : string.Empty,
                Email = user.email,
                Role = user.Role,
                Passcode = user.Passcode,
                last_login_at = user.last_login_at
            };

            return Ok(userDto);
        }

        /// <summary>
        /// Checks if an admin user exists
        /// </summary>
        /// <returns>Boolean indicating if admin exists</returns>
        /// <response code="200">Returns true if admin exists, false otherwise</response>
        [HttpGet("Users/AdminExists")]
        public async Task<IActionResult> CheckAdminExists()
        {
            var adminExists = await _context.Users
                .AnyAsync(u => u.Role <= 2);
            return Ok(adminExists);
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="user">The user to create</param>
        /// <returns>The created user</returns>
        /// <response code="201">Returns the created user</response>
        /// <response code="400">If the user data is invalid</response>
        [HttpPost("Users")]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { userId = user.id }, user);
        }

        /// <summary>
        /// Updates an existing user
        /// </summary>
        /// <param name="userId">The ID of the user to update</param>
        /// <param name="user">The updated user data</param>
        /// <returns>No content</returns>
        /// <response code="204">If the user was successfully updated</response>
        /// <response code="404">If the user was not found</response>
        [HttpPut("Users/{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, UpdateUserDto updateDto)
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }

                user.username = updateDto.Username;
                user.email = updateDto.Email;
                user.Role = updateDto.Role;
                user.Passcode = updateDto.Passcode;
                user.last_login_at = updateDto.UpdatedAt;

                try
                {
                    _context.Entry(user).State = EntityState.Modified;

                    _context.Entry(user).Property(x => x.created_at).IsModified = false;
                    _context.Entry(user).Property(x => x.updated_at).IsModified = false;

                    _context.Entry(user).Property(x => x.password_hash).IsModified = false;

                    await _context.SaveChangesAsync();
                    return NoContent();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await UserExists(userId))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }

        /// <summary>
        /// Deletes a specific user
        /// </summary>
        /// <param name="userId">The ID of the user to delete</param>
        /// <returns>No content</returns>
        /// <response code="204">If the user was successfully deleted</response>
        /// <response code="404">If the user was not found</response>
        [HttpDelete("Users/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> UserExists(int userId)
        {
            return await _context.Users.AnyAsync(e => e.id == userId);
        }
        #endregion


        #region Stock Update
        [HttpPut("stock-adjustment")]
        public async Task<IActionResult> UpdateProductStock([FromBody] StockAdjustmentDto adjustment)
        {
            if (adjustment == null)
            {
                return BadRequest("Adjustment data is required.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Get the product by barcode
                var product = await _context.Products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.EAN13Barcode == adjustment.Ean13Barcode);

                // Store initial state for logging
                var oldValues = new
                {
                    StockLevel = product?.CurrentStockLevel ?? 0,
                    Name = product?.Name ?? string.Empty,
                    BrandName = product?.BrandName ?? string.Empty,
                    CategoryName = product?.Category?.Name ?? string.Empty,
                    WholesalePrice = product?.WholesalePrice ?? 0m,
                    Price = product?.Price ?? 0m
                };

                if (product == null)
                {
                    // Product doesn't exist, create new one
                    var category = await _context.Categories
                        .FirstOrDefaultAsync(c => c.Name == adjustment.CategoryName);

                    if (category == null)
                    {
                        category = new Category { Name = adjustment.CategoryName };
                        await _context.Categories.AddAsync(category);
                        await _context.SaveChangesAsync();
                    }

                    product = new Product
                    {
                        EAN13Barcode = adjustment.Ean13Barcode,
                        Name = adjustment.Name,
                        BrandName = adjustment.BrandName ?? string.Empty,
                        CategoryId = category.CategoryId,
                        Weight = adjustment.Weight ?? string.Empty,
                        CurrentStockLevel = 0,
                        MinimumStockLevel = 0,
                        WholesalePrice = adjustment.WholesalePrice ?? 0,
                        Price = adjustment.Price ?? 0,
                        ImagePath = string.Empty
                    };

                    await _context.Products.AddAsync(product);
                    await _context.SaveChangesAsync();
                }

                // Update existing product
                if (!string.IsNullOrEmpty(adjustment.Name))
                    product.Name = adjustment.Name;
                if (!string.IsNullOrEmpty(adjustment.BrandName))
                    product.BrandName = adjustment.BrandName;
                if (!string.IsNullOrEmpty(adjustment.CategoryName))
                {
                    var category = await _context.Categories
                        .FirstOrDefaultAsync(c => c.Name == adjustment.CategoryName);
                    if (category != null)
                        product.CategoryId = category.CategoryId;
                }
                if (!string.IsNullOrEmpty(adjustment.Weight))
                    product.Weight = adjustment.Weight;
                if (adjustment.WholesalePrice.HasValue)
                    product.WholesalePrice = adjustment.WholesalePrice.Value;
                if (adjustment.Price.HasValue)
                    product.Price = adjustment.Price.Value;

                // Adjust stock level
                var newStockLevel = product.CurrentStockLevel + adjustment.StockAdjustment;
                if (newStockLevel < 0)
                {
                    return BadRequest("Stock level cannot be negative.");
                }
                product.CurrentStockLevel = newStockLevel;

                // Create inventory log entry
                var log = new InventoryLog
                {
                    ProductID = product.ID,
                    Barcode = product.EAN13Barcode,
                    ChangeType = BuildChangeTypeString(product, oldValues),
                    StockOldValue = oldValues.StockLevel.ToString(),
                    StockNewValue = product.CurrentStockLevel.ToString(),
                    ReductionReason = adjustment.StockAdjustment < 0 ? adjustment.ReductionReason : null,
                    NameOldValue = oldValues.Name,
                    NameNewValue = product.Name,
                    BrandOldValue = oldValues.BrandName,
                    BrandNewValue = product.BrandName,
                    CategoryOldValue = oldValues.CategoryName,
                    CategoryNewValue = product.Category?.Name,
                    PriceOldValue = oldValues.Price,
                    PriceNewValue = product.Price,
                    WholesalePriceOldValue = oldValues.WholesalePrice,
                    WholesalePriceNewValue = product.WholesalePrice,
                    Timestamp = DateTime.UtcNow
                };

                await _context.InventoryLog.AddAsync(log);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    Message = "Stock updated successfully",
                    NewStockLevel = product.CurrentStockLevel,
                    ProductId = product.ID
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { Message = $"Error updating stock: {ex.Message}" });
            }
        }

        private string BuildChangeTypeString(Product product, dynamic oldValues)
        {
            var changes = new List<string>();

            if (oldValues.StockLevel != product.CurrentStockLevel)
                changes.Add("Stock");
            if (oldValues.Name != product.Name)
                changes.Add("Name");
            if (oldValues.BrandName != product.BrandName)
                changes.Add("Brand");
            if (oldValues.CategoryName != product.Category.Name)
                changes.Add("Category");
            if (oldValues.WholesalePrice != product.WholesalePrice)
                changes.Add("WholesalePrice");
            if (oldValues.Price != product.Price)
                changes.Add("Price");

            return string.Join(",", changes);
        }


        #endregion
    }
}

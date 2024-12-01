using HamAndCheeseToastie.Database;
using HamAndCheeseToastie.DTOs;
using HamAndCheeseToastie.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly DatabaseContext _context;

    public TransactionController(DatabaseContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTransactions(
    [FromQuery] DateTime? dateFrom = null,
    [FromQuery] DateTime? dateTo = null)
    {
        var fromDate = (dateFrom ?? DateTime.Now.AddDays(-30)).ToUniversalTime();
        var toDate = (dateTo ?? DateTime.Now).ToUniversalTime();

        if (fromDate > toDate)
        {
            return BadRequest("Invalid date range: 'dateFrom' must be earlier than 'dateTo'.");
        }

        var transactions = await _context.Transaction
            .Where(t => t.TransactionDate >= fromDate && t.TransactionDate <= toDate)
            .OrderByDescending(t => t.TransactionDate)
            .Join(
                _context.Customer,
                t => t.CustomerId,
                c => c.CustomerId,
                (t, c) => new { Transaction = t, Customer = c })
            .Join(
                _context.Users,
                tc => tc.Transaction.UserId,
                u => u.id,
                (tc, u) => new
                {
                    tc.Transaction.TransactionId,
                    tc.Transaction.TransactionDate,
                    tc.Transaction.TotalAmount,
                    tc.Transaction.Discount,
                    tc.Transaction.PaymentMethod,
                    tc.Transaction.TaxAmount,
                    UserId = tc.Transaction.UserId,
                    CashierName = u.username,
                    Customer = new
                    {
                        tc.Customer.CustomerId,
                        tc.Customer.FirstName,
                        tc.Customer.LastName,
                        tc.Customer.Email,
                        tc.Customer.PhoneNumber,
                        tc.Customer.IsLoyaltyMember
                    }
                })
            .ToListAsync();

        if (!transactions.Any())
        {
            return NotFound("No transactions found for the specified date range.");
        }

        var transactionIds = transactions.Select(t => t.TransactionId).ToList();
        var transactionItems = await _context.TransactionItem
            .Where(ti => transactionIds.Contains(ti.TransactionId))
            .Join(
                _context.Products,
                ti => ti.ProductId,
                p => p.ID,
                (ti, p) => new
                {
                    ti.TransactionId,
                    ti.Id,
                    ti.Quantity,
                    ti.UnitPrice,
                    Product = new
                    {
                        p.ID,
                        p.Name,
                        p.BrandName,
                        p.Weight,
                        p.CategoryId,
                        p.CurrentStockLevel,
                        p.MinimumStockLevel,
                        p.Price,
                        p.WholesalePrice,
                        p.EAN13Barcode
                    }
                })
            .ToListAsync();

        var result = transactions.Select(t => new
        {
            t.TransactionId,
            t.TransactionDate,
            t.TotalAmount,
            t.Discount,
            t.PaymentMethod,
            t.TaxAmount,
            t.Customer,
            t.UserId,
            t.CashierName,
            TransactionItems = transactionItems
                .Where(ti => ti.TransactionId == t.TransactionId)
                .Select(ti => new
                {
                    ti.Id,
                    ti.Product,
                    ti.Quantity,
                    ti.UnitPrice
                })
        });

        return Ok(result);
    }



    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionDto>> GetTransaction(int id)
    {
        var transaction = await _context.Transaction
            .Where(t => t.TransactionId == id)
            .Select(t => new TransactionDto
            {
                TransactionId = t.TransactionId,
                TransactionDate = t.TransactionDate,
                TotalAmount = t.TotalAmount,
                Discount = t.Discount,
                TaxAmount = t.TaxAmount,
                UserId = t.UserId,
                PaymentMethod = t.PaymentMethod.ToString()
            })
            .FirstOrDefaultAsync();

        if (transaction == null)
        {
            return NotFound(new { Message = $"Transaction with ID {id} not found." });
        }

        // Get transaction items separately
        var items = await _context.TransactionItem
            .Where(ti => ti.TransactionId == id)
            .Join(
                _context.Products,
                ti => ti.ProductId,
                p => p.ID,
                (ti, p) => new TransactionItemDto
                {
                    Id = ti.Id,
                    ProductId = ti.ProductId,
                    Quantity = ti.Quantity,
                    UnitPrice = ti.UnitPrice,
                    TotalPrice = ti.TotalPrice,
                    Product = new ProductDto
                    {
                        ID = p.ID,
                        Name = p.Name,
                        BrandName = p.BrandName,
                        Weight = p.Weight
                    }
                })
            .ToListAsync();

        transaction.TransactionItems = items;
        return Ok(transaction);
    }



    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTransaction(int id, TransactionDto transactionDto)
    {
        if (id != transactionDto.TransactionId)
        {
            return BadRequest(new { Message = "Transaction ID mismatch." });
        }

        var transaction = new Transaction
        {
            TransactionId = transactionDto.TransactionId,
            TransactionDate = transactionDto.TransactionDate,
            TotalAmount = transactionDto.TotalAmount,
            Discount = transactionDto.Discount,
            PaymentMethod = Enum.Parse<PaymentMethod>(transactionDto.PaymentMethod),
            TaxAmount = transactionDto.TaxAmount,
            UserId = transactionDto.UserId,
            CustomerId = transactionDto.CustomerId
        };

        _context.Entry(transaction).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TransactionExists(id))
            {
                return NotFound(new { Message = "Transaction not found for update." });
            }
            throw;
        }

        return NoContent();
    }



    [HttpPost]
    public async Task<ActionResult<TransactionDto>> PostTransaction([FromBody] TransactionDto transactionDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var transaction = new Transaction
        {
            TransactionDate = transactionDto.TransactionDate,
            TotalAmount = transactionDto.TotalAmount,
            Discount = transactionDto.Discount,
            PaymentMethod = Enum.Parse<PaymentMethod>(transactionDto.PaymentMethod),
            TaxAmount = transactionDto.TaxAmount,
            UserId = transactionDto.UserId,
            CustomerId = transactionDto.CustomerId
        };

        _context.Transaction.Add(transaction);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetTransaction),
            new { id = transaction.TransactionId },
            new { TransactionId = transaction.TransactionId });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTransaction(int id)
    {
        var transaction = await _context.Transaction.FindAsync(id);
        if (transaction == null)
        {
            return NotFound(new { Message = $"Transaction with ID {id} not found." });
        }

        // First delete related transaction items
        var relatedItems = await _context.TransactionItem
            .Where(ti => ti.TransactionId == id)
            .ToListAsync();

        _context.TransactionItem.RemoveRange(relatedItems);
        _context.Transaction.Remove(transaction);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("api/maui/transactions")]
    public async Task<IActionResult> GetTransactions(DateTime? dateFrom = null, DateTime? dateTo = null, int count = 100)
    {
        dateFrom ??= DateTime.MinValue;
        dateTo ??= DateTime.MaxValue;

        var transactions = await _context.Transaction
            .Where(t => t.TransactionDate >= dateFrom && t.TransactionDate <= dateTo)
            .OrderByDescending(t => t.TransactionDate)
            .Take(count)
            .Select(t => new MauiTransactionDto
            {
                Id = t.TransactionId,
                DateTime = t.TransactionDate,
                TotalAmount = t.TotalAmount,
                Discount = t.Discount,
                GServiceTax = t.TaxAmount,
                CustomerId = t.CustomerId
            })
            .ToListAsync();

        return Ok(transactions);
    }

    private bool TransactionExists(int id)
    {
        return _context.Transaction.Any(e => e.TransactionId == id);
    }
}
using System.Reflection.Metadata.Ecma335;
using backend.Db;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : Controller
{
    AppDb _db;

    public ProductsController(AppDb db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = new List<Product>();
        await _db.Connection.OpenAsync();
        var cmd = _db.Connection.CreateCommand();
        cmd.CommandText = "SELECT * FROM product";
        var reader = await cmd.ExecuteReaderAsync();
        using(reader)
        {
            while(await reader.ReadAsync())
            {
                var product = new Product();
                product.Id = reader.GetInt32("id");
                product.Title = reader.GetString("title");
                product.Price = reader.GetDouble("price");
                product.Quantity = reader.GetInt32("quantity");
                products.Add(product);
            }
        }

        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(string id)
    {
        await _db.Connection.OpenAsync();

        using var cmd = _db.Connection.CreateCommand();
        cmd.CommandText = "SELECT * FROM product WHERE id=" + id;
        var reader = await cmd.ExecuteReaderAsync();
        var product = new Product();

        using (reader)
        {
            await reader.ReadAsync();
            product.Id = reader.GetInt32("Id");
            product.Title = reader.GetString("Title");
            product.Price = reader.GetDouble("Price");
            product.Quantity = reader.GetInt32("Quantity");
        }

        return Ok(product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProductById(Product product)
    {
        await _db.Connection.OpenAsync();

        using var cmd = _db.Connection.CreateCommand();
        cmd.CommandText = $"UPDATE product SET " +
                                        " title='" + product.Title + "'," +
                                        " price=" +  product.Price + "," + 
                                        " quantity=" + product.Quantity + 
                                        " WHERE id=" + product.Id;
        var reader = await cmd.ExecuteNonQueryAsync();

        return Ok(product);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductById(string id)
    {
        await _db.Connection.OpenAsync();

        using var cmd = _db.Connection.CreateCommand();
        cmd.CommandText = "DELETE FROM product WHERE id=" + id;
        var reader = await cmd.ExecuteNonQueryAsync(); ;

        return Ok(true);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] Product product)
    {
        await _db.Connection.OpenAsync();

        using var cmd = _db.Connection.CreateCommand();
        var sql = "INSERT INTO product (title,price,quantity) VALUES ('"
                                                + product.Title 
                                                + "', " 
                                                + product.Price 
                                                + ", " 
                                                + product.Quantity + ")";
        Console.WriteLine(sql);
        cmd.CommandText = sql;
        var reader = await cmd.ExecuteNonQueryAsync();

        return Ok(true);
    }
}
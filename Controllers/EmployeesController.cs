using EmployeeManagementApp.Data;
using EmployeeManagementApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementApp.Controllers;

[ApiController]
[Route("api/[controller]")]  // ensures /api/employees
public class EmployeesController : ControllerBase
{
    private readonly AppDbContext _db;

    public EmployeesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IEnumerable<Employee>> GetEmployees() =>
        await _db.Employees.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetEmployee(int id)
    {
        var emp = await _db.Employees.FindAsync(id);
        return emp is null ? NotFound() : emp;
    }

    [HttpPost]
    public async Task<IActionResult> AddEmployee(Employee emp)
    {
        _db.Employees.Add(emp);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetEmployee), new { id = emp.Id }, emp);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, Employee input)
    {
        var emp = await _db.Employees.FindAsync(id);
        if (emp is null) return NotFound();

        emp.Name = input.Name;
        emp.Position = input.Position;
        emp.Salary = input.Salary;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var emp = await _db.Employees.FindAsync(id);
        if (emp is null) return NotFound();

        _db.Employees.Remove(emp);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}


using Lab13AlberthMCuevas.Domain.Ports;
using Lab13AlberthMCuevas.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Lab13AlberthMCuevas.Infrastructure.Adapters;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly LinqContext _context;

    public Repository(LinqContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T?> GetById(object id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task Add(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public async Task Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public async Task Delete(object id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<T>().Remove(entity);
        }
    }
}
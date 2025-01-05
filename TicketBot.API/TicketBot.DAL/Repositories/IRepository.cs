using System.Linq.Expressions;
using TicketBot.DAL.Models;

namespace TicketBot.DAL.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task SaveChangesAsync();
    }

    public interface IMovieRepository : IRepository<Movie> { }
    public interface IMovieHallRepository : IRepository<MovieHall> { }
    public interface IScheduleRepository : IRepository<Schedule> { }
    public interface ITicketRepository : IRepository<Ticket> { }
}

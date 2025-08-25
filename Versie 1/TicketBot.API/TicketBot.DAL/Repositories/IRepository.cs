using System.Linq.Expressions;
using TicketBot.DAL.Models;

namespace TicketBot.DAL.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        Task<T> GetByIDAsync(int id, params Expression<Func<T, object>>[] includes);

        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task SaveChangesAsync();

        IEnumerable<T> Get(
                Expression<Func<T, bool>> filter = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                params Expression<Func<T, object>>[] includes);
    }

    public interface IMovieRepository : IRepository<Movie> { }
    public interface IMovieHallRepository : IRepository<MovieHall> { }
    public interface IScheduleRepository : IRepository<Schedule> { }
    public interface ITicketRepository : IRepository<Ticket> { }
}

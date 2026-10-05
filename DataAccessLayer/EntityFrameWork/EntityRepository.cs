using App_Model;
using Microsoft.EntityFrameworkCore;
using Contracts;

namespace DataAccessLayer
{
    /// <summary>
    /// Универсальный репозиторий для работы с сущностями через Entity Framework Core.
    /// Реализует основные CRUD-операции, определенные интерфейсом IRepository.
    /// </summary>
    /// <typeparam name="T">
    /// Тип сущности, реализующей интерфейс IDomainObject.
    /// </typeparam>
    public class EntityRepository<T> : IRepository<T>
        where T : class, IDomainObject
    {
        /// <summary>
        /// Добавляет новую сущность в базу данных.
        /// </summary>
        /// <param name="item">Добавляемая сущность.</param>
        public void Add(T item)
        {
            using DBContext context = new DBContext();

            context.Set<T>().Add(item);
            context.SaveChanges();
        }

        /// <summary>
        /// Удаляет сущность из базы данных по ее идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор удаляемой сущности.</param>
        public void Delete(int id)
        {
            using DBContext context = new DBContext();

            T? item = context.Set<T>().Find(id);

            if (item != null)
            {
                context.Set<T>().Remove(item);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Возвращает все сущности данного типа из базы данных.
        /// Для Trainer и Athlete также загружает связанные сущности.
        /// </summary>
        /// <returns>Коллекция найденных сущностей.</returns>
        public IEnumerable<T> List()
        {
            using DBContext context = new DBContext();

            if (typeof(T) == typeof(Trainer))
            {
                return context.Set<T>()
                    .Include("Athlete")
                    .AsNoTracking()
                    .ToList();
            }

            if (typeof(T) == typeof(Athlete))
            {
                return context.Set<T>()
                    .Include("trainer")
                    .AsNoTracking()
                    .ToList();
            }

            return context.Set<T>()
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Выполняет поиск сущности по ее идентификатору.
        /// Для Trainer и Athlete также загружает связанные сущности.
        /// </summary>
        /// <param name="id">Идентификатор искомой сущности.</param>
        /// <returns>
        /// Найденная сущность или null, если сущность с указанным идентификатором отсутствует.
        /// </returns>
        public T? ReadById(int id)
        {
            using DBContext context = new DBContext();

            if (typeof(T) == typeof(Trainer))
            {
                return context.Set<T>()
                    .Include("Athlete")
                    .AsNoTracking()
                    .FirstOrDefault(x => x.Id == id);
            }

            if (typeof(T) == typeof(Athlete))
            {
                return context.Set<T>()
                    .Include("trainer")
                    .AsNoTracking()
                    .FirstOrDefault(x => x.Id == id);
            }

            return context.Set<T>()
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Обновляет данные существующей сущности в базе данных.
        /// </summary>
        /// <param name="item">Сущность с обновленными данными.</param>
        public void Update(T item)
        {
            using DBContext context = new DBContext();

            context.Set<T>().Update(item);
            context.SaveChanges();
        }
    }
}
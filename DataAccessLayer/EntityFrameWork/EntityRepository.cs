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
        private readonly DBContext _context;

        public EntityRepository(DBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Добавляет новую сущность в базу данных.
        /// </summary>
        /// <param name="item">Добавляемая сущность.</param>
        public void Add(T item)
        {
            _context.Set<T>().Add(item);
        }

        /// <summary>
        /// Удаляет сущность из базы данных по ее идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор удаляемой сущности.</param>
        public void Delete(int id)
        {
            T? item = _context.Set<T>().Find(id);

            if (item != null)
            {
                _context.Set<T>().Remove(item);
            }
        }

        /// <summary>
        /// Возвращает все сущности данного типа из базы данных.
        /// Для Trainer и Athlete также загружает связанные сущности.
        /// </summary>
        /// <returns>Коллекция найденных сущностей.</returns>
        public IEnumerable<T> List()
        {
            if (typeof(T) == typeof(Trainer))
            {
                return _context.Set<T>()
                    .Include("Athlete")
                    .ToList();
            }

            if (typeof(T) == typeof(Athlete))
            {
                return _context.Set<T>()
                    .Include("trainer")
                    .ToList();
            }

            return _context.Set<T>().ToList();
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
            if (typeof(T) == typeof(Trainer))
            {
                return _context.Set<T>()
                    .Include("Athlete")
                    .FirstOrDefault(x => x.Id == id);
            }

            if (typeof(T) == typeof(Athlete))
            {
                return _context.Set<T>()
                    .Include("trainer")
                    .FirstOrDefault(x => x.Id == id);
            }

            return _context.Set<T>().FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Обновляет данные существующей сущности в базе данных.
        /// </summary>
        /// <param name="item">Сущность с обновленными данными.</param>
        public void Update(T item)
        {
            _context.Set<T>().Update(item);
        }
    }
}

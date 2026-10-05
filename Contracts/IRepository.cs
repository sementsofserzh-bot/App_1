using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    /// <summary>
    /// Определяет общий интерфейс репозитория для работы с сущностями.
    /// Содержит основные CRUD-операции для добавления, чтения,
    /// обновления и удаления данных.
    /// </summary>
    /// <typeparam name="T">
    /// Тип сущности, реализующей интерфейс IDomainObject.
    /// </typeparam>
    public interface IRepository<T> where T : class, IDomainObject
    {
        /// <summary>
        /// Добавляет новую сущность.
        /// </summary>
        /// <param name="item">Добавляемая сущность.</param>
        void Add(T item);

        /// <summary>
        /// Удаляет сущность по ее идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор удаляемой сущности.</param>
        void Delete(int id);

        /// <summary>
        /// Возвращает все сущности данного типа.
        /// </summary>
        /// <returns>Коллекция сущностей.</returns>
        IEnumerable<T> List();

        /// <summary>
        /// Возвращает сущность по ее идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор искомой сущности.</param>
        /// <returns>
        /// Найденная сущность или null, если сущность с указанным идентификатором отсутствует.
        /// </returns>
        T? ReadById(int id);

        /// <summary>
        /// Обновляет данные существующей сущности.
        /// </summary>
        /// <param name="item">Сущность с обновленными данными.</param>
        void Update(T item);
    }
}
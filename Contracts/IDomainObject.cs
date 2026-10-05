using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    /// <summary>
    /// Определяет базовый интерфейс для доменных объектов.
    /// Гарантирует наличие уникального идентификатора Id у сущности.
    /// </summary>
    public interface IDomainObject
    {
        /// <summary>
        /// Идентификатор сущности.
        /// </summary>
        int Id { get; set; }
    }
}
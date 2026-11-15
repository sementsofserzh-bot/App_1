using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    /// <summary>
    /// Определяет общий контракт представления.
    /// Содержит методы для отображения информационных сообщений
    /// и ошибок пользователю.
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// Отображает пользователю информационное сообщение.
        /// </summary>
        /// <param name="message">Текст информационного сообщения.</param>
        void ShowMessage(string message);
        /// <summary>
        /// Отображает пользователю сообщение об ошибке.
        /// </summary>
        /// <param name="message">Текст сообщения об ошибке.</param>
        void ShowError(string message);
    }
}

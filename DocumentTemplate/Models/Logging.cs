namespace DocumentTemplate.Models
{
    /// <summary>
    /// Логгирование
    /// </summary>
    public class Logging
    {
        /// <summary>
        /// Индетификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Индетификатор пользователя
        /// </summary>
        public Guid  UserId { get; set; }

        /// <summary>
        /// Дата и время создания записи
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// Действие
        /// </summary>
        public string ActionUser { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        public User User { get; set; }
    }
}

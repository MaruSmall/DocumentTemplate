using System.Security.Cryptography.X509Certificates;

namespace DocumentTemplate.Models
{
    /// <summary>
    /// Электронное письмо
    /// </summary>
    public class Email
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Откого
        /// </summary>
        public string From { get; set; }
        
        /// <summary>
        /// Кому
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Тема
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Тело
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Вложение
        /// </summary>
        public byte[] Attachment { get; set; }
    }
}

using DocumentTemplate.Models;

namespace DocumentTemplate.ViewModels
{
    /// <summary>
    /// Модель для представления для заполнения полей
    /// </summary>
    public class FillingOutDocumentVm
    {
        public FillingOutDocumentVm() 
        {
            Fields = new List<Field>();
        }

        /// <summary>
        /// Поля
        /// </summary>
        public List<Field> Fields { get; set; }

        /// <summary>
        /// Массив с файлом
        /// </summary>
        public byte[] File { get; set; }

        /// <summary>
        /// Тип файла
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Название файла
        /// </summary>
        public string FileName { get; set; }
    }
}

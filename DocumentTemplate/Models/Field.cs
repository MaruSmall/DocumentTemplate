namespace DocumentTemplate.Models
{
    /// <summary>
    /// Поле
    /// </summary>
    public class Field
    {
        /// <summary>
        /// Индетиикатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название 
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Тэг в тесте 
        /// </summary>
        public string? Tag { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public string? Value { get; set; }
    }
}

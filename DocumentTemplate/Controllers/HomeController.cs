using System.Diagnostics;
using System.Text;
using DocumentTemplate.Data;
using DocumentTemplate.Models;
using DocumentTemplate.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DocumentTemplate.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DocumentTemplateDbContext _context;

        public HomeController(ILogger<HomeController> logger, DocumentTemplateDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Обработка загруженного файла
        /// </summary>
        /// <param name="uploadedFile">Файл</param>
        /// <returns>Новое предстваление для заполнения данных для тэгов, которые нашли в файле</returns>
        [HttpPost]
        public IActionResult FileProcessing(IFormFile uploadedFile)
        {
            var vm = new FillingOutDocumentVm();
            var fileLines = new List<string>();
            using (var inputStreamReader = new StreamReader(uploadedFile.OpenReadStream(), Encoding.Default))
            {
                string line;

                while ((line = inputStreamReader.ReadLine()) != null)
                {
                    fileLines.Add(line);
                }
            }

            var tags = new List<string>();
            var tag = new StringBuilder();

            foreach (var line in fileLines)
            {
                if (line.Contains('<') && line.Contains('>'))
                {
                    var flag = false;

                    for (var i = 0; i < line.Length; i++)
                    {
                        if (line[i] == '<')
                        {
                            tag.Append(line[i]);
                            flag = true;
                            continue;
                        }
                        if (line[i] == '>')
                        {
                            tag.Append(line[i]);
                            tags.Add(tag.ToString());
                            tag.Clear();
                            flag = false;

                        }
                        if (flag)
                        {
                            tag.Append(line[i]);
                        }

                    }
                }
            }

            vm.File = fileLines.SelectMany(s => Encoding.UTF8.GetBytes(s)).ToArray();
            vm.ContentType = uploadedFile.ContentType;

            vm.FileName = uploadedFile.FileName;

            foreach (var item in tags)
            {
                var field = _context.Fields.SingleOrDefault(t => t.Tag == item);

                if (field == null)
                {
                    var fieldNew = new Field { Tag = item };
                    _context.Fields.Add(fieldNew);

                    field = fieldNew;
                }

                vm.Fields.Add(field);
            }

            Logging("Открытие файла, поиск тэгов.", $"Пользователь выбрал файл с названием {vm.FileName}, найденые тэги {string.Join(",", tags)}");

            return View("FillingOutDocument", vm);
        }

        /// <summary>
        /// Заполнение файла
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult FillingInFile(FillingOutDocumentVm vm)
        {
            //вставляем данные, которые заполнели, и возвращаем файл, делаем запись в таблицу логов 
            var doc = Encoding.UTF8.GetString(vm.File);

            foreach (var tag in vm.Fields)
            {
                doc = doc.Replace(tag.Tag, tag.Value);
            }

            Logging("Изменение тэгов", "Изменение тэгов полей на значение, которое ввели на форме, тэги и значения: ");

            return File(Encoding.UTF8.GetBytes(doc), vm.ContentType, vm.FileName);
        }

        /// <summary>
        /// Отправка файла по почте
        /// </summary>
        /// <returns></returns>
        public IActionResult SendEmail()
        {
            //отображение полей для заполнения 
            return View();
        }

        [HttpPost]
        public IActionResult SendEmail([Bind("")] int value)
        {
            Logging("Отправка письма", $"Отправка письма на почту: {value}"); ;
            //отправляем файл по почте, записываем действие в таблицу логов
            return View();
        }

        /// <summary>
        /// Заглушка для получения пользователя
        /// </summary>
        /// <returns> Объект пользователя</returns>
        private User GetUser()
        {
            var user = _context.Users.FirstOrDefault();

            if (user == null)
            {
                user = new User { Id = Guid.NewGuid(), FirstName = "Test", LastName = "Test", Patronymic = "Test", Email = "test" };
                
                _context.Users.Add(user);
                _context.SaveChanges();
            }

            return user;
        }

        /// <summary>
        /// Записываем действие в логи
        /// </summary>
        /// <param name="actionUser"> Действие</param>
        /// <param name="description"> Описание</param>
        private void Logging(string actionUser, string description)
        {
            var log = new Logging
            {
                UserId = GetUser().Id, //берем пока новый гуид, тк нет реализации авторизации, если бы это было в системе, где есть авторизация, то сюда бы записи гуид авторизованного пользователя
                ActionUser = actionUser,
                CreateDateTime = DateTime.Now,
                Description = description
            };

            _context.Loggings.Add(log);
            _context.SaveChanges();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

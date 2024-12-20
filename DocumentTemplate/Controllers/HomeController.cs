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
        /// ��������� ������������ �����
        /// </summary>
        /// <param name="uploadedFile">����</param>
        /// <returns>����� ������������� ��� ���������� ������ ��� �����, ������� ����� � �����</returns>
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

            Logging("�������� �����, ����� �����.", $"������������ ������ ���� � ��������� {vm.FileName}, �������� ���� {string.Join(",", tags)}");

            return View("FillingOutDocument", vm);
        }

        /// <summary>
        /// ���������� �����
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult FillingInFile(FillingOutDocumentVm vm)
        {
            //��������� ������, ������� ���������, � ���������� ����, ������ ������ � ������� ����� 
            var doc = Encoding.UTF8.GetString(vm.File);

            foreach (var tag in vm.Fields)
            {
                doc = doc.Replace(tag.Tag, tag.Value);
            }

            Logging("��������� �����", "��������� ����� ����� �� ��������, ������� ����� �� �����, ���� � ��������: ");

            return File(Encoding.UTF8.GetBytes(doc), vm.ContentType, vm.FileName);
        }

        /// <summary>
        /// �������� ����� �� �����
        /// </summary>
        /// <returns></returns>
        public IActionResult SendEmail()
        {
            //����������� ����� ��� ���������� 
            return View();
        }

        [HttpPost]
        public IActionResult SendEmail([Bind("")] int value)
        {
            Logging("�������� ������", $"�������� ������ �� �����: {value}"); ;
            //���������� ���� �� �����, ���������� �������� � ������� �����
            return View();
        }

        /// <summary>
        /// �������� ��� ��������� ������������
        /// </summary>
        /// <returns> ������ ������������</returns>
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
        /// ���������� �������� � ����
        /// </summary>
        /// <param name="actionUser"> ��������</param>
        /// <param name="description"> ��������</param>
        private void Logging(string actionUser, string description)
        {
            var log = new Logging
            {
                UserId = GetUser().Id, //����� ���� ����� ����, �� ��� ���������� �����������, ���� �� ��� ���� � �������, ��� ���� �����������, �� ���� �� ������ ���� ��������������� ������������
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

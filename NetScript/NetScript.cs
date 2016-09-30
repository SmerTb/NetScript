using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NetScript
{
    public class NetScript
    {
        /// <summary>
        /// Ядро скрипта
        /// </summary>
        private Core core;

        /// <summary>
        /// Код скрипта
        /// </summary>
        private string code = string.Empty;

        /// <summary>
        /// Конструктор
        /// </summary>
        public NetScript()
        {
            core = new Core();
        }

        /// <summary>
        /// Поиск и добавление скриптов в указаном каталоге
        /// </summary>
        /// <param name="folder">Путь к каталогу</param>
        /// <param name="subFolders">Включение всех подкаталогов</param>
        public void AddFolder(string folder, bool subFolders)
        {
            foreach (var item in Directory.GetFiles(folder,"*.cs"))
            {
                using (StreamReader sr = new StreamReader(item))
                {
                    code += sr.ReadToEnd() + "\r\n";
                }
            }
            if (subFolders)
            {
                foreach (var item in Directory.GetDirectories(folder))
                {
                    AddFolder(item, subFolders);
                }
            }
        }

        /// <summary>
        /// Добавить файл скрипта
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        public void AddFile(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                code += sr.ReadToEnd() + "\r\n";
            }
        }
        /// <summary>
        /// Компиляция, только после регистрации всех библиотек и using 
        /// </summary>
        public void Compile()
        {
            core.SetCode(code);
            core.Compile();
        }

        /// <summary>
        /// Регистрация библиотеки
        /// </summary>
        /// <param name="library">Имя библиотеки прим. System.dll System.Xml.dll ...</param>
        public void RegistrationReferance(string library)
        {
            core.referances.Add(library);
        }

        public void SetCompiller(CompileProvider compiller)
        {
            switch (compiller)
            {
                case CompileProvider.SharpCompiller:
                    core.CompileProvider = Activator.CreateInstance<Microsoft.CSharp.CSharpCodeProvider>();
                    break;
                case CompileProvider.VisualBasicCompiller:
                    core.CompileProvider = Activator.CreateInstance<Microsoft.VisualBasic.VBCodeProvider>();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Регистрация дерективы using
        /// </summary>
        /// <param name="_using">имя using прим. System System.IO ...</param>
        public void RegistrationUsingDirective(string _using)
        {
            core.usings.Add(_using);
        }

        /// <summary>
        /// Регистрация глобальной переменной
        /// </summary>
        /// <param name="globalvar"></param>
        public void RegistrationGlobalConst(GlobalVariable globalvar)
        {
            core.globals.Add(globalvar);
        }

        /// <summary>
        /// Установка значения глобальной переменной
        /// </summary>
        /// <param name="_var">Имя переменной</param>
        /// <param name="value">Значение переменной</param>
        public void SetConstValue(string _var, object value)
        {
            core.lib.GetType("Globals").GetField(_var).SetValue(core.lib.GetType("Core").GetField("global").GetValue(core.libData), value);
        }

        /// <summary>
        /// Вызов метода скрипта
        /// </summary>
        /// <param name="_class">Класс содержиащий метод</param>
        /// <param name="_method">Вызываемый метод</param>
        /// <param name="data">Параметры вызова</param>
        /// <returns></returns>
        public object CALL(string _class, string _method, params object[] data)
        {
            return core.lib.GetType(_class).GetMethod(_method).Invoke(core.libData, data);
        }

        /// <summary>
        /// Ошибки компиляции
        /// </summary>
        /// <returns>Ошибки компиляции</returns>
        public string GetErrorBuffer()
        {
            string s = core.errorBuffer;
            core.errorBuffer = string.Empty;
            return s;
        }

        /// <summary>
        /// Клонирование скомпилированого скрипта с новым обьектом глобальных переменных
        /// </summary>
        /// <returns>Новый экземпляр Core</returns>
        public NetScript Copy()
        {
            NetScript ns = new NetScript();
            ns.core = (Core)this.core.Clone();
            return ns;
        }
    }
}

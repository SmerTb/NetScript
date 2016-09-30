using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;

namespace NetScript
{
    internal class Core : ICloneable
    {
        /// <summary>
        /// Установленый код
        /// </summary>
        internal string code = string.Empty;

        /// <summary>
        /// Буффер ошибок компиляции
        /// </summary>
        internal string errorBuffer = string.Empty;

        /// <summary>
        /// Компилятор
        /// </summary>
        internal CodeDomProvider CompileProvider = new Microsoft.CSharp.CSharpCodeProvider();
        
        /// <summary>
        /// Скомпилирована библиотека
        /// </summary>
        internal Assembly lib;

        /// <summary>
        /// Входная точка Core
        /// </summary>
        internal object libData;

        /// <summary>
        /// Подключаемые библиотеки
        /// </summary>
        internal List<string> referances = new List<string>();

        /// <summary>
        /// using
        /// </summary>
        internal List<string> usings = new List<string>();

        /// <summary>
        /// Глобальные переменные 
        /// </summary>
        internal List<GlobalVariable> globals = new List<GlobalVariable>();

        /// <summary>
        /// Установка кода скрипта
        /// </summary>
        /// <param name="code"></param>
        internal void SetCode(string code)
        {
            this.code = code;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        internal Core()
        {

        }

        /// <summary>
        /// Клонирование скомпилированого скрипта с новым обьектом глобальных переменных
        /// </summary>
        /// <returns>Новый экземпляр Core</returns>
        public object Clone()
        { 
            Core c = new Core();
            c.lib = this.lib;
            c.libData = Activator.CreateInstance(lib.GetType("Core"));
            return c;
        }

        /// <summary>
        /// Предкомпиляция деректив using и глобальный переменных
        /// </summary>
        /// <param name="cp"></param>
        private void PreCompile(ref CompilerParameters cp)
        {
            foreach (var item in referances)
            {
                // Add an assembly reference.
                cp.ReferencedAssemblies.Add(item);
            }
            foreach (var item in usings)
            {
                this.code = string.Format("using {0};\r\n", item) + this.code;
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("public class Globals {");
            foreach (var item in globals)
            {
                sb.AppendFormat("public {0} {1};\r\n", item.type, item.name);
            }
            sb.AppendLine("}");
                this.code += sb.ToString();

        }

        /// <summary>
        /// Компиляция скриптов
        /// </summary>
        internal void Compile()
        { 
            // Build the parameters for source compilation.
            CompilerParameters cp = new CompilerParameters();

            PreCompile(ref cp);

            // Generate an executable instead of
            // a class library.
            cp.GenerateExecutable = false;

            // Save the assembly as a physical file.
            cp.GenerateInMemory = true;

            // Invoke compilation.
            CompilerResults cr = CompileProvider.CompileAssemblyFromSource(cp, this.code);

            if (cr.Errors.Count > 0)
            {
                foreach (CompilerError ce in cr.Errors)
                {
                    errorBuffer += ce.ToString() + "\r\n";
                }
                throw new Exception("Ошибка во время компиляции");
            }
            lib = cr.CompiledAssembly;
            libData = Activator.CreateInstance(lib.GetType("Core"));
        }
    }
}

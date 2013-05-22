using System;
using System.Xml;
using System.Threading;
using Microsoft.CSharp;
using System.Reflection;
using System.CodeDom.Compiler;
using System.Collections.Specialized;
using System.Collections.Generic;
using Microsoft.VisualBasic;

namespace DeviceManager
{
    /// <summary>
    /// Реализует выполнение сценария
    /// </summary>
    public partial class Script
    {
        protected bool createdScript;                   // была попытка скомпилировать скрипт или нет

        protected string _script;                       // выполняемый сценарий
        protected IScript script;                       // выполняемый скрипт

        protected string _namespace;                    // используемое пространство имен
        protected string _classname;                    // используемый класс

        protected List<string> assembly;               // список подключаемых сборок

        /// <summary>
        /// Код сценария
        /// </summary>
        public string ScriptCode
        {
            get
            {
                if (_slim.TryEnterReadLock(100))
                {
                    try
                    {
                        return _script;
                    }
                    finally
                    {
                        _slim.ExitReadLock();
                    }
                }

                return string.Empty;
            }

            set
            {
                if (_slim.TryEnterWriteLock(300))
                {
                    try
                    {
                        _script = value;
                    }
                    finally
                    {
                        _slim.ExitWriteLock();
                    }
                }
            }
        }

        /// <summary>
        /// Используемое пространство имен
        /// </summary>
        public string Namespace
        {
            get
            {
                if (_slim.TryEnterReadLock(100))
                {
                    try
                    {
                        return _namespace;
                    }
                    finally
                    {
                        _slim.ExitReadLock();
                    }
                }

                return string.Empty;
            }

            set
            {
                if (_slim.TryEnterWriteLock(300))
                {
                    try
                    {
                        _namespace = value;
                    }
                    finally
                    {
                        _slim.ExitWriteLock();
                    }
                }
            }
        }

        /// <summary>
        /// Используемые сборки скриптом
        /// </summary>
        public List<string> Assemblies
        {
            get { return assembly; }
        }

        /// <summary>
        /// Имя используемого класса в скрипте
        /// </summary>
        public string ClassName
        {
            get
            {
                if (_slim.TryEnterReadLock(100))
                {
                    try
                    {
                        return _classname;
                    }
                    finally
                    {
                        _slim.ExitReadLock();
                    }
                }

                return string.Empty;
            }

            set
            {
                if (_slim.TryEnterWriteLock(300))
                {
                    try
                    {
                        _classname = value;
                    }
                    finally
                    {
                        _slim.ExitWriteLock();
                    }
                }
            }
        }

        /// <summary>
        /// Выполнить компиляцию скрипта
        /// </summary>
        /// <param name="__script">Компилируемый скрипт</param>
        /// <param name="assemblies">Список используемых сборок</param>
        /// <returns>Результат компиляции</returns>
        public static StringCollection Compile(string __script, string[] __assemblies)
        {
            try
            {
                CSharpCodeProvider codeCompiler = new CSharpCodeProvider();
                CompilerParameters parameters = new CompilerParameters();

                parameters.GenerateInMemory = true;

                string path = Assembly.GetExecutingAssembly().Location;
                parameters.ReferencedAssemblies.Add(path);

                if (__assemblies != null && __assemblies.Length > 0)
                {
                    foreach (String __assembly in __assemblies)
                    {
                        if (__assembly != string.Empty)
                        {
                            parameters.ReferencedAssemblies.Add(__assembly);
                        }
                    }
                }

                CompilerResults results = codeCompiler.CompileAssemblyFromSource(parameters, __script);
                return results.Output;
            }
            catch (Exception ex)
            {
                StringCollection _collection = new StringCollection();
                _collection.Add(ex.Message);
                return _collection;
            }
        }

        /// <summary>
        /// Скомпилировать и загрузить скрипт
        /// </summary>
        /// <param name="__script">Текст скрипта</param>
        /// <returns>Загруженый скрипт</returns>
        protected IScript InstanceScript(string __script, string __namespace, string __classname,
            string[] __assemblies)
        {
            try
            {
                CSharpCodeProvider codeCompiler = new CSharpCodeProvider();
                CompilerParameters parameters = new CompilerParameters();

                parameters.GenerateInMemory = true;

                string path = Assembly.GetExecutingAssembly().Location;
                parameters.ReferencedAssemblies.Add(path);

                if (__assemblies != null && __assemblies.Length > 0)
                {
                    foreach (String __assembly in __assemblies)
                    {
                        if (__assembly != string.Empty)
                        {
                            parameters.ReferencedAssemblies.Add(__assembly);
                        }
                    }
                }

                CompilerResults results = codeCompiler.CompileAssemblyFromSource(parameters, __script);
                if (results.Errors.HasErrors)
                {
                    return null;
                }

                object objMacro = results.CompiledAssembly.CreateInstance(string.Format("{0}.{1}",
                    __namespace, __classname));

                if (objMacro != null) return objMacro as IScript;
            }
            catch { }
            return null;
        }

        /// <summary>
        /// Сбросить состояние макроcа в начальное состояние (по умолчанию)
        /// </summary>
        public void Reset()
        {
            if (_slim.TryEnterWriteLock(300))
            {
                try
                {
                    script = null;
                    createdScript = false;
                }
                finally
                {
                    _slim.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Шаблок скрипта
        /// </summary>
        public static string TempladeScriptCode
        {
            get
            {
                return "using System;\r\nusing DeviceManager;\r\n\r\n// При реализации конкретного сценария, необходимо заменить имя \r\n// класса и имя пространства имен на новые.\r\n\r\nnamespace TemplateScriptNamespace\r\n{\r\n\t/*\r\n\t * \r\n\t * Извлекать/присваивать значения параметров рекомендуется\r\n\t * следующим образом: \r\n\t *\r\n\t *  1.Если необходимо получить/присвоить значение параметру типа \"Захват канала\",\r\n\t *    то необходимо работать через интерфейс приведенный ниже:\r\n\t * \r\n\t *  \t\tПолучить значение формулы : _app.Converter.GetValue(number);\r\n\t *  \t\tПрисвоить значение формуле: _app.Converter.SetValue(number, nvalue);\r\n\t *  \r\n\t *  где:\r\n\t *      number - номер параметра, соответсвующий номеру присвоенному при добавлении функции в конвертор;\r\n\t *      nvalue - присваиваемое значение параметру, находящегося в конверторе;\r\n\t * \r\n\t *  2. Если необходимо извлечь/присвоить значение параметру не являющегося типом \"Захват канала\", то\r\n\t *     необходимо использовать передаваемые массивы signals и results в процедуру Run(...) сценария.\r\n\t * \r\n\t * при изменении значения параметра необходимо учитывать тот факт, что \r\n\t * параметры в конверторе вычисляеются последовательно, следовательно\r\n\t * если присвоить значение конкретному параметру до его вычисления в конверторе,\r\n\t * то присвоенное значение будет утеряно. Сценарии/Скрипты лучше всего размещать\r\n\t * в конце списка вычисляемых параметров.\r\n\t * \r\n\t * */\t \r\n\t \r\n    /// <summary>\r\n    /// Реализует конкретный сценарий\r\n    /// </summary>\r\n\tpublic sealed class TemplateScriptClass : IScript\r\n\t{\r\n\t\tprivate Application _app = null;\t\t// ссылка на интерфейсы DeviceManager\r\n\t\t\r\n\t\t/// <summary>\r\n\t\t/// Конструктор класса по умолчанию.\r\n\t\t/// </summary>\r\n\t\tpublic TemplateScriptClass()\r\n\t\t{\r\n\t\t\t// Получаем ссылку на DeviceManager\r\n\t\t\t_app = Application.CreateInstance();\r\n\t\t}\r\n\t\t\r\n\t\t/// <summary>\r\n\t\t/// Процедура, вызываемая DeviceManager\r\n\t\t/// </summary>\r\n\t\t/// <param name=\"signals\">Список значений каналов(сигналы с устройств)</param>\r\n\t\t/// <param name=\"results\">Список значений конвертора(вычисленные значения)</param>\r\n\t\t/// <returns>Результат работы сценария</returns>\r\n\t\tpublic float Run(Float[] signals, Float[] results)\r\n\t\t{\r\n\t\t\ttry\r\n\t\t\t{\r\n\t\t\t\tif (_app != null)\r\n\t\t\t\t{\r\n\t\t\t\t\t// ----- код скрипта -----\r\n\t\t\t\t}\r\n\t\t\t}\r\n\t\t\tcatch { }\r\n\t\t\treturn float.NaN;\r\n\t\t}\r\n\t}\r\n}";
            }
        }

        /// <summary>
        /// Пространство имен по умолчанию
        /// </summary>
        public static string TemplateScriptNamespace
        {
            get
            {
                return "TemplateScriptNamespace";
            }
        }

        /// <summary>
        /// Имя класса по умолчанию
        /// </summary>
        public static string TemplateClassName
        {
            get
            {
                return "TemplateScriptClass";
            }
        }

        protected static string[] ___assemblies = { "mscorlib.dll", "system.dll" };
        /// <summary>
        /// Список предлагаемых к подключению сборок
        /// </summary>
        public static String[] TemplateAssemblies
        {
            get
            {
                return ___assemblies;
            }
        }
    }
}

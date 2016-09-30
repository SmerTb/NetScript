using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetScript;

namespace NetScriptTest
{
    class Program
    {
        static Dictionary<int, TScript> scripts = new Dictionary<int, TScript>();
        static void Main(string[] args)
        {
            scripts.Add(0, new TScript(0));
            scripts.Add(1, new TScript(1));
            Random r = new Random();
            for (int i = 0; i < 21; i++)
            {
                scripts[r.Next(0, 2)].TestCall();
            }
            scripts[0].ShowCallCount();
            scripts[1].ShowCallCount();
            Console.ReadKey();
        }
    }
    public class TScript
    {
        private static NetScript.NetScript cscript;
        private NetScript.NetScript script;


        public int id;
        public int ID { get { return id; } }

        public TScript(int id)
        {
            this.id = id;
            if (cscript == null)
            {
                cscript = new NetScript.NetScript();
                cscript.AddFolder(Environment.CurrentDirectory + "\\scripts\\", true);
                cscript.RegistrationReferance("System.dll");
                cscript.RegistrationReferance("System.Linq.dll");
                cscript.RegistrationReferance("NetScriptTest.exe");
                cscript.RegistrationUsingDirective("System");
                cscript.RegistrationUsingDirective("System.Collections.Generic");
                cscript.RegistrationUsingDirective("System.Linq");
                cscript.RegistrationUsingDirective("System.Text");
                cscript.RegistrationGlobalConst(TypeHelper.GenerateVarible("NetScriptTest.TScript", "Character"));
                //cscript.SetCompiller(CompileProvider.SharpCompiller);
                cscript.Compile();
            }
            script = cscript.Copy();
            script.SetConstValue("Character", this);
            script.CALL("Core", "initialize", null);
        }
        public void TestCall()
        {
            script.CALL("Core", "initialize", null);
        }
        public void ShowCallCount()
        { 
            Console.WriteLine(string.Format("CallCount {0} for id:{1}",script.CALL("Core", "GetCallCount",null),id));
        }
        public void Debug(string value)
        {
            Console.WriteLine(value);
        }
    }
}

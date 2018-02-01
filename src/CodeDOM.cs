using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom.Compiler;
using System.CodeDom;
using Microsoft.CSharp;
using System.IO;

namespace H2F
{

    class CodeDOM
    {
        public static List<string> Compilar(string src, string saida, string icon = null)
        {
            List<string> Retorno = new List<string>();
            CompilerParameters Par = new CompilerParameters();
            Par.GenerateExecutable = true;
            Par.TreatWarningsAsErrors = false;
            Par.OutputAssembly = saida;
//            using var res as new  ResourceWriter(Application.StartupPath & "\resources.resources")

//            res.AddResource("InstallBG", Image.FromFile(Application.StartupPath & "\Resources\InstallBG.png"))

//            res.Generate()

//            res.Close()
//6
        //parameters.EmbeddedResources.Add(Application.StartupPath & "\resources.resources")
             
            //Referencias
            Par.ReferencedAssemblies.Add("System.dll");
            Par.ReferencedAssemblies.Add("System.Drawing.dll");
            Par.ReferencedAssemblies.Add("System.Linq.dll");
            Par.ReferencedAssemblies.Add("System.Windows.dll");
            Par.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            Par.ReferencedAssemblies.Add("System.IO.Compression.FileSystem.dll");
            Par.ReferencedAssemblies.Add("System.Net.Http.dll");

            if (icon != null)
            {
                Par.CompilerOptions += "/platform:x86 /win32icon:" + icon;
                Par.EmbeddedResources.Add("icon.ico");
            }
            else
                Par.CompilerOptions += "/platform:x86";

            CodeDomProvider Compiler = CodeDomProvider.CreateProvider("CSharp");
            CompilerResults Res = Compiler.CompileAssemblyFromSource(Par, src);            
            foreach(var i in Res.Errors)
            {
                Retorno.Add(i.ToString());
            }
            return Retorno;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IOChain;

namespace Examples_HackerRank
{
    struct TestCase
    {
        public string InputFile;
        public string OutputFile;
    }


    abstract class CExercise : IExerciseModule
    {
        public static string CasesPath = "\\Cases\\";
    
        public virtual string ID { get; protected set; }

        private IEnumerable<string> LoadCases (string path, string file_prefix, string file_ext = ".txt")
        {
            var case_files = Directory.GetFiles(path, file_prefix + "*" + file_ext);
            yield break;
        }

        private string AssemblyPath ()
        {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }

        public abstract void Execute (CIO io);

        public virtual IEnumerable<TestCase> TestCases
        {
            get
            {
                var input_files = Directory.GetFiles
                (
                    AssemblyPath() +
                    CasesPath +
                    this.ID.Replace('.', '\\'),
                    "input" + "*" + ".txt",
                    SearchOption.TopDirectoryOnly
                );

                foreach (string f in input_files)
                {
                    yield return new TestCase
                    {
                        InputFile = f,
                        OutputFile = f.Replace("\\input", "\\output"),
                    };
                }
            } 
        }

        public virtual IEnumerable<string> OutputCases
        {
            get { return LoadCases(AssemblyPath() + CasesPath + this.ID, "output"); } 
        }

        public string ExerciseHint { get { return $"Exercise [{ID}]"; } }
 
        public string CaseHint (TestCase test_case, string msg)
        {
            return $"     {msg} [input = {Path.GetFileNameWithoutExtension(test_case.InputFile)}, output = {Path.GetFileNameWithoutExtension(test_case.OutputFile)}]";
        }
    }
}


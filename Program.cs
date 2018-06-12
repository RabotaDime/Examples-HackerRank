
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IOChain;



namespace Examples_HackerRank
{
    partial class CProgram
    {
        static void Main (string[] args)
        {
            //TextReader tx1 = File.OpenText(@"D:\DESKTOP\Code\Examples_HackerRank\bin\Debug\Cases\Data Structures\Stack\Cache Maximums\input00.txt");
            //Console.SetIn(tx1);
            //TextReader tx2 = Console.In;
            //int d = tx2.Read();
            //return;


            CExercise run_exe;

            var exe_list = new List<CExercise>
            {
                (run_exe = new DataStructures.Stack.CECacheMaximums()),
                new DataStructures.Stack.CEBalancedBrackets(),
            };

            var exercises = new Dictionary<string, CExercise> ();
            foreach (CExercise exe_item in exe_list) 
            {
                exercises.Add(exe_item.ID, exe_item);
            };

            CExercise exe = exercises[run_exe.ID];

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(exe.ExerciseHint.PadRight(Console.BufferWidth));
            Console.CursorTop--;
            Console.ResetColor();

            //int case_num = 1;
            foreach (TestCase test_case in exe.TestCases)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(exe.CaseHint(test_case, $"Case") + " RUN...");

                TextReader       istream = MakeInput  (test_case.InputFile);
                CWriterMediator  ostream = MakeOutput (test_case.OutputFile);

                TextReader std_in  = Console.In;
                TextWriter std_out = Console.Out;

                Console.SetIn  (istream);
                Console.SetOut (ostream);

                Exception fail_result = null;
                try
                {
                    CIO io = new CIO (Console.In);
                    exe.Execute(io);
                }
                catch (Exception e)
                {
                    fail_result = e;
                }

                Console.SetIn  (std_in);
                Console.SetOut (std_out);

                Console.CursorLeft -= 6;
                if ((ostream.Mismatches > 0) || (fail_result != null))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"FAIL   "); // ostream.Mismatches
                    Console.CursorTop--;
                    Console.WriteLine("   - ");
                    Console.ResetColor();

                    if (fail_result != null)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(fail_result.Message);
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"PASS   "); // ostream.Mismatches
                    Console.CursorTop--;
                    Console.WriteLine("   + ");
                    Console.ResetColor();
                }
            }

            Console.WriteLine($"Press any key to quit...");
            Console.ReadKey();
        }
    }



    public interface IExerciseModule
    {
        void Execute (CIO io);
    }



    partial class CProgram
    {
        public static TextReader MakeInput (string file)
        {
            return File.OpenText(file);
        }

        //public static TextReader MakeInput (string[] data)
        //{
        //    var stream = new MemoryStream (1024 * 16);

        //    var writer = new StreamWriter (stream);

        //    foreach (string line in data)
        //    {
        //        writer.WriteLine(line);
        //    }

        //    writer.Flush();

        //    stream.Seek(0, SeekOrigin.Begin);

        //    var reader = new StreamReader (stream);
        //    return reader;
        //}

        public static CWriterMediator MakeOutput (string control_file)
        {
            TextReader control_stream = MakeInput(control_file);
            return new CWriterMediator(Console.Out, control_stream);
        }

        //public static CWriterMediator MakeOutput (string[] control_data)
        //{
        //    return new CWriterMediator(Console.Out, MakeInput(control_data));
        //}

        public class CWriterMediator : TextWriter
        {
            TextWriter _BaseWriter;
            TextReader _ControlReader;

            public CWriterMediator (TextWriter base_writer, TextReader control_reader)
            {
                _BaseWriter = base_writer;
                _ControlReader = control_reader;

                Mismatches = 0;
            }

            public override Encoding Encoding
            {
                get { return _BaseWriter.Encoding; }
            }

            public override void Write (char value)
            {
                //_BaseWriter.Write(value);

                int char_value = _ControlReader.Read();
                if (char_value >= 0)
                {
                    char s = Convert.ToChar(char_value);
                    if (s != value)
                    {
                        this.Mismatches++;
                    }
                }
                //  В конце потока допустим один символ перевода строки
                else if (value == '\n')
                    this.Mismatches += 0;
                else
                    this.Mismatches++;
            }

            public int Mismatches { get; private set; }
        }
    }
}

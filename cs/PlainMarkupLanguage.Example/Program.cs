using System.Diagnostics;
using System.IO;
using static PlainMarkupLanguage.MarkupLanguage;
using static System.Console;

namespace PlainMarkupLanguage.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var reader = new StreamReader("testFile"))
            {
                var text = reader.ReadToEnd();
                var watch = Stopwatch.StartNew();
                var res = Parse(text);
                watch.Stop();
                WriteLine("Finished in " + watch.ElapsedMilliseconds + " ms");
                ReadLine();
            }
        }
    }
}

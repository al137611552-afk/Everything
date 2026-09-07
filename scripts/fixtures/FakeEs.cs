using System;
using System.Text;
using System.Threading;
class FakeEs {
 static void Export(string[] args){int i=Array.IndexOf(args,"-export-txt");if(i<0)throw new Exception("UTF-8 export required");System.IO.File.WriteAllText(args[i+1],@"D:\测试目录\测试文件.txt",new UTF8Encoding(true));Console.OutputEncoding=Encoding.GetEncoding(936);Console.WriteLine("控制台不是UTF8");}
 static int Main(string[] args){Console.OutputEncoding=new UTF8Encoding(false);string query=args[args.Length-1];if(Array.IndexOf(args,"-argv")<0||Array.IndexOf(args,"-txt")<0)return 6;if(query=="<startup>"){string marker=Environment.GetEnvironmentVariable("QUICKPANEL_TEST_MARKER");int count=System.IO.File.Exists(marker)?Int32.Parse(System.IO.File.ReadAllText(marker)):0;System.IO.File.WriteAllText(marker,(count+1).ToString());if(count<2)return 8;Export(args);return 0;}if(query=="<invalid>")return 6;if(query=="<missing>")return 8;if(query=="<slow>"){Thread.Sleep(20000);return 0;}if(query!="<ext:txt 测试>")return 6;Export(args);return 0;}
}

using System;
using System.Text;
using System.Threading;
class FakeEs {
 static int Main(string[] args){Console.OutputEncoding=new UTF8Encoding(false);string query=args[args.Length-1];if(Array.IndexOf(args,"-argv")<0||Array.IndexOf(args,"-txt")<0)return 6;if(query=="<startup>"){string marker=Environment.GetEnvironmentVariable("QUICKPANEL_TEST_MARKER");int count=System.IO.File.Exists(marker)?Int32.Parse(System.IO.File.ReadAllText(marker)):0;System.IO.File.WriteAllText(marker,(count+1).ToString());if(count<2)return 8;Console.WriteLine("D:\\测试目录\\测试文件.txt".Replace("测试目录", "测试目录"));return 0;}if(query=="<invalid>")return 6;if(query=="<missing>")return 8;if(query=="<slow>"){Thread.Sleep(20000);return 0;}if(query!="<ext:txt 测试>")return 6;Console.WriteLine("D:\\测试目录\\测试文件.txt");return 0;}
}

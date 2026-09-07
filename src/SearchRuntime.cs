using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace QuickPanel {
 public static class SearchRuntime {
  public static string Tools {get{return Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"tools","Everything");}}
  public static string Data {get{return Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"data","Everything");}}
  [DllImport("user32.dll",CharSet=CharSet.Unicode)] static extern IntPtr FindWindow(string name,string title);
  static Process owned;
  public static bool DefaultRunning {get{return FindWindow("EVERYTHING_TASKBAR_NOTIFICATION",null)!=IntPtr.Zero;}}
  public static string Ensure(){if(DefaultRunning)return "";string executable=Path.Combine(Tools,"Everything.exe");if(!File.Exists(executable))throw new IOException("搜索组件缺失，请完整解压新版安装包。");
   if(owned==null||owned.HasExited){Directory.CreateDirectory(Data);var ini=Path.Combine(Data,"Everything.ini");if(!File.Exists(ini)){string profile=Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);if(String.IsNullOrEmpty(profile))profile=Environment.GetEnvironmentVariable("USERPROFILE");var folders=new[]{Path.Combine(profile,"Desktop"),Path.Combine(profile,"Documents"),Path.Combine(profile,"Downloads")}.Where(Directory.Exists).Concat(new[]{AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\')}).Distinct().ToArray();string text="[Everything]\r\napp_data=0\r\nrun_as_admin=0\r\nrun_in_background=1\r\nshow_tray_icon=0\r\ncheck_for_updates_on_startup=0\r\nntfs_auto_include_fixed_volumes=0\r\nntfs_auto_include_removable_volumes=0\r\nrefs_auto_include_fixed_volumes=0\r\nfolders="+String.Join(",",folders.Select(f=>"\""+f.Replace("\\","\\\\")+"\""))+"\r\nfolder_monitor_changes="+String.Join(",",folders.Select(f=>"1"))+"\r\n";File.WriteAllText(ini,text,new UTF8Encoding(true));}
    owned=Process.Start(new ProcessStartInfo(executable,"-instance QuickPanelV02 -config "+Services.Quote(ini)+" -db "+Services.Quote(Path.Combine(Data,"Everything.db"))+" -startup"){UseShellExecute=false,CreateNoWindow=true,WindowStyle=ProcessWindowStyle.Hidden,WorkingDirectory=Data});
   }return "-instance QuickPanelV02 ";
  }
  public static void Options(){var prefix=Ensure();if(prefix.Length==0){Services.Open("https://www.voidtools.com/support/everything/folder_indexing/");return;}Process.Start(new ProcessStartInfo(Path.Combine(Tools,"Everything.exe"),"-instance QuickPanelV02 -config "+Services.Quote(Path.Combine(Data,"Everything.ini"))){UseShellExecute=true});}
  public static void Stop(){if(owned==null)return;try{Process.Start(new ProcessStartInfo(Path.Combine(Tools,"Everything.exe"),"-instance QuickPanelV02 -exit"){UseShellExecute=false,CreateNoWindow=true,WindowStyle=ProcessWindowStyle.Hidden});}catch{}owned=null;}
 }
}

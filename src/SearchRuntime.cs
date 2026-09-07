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
  public static string[] SearchRoots(){var roots=new System.Collections.Generic.List<string>();foreach(var drive in DriveInfo.GetDrives()){try{if(drive.IsReady&&(drive.DriveType==DriveType.Fixed||drive.DriveType==DriveType.Removable))roots.Add(drive.RootDirectory.FullName);}catch(IOException){}catch(UnauthorizedAccessException){}}return roots.Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(p=>p).ToArray();}
  public static string ScopeDescription {get{return DefaultRunning?"已连接标准 Everything · 沿用其索引设置":"全局搜索 · "+String.Join("  ",SearchRoots());}}
  public static string BuildConfig(string[] roots){return "[Everything]\r\napp_data=0\r\nrun_as_admin=0\r\nrun_in_background=1\r\nshow_tray_icon=0\r\ncheck_for_updates_on_startup=0\r\nntfs_auto_include_fixed_volumes=0\r\nntfs_auto_include_removable_volumes=0\r\nrefs_auto_include_fixed_volumes=0\r\nexclude_hidden_files_and_folders=0\r\nexclude_system_files_and_folders=0\r\nfolders="+String.Join(",",roots.Select(f=>"\""+f.Replace("\\","\\\\")+"\""))+"\r\nfolder_monitor_changes="+String.Join(",",roots.Select(f=>"1"))+"\r\nfolder_rescan_if_full_list="+String.Join(",",roots.Select(f=>"1"))+"\r\n";}
  public static bool DefaultRunning {get{return FindWindow("EVERYTHING_TASKBAR_NOTIFICATION",null)!=IntPtr.Zero;}}
  public static string Ensure(){if(DefaultRunning)return "";string executable=Path.Combine(Tools,"Everything.exe");if(!File.Exists(executable))throw new IOException("搜索组件缺失，请完整解压新版安装包。");
   if(owned==null||owned.HasExited){Directory.CreateDirectory(Data);var ini=Path.Combine(Data,"Everything.ini");var roots=SearchRoots();if(roots.Length==0)throw new IOException("没有可访问的本地磁盘。");var marker=Path.Combine(Data,"scope.txt");var signature=String.Join("|",roots);if(!File.Exists(ini)||!File.Exists(marker)||File.ReadAllText(marker)!=signature){File.WriteAllText(ini,BuildConfig(roots),new UTF8Encoding(true));File.WriteAllText(marker,signature,Encoding.UTF8);}
    owned=Process.Start(new ProcessStartInfo(executable,"-instance QuickPanelV03 -config "+Services.Quote(ini)+" -db "+Services.Quote(Path.Combine(Data,"Everything.db"))+" -startup"){UseShellExecute=false,CreateNoWindow=true,WindowStyle=ProcessWindowStyle.Hidden,WorkingDirectory=Data});
   }return "-instance QuickPanelV03 ";
  }
  public static void Options(){var prefix=Ensure();if(prefix.Length==0){Services.Open(Path.Combine(Tools,"Everything.exe"));return;}Process.Start(new ProcessStartInfo(Path.Combine(Tools,"Everything.exe"),"-instance QuickPanelV03 -config "+Services.Quote(Path.Combine(Data,"Everything.ini"))){UseShellExecute=true});}
  public static void Stop(){if(owned==null)return;try{Process.Start(new ProcessStartInfo(Path.Combine(Tools,"Everything.exe"),"-instance QuickPanelV03 -exit"){UseShellExecute=false,CreateNoWindow=true,WindowStyle=ProcessWindowStyle.Hidden});}catch{}owned=null;}
 }
}

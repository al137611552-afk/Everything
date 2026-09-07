using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace QuickPanel {
 public static class InputLayoutTests {
  static T Find<T>(DependencyObject root) where T:DependencyObject {
   if(root is T)return (T)root;
   for(int i=0;i<VisualTreeHelper.GetChildrenCount(root);i++){var found=Find<T>(VisualTreeHelper.GetChild(root,i));if(found!=null)return found;}
   return null;
  }
  public static int Run(){
   var log=new StringBuilder();int failures=0;
   var app=new Application();Window window;
   using(var file=File.OpenRead(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Main.xaml")))window=(Window)XamlReader.Load(file);
   window.Show();var search=(TextBox)window.FindName("Search");
   foreach(double scale in new[]{1.0,1.25,1.5,2.0})foreach(string sample in new[]{"power","记事本 计算器 翻译","Agjpqy 中文",new string('W',100)+"中文gy"}){
    search.LayoutTransform=new ScaleTransform(scale,scale);search.Text=sample;search.CaretIndex=sample.Length;window.UpdateLayout();search.ScrollToEnd();window.UpdateLayout();
    var host=(ScrollViewer)search.Template.FindName("PART_ContentHost",search);var viewport=Find<ScrollContentPresenter>(host);
    var rect=search.GetRectFromCharacterIndex(sample.Length-1);var p=search.TransformToDescendant(viewport).Transform(new Point(rect.X,rect.Y));
    bool pass=!rect.IsEmpty&&p.Y>=-0.5&&p.Y+rect.Height<=viewport.ActualHeight+0.5;
    log.AppendLine((pass?"PASS":"FAIL")+" scale="+scale+" length="+sample.Length+" glyphY="+p.Y+" glyphHeight="+rect.Height+" viewportHeight="+viewport.ActualHeight);if(!pass)failures++;
    if(sample=="power"){
     var bitmap=new RenderTargetBitmap((int)Math.Ceiling(window.ActualWidth*scale),(int)Math.Ceiling(window.ActualHeight*scale),96*scale,96*scale,PixelFormats.Pbgra32);bitmap.Render(window);var png=new PngBitmapEncoder();png.Frames.Add(BitmapFrame.Create(bitmap));using(var output=File.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"input-"+scale+".png")))png.Save(output);
    }
   }
   window.Close();File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"input-layout-results.txt"),log.ToString());return failures==0?0:1;
  }
 }
}

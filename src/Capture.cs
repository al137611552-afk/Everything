using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Forms=System.Windows.Forms;
using Drawing=System.Drawing;

namespace QuickPanel {
 public class CaptureWindow:Window {
  Canvas canvas; Rectangle selection; Point start; Rect region; bool dragging,selected; BitmapSource desktop; StackPanel toolbar; double ratioX,ratioY;
  public CaptureWindow(){var screen=Forms.Screen.FromPoint(Forms.Cursor.Position);var bounds=screen.Bounds;using(var bitmap=new Drawing.Bitmap(bounds.Width,bounds.Height)){using(var g=Drawing.Graphics.FromImage(bitmap))g.CopyFromScreen(bounds.Location,Drawing.Point.Empty,bounds.Size);using(var stream=new MemoryStream()){bitmap.Save(stream,Drawing.Imaging.ImageFormat.Png);stream.Position=0;var image=new BitmapImage();image.BeginInit();image.CacheOption=BitmapCacheOption.OnLoad;image.StreamSource=stream;image.EndInit();image.Freeze();desktop=image;}}
   WindowStyle=WindowStyle.None;ResizeMode=ResizeMode.NoResize;Topmost=true;ShowInTaskbar=false;Background=Brushes.Black;Cursor=Cursors.Cross;Title="框选截图 · Esc 取消";
   canvas=new Canvas{Background=new ImageBrush(desktop){Stretch=Stretch.Fill}};Content=canvas;selection=new Rectangle{Stroke=new SolidColorBrush(Color.FromRgb(85,214,190)),StrokeThickness=2,Fill=new SolidColorBrush(Color.FromArgb(30,85,214,190)),Visibility=Visibility.Collapsed};canvas.Children.Add(selection);
   var tip=new TextBlock{Text="拖动框选区域    Enter 复制    Ctrl+S 保存    Esc 取消",Foreground=Brushes.White,Background=new SolidColorBrush(Color.FromArgb(230,24,26,30)),Padding=new Thickness(16),IsHitTestVisible=false};Canvas.SetLeft(tip,20);Canvas.SetTop(tip,20);canvas.Children.Add(tip);
   toolbar=new StackPanel{Orientation=Orientation.Horizontal,Background=new SolidColorBrush(Color.FromRgb(24,26,30)),Visibility=Visibility.Collapsed};Add("复制",()=>Finish(false));Add("保存",()=>Finish(true));Add("取消",Close);canvas.Children.Add(toolbar);
   SourceInitialized+=(s,e)=>{var source=PresentationSource.FromVisual(this);var matrix=source.CompositionTarget.TransformFromDevice;var p=matrix.Transform(new Point(bounds.Left,bounds.Top));Left=p.X;Top=p.Y;Width=bounds.Width*matrix.M11;Height=bounds.Height*matrix.M22;};
   Loaded+=(s,e)=>{ratioX=desktop.PixelWidth/ActualWidth;ratioY=desktop.PixelHeight/ActualHeight;Activate();Focus();};
   canvas.MouseLeftButtonDown+=(s,e)=>{if(toolbar.IsMouseOver)return;start=e.GetPosition(canvas);dragging=true;selected=false;toolbar.Visibility=Visibility.Collapsed;selection.Visibility=Visibility.Visible;region=new Rect(start,start);Draw();canvas.CaptureMouse();};
   canvas.MouseMove+=(s,e)=>{if(!dragging)return;var point=e.GetPosition(canvas);point.X=Math.Max(0,Math.Min(ActualWidth,point.X));point.Y=Math.Max(0,Math.Min(ActualHeight,point.Y));region=new Rect(start,point);Draw();};
   canvas.MouseLeftButtonUp+=(s,e)=>{if(!dragging)return;dragging=false;canvas.ReleaseMouseCapture();selected=region.Width>2&&region.Height>2;if(selected){toolbar.Visibility=Visibility.Visible;Canvas.SetLeft(toolbar,Math.Max(0,Math.Min(region.Left,ActualWidth-250)));Canvas.SetTop(toolbar,Math.Max(0,Math.Min(region.Bottom+8,ActualHeight-55)));}};
   PreviewKeyDown+=(s,e)=>{if(e.Key==Key.Escape){Close();e.Handled=true;}else if(e.Key==Key.Enter){Finish(false);e.Handled=true;}else if(e.Key==Key.S&&(Keyboard.Modifiers&ModifierKeys.Control)!=0){Finish(true);e.Handled=true;}};
  }
  void Add(string label,Action action){var b=new Button{Content=label,Padding=new Thickness(15,9,15,9),Margin=new Thickness(4)};b.Click+=(s,e)=>action();toolbar.Children.Add(b);}
  void Draw(){Canvas.SetLeft(selection,region.Left);Canvas.SetTop(selection,region.Top);selection.Width=region.Width;selection.Height=region.Height;}
  void Finish(bool save){if(!selected)return;try{int x=Math.Max(0,(int)(region.X*ratioX)),y=Math.Max(0,(int)(region.Y*ratioY));int w=Math.Min(desktop.PixelWidth-x,Math.Max(1,(int)(region.Width*ratioX))),h=Math.Min(desktop.PixelHeight-y,Math.Max(1,(int)(region.Height*ratioY)));var crop=new CroppedBitmap(desktop,new Int32Rect(x,y,w,h));if(save){var dialog=new Microsoft.Win32.SaveFileDialog{Filter="PNG 图片|*.png",FileName="截图-"+DateTime.Now.ToString("yyyyMMdd-HHmmss")+".png"};Topmost=false;if(dialog.ShowDialog(this)!=true){Topmost=true;return;}var encoder=new PngBitmapEncoder();encoder.Frames.Add(BitmapFrame.Create(crop));using(var f=File.Create(dialog.FileName))encoder.Save(f);}else Clipboard.SetImage(crop);Close();}catch(Exception e){Topmost=false;MessageBox.Show("截图未保存："+e.Message);Topmost=true;}}
 }
}

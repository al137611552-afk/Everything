using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace QuickPanel {
 public static class DialogChrome {
  public static Window Create(Window owner,string title,double width,double height){
   var window=new Window{Title=title,Width=width,Height=height,Owner=owner,WindowStartupLocation=WindowStartupLocation.CenterOwner,
    WindowStyle=WindowStyle.None,ResizeMode=ResizeMode.NoResize,AllowsTransparency=true,Background=Brushes.Transparent,
    Foreground=owner.Foreground,FontFamily=owner.FontFamily,FontSize=14,ShowInTaskbar=false,MaxHeight=Math.Max(360,SystemParameters.WorkArea.Height-24)};
   window.Resources=owner.Resources;
   var frame=new Border{Background=new SolidColorBrush(Color.FromRgb(24,26,30)),BorderBrush=new SolidColorBrush(Color.FromRgb(65,71,81)),
    BorderThickness=new Thickness(1),CornerRadius=new CornerRadius(12),Margin=new Thickness(8),
    Effect=new DropShadowEffect{Color=Colors.Black,BlurRadius=12,ShadowDepth=2,Opacity=0.3}};
   var layout=new DockPanel();frame.Child=layout;window.Content=frame;
   var caption=new Grid{Height=46,Margin=new Thickness(18,0,10,0)};
   caption.ColumnDefinitions.Add(new ColumnDefinition());caption.ColumnDefinitions.Add(new ColumnDefinition{Width=GridLength.Auto});
   var heading=new TextBlock{Text=title,FontSize=13,Foreground=new SolidColorBrush(Color.FromRgb(169,175,186)),VerticalAlignment=VerticalAlignment.Center};caption.Children.Add(heading);
   var close=new Button{Content="×",Width=30,Height=28,Padding=new Thickness(0),Background=Brushes.Transparent,BorderBrush=Brushes.Transparent,ToolTip="关闭",IsCancel=true};Grid.SetColumn(close,1);caption.Children.Add(close);close.Click+=(s,e)=>window.Close();
   caption.MouseLeftButtonDown+=(s,e)=>{if(!close.IsMouseOver&&e.LeftButton==MouseButtonState.Pressed)window.DragMove();};
   var divider=new Border{BorderBrush=new SolidColorBrush(Color.FromRgb(48,52,59)),BorderThickness=new Thickness(0,0,0,1),Child=caption};DockPanel.SetDock(divider,Dock.Top);layout.Children.Add(divider);
   var body=new ContentControl{Content=new StackPanel{Margin=new Thickness(22)}};layout.Children.Add(body);window.Tag=body;
   window.PreviewKeyDown+=(s,e)=>{if(e.Key==Key.Escape){window.Close();e.Handled=true;}};
   return window;
  }
 }
}

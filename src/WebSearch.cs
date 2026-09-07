using System;
namespace QuickPanel {
 public static class WebSearch {
  public static Entry Match(string input){
   string text=(input??"").Trim();int end=0;while(end<text.Length&&!Char.IsWhiteSpace(text[end]))end++;
   string keyword=text.Substring(0,end).ToLowerInvariant(),name,url;
   switch(keyword){case "bd":name="百度";url="https://www.baidu.com/s?wd=";break;case "zh":name="知乎";url="https://www.zhihu.com/search?type=content&q=";break;case "gg":name="Google";url="https://www.google.com/search?q=";break;default:return null;}
   string query=text.Substring(end).Trim();return new Entry{Name=query.Length==0?name+"搜索 · 输入搜索内容":name+"搜索："+query,Detail=query.Length==0?keyword+" 搜索内容":new Uri(url).Host+" · Enter 在默认浏览器搜索",Kind="网页",Glyph="\uE721",Target=query.Length==0?null:url+Uri.EscapeDataString(query)};
  }
 }
}

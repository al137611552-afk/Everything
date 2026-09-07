# QuickPanel / 快捷面板

面向 Windows 的本地快捷工具面板，免费使用。当前为 0.2 内测版本。仓库为公开源码存放位置；项目自身尚未选定开源许可证，不代表授予任意再分发许可。第三方组件遵循各自许可证。

## 功能

- 开始菜单应用搜索，常用中文首字母匹配。
- 记事本、计算器、画图、任务管理器和命令提示符直接入口，支持中英文与拼音别名。
- 内置官方 Everything 1.4.1.1032 和 ES 1.1.0.37 的打包流程，无需手填 CLI 路径。
- 优先连接已运行的标准 Everything；否则使用独立便携实例，默认索引桌面、文档、下载和面板目录，可在设置中管理目录。
- 区域截图、翻译服务配置、PowerShell 命令与收藏。
- C# / WPF 深色界面，透明圆角窗口、深色滚动条、分类下划线和系统应用图标。视觉方向来自图像设计工具，没有使用 JavaScript 绘制 UI。

## 构建

在 Windows x64、具备系统 .NET Framework C# 编译器和 WPF 运行库的环境运行：

```powershell
.\scripts\build.ps1
.\scripts\fetch-search-tools.ps1
```

输出为 `outputs/QuickPanel-v0.2`。完整分发该目录的程序、Main.xaml、tools 和第三方许可证；**不要分发 data、用户配置或测试索引**。目前没有签名、安装器或自动更新。

## 使用

运行 QuickPanel.exe，Alt+Space 唤起。输入应用名称打开；`f 关键词` 搜索文件；`tr 文本` 后 Ctrl+Enter 翻译；`> 命令` 进入命令模式。Tab 展开文件操作，Esc 返回。关闭按钮隐藏至托盘，右键托盘可退出。

翻译需在设置中填入完整 HTTPS Chat Completions 兼容接口、模型和自己的 API Key。只主动提交文字，不后台上传剪贴板。密钥使用当前用户 DPAPI 保存。

## 验证与限制

0.2 已通过系统应用发现、命令参数、中文字符、CLI 取消/失败处理等检查，以及真实 Everything 自动启动、索引与搜索测试。窗口在实际桌面检查中已确认移除顶部白边。

尚未完成：全套交互端到端测试、多屏截图兼容、翻译服务联网与正常桌面用户 DPAPI 验证、自定义全局快捷键、完整全拼搜索、所有商店应用发现。设置页和图标已继续调整，最终版本的完整桌面回归因用户停止桌面自动化而未完成。当前不声称逐像素还原全部设计稿。

便携文件夹索引不是全盘 NTFS 索引；需要全盘即时索引时，可使用已经配置好的标准 Everything。数据保存在面板目录下的 data/Everything，翻译和收藏配置保存在当前用户 LocalAppData/QuickPanelPreview。

## 第三方

- [Everything / voidtools](https://www.voidtools.com/)：分发时保留 Everything-LICENSE.txt。
- [ES / voidtools](https://github.com/voidtools/ES)：分发时保留 ES-LICENSE.txt。

源码提交不包含第三方二进制、用户数据、密钥或个人桌面截图。

PDFBinder v2.0.0
==============

## 🇺🇸 **English**

A simple tool to merge several PDF documents into one or split a PDF document.
Enhanced from the base project **PDFBinder v1.2** (see Original GitHub repository), it adds multiple new features such as multi-selection, batch move up/down, sorting, custom page ranges, and more.

## 🇨🇳 **简体中文**

PDFBinder2 是一个简单而易于使用的 PDF 文档合并／拆分工具。
它以 **PDFBinder v1.2** 为基础（见原项目 GitHub 仓库），新增列表多选、排序、批量移动、合并顺序调整、设定页码范围等功能，帮助用户更高效地处理 PDF 文件。

## 🇹🇼 **繁體中文**

PDFBinder2 是一款簡單易用的 PDF 文件合併／拆分工具。
它以 **PDFBinder v1.2** 為基礎（請參見原始 GitHub 專案），加入多選功能、批次上下移動、排序、可自訂合併頁碼範圍等多項改進，協助使用者更快速、高效率地處理 PDF 文件。

## 🇩🇪 **Deutsch**

PDFBinder2 ist ein einfaches und benutzerfreundliches Werkzeug zum Zusammenführen mehrerer PDF-Dateien oder zum Aufteilen eines PDF-Dokuments.
Es basiert auf **PDFBinder v1.2** (siehe ursprüngliches GitHub-Repository) und erweitert das Programm um Funktionen wie Mehrfachauswahl, Stapel-Verschieben nach oben/unten, Sortierung sowie benutzerdefinierte Seitenbereiche, um die Arbeit mit PDFs deutlich zu erleichtern.

## 🇯🇵 **日本語**

PDFBinder2 は、複数の PDF を 1 つに結合したり、PDF を分割したりできるシンプルで使いやすいツールです。
本ツールは **PDFBinder v1.2**（元の GitHub リポジトリ参照）をベースに、複数選択、ファイルの一括移動、並べ替え、ページ範囲の指定など、利便性を高める機能が追加されています。

## 🇫🇷 **Français**

PDFBinder2 est un outil simple et convivial permettant de fusionner plusieurs documents PDF en un seul ou de diviser un PDF en plusieurs parties.
Basé sur **PDFBinder v1.2** (voir le dépôt GitHub original), il ajoute de nouvelles fonctionnalités telles que la sélection multiple, le déplacement par lots, le tri et la définition d’une plage de pages personnalisée, afin de faciliter et d’accélérer le traitement des fichiers PDF.


## 🌟 What's New in PDFBinder v2.0 （新增內容）
PDFBinder v2.0 introduces several improvements and refinements over the legacy v1.2 codebase:

### ✔ Modernized Features
- Added **multiple document selection** for more efficient workflows  
- Added **batch move up/down** for reordering items quickly  
- Added **automatic sorting options**  
- Added **custom page-range merging** for advanced users  
- Improved **drag-and-drop handling**  
- New, clearer UI layout and icons

### ✔ Codebase Enhancements
- Updated C# project structure for modern Visual Studio versions  
- Removed deprecated or redundant components  
- Cleaned and reorganized source directories  
- Improved overall stability and error-handling behavior  

### ✔ Installer & Packaging
- Updated Windows installer for smoother installation  
- Improved file-association handling for context-menu operations  

### ✔ Internationalization
- Added improved bilingual README (English + Simplified Chinese)  
- Enhanced documentation clarity and formatting  

---

![screenshot](https://github.com/kancheng/PDFBinder2/blob/main/docs/PDFBinder_v2_icon.png?raw=true)

![screenshot](https://github.com/kancheng/PDFBinder2/blob/main/docs/main.png?raw=true)

Contents:

  1. Installation and usage
  2. Contributing to the project
  3. Licensing

Installation and usage
----------------------

PDFBinder can be installed on Microsoft Windows systems using a pretty
installer, which can be downloaded from the GitHub repository.

In order to use PDFBinder on other platforms - or if the installer seems
like a bad choice for other reasons - PDFBinder can be built installed from
source. Grab the latest source package the GitHub repository. Use
whatever C# compiler you have available to build the project, or use the
provided project file for Microsoft Visual Studio 2010 - 2015.

Hopefully, the user interface of PDFBinder is pretty self-explanatory. You
can add source PDF documents by using the "Add source documents..." button,
or by dragging the files in from a file browser. Documents can be moved up,
moved down or remoevd by pressing the respective buttons in the toolbar.
Press the "Bind!" button when your list of documents seems fine.

If PDFBinder was installed using the pre-built installer, an extra option
should have been automatically added to your context menu (the menu that on
Windows displays when right clicking a file) for all PDF files. Select any
number of PDFs in a file browser and choose the "Add to PDFBinder..." option
to bring up the PDFBinder interface with those files already in the list.

Contributing to the project
---------------------------

Any kind of contibutions to the project are very welcome. Issues should be
reported directly in the issue tracker on the GitHub repository, and
reporters are very welcome to attach patches to their reports.

If you wish to encourage further development of PDFBinder by donating to the
project, please get in contact with the project owner (e-mail:kacarton@msn.com), 
and we will find some way for you to transfer a reasonable amount of money
or beer to the project.

Licensing
---------

PDFBinder is released under the terms of the GNU General Public License.
Please see LICENSE.txt for the complete legal text.

All of the PDF magic is done using the iTextSharp 4.1.6 library, released
under both MPL and LGPL. Please refer to the iTextSharp project site.


[GitHub repository]:          https://github.com/kacarton/pdfbinder2
[Original gitHub repository]: https://github.com/schourode/pdfbinder
[iTextSharp project site]:    http://itextsharp.sourceforge.net/

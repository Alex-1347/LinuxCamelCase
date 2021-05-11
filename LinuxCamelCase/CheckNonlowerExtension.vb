'Long fragments of this code generated automatically by string of bottom of this file and allow check and individual procesing each type of files existing in my site
Partial Module Program

    Dim HtmlCount As Integer, PdfCount As Integer, JpgCount As Integer, PngCount As Integer, GifCount As Integer, TxtCount As Integer, Mp3Count As Integer, XmlCount As Integer,
    MsiCount As Integer, ZipCount As Integer, ExeCount As Integer, DllCount As Integer, CssCount As Integer, UrlCount As Integer, WsdlCount As Integer, IcoCount As Integer,
    BmpCount As Integer, ConfigCount As Integer, JsCount As Integer, SqlCount As Integer, SwfCount As Integer, IsoCount As Integer, ScssCount As Integer, WoffCount As Integer,
    SwzCount As Integer, LogCount As Integer, IniCount As Integer, EpubCount As Integer, WimCount As Integer, XlsCount As Integer, AirCount As Integer, _7zCount As Integer,
    MhtCount As Integer, GitattributesCount As Integer, GitignoreCount As Integer, Ps1Count As Integer, MdCount As Integer, CmdCount As Integer, OdsCount As Integer,
    GzCount As Integer, BakCount As Integer, AspCount As Integer, ConfCount As Integer, SampleCount As Integer, AdsiCount As Integer, RpmnewCount As Integer,
    OrigCount As Integer, TemplateCount As Integer, AelCount As Integer, LuaCount As Integer, IncCount As Integer, SwcCount As Integer, AscCount As Integer, ChmCount As Integer,
    DbCount As Integer, RW2Count As Integer, AviCount As Integer, PsdCount As Integer, IonCount As Integer, FlvCount As Integer, VbCount As Integer, MyappCount As Integer,
    ResxCount As Integer, SettingsCount As Integer, MxmlCount As Integer, DylibCount As Integer, JnilibCount As Integer, SoCount As Integer, WavCount As Integer, JarCount As Integer,
    PkgCount As Integer, CacheCount As Integer, OggCount As Integer, VbprojCount As Integer, UserCount As Integer, PdbCount As Integer, PubxmlCount As Integer, ResourcesCount As Integer,
    SvgCount As Integer, HhcCount As Integer, HhkCount As Integer, TorrentCount As Integer, SlnCount As Integer, SuoCount As Integer, HtmCount As Integer, JpegCount As Integer, TextCount As Integer
    Dim NewExt As New List(Of String)
    Dim AllExt As New List(Of String)

    Sub CheckNonlowerExtension()
        ReadFileList(Root)
        ReadDirList(Root)

        Console.WriteLine("All extension:" & String.Join(",", AllExt.OrderBy(Function(X) X)))
        Console.WriteLine()
        Console.WriteLine("Fount new extension:" & String.Join(",", NewExt.OrderBy(Function(X) X)))
        Console.WriteLine()
        Console.WriteLine($"Found files with non lower extension {HtmlCount} html-files, {HtmCount} htm-files, {PdfCount} Pdf-files, {JpgCount} Jpg-files, {JpegCount} Jpeg-files, {PngCount} Png-files, {GifCount} Gif-files, " &
        $"{TxtCount} Txt-files, {TextCount} Text-files, {Mp3Count} Mp3-files,  {XmlCount} Xml-files, {MsiCount} Msi-files, {ZipCount} zip-files," &
        $"{ExeCount} Exe-files, {DllCount} Dll-files, {CssCount} Css-files, {UrlCount} Url-files, {WsdlCount} Wsdl-files, {IcoCount} Ico-files, {BmpCount} Bmp-Files, {ConfigCount} Config-files, {JsCount} JS-files, " &
        $"{SqlCount} Sql-files, {SwfCount} Swf-files, {IsoCount} Iso-files, {ScssCount} Scss-files, {WoffCount} Woff-files, {SwzCount} Swz-files, {LogCount} Log-files, {IniCount} Ini-files, " &
        $"{EpubCount} Epub-files, {WimCount} Wim-files, {XlsCount} Xls-files, {AirCount} Air-files, {_7zCount} 7z-files, {MhtCount} Mht-files, {GitattributesCount} Gitattributes-files,{GitignoreCount} Gitignore-files, " &
        $"{Ps1Count} Ps1-files, {MdCount} Md-files, {CmdCount} Cmd-files, {OdsCount} Ods#-files, {OdsCount} Ods-files, {GzCount} Gz-files, {BakCount} Bak-files, {AspCount} Asp-files, {ConfCount} Conf-files, " &
        $"{SampleCount} Sample-files, {AdsiCount} Adsi-files, {RpmnewCount} Rpmnew-files, {OrigCount} Orig-files, {TemplateCount} Template-files, {AelCount} Ael-files, {LuaCount} Lua-files, " &
        $"{IncCount} Inc-files, {SwcCount} Swc-files, {AscCount} Asc-files, {ChmCount} Chm-files, {DbCount} Db-files, {RW2Count} RW2-files, {AviCount} Avi-files, {PsdCount} Psd-files, " &
        $"{IonCount} Ion-files, {FlvCount} Flv-files, {VbCount} Vb-files, {MyappCount} Myapp-files, {ResxCount} Resx-files, {SettingsCount} Settings-files, {MxmlCount} Mxml-files, {DylibCount} Dylib-files," &
        $"{JnilibCount} Jnilib-files, {SoCount} So-files, {WavCount} Wav-files, {JarCount} Jar-files, {PkgCount} Pkg-files, {CacheCount} Cache-files, {OggCount} Ogg-files, {VbprojCount} Vbproj-files, " &
        $"{UserCount} User-files, {PdbCount} Pdb-files, {PubxmlCount} Pubxml-files, {ResourcesCount} Resources-files, {SvgCount} Svg-files, {HhcCount} Hhc-files, {HhkCount} Hhk-files, {TorrentCount} Torrent-files, " &
        $"{SlnCount} Sln-files, {SuoCount} Suo-files)")
    End Sub

    Sub ReadDirList(Dir As String)
        If Not Dir.Contains("AspNet-DocAndSamples") And Not Dir.Contains("Cisco_PIX_html_help") And Not Dir.Contains("VS2010_NET4_TrainingKit") And Not Dir.Contains(".svn") And Not Dir.Contains(".git") Then
            For Each OneDir As String In IO.Directory.GetDirectories(Dir)
                ReadFileList(OneDir)
                ReadDirList(OneDir)
            Next
        End If
    End Sub

    Sub ReadFileList(Dir As String)
        For Each OneFile As String In IO.Directory.GetFiles(Dir)
            Dim CurExt As String = IO.Path.GetExtension(OneFile)
            For Each One As String In AllExt.ToList
                If CurExt.ToLower = One.ToLower Then
                    GoTo ExistAllExt
                End If
            Next
            AllExt.Add(CurExt)
ExistAllExt:
            If CurExt.ToLower = ".htm" And CurExt <> ".htm" Then
                HtmCount += 1
            ElseIf CurExt.ToLower = ".html" And CurExt <> ".html" Then
                HtmlCount += 1
            ElseIf CurExt.ToLower = ".pdf" And CurExt <> ".pdf" Then
                PdfCount += 1
            ElseIf CurExt.ToLower = ".jpg" And CurExt <> ".jpg" Then
                '9224 Jpg-files
                System.IO.File.Move(OneFile, OneFile.Replace(CurExt, ".jpg"))
                JpgCount += 1
            ElseIf CurExt.ToLower = ".jpeg" And CurExt <> ".jpeg" Then
                JpegCount += 1
            ElseIf CurExt.ToLower = ".png" And CurExt <> ".png" Then
                PngCount += 1
            ElseIf CurExt.ToLower = ".gif" And CurExt <> ".gif" Then
                '10 Gif-files
                System.IO.File.Move(OneFile, OneFile.Replace(CurExt, ".gif"))
                GifCount += 1
            ElseIf CurExt.ToLower = ".bmp" And CurExt <> ".bmp" Then
                BmpCount += 1
            ElseIf CurExt.ToLower = ".mp3" And CurExt <> ".mp3" Then
                System.IO.File.Move(OneFile, OneFile.Replace(CurExt, ".gif"))
                'Console.WriteLine(OneFile)
                Mp3Count += 1
            ElseIf CurExt.ToLower = ".xml" And CurExt <> ".xml" Then
                XmlCount += 1
            ElseIf CurExt.ToLower = ".txt" And CurExt <> ".txt" Then
                TxtCount += 1
            ElseIf CurExt.ToLower = ".text" And CurExt <> ".text" Then
                TextCount += 1
            ElseIf CurExt.ToLower = ".exe" And CurExt <> ".exe" Then
                ExeCount += 1
            ElseIf CurExt.ToLower = ".dll" And CurExt <> ".dll" Then
                DllCount += 1
            ElseIf CurExt.ToLower = ".msi" And CurExt <> ".msi" Then
                MsiCount += 1
            ElseIf CurExt.ToLower = ".zip" And CurExt <> ".zip" Then
                ZipCount += 1
            ElseIf CurExt.ToLower = ".css" And CurExt <> ".css" Then
                CssCount += 1
            ElseIf CurExt.ToLower = ".url" And CurExt <> ".url" Then
                UrlCount += 1
            ElseIf CurExt.ToLower = ".wsdl" And CurExt <> ".wsdl" Then
                WsdlCount += 1
            ElseIf CurExt.ToLower = ".ico" And CurExt <> ".ico" Then
                IcoCount += 1
            ElseIf CurExt.ToLower = ".config" And CurExt <> ".config" Then
                ConfigCount += 1
            ElseIf CurExt.ToLower = ".js" And CurExt <> ".js" Then
                JsCount += 1
            ElseIf CurExt.ToLower = ".sql" And CurExt <> ".sql" Then
                SqlCount += 1
            ElseIf CurExt.ToLower = ".swf" And CurExt <> ".swf" Then
                SwfCount += 1
            ElseIf CurExt.ToLower = ".iso" And CurExt <> ".iso" Then
                IsoCount += 1
            ElseIf CurExt.ToLower = ".scss" And CurExt <> ".scss" Then
                ScssCount += 1
            ElseIf CurExt.ToLower = ".woff" And CurExt <> ".woff" Then
                WoffCount += 1
            ElseIf CurExt.ToLower = ".swz" And CurExt <> ".swz" Then
                SwzCount += 1
            ElseIf CurExt.ToLower = ".log" And CurExt <> ".log" Then
                LogCount += 1
            ElseIf CurExt.ToLower = ".ini" And CurExt <> ".ini" Then
                IniCount += 1
            ElseIf CurExt.ToLower = ".epub" And CurExt <> ".epub" Then
                EpubCount += 1
            ElseIf CurExt.ToLower = ".wim" And CurExt <> ".wim" Then
                WimCount += 1
            ElseIf CurExt.ToLower = ".xls" And CurExt <> ".xls" Then
                XlsCount += 1
            ElseIf CurExt.ToLower = ".air" And CurExt <> ".air" Then
                AirCount += 1
            ElseIf CurExt.ToLower = ".7z" And CurExt <> ".7z" Then
                _7zCount += 1
            ElseIf CurExt.ToLower = ".mht" And CurExt <> ".mht" Then
                MhtCount += 1
            ElseIf CurExt.ToLower = ".gitattributes" And CurExt <> ".gitattributes" Then
                GitattributesCount += 1
            ElseIf CurExt.ToLower = ".gitignore" And CurExt <> ".gitignore" Then
                GitignoreCount += 1
            ElseIf CurExt.ToLower = ".ps1" And CurExt <> ".ps1" Then
                Ps1Count += 1
            ElseIf CurExt.ToLower = ".md" And CurExt <> ".md" Then
                MdCount += 1
            ElseIf CurExt.ToLower = ".cmd" And CurExt <> ".cmd" Then
                CmdCount += 1
            ElseIf CurExt.ToLower = ".ods" And CurExt <> ".ods" Then
                OdsCount += 1
            ElseIf CurExt.ToLower = ".gz" And CurExt <> ".gz" Then
                GzCount += 1
            ElseIf CurExt.ToLower = ".bak" And CurExt <> ".bak" Then
                BakCount += 1
            ElseIf CurExt.ToLower = ".asp" And CurExt <> ".asp" Then
                AspCount += 1
            ElseIf CurExt.ToLower = ".conf" And CurExt <> ".conf" Then
                ConfCount += 1
            ElseIf CurExt.ToLower = ".sample" And CurExt <> ".sample" Then
                SampleCount += 1
            ElseIf CurExt.ToLower = ".adsi" And CurExt <> ".adsi" Then
                AdsiCount += 1
            ElseIf CurExt.ToLower = ".rpmnew" And CurExt <> ".rpmnew" Then
                RpmnewCount += 1
            ElseIf CurExt.ToLower = ".orig" And CurExt <> ".orig" Then
                OrigCount += 1
            ElseIf CurExt.ToLower = ".template" And CurExt <> ".template" Then
                TemplateCount += 1
            ElseIf CurExt.ToLower = ".ael" And CurExt <> ".ael" Then
                AelCount += 1
            ElseIf CurExt.ToLower = ".lua" And CurExt <> ".lua" Then
                LuaCount += 1
            ElseIf CurExt.ToLower = ".inc" And CurExt <> ".inc" Then
                IncCount += 1
            ElseIf CurExt.ToLower = ".swc" And CurExt <> ".swc" Then
                SwcCount += 1
            ElseIf CurExt.ToLower = ".asc" And CurExt <> ".asc" Then
                AscCount += 1
            ElseIf CurExt.ToLower = ".chm" And CurExt <> ".chm" Then
                ChmCount += 1
            ElseIf CurExt.ToLower = ".db" And CurExt <> ".db" Then
                DbCount += 1
            ElseIf CurExt.ToLower = ".rw2" And CurExt <> ".rw2" Then
                System.IO.File.Move(OneFile, OneFile.Replace(CurExt, ".rw2"))
                'Console.WriteLine(OneFile)
                RW2Count += 1
            ElseIf CurExt.ToLower = ".avi" And CurExt <> ".avi" Then
                System.IO.File.Move(OneFile, OneFile.Replace(CurExt, ".avi"))
                '35 Avi-files
                AviCount += 1
            ElseIf CurExt.ToLower = ".psd" And CurExt <> ".psd" Then
                PsdCount += 1
            ElseIf CurExt.ToLower = ".ion" And CurExt <> ".ion" Then
                IonCount += 1
            ElseIf CurExt.ToLower = ".flv" And CurExt <> ".flv" Then
                FlvCount += 1
            ElseIf CurExt.ToLower = ".vb" And CurExt <> ".vb" Then
                VbCount += 1
            ElseIf CurExt.ToLower = ".myapp" And CurExt <> ".myapp" Then
                MyappCount += 1
            ElseIf CurExt.ToLower = ".resx" And CurExt <> ".resx" Then
                ResxCount += 1
            ElseIf CurExt.ToLower = ".settings" And CurExt <> ".settings" Then
                SettingsCount += 1
            ElseIf CurExt.ToLower = ".mxml" And CurExt <> ".mxml" Then
                MxmlCount += 1
            ElseIf CurExt.ToLower = ".dylib" And CurExt <> ".dylib" Then
                DylibCount += 1
            ElseIf CurExt.ToLower = ".jnilib" And CurExt <> ".jnilib" Then
                JnilibCount += 1
            ElseIf CurExt.ToLower = ".so" And CurExt <> ".so" Then
                SoCount += 1
            ElseIf CurExt.ToLower = ".wav" And CurExt <> ".wav" Then
                WavCount += 1
            ElseIf CurExt.ToLower = ".jar" And CurExt <> ".jar" Then
                JarCount += 1
            ElseIf CurExt.ToLower = ".pkg" And CurExt <> ".pkg" Then
                PkgCount += 1
            ElseIf CurExt.ToLower = ".cache" And CurExt <> ".cache" Then
                System.IO.File.Move(OneFile, OneFile.Replace(CurExt, ".cache"))
                'Console.WriteLine(OneFile)
                CacheCount += 1
            ElseIf CurExt.ToLower = ".ogg" And CurExt <> ".ogg" Then
                OggCount += 1
            ElseIf CurExt.ToLower = ".vbproj" And CurExt <> ".vbproj" Then
                VbprojCount += 1
            ElseIf CurExt.ToLower = ".user" And CurExt <> ".user" Then
                UserCount += 1
            ElseIf CurExt.ToLower = ".pdb" And CurExt <> ".pdb" Then
                PdbCount += 1
            ElseIf CurExt.ToLower = ".pubxml" And CurExt <> ".pubxml" Then
                PubxmlCount += 1
            ElseIf CurExt.ToLower = ".resources" And CurExt <> ".resources" Then
                ResourcesCount += 1
            ElseIf CurExt.ToLower = ".svg" And CurExt <> ".svg" Then
                SvgCount += 1
            ElseIf CurExt.ToLower = ".hhc" And CurExt <> ".hhc" Then
                HhcCount += 1
            ElseIf CurExt.ToLower = ".hhk" And CurExt <> ".hhk" Then
                HhkCount += 1
            ElseIf CurExt.ToLower = ".torrent" And CurExt <> ".torrent" Then
                TorrentCount += 1
            ElseIf CurExt.ToLower = ".sln" And CurExt <> ".sln" Then
                SlnCount += 1
            ElseIf CurExt.ToLower = ".suo" And CurExt <> ".suo" Then
                SuoCount += 1
            Else
                For Each One As String In NewExt.ToList
                    If CurExt.ToLower = One.ToLower Then
                        GoTo ExistNewExt
                    End If
                Next
                NewExt.Add(CurExt)
                Dim ExtDot As String = CurExt.Replace(".", "")
                Dim ExtUp As String = UpFirstLetter(ExtDot)
                'Console.WriteLine(ExtUp)
ExistNewExt:
                'Console.WriteLine(IO.Path.GetExtension(OneFile))
                'Console.WriteLine(OneFile)
            End If
        Next
    End Sub
End Module

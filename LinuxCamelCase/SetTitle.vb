Imports System.Text.RegularExpressions

Partial Module Program
    Dim HAP As HtmlAgilityPack.HtmlDocument
    Sub SetTitle()
        HAP = New HtmlAgilityPack.HtmlDocument
        ReadFileList8(Root)
        ReadDirList8(Root)
    End Sub

    Sub ReadDirList8(Dir As String)
        For Each OneDir As String In IO.Directory.GetDirectories(Dir)
            ReadFileList8(OneDir)
            ReadDirList8(OneDir)
        Next
    End Sub

    Sub ReadFileList8(Dir As String)
        For Each OneFile As String In IO.Directory.GetFiles(Dir)
            Dim CurExt As String = IO.Path.GetExtension(OneFile)
            Dim DirName As String = IO.Path.GetDirectoryName(OneFile)
            If CurExt = ".htm" Or CurExt = ".html" Then
                Console.Write(".")
                Dim HTML As String = ""
                Try
                    HTML = IO.File.ReadAllText(OneFile)
                Catch ex As Exception
                    Console.WriteLine(OneFile & " : " & ex.Message)
                End Try
                If Not String.IsNullOrEmpty(HTML) Then
                    HAP.LoadHtml(HTML)
                    Dim titleRegex = New Regex("<title>(.*?)</title>", RegexOptions.Compiled Or RegexOptions.IgnoreCase)
                    Dim Titles As MatchCollection = titleRegex.Matches(HTML)
                    Dim h1 As HtmlAgilityPack.HtmlNodeCollection = HAP.DocumentNode.SelectNodes("//h1")
                    Dim h2 As HtmlAgilityPack.HtmlNodeCollection = HAP.DocumentNode.SelectNodes("//h2")
                    Dim h3 As HtmlAgilityPack.HtmlNodeCollection = HAP.DocumentNode.SelectNodes("//h3")
                    Dim h4 As HtmlAgilityPack.HtmlNodeCollection = HAP.DocumentNode.SelectNodes("//h3")
                    Dim htitle As HtmlAgilityPack.HtmlNodeCollection = HAP.DocumentNode.SelectNodes("//title")
                    If Titles.Count > 0 Then
                        Dim OldTitle As String = Titles(0).Value.Replace("<title>", "").Replace("</title>", "").Replace("<TITLE>", "").Replace("</TITLE>", "")
                        Dim NewTitle As String = ""
                        Dim H4Res As (String, Boolean) = CheckTag(h4)
                        Dim H3Res As (String, Boolean) = CheckTag(h3)
                        Dim H2Res As (String, Boolean) = CheckTag(h2)
                        Dim H1Res As (String, Boolean) = CheckTag(h1)
                        Dim HTitleRes As (String, Boolean) = CheckTag(htitle)
                        If Titles(0).Value.Contains("Viacheslav Eremin") Then
                            If OldTitle.Contains("<") Or OldTitle.Contains(">") Then
                                NewTitle = "<title>" & HTitleRes.Item1 & "</title>"
                            Else
                                If H3Res.Item2 And H2Res.Item2 And H1Res.Item2 And H4Res.Item2 Then
                                    NewTitle = "<title>Viacheslav Eremin | Professional Programming, Visual Studio, Visual Basic, Vb.Net, C#, Sql Server, Asp, Asp Net Classic, Asp Net Mvc, Asp Net Core, Blazor .Net, Dot Net, Net Framework, Net Core</title>"
                                    AddTitle(OneFile, "No H1/H2/H3/H4 for Title")
                                Else
                                    If Not H1Res.Item2 Then
                                        NewTitle = "<title>Viacheslav Eremin | " & H1Res.Item1 & "</title>"
                                    ElseIf Not H2Res.Item2 Then
                                        NewTitle = "<title>Viacheslav Eremin | " & H2Res.Item1 & "</title>"
                                    ElseIf Not H3Res.Item2 Then
                                        NewTitle = "<title>Viacheslav Eremin | " & H3Res.Item1 & "</title>"
                                    ElseIf Not H4Res.Item2 Then
                                        NewTitle = "<title>Viacheslav Eremin | " & H4Res.Item1 & "</title>"
                                    End If
                                End If
                            End If
                            AddTitle(OneFile, "SetTitle1 : " & NewTitle.Replace("'", ""))
                        Else
                            If OneFile.Contains("/AspNet-DocAndSamples-2017/") Then
                                If H3Res.Item2 And H2Res.Item2 And H1Res.Item2 Then
                                    NewTitle = "<title>Viacheslav Eremin | ASP.NET 2017 Reference</title>"
                                Else
                                    If Not H1Res.Item2 Then
                                        NewTitle = "<title>Viacheslav Eremin | ASP.NET 2017 Reference | " & H1Res.Item1 & "</title>"
                                    ElseIf Not H2Res.Item2 Then
                                        NewTitle = "<title>Viacheslav Eremin | ASP.NET 2017 Reference | " & H2Res.Item1 & "</title>"
                                    ElseIf Not H3Res.Item2 Then
                                        NewTitle = "<title>Viacheslav Eremin | ASP.NET 2017 Reference | " & H3Res.Item1 & "</title>"
                                    ElseIf Not H4Res.Item2 Then
                                        NewTitle = "<title>Viacheslav Eremin | ASP.NET 2017 Reference | " & H4Res.Item1 & "</title>"
                                    End If
                                End If
                            ElseIf OneFile.Contains("/Sql/Sql/") Then
                                If H3Res.Item2 And H2Res.Item2 And H1Res.Item2 Then
                                    NewTitle = "<title>Viacheslav Eremin | Transact-SQL Reference for SQL 2000</title>"
                                Else
                                    If Not H1Res.Item2 Then
                                        NewTitle = "<title>Viacheslav Eremin | Transact-SQL Reference for SQL 2000 | " & H1Res.Item1 & "</title>"
                                    ElseIf Not H2Res.Item2 Then
                                        NewTitle = "<title>Viacheslav Eremin | Transact-SQL Reference for SQL 2000 | " & H2Res.Item1 & "</title>"
                                    ElseIf Not H3Res.Item2 Then
                                        NewTitle = "<title>Viacheslav Eremin | Transact-SQL Reference for SQL 2000 | " & H3Res.Item1 & "</title>"
                                    End If
                                End If
                            ElseIf OneFile.Contains("/Dotnet/Vb/") Then
                                If H3Res.Item2 And H2Res.Item2 And H1Res.Item2 Then
                                    NewTitle = "<title>Viacheslav Eremin | Visual Basic Language Reference</title>"
                                Else
                                    If Not H1Res.Item2 Then
                                        NewTitle = "<title>Viacheslav Eremin | Visual Basic Language Reference | " & H1Res.Item1 & "</title>"
                                    ElseIf Not H2Res.Item2 Then
                                        NewTitle = "<title>Viacheslav Eremin | Visual Basic Language Reference | " & H2Res.Item1 & "</title>"
                                    ElseIf Not H3Res.Item2 Then
                                        NewTitle = "<title>Viacheslav Eremin | Visual Basic Language Reference | " & H3Res.Item1 & "</title>"
                                    End If
                                End If
                            ElseIf OneFile.Contains("/My_cisco/") Then
                                If H3Res.Item2 And H2Res.Item2 And H1Res.Item2 Then
                                    NewTitle = "<title>Viacheslav Eremin | Cisco PIX Firewall Reference</title>"
                                Else
                                    If Not H1Res.Item2 Then
                                        NewTitle = "<title>Viacheslav Eremin | Cisco PIX Firewall Reference | " & H1Res.Item1 & "</title>"
                                    ElseIf Not H2Res.Item2 Then
                                        NewTitle = "<title>Viacheslav Eremin | Cisco PIX Firewall Reference | " & H2Res.Item1 & "</title>"
                                    ElseIf Not H3Res.Item2 Then
                                        NewTitle = "<title>Viacheslav Eremin | Cisco PIX Firewall Reference | " & H3Res.Item1 & "</title>"
                                    End If
                                End If
                            ElseIf OneFile.Contains("/Freebsd/") Then
                                If H3Res.Item2 And H2Res.Item2 And H1Res.Item2 Then
                                    NewTitle = "<title>Viacheslav Eremin | Freebsd Reference</title>"
                                Else
                                    If Not H1Res.Item2 Then
                                        NewTitle = "<title>Viacheslav Eremin | Freebsd Reference | " & H1Res.Item1 & "</title>"
                                    ElseIf Not H2Res.Item2 Then
                                        NewTitle = "<title>Viacheslav Eremin | Freebsd Reference | " & H2Res.Item1 & "</title>"
                                    ElseIf Not H3Res.Item2 Then
                                        NewTitle = "<title>Viacheslav Eremin | Freebsd Reference | " & H3Res.Item1 & "</title>"
                                    End If
                                End If
                            ElseIf OneFile.Contains("/VB2015/") Then
                                If H3Res.Item2 And H2Res.Item2 And H1Res.Item2 Then
                                    NewTitle = "<title>Viacheslav Eremin | Visual Basic 2015 Reference</title>"
                                Else
                                    If Not H1Res.Item2 Then
                                        NewTitle = "<title>Viacheslav Eremin | Visual Basic 2015 Reference | " & H1Res.Item1 & "</title>"
                                    ElseIf Not H2Res.Item2 Then
                                        NewTitle = "<title>Viacheslav Eremin | Visual Basic 2015 Reference | " & H2Res.Item1 & "</title>"
                                    ElseIf Not H3Res.Item2 Then
                                        NewTitle = "<title>Viacheslav Eremin | Visual Basic 2015 Reference | " & H3Res.Item1 & "</title>"
                                    End If
                                End If
                            ElseIf OneFile.Contains("/VS2010_NET4_TrainingKit/") Then
                                If H3Res.Item2 And H2Res.Item2 And H1Res.Item2 Then
                                    NewTitle = "<title>Viacheslav Eremin | Visual Studio 2010 and .NET Framework 4 Training Kit</title>"
                                Else
                                    If Not H1Res.Item2 Then
                                        NewTitle = "<title>Viacheslav Eremin | Visual Studio 2010 and .NET Framework 4 Training Kit | " & H1Res.Item1 & "</title>"
                                    ElseIf Not H2Res.Item2 Then
                                        NewTitle = "<title>Viacheslav Eremin | Visual Studio 2010 and .NET Framework 4 Training Kit | " & H2Res.Item1 & "</title>"
                                    ElseIf Not H3Res.Item2 Then
                                        NewTitle = "<title>Viacheslav Eremin | Visual Studio 2010 and .NET Framework 4 Training Kit | " & H3Res.Item1 & "</title>"
                                    End If
                                End If
                            ElseIf OneFile.Contains("/Windows/Installer/") Then
                                NewTitle = "<title>Viacheslav Eremin | Windows Installer Reference</title>"
                            ElseIf OneFile.Contains("/Hosting/Qmail/") Then
                            Else
                                NewTitle = "<title>Viacheslav Eremin | Qmail Reference</title>"
                            End If
                            AddTitle(OneFile, "SetTitle2 : " & NewTitle.Replace("'", ""))
                        End If
                        Dim StartHTML As String = Left(HTML, Titles(0).Index)
                        Dim EndHTML As String = Mid(HTML, Titles(0).Index + Titles(0).Length + 1)
                        IO.File.WriteAllText(OneFile, StartHTML & NewTitle & EndHTML)
                    Else
                        AddTitle(OneFile, "No Title")
                    End If
                Else
                    AddTitle(OneFile, "No File")
                End If
            End If
        Next
    End Sub

    Function CheckTag(Tags As HtmlAgilityPack.HtmlNodeCollection) As (String, Boolean)
        Dim NewTitle As String = ""
        Dim NoTitle As Boolean = False
        If Tags IsNot Nothing Then
            If Not String.IsNullOrEmpty(Tags.First.InnerText) Then
                NewTitle = Tags.First.InnerText.Replace("<<", "").Replace(" назад", "").Replace(vbCrLf, "").Replace(" &nbsp;", "").Replace("  ", " ")
            Else
                NoTitle = True
            End If
        Else
            NoTitle = True
        End If
        Return (NewTitle, NoTitle)
    End Function

End Module


Imports System.Text.RegularExpressions

Partial Module Program

    Public Enum LinkType
        Href = 1
        Src = 2

    End Enum

    Sub ParseOneFile(FileName As String, ByRef HTML As String)
        Dim HrefRegex = New Regex("<a\s.*?href=(?:'|"")([^'"">]+)(?:'|"")", RegexOptions.Compiled Or RegexOptions.IgnoreCase)
        RecursiveProcessingOneInternalLink(FileName, HTML, HrefRegex, LinkType.Href, 0)
        Dim LocationRegex = New Regex("location.href=(?:'|"")([^'"">]+)(?:'|"")", RegexOptions.Compiled Or RegexOptions.IgnoreCase)
        RecursiveProcessingOneInternalLink(FileName, HTML, LocationRegex, LinkType.Href, 0)
        Dim SrcRegex = New Regex("<img\s.*?src=(?:'|"")([^'"">]+)(?:'|"")", RegexOptions.Compiled Or RegexOptions.IgnoreCase)
        RecursiveProcessingOneInternalLink(FileName, HTML, SrcRegex, LinkType.Src, 0)
        Dim LinkRegex = New Regex("<link\s.*?href=(?:'|"")([^'"">]+)(?:'|"")", RegexOptions.Compiled Or RegexOptions.IgnoreCase)
        RecursiveProcessingOneInternalLink(FileName, HTML, LinkRegex, LinkType.Href, 0)
        Dim ScriptRegex = New Regex("<script\s.*?src=(?:'|"")([^'"">]+)(?:'|"")", RegexOptions.Compiled Or RegexOptions.IgnoreCase)
        RecursiveProcessingOneInternalLink(FileName, HTML, ScriptRegex, LinkType.Src, 0)
    End Sub

    Sub RecursiveProcessingOneInternalLink(FileName As String, ByRef HTML As String, Regex As Regex, Type As LinkType, StartIndex As Integer)
        Dim Links As MatchCollection = Regex.Matches(HTML)
        If StartIndex <= Links.Count - 1 Then
            If Links(StartIndex).Value.ToLower.Contains("vb-net.com") And Not Links(StartIndex).Value.ToLower.Contains("forum.vb-net.com") And Not Links(StartIndex).Value.ToLower.Contains("products.vb-net.com") And Not Links(StartIndex).Value.ToLower.Contains("bug.vb-net.com") And Not Links(StartIndex).Value.ToLower.Contains("freeware.vb-net.com") Or
                Not Links(StartIndex).Value.ToLower.Contains("http") And Not Links(StartIndex).Value.ToLower.Contains("href=""#""") And Not Links(StartIndex).Value.ToLower.Contains("vb-net") And Not Links(StartIndex).Value.ToLower.Contains("forum.vb-net.com") Then
                'processing internal link
                'Debug.Print($"{StartIndex}: {Links(StartIndex).Index}:{Links(StartIndex).Value}")
                ReplaceOneLink(FileName, HTML, Links(StartIndex).Index, Links(StartIndex).Value, Type)
                RecursiveProcessingOneInternalLink(FileName, HTML, Regex, Type, StartIndex + 1)
            Else
                'look to next link
                RecursiveProcessingOneInternalLink(FileName, HTML, Regex, Type, StartIndex + 1)
            End If
        End If
    End Sub

    Sub ReplaceOneLink(FileName As String, ByRef HTML As String, LinkPosition As Integer, LinkText As String, Type As LinkType)
        Dim Str1 As New Text.StringBuilder()
        Str1.Append(Left(HTML, LinkPosition))               'add left HTML part outside of link
        Dim Res1 As String = LinkText.Replace("http://www.vb-net.com", "").Replace("http://vb-net.com", "")
        Dim Pos1 As Integer
        Select Case Type
            Case Type.Href
                Pos1 = InStr(Res1.ToLower, "href=", CompareMethod.Text)
            Case Type.Src
                Pos1 = InStr(Res1.ToLower, "src=", CompareMethod.Text)
        End Select
        If Pos1 > 0 Then
            Dim Pos2 = InStr(Pos1 + 1, Res1.ToLower, """", CompareMethod.Text)
            If Pos2 <= 0 Then
                Pos2 = InStr(Pos1 + 1, Res1.ToLower, "'", CompareMethod.Text)
            End If
            If Pos2 <= 0 Then
                Debug.Print("Link start not found :" & LinkText)
                WriteError("Link start not found :", LinkText)
            Else
                Dim Pos3 As Integer = InStr(Pos2 + 1, Res1.ToLower, """", CompareMethod.Text)
                If Pos3 <= 0 Then
                    Pos3 = InStr(Pos2 + 1, Res1.ToLower, "'", CompareMethod.Text)
                End If
                If Pos3 <= 0 Then
                    Debug.Print("Link end not found :" & LinkText)
                    WriteError("Link end not found :", LinkText)
                Else
                    Dim Pos4 As Integer = InStr(Pos2 + 1, Res1.ToLower, "#", CompareMethod.Text)
                    If Pos4 > 0 Then
                        'check link like /FreelanceProjects/Index.htm#a7
                        Pos3 = Pos4
                    End If
                    Dim ClearSiteLink As String = Mid(Res1, Pos2 + 1, Pos3 - Pos2 - 1)
                    If ClearSiteLink.StartsWith("//") Then
                        Str1.Append(LinkText)                   'not change
                    ElseIf ClearSiteLink.StartsWith("#") Then
                        Str1.Append(LinkText)                   'not change
                    Else
                        Str1.Append(Left(Res1, Pos2))           'add left part of link
                        If Mid(Res1, Pos3 - 1, 1) = "/" Then
                            'link like "/2007/"
                            ClearSiteLink &= "Index.htm"
                        End If
                        Str1.Append(GetNewCorrectLinkFromDb(FileName, ClearSiteLink))      'add new link
                        Str1.Append(Mid(Res1, Pos3 + 1))        'add right part of link
                    End If
                End If
            End If
        Else
            Debug.Print("Link not found : " & LinkText)
            WriteError("Link not found : ", LinkText)
        End If
        Str1.Append(Mid(HTML, LinkPosition + Len(LinkText)))    'add right HTML part outside of link
        HTML = Str1.ToString
    End Sub

    Function GetNewCorrectLinkFromDb(FileName As String, ClearSiteLink As String) As String
        If ClearSiteLink.StartsWith("http:") Then 'http://rzd.vb-net.com/Index.htm and similar
            Return ClearSiteLink
        ElseIf ClearSiteLink.ToLower.Contains("selector/styles.css") Then
            Return "/Selector/Styles.css"
        ElseIf ClearSiteLink.ToLower.Contains("selector/csharp.css") Then
            Return "/Selector/Csharp.css"
        ElseIf ClearSiteLink.ToLower.Contains("ms-help://") Then
            Return ClearSiteLink
        ElseIf ClearSiteLink.ToLower.Contains("/Sql/Basic/") Or ClearSiteLink.ToLower.Contains("/Sql/Sql/") Or ClearSiteLink.ToLower.Contains("/Sql/Func/") Or ClearSiteLink.ToLower.Contains("/Sql/Access/") Or ClearSiteLink.ToLower.Contains("/Sql/Login/") Or ClearSiteLink.ToLower.Contains("/Sql/Performance/") Or ClearSiteLink.ToLower.Contains("/Sql/Reserved") Or ClearSiteLink.ToLower.Contains("/Sql/Search") Then
            Return ClearSiteLink
        ElseIf ClearSiteLink = "mailTo:ru.vbnet2000@yahoo.com" Then
            Return "mailTo:spam@google.com"
        ElseIf ClearSiteLink.ToLower.StartsWith("mailto:") Then
            Return ClearSiteLink
        ElseIf FileName.Contains("/Windows/Framework2007/") Then
            Return ClearSiteLink
        ElseIf FileName.Contains("/Standard/Rfc/") Then
            Return ClearSiteLink
        ElseIf FileName.Contains("/Dotnet/MSDN/") Then
            Return ClearSiteLink
        ElseIf FileName.Contains("/Dotnet/Relation/") Or FileName.Contains("/Dotnet/Tour/") Then
            Return ClearSiteLink
        End If
        Select Case ClearSiteLink
            'replace well known broken link without DB
            Case "" : Return "" '<a href="#Write">
            Case "/Mvc" : Return "/Mvc/Index.htm"
            Case "/asp2" : Return "/Asp2/Index.htm"
            Case "/dotnet" : Return "/Dotnet/Index.htm"
            Case "/Terminal" : Return "/Terminal/Index.htm"
            Case "/Flex" : Return "/Flex/Index.htm"
            Case "/sql" : Return "/Sql/Index.htm"
            Case "/windows" : Return "/Windows/Index.htm"
            Case "/Linux" : Return "/Linux/Index.htm"
            Case "/LowCostAspNet" : Return "/LowCostAspNet/Index.htm"
            Case "/Software" : Return "/Software/Index.htm"
            Case "/Documentation" : Return "/Documentation/Index.htm"
            Case "/EnglishArticles" : Return "/EnglishArticles/Index.htm"
            Case "/ConnectToMe" : Return "/ConnectToMe/Index.htm"
            Case "/tour14/index.htm" : Return "/Dotnet/Tour14/Index.htm"
            Case "/tour16/index.htm" : Return "/Dotnet/Tour16/Index.htm"
            Case "/tour17/index.htm" : Return "/Dotnet/Tour17/Index.htm"
            Case "/tour13/index.htm" : Return "/Dotnet/Tour13/Index.htm"
            Case "/tour15/index.htm" : Return "/Dotnet/Tour15/Index.htm"
            Case "/tour9/index.htm" : Return "/Dotnet/Tour9/Index.htm"
            Case "/tour7/index.htm" : Return "/Dotnet/Tour7/Index.htm"
            Case "/../asp2/41/ironlogic.htm" : Return "/Asp2/41/Ironlogic.htm"
            Case "../IDisposable/Index.htm" : Return "/IDisposable/Index.htm"
            Case "../jquery-1.4.2.min.js" : Return "https://code.jquery.com/jquery-3.6.0.min.js"
            Case Else
                'calculate right up/down folder position ../../../
                Dim CurDepth As List(Of String) = GetPageDepth(FileName)
                If ClearSiteLink.StartsWith("/") Then
                    'from root
                    Return GetLinkFromDb(FileName, ClearSiteLink)
                ElseIf ClearSiteLink.StartsWith("../") Then
                    'up
                    Debug.Print($"                Up from {CurDepth.Count}: {FileName.Replace(Root, "")} : {ClearSiteLink} : {String.Join("+", CurDepth)}")
                    Dim UpFolderLink As String
                    If ClearSiteLink.StartsWith("../") Then
                        UpFolderLink = GetUpFolder(FileName, ClearSiteLink, CurDepth, 1)
                    ElseIf ClearSiteLink.StartsWith("../../") Then
                        UpFolderLink = GetUpFolder(FileName, ClearSiteLink, CurDepth, 2)
                        WriteError("Up Link 2 in " & FileName, ClearSiteLink)
                    ElseIf ClearSiteLink.StartsWith("../../../") Then
                        UpFolderLink = GetUpFolder(FileName, ClearSiteLink, CurDepth, 3)
                        WriteError("Up Link 3 in " & FileName, ClearSiteLink)
                    ElseIf ClearSiteLink.StartsWith("../../../../") Then
                        UpFolderLink = GetUpFolder(FileName, ClearSiteLink, CurDepth, 4)
                        WriteError("Up Link 4 in " & FileName, ClearSiteLink)
                    Else
                        'strong, repair it manually
                        WriteError("Up Link in " & FileName, ClearSiteLink)
                        Return ClearSiteLink
                    End If
                    Return GetLinkFromDb(FileName, UpFolderLink)
                Else
                    'next down 
                    If Not ClearSiteLink.Contains("../") Then
                        'clear down link
                        Dim DownLink As String
                        If CurDepth.Count = 0 Then
                            DownLink = Spliter & ClearSiteLink
                        ElseIf CurDepth.Count = 1 Then
                            DownLink = Spliter & CurDepth(0) & Spliter & ClearSiteLink
                        Else
                            DownLink = Spliter & String.Join(Spliter, CurDepth) & Spliter & ClearSiteLink
                        End If
                        Debug.Print($"                Down from {CurDepth.Count}: {FileName.Replace(Root, "")} : {ClearSiteLink} : {DownLink}")
                        Return GetLinkFromDb(FileName, DownLink)
                    Else
                        'strong, repair it manually
                        WriteError("DownUp Link in " & FileName, ClearSiteLink)
                        Return ClearSiteLink
                    End If
                End If
        End Select
    End Function

    Function GetPageDepth(FileName As String) As List(Of String)
        Dim FileNameStartFromRoot As String = Spliter & FileName.Replace(Root, "")
        Dim CurExt As String = IO.Path.GetExtension(FileNameStartFromRoot)
        Dim DirNameFromRoot As String = IO.Path.GetDirectoryName(FileNameStartFromRoot)
        Dim Arr1 As String() = (DirNameFromRoot).Split(Spliter)
        Dim Arr2 As New List(Of String)
        For Each One As String In Arr1
            If Not String.IsNullOrEmpty(One) Then
                Arr2.Add(One)
            End If
        Next
        Return Arr2
    End Function

    Function GetLinkFromDb(FileName As String, ClearSiteLink As String) As String
        Dim DbRequest As (Integer, String) = CheckLowerFileInDb(Root & ClearSiteLink)
        If DbRequest.Item1 = 0 Then
            'link not found - mark it as broken
            Console.WriteLine("-,")
            Debug.Print($"Link not found in DB : {FileName.Replace(Root, "")} : {ClearSiteLink}")
            WriteError("Link not found in DB", $"{FileName.Replace(Root, "")} : {ClearSiteLink}")
            Return ClearSiteLink
        Else
            Dim Ret1 As String = Spliter & DbRequest.Item2.Replace(Root, "")
            Debug.Print($"{FileName.Replace(Root, "")} : {DbRequest.Item1} : {ClearSiteLink} : {Ret1}")
            Return Ret1
        End If
    End Function

    Function GetUpFolder(FileName As String, ClearSiteLink As String, CurDepth As List(Of String), Up As Integer) As String
        'Up from 0: /Content.htm : ../Wanted/index.htm 
        'Up from 1: /Forest/Index.htm : ../Flowers/Tree/P1010401_1.jpg : Forest
        'need Up on CarDepth and add ClearLink without ../
        If Up = 1 And (CurDepth.Count = 0 Or CurDepth.Count = 1) Then
            Return ClearSiteLink.Replace("..", "")
        Else
            Debug.Print($"UpLink,")
            Return ClearSiteLink
        End If
    End Function

End Module

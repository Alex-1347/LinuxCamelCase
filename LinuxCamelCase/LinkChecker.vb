Imports System.Net
Imports System.Text.RegularExpressions

Partial Module Program

    Sub LinkCheck(FromUrl As String, Url As String)
        If Not Url.ToLower.Contains("vb-net.com") And Not Url.ToLower.Contains("http://") And Not Url.ToLower.Contains("https://") Then
            Url = "http://vb-net.com" & Url
        End If
        Dim UrlId As Integer = AddLink(Url, FromUrl)
        If UrlId = 0 Then Return 'duplicate
        Console.WriteLine("(" & UrlId & ") " & Url)
        Dim HTML As String
        Dim Client As New WebClient
        Try
            If Url.ToLower.Contains("vb-net.com") Then
                Dim Resp As Byte() = Client.DownloadData(Url)
                SetLinkCode(UrlId, 200, Resp.Length)
                Dim Headers() As String = Client.ResponseHeaders.GetValues("Content-Type")
                If Headers(0) = "text/html" Then
                    HTML = Text.UTF8Encoding.UTF8.GetString(Resp)
                    ProcessingOneUrl(Url, HTML)
                End If
            End If
        Catch ex As Exception
            SetLinkCode(UrlId, 404, 0)
        End Try
    End Sub

    Sub ProcessingOneUrl(FromUrl As String, HTML As String)
        Dim HrefRegex = New Regex("<a\s.*?href=(?:'|"")([^'"">]+)(?:'|"")", RegexOptions.Compiled Or RegexOptions.IgnoreCase)
        RecursiveProcessingLinks(FromUrl, HTML, HrefRegex, LinkType.Href, 0)
        Dim LocationRegex = New Regex("location.href=(?:'|"")([^'"">]+)(?:'|"")", RegexOptions.Compiled Or RegexOptions.IgnoreCase)
        RecursiveProcessingLinks(FromUrl, HTML, LocationRegex, LinkType.Href, 0)
        Dim SrcRegex = New Regex("<img\s.*?src=(?:'|"")([^'"">]+)(?:'|"")", RegexOptions.Compiled Or RegexOptions.IgnoreCase)
        RecursiveProcessingLinks(FromUrl, HTML, SrcRegex, LinkType.Src, 0)
        Dim LinkRegex = New Regex("<link\s.*?href=(?:'|"")([^'"">]+)(?:'|"")", RegexOptions.Compiled Or RegexOptions.IgnoreCase)
        RecursiveProcessingLinks(FromUrl, HTML, LinkRegex, LinkType.Href, 0)
        Dim ScriptRegex = New Regex("<script\s.*?src=(?:'|"")([^'"">]+)(?:'|"")", RegexOptions.Compiled Or RegexOptions.IgnoreCase)
        RecursiveProcessingLinks(FromUrl, HTML, ScriptRegex, LinkType.Src, 0)
    End Sub

    Sub RecursiveProcessingLinks(FromUrl As String, ByRef HTML As String, Regex As Regex, Type As LinkType, StartIndex As Integer)
        Dim Links As MatchCollection = Regex.Matches(HTML)
        If StartIndex <= Links.Count - 1 Then
            CheckOneLink(FromUrl, HTML, Links(StartIndex).Index, Links(StartIndex).Value, Type)
            RecursiveProcessingLinks(FromUrl, HTML, Regex, Type, StartIndex + 1)
        End If
    End Sub

    Sub CheckOneLink(FromUrl As String, ByRef HTML As String, LinkPosition As Integer, LinkText As String, Type As LinkType)
        Dim Pos1 As Integer
        Select Case Type
            Case Type.Href
                Pos1 = InStr(LinkText, "href=", CompareMethod.Text)
            Case Type.Src
                Pos1 = InStr(LinkText, "src=", CompareMethod.Text)
        End Select
        If Pos1 > 0 Then
            Dim Pos2 = InStr(Pos1 + 1, LinkText, """", CompareMethod.Text)
            If Pos2 <= 0 Then
                Pos2 = InStr(Pos1 + 1, LinkText, "'", CompareMethod.Text)
            End If
            If Pos2 <= 0 Then
                Debug.Print("Link start not found :" & LinkText)
                WriteError("Link start not found :", LinkText)
            Else
                Dim Pos3 As Integer = InStr(Pos2 + 1, LinkText, """", CompareMethod.Text)
                If Pos3 <= 0 Then
                    Pos3 = InStr(Pos2 + 1, LinkText, "'", CompareMethod.Text)
                End If
                If Pos3 <= 0 Then
                    Debug.Print("Link end not found :" & LinkText)
                    WriteError("Link end not found :", LinkText)
                Else
                    Dim ClearSiteLink As String = Mid(LinkText, Pos2 + 1, Pos3 - Pos2 - 1)
                    If Not ClearSiteLink.ToLower.Contains("vb-net.com") And Not ClearSiteLink.ToLower.Contains("http://") And Not ClearSiteLink.ToLower.Contains("https://") Then
                        ClearSiteLink = "http://vb-net.com" & ClearSiteLink
                    End If
                    LinkCheck(FromUrl, ClearSiteLink)
                End If
            End If
        Else
            Debug.Print("Link not found : " & LinkText)
        End If
    End Sub

End Module

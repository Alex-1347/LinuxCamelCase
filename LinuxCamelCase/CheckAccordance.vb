Partial Module Program
    Dim IsCheckOnly As Boolean = True
    Dim StartPage As String = ""
    Dim IsStartPageFound As Boolean = False
    Sub CheckAccordance()
        CN.Open()
        ReadFileList4(Root)
        ReadDirList4(Root)
        CN.Close()
        Console.ReadLine()
    End Sub

    Sub ReadDirList4(Dir As String)
        If Not Dir.Contains(".svn") And Not Dir.Contains(".git") Then
            For Each OneDir As String In IO.Directory.GetDirectories(Dir)
                ReadFileList4(OneDir)
                ReadDirList4(OneDir)
            Next
        End If
    End Sub

    Sub ReadFileList4(Dir As String)
        If Not Dir.Contains(".svn") And Not Dir.Contains(".git") Then
            For Each OneFile As String In IO.Directory.GetFiles(Dir)
                If StartPage = "" Then
                    ProcessingOneFile(OneFile)
                Else
                    If Not IsStartPageFound And (Root & StartPage) = OneFile Then
                        IsStartPageFound = True
                        ProcessingOneFile(OneFile)
                    ElseIf IsStartPageFound Then
                        ProcessingOneFile(OneFile)
                    End If
                End If
            Next
        End If
    End Sub

    Sub ProcessingOneFile(OneFile As String)
        FileChecker(OneFile)
        If Not IsCheckOnly Then
            Dim CurExt As String = IO.Path.GetExtension(OneFile)
            Dim DirName As String = IO.Path.GetDirectoryName(OneFile)
            If CurExt = ".htm" Or CurExt = ".html" Then
                Console.WriteLine(OneFile)
                Dim HTML As String = ""
                Try
                    HTML = IO.File.ReadAllText(OneFile)
                Catch ex As Exception
                    Console.WriteLine(OneFile & " : " & ex.Message)
                End Try
                If Not String.IsNullOrEmpty(HTML) Then
                    ParseOneFile(OneFile, HTML)
                    WriteLog(OneFile)
                    IO.File.WriteAllText(OneFile, HTML)
                End If
            End If
        End If
    End Sub


    Sub FileChecker(FileName As String)
        Dim CheckResult As (Integer, String) = CheckIsLowerFileNameIsPresentInDb(FileName)
        If CheckResult.Item1 = 0 Then
            Console.WriteLine()
            Console.WriteLine("Error: " & FileName)
            Console.WriteLine()
        Else
            Console.Write(CheckResult.Item1 & ",")
        End If
    End Sub

    Function CheckIsLowerFileNameIsPresentInDb(RawFileName As String) As (Integer, String)
        Dim CmdString As String = $"SELECT i, REPLACE(CONCAT(`Folder`, '{Spliter}', `FileName`),'{Root}','') as URL FROM vbnet.Files where LOWER(CONCAT(`Folder`, '{Spliter}', `FileName`))=LOWER('{RawFileName}');"
        'Console.WriteLine(CmdString)
        Dim CMD As New MySqlCommand(CmdString, CN)
        Dim RDR As MySqlDataReader = CMD.ExecuteReader()
        Dim Ret1 As String = ""
        Dim Ind1 As Integer = 0
        Dim Count As Integer = 0
        While RDR.Read
            Count += 1
            Ret1 = RDR("URL")
            Ind1 = RDR("i")
            If Count > 1 Then
                Console.WriteLine("Warning: " & RDR("i") & " : " & RDR("URL"))
            End If
        End While
        RDR.Close()
        Return (Ind1, Ret1.TrimStart(Spliter))
    End Function

End Module

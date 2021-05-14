Partial Module Program
    Dim IsCheckOnly As Boolean = True
    Dim StartPage As String = ""
    Dim IsStartPageFound As Boolean = False
    Sub CheckAccordance()
        If CN.State <> Data.ConnectionState.Open Then CN.Open()
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
                    'trigger StartPage
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
        Dim CheckResult As (Integer, String) = CheckFileInDb(FileName)
        If CheckResult.Item1 = 0 Then
            Console.WriteLine("Error: " & FileName)
            WriteError($"MySQLError file is nothing", FileName)
        Else
            Console.Write(CheckResult.Item1 & ",")
        End If
    End Sub

End Module

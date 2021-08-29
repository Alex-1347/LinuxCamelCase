Partial Module Program

    Sub SearchString2(Str1 As String, Str2 As String)
        ReadFileList7(Root, Str1, Str2)
        ReadDirList7(Root, Str1, Str2)
    End Sub

    Sub ReadDirList7(Dir As String, Str1 As String, Str2 As String)
        For Each OneDir As String In IO.Directory.GetDirectories(Dir)
            ReadFileList6(OneDir, Str1, Str2)
            ReadDirList6(OneDir, Str1, Str2)
        Next
    End Sub

    Sub ReadFileList7(Dir As String, Str1 As String, Str2 As String)
        For Each OneFile As String In IO.Directory.GetFiles(Dir)
            Dim CurExt As String = IO.Path.GetExtension(OneFile)
            Dim DirName As String = IO.Path.GetDirectoryName(OneFile)
            If CurExt = ".htm" Or CurExt = ".html" Then
                Dim HTML As String = ""
                Try
                    HTML = IO.File.ReadAllText(OneFile)
                Catch ex As Exception
                    Console.WriteLine(OneFile & " : " & ex.Message)
                End Try
                If Not String.IsNullOrEmpty(HTML) Then
                    If HTML.Contains(Str1) Then
                        Console.WriteLine(OneFile)
                        If Str2 IsNot Nothing Then
                            Try
                                IO.File.WriteAllText(OneFile, HTML.Replace(Str1, Str2))
                                Console.Write(" Replaced " & OneFile)
                            Catch ex As Exception
                                Console.WriteLine(OneFile & " : " & ex.Message)
                            End Try
                        End If
                    End If
                End If
            End If
        Next
    End Sub

End Module


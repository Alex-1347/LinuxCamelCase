Partial Module Program
    Sub RenameFiles(Dir As String)
        For Each OneFile As String In IO.Directory.GetFiles(Dir)
            Dim FileName As String = IO.Path.GetFileName(OneFile)
            Dim DirName As String = IO.Path.GetDirectoryName(OneFile)
            Dim ExtName As String = IO.Path.GetExtension(OneFile)
            Try
                If FileName = "index.htm" Then
                    Console.WriteLine($"{OneFile}->{DirName}/Index.htm")
                    IO.File.Move(OneFile, $"{DirName}/Index.htm")
                ElseIf FileName = "index.html" Then
                    Console.WriteLine($"{OneFile}->{DirName}/Index.html")
                    IO.File.Move(OneFile, $"{DirName}/Index.html")
                End If
            Catch ex As Exception
                Console.WriteLine(OneFile & ex.Message)
            End Try

        Next
        For Each OneDir As String In IO.Directory.GetDirectories(Dir)
            RenameFiles(OneDir)
        Next
    End Sub
End Module

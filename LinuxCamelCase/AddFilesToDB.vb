Partial Module Program

    Sub AddAllFilesToDB()
        If CN.State <> Data.ConnectionState.Open Then CN.Open()
        ReadFileList3(Root)
        ReadDirList3(Root)
        CN.Close()
    End Sub
    Sub ReadDirList3(Dir As String)
        For Each OneDir As String In IO.Directory.GetDirectories(Dir)
            AddDirToDb(OneDir)
            ReadFileList3(OneDir)
            ReadDirList3(OneDir)
        Next
    End Sub
    Sub ReadFileList3(Dir As String)
        For Each OneFile As String In IO.Directory.GetFiles(Dir)
            AddFileToDb(OneFile)
        Next
    End Sub

    Sub AddOnlyNewFiles()
        If CN.State <> Data.ConnectionState.Open Then CN.Open()
        ReadFileList5(Root)
        ReadDirList5(Root)
        CN.Close()
    End Sub

    Sub ReadDirList5(Dir As String)
        If Not Dir.Contains(".svn") And Not Dir.Contains(".git") Then
            For Each OneDir As String In IO.Directory.GetDirectories(Dir)
                Dim CheckResult As (Integer, String) = CheckDirInDb(OneDir)
                If CheckResult.Item1 = 0 Then
                    AddDirToDb(OneDir)
                End If
                ReadFileList5(OneDir)
                ReadDirList5(OneDir)
            Next
        End If
    End Sub

    Sub ReadFileList5(Dir As String)
        If Not Dir.Contains(".svn") And Not Dir.Contains(".git") Then
            For Each OneFile As String In IO.Directory.GetFiles(Dir)
                Dim CheckResult As (Integer, String) = CheckFileInDb(OneFile)
                If CheckResult.Item1 = 0 Then
                    AddFileToDb(OneFile)
                End If
            Next
        End If
    End Sub

    Function GetLen(ByVal FileName As String) As Integer
        Return New IO.FileInfo(FileName).Length
    End Function

    Function GetHash(ByVal FileName As String) As Byte()
        Try
            Dim MD5 As System.Security.Cryptography.MD5 = Security.Cryptography.MD5.Create
            Dim FS As New System.IO.FileStream(FileName, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
            Return MD5.ComputeHash(FS)
            FS.Close()
        Catch ex As Exception
            'File buzy
            Dim X(15) As Byte
            Return X
        End Try
    End Function

End Module

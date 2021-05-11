Partial Module Program
    Dim DirFile As Boolean = False
    Function UpFirstLetter(Str1 As String) As String
        If Not String.IsNullOrEmpty(Str1) Then
            Dim Arr1 As Char() = Str1.ToCharArray
            Arr1(0) = Arr1(0).ToString.ToUpper
            Return New String(Arr1)
        Else
            Return ""
        End If
    End Function

    Sub RenameDirSegment()
        DirFile = True
        ReadDirList2(Root)
    End Sub
    Sub RenamePathSegment()
        DirFile = False
        ReadDirList2(Root)
    End Sub
    Sub ReadDirList2(Dir As String)
        If Not Dir.Contains("AspNet-DocAndSamples") And Not Dir.Contains("Cisco_PIX_html_help") And Not Dir.Contains("VS2010_NET4_TrainingKit") And Not Dir.Contains(".svn") And Not Dir.Contains(".git") Then
            For Each OneDir As String In IO.Directory.GetDirectories(Dir)
                If Not DirFile Then
                    ReadFileList2(OneDir)
                End If
                ReadDirList2(OneDir)
            Next
            If DirFile Then
                If Dir <> GetNewDirName(Dir) Then
                    Try
                        Console.WriteLine($"{Dir} -> {GetNewDirName(Dir)}")
                        System.IO.Directory.Move(Dir, GetNewDirName(Dir))
                    Catch ex As Exception
                        Console.WriteLine(ex.Message)
                    End Try
                End If
            End If
        End If
    End Sub
    Function GetNewDirName(Dir As String) As String
        Dim RelativeDirName As String = Dir.Replace(Root, "")
        Dim DirSegmentArr() As String = RelativeDirName.Split(Spliter)
        For i As Integer = 0 To DirSegmentArr.Count - 1
            DirSegmentArr(i) = UpFirstLetter(DirSegmentArr(i))
        Next
        Dim NewDirShort As String = String.Join(Spliter, DirSegmentArr)
        Return Root & NewDirShort
    End Function

    Sub ReadFileList2(Dir As String)
        For Each OneFile As String In IO.Directory.GetFiles(Dir)
            If Not Dir.Contains("AspNet-DocAndSamples") And Not Dir.Contains("Cisco_PIX_html_help") And Not Dir.Contains("VS2010_NET4_TrainingKit") And Not Dir.Contains(".svn") And Not Dir.Contains(".git") Then
                Dim FileName As String = IO.Path.GetFileName(OneFile)
                Dim DirName As String = IO.Path.GetDirectoryName(OneFile)
                Dim NewFileName As String = UpFirstLetter(FileName)
                If FileName <> UpFirstLetter(FileName) Then
                    Console.WriteLine($"{IO.Path.Combine(DirName, FileName)} -> {IO.Path.Combine(DirName, NewFileName)}")
                    Try
                        System.IO.File.Move(IO.Path.Combine(DirName, FileName), IO.Path.Combine(DirName, NewFileName))
                    Catch ex As Exception
                        Console.WriteLine(ex.Message)
                    End Try
                End If
            End If
        Next
    End Sub
End Module

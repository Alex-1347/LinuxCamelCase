Partial Module Program

    Sub AddAllFilesToDB()
        CN.Open()
        ReadFileList3(Root)
        ReadDirList3(Root)
        CN.Close()
    End Sub

    Sub ReadDirList3(Dir As String)
        For Each OneDir As String In IO.Directory.GetDirectories(Dir)
            Dim Str1 As String = $"INSERT INTO `vbnet`.`Files`(`Folder`,`Date`)VALUES('{OneDir}',STR_TO_DATE('{Now.ToString("MM/dd/yyyy hh:mm:ss")}','%m/%d/%Y %H:%i:%s'));"
            Console.WriteLine(Str1)
            Try
                Dim CMD1 As New MySqlCommand(Str1, CN)
                CMD1.ExecuteNonQuery()
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
            ReadFileList3(OneDir)
            ReadDirList3(OneDir)
        Next
    End Sub

    Sub ReadFileList3(Dir As String)
        For Each OneFile As String In IO.Directory.GetFiles(Dir)
            Dim FileName As String = IO.Path.GetFileName(OneFile)
            Dim DirName As String = IO.Path.GetDirectoryName(OneFile)
            Dim Str2 As String = $"INSERT INTO `vbnet`.`Files`(`Folder`,`FileName`,`Date`,`Length`,`MD5`)VALUES('{DirName}','{FileName}',STR_TO_DATE('{Now.ToString("MM/dd/yyyy hh:mm:ss")}','%m/%d/%Y %H:%i:%s'),'{GetLen(OneFile)}', @MD5);"
            Console.WriteLine(Str2)
            Try
                Dim CMD2 As New MySqlCommand(Str2, CN)
                CMD2.Parameters.Add("@MD5", MySqlDbType.Blob).Value = GetHash(OneFile)
                CMD2.ExecuteNonQuery()
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
        Next
    End Sub

    Sub TestDbConnction()
        Try
            CN.Open()
        Catch ex As Exception
            Console.WriteLine(ConnectionString & vbCrLf & ex.Message)
            Console.WriteLine("Check - " & "mysql --user=user_name --password db_name" & vbCrLf & "show databases; show tables;")
            End
        End Try

        Dim CMD As New MySqlCommand("select * from Entrance;", CN)
        Dim RDR As MySqlDataReader = CMD.ExecuteReader()
        While RDR.Read
            Console.WriteLine(RDR("TXT"))
        End While
        CN.Close()
        Console.ReadLine()
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

    Function CheckFileInDb(RawFileName As String) As (Integer, String)
        Dim CmdString As String = $"SELECT i, REPLACE(CONCAT(`Folder`, '{Spliter}', `FileName`),'{Root}','') as URL FROM vbnet.Files where LOWER(CONCAT(`Folder`, '{Spliter}', `FileName`))=LOWER('{RawFileName}');"
        'Debug.Print(CmdString.Replace("\", "\\"))
        Dim CMD As New MySqlCommand(CmdString.Replace("\", "\\"), CN)
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

    Sub WriteError(Name As String, Message As String)
        Dim CmdString As String = $"INSERT INTO `vbnet`.`FileError`(`CrDate`,`Name`,`Message`)VALUES(STR_TO_DATE('{Now.ToString("MM/dd/yyyy hh:mm:ss")}','%m/%d/%Y %H:%i:%s'),'{Name}','{Message}');"
        'Debug.Print(CmdString.Replace("\", "\\"))
        Dim CMD As New MySqlCommand(CmdString.Replace("\", "\\"), CN)
        CMD.ExecuteNonQuery()
    End Sub

    Sub WriteLog(FileName As String)
        Dim CmdString As String = $"INSERT INTO `vbnet`.`WriteLog`(`CrDate`,`FileName`)VALUES(STR_TO_DATE('{Now.ToString("MM/dd/yyyy hh:mm:ss")}','%m/%d/%Y %H:%i:%s'),'{FileName}');"
        'Debug.Print(CmdString.Replace("\", "\\"))
        Dim CMD As New MySqlCommand(CmdString.Replace("\", "\\"), CN)
        CMD.ExecuteNonQuery()
    End Sub

End Module

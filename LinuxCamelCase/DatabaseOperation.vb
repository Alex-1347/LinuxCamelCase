Partial Module Program

    Sub TestDbConnction()
        Try
            If CN.State <> Data.ConnectionState.Open Then CN.Open()
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

    Sub GetCorrectLinks(ProcessingLink As Func(Of String, String, String, Boolean))
        Dim Str1 As String = $"SELECT `i`,`Message`,`CorrectLink` FROM `vbnet`.`FileError` where `Name`='Link not found in DB' and `CorrectLink` IS NOT NULL;"
        Dim CMD As MySqlCommand
        Dim RDR As MySqlDataReader
        Dim SuccessChange As New List(Of Integer)
        Try
            CMD = New MySqlCommand(Str1, CN)
            RDR = CMD.ExecuteReader()
            While RDR.Read
                Dim I As Integer = RDR("i")
                Dim Arr1 As String() = RDR("Message").ToString.Split(" : ")
                If ProcessingLink.Invoke(Arr1(0), Arr1(1), RDR("CorrectLink")) Then
                    SuccessChange.Add(I)
                End If
            End While
            RDR.Close()
            Console.WriteLine("Updated " & SuccessChange.Count & " record.")
            SuccessChange.ForEach(Sub(X) DeleteRecord(X))
        Catch ex As Exception
            Console.WriteLine(Str1 & vbCrLf & ex.Message)
            WriteError("MySQLError", Str1 & vbCrLf & ex.Message)
        End Try
    End Sub

    Function GetOneWrongLink(Ind As Integer) As String
        Dim Str1 As String = $"SELECT `i`,`Message` FROM vbnet.FileError where Name='Link not found in DB' and I={Ind};"
        Dim CMD As MySqlCommand
        Dim RDR As MySqlDataReader
        Dim Ret1 As String = ""
        Try
            Dim CN1 As New MySqlConnection(ConnectionString)
            CN1.Open()
            CMD = New MySqlCommand(Str1, CN1)
            RDR = CMD.ExecuteReader()
            While RDR.Read
                Dim I As Integer = RDR("i")
                Dim Arr1 As String() = RDR("Message").ToString.Split(" : ")
                Ret1 = Arr1(1)
            End While
            RDR.Close()
            CN1.Close()
            Return Ret1
        Catch ex As Exception
            Console.WriteLine(Str1 & vbCrLf & ex.Message)
            WriteError("MySQLError", Str1 & vbCrLf & ex.Message)
        End Try
    End Function

    Sub PageUpdated(FileName As String)
        Dim Str1 As String = $"INSERT INTO `vbnet`.`ManualUpdateLog`(`CrDate`,`Page`)VALUES(STR_TO_DATE('{Now.ToString("MM/dd/yyyy hh:mm:ss")}','%m/%d/%Y %H:%i:%s'),'{FileName}');"
        Try
            Dim CN1 As New MySqlConnection(ConnectionString)
            CN1.Open()
            Dim CMD As MySqlCommand
            CMD = New MySqlCommand(Str1, CN1)
            CMD.ExecuteNonQuery()
            CN1.Close()
        Catch ex As Exception
            Console.WriteLine(FileName & vbCrLf & ex.Message)
            WriteError("MySQLError", Str1 & vbCrLf & ex.Message)
        End Try
    End Sub

    Sub DeleteRecord(I As Integer)
        Dim Str1 As String = $"DELETE FROM `vbnet`.`FileError` where I={I};"
        Dim CMD As MySqlCommand
        Try
            CMD = New MySqlCommand(Str1, CN)
            CMD.ExecuteNonQuery()
        Catch ex As Exception
            Console.WriteLine(I & vbCrLf & ex.Message)
            WriteError("MySQLError", Str1 & vbCrLf & ex.Message)
        End Try
    End Sub

    Sub AddFileToDb(File As String)
        Dim FileName As String = IO.Path.GetFileName(File)
        Dim DirName As String = IO.Path.GetDirectoryName(File)
        Dim Str2 As String = $"INSERT INTO `vbnet`.`Files`(`Folder`,`FileName`,`Date`,`Length`,`MD5`)VALUES('{DirName}','{FileName}',STR_TO_DATE('{Now.ToString("MM/dd/yyyy hh:mm:ss")}','%m/%d/%Y %H:%i:%s'),'{GetLen(File)}', @MD5);"
        Console.WriteLine(Str2)
        Try
            Dim CMD2 As New MySqlCommand(Str2.Replace("\", "\\"), CN)
            CMD2.Parameters.Add("@MD5", MySqlDbType.Blob).Value = GetHash(File)
            CMD2.ExecuteNonQuery()
        Catch ex As Exception
            Console.WriteLine(Dir() & vbCrLf & ex.Message)
            WriteError("MySQLError", Str2 & vbCrLf & ex.Message)
        End Try
    End Sub

    Sub AddDirToDb(Dir As String)
        Dim Str1 As String = $"INSERT INTO `vbnet`.`Files`(`Folder`,`Date`)VALUES('{Dir}',STR_TO_DATE('{Now.ToString("MM/dd/yyyy hh:mm:ss")}','%m/%d/%Y %H:%i:%s'));"
        Console.WriteLine(Str1)
        Try
            Dim CMD1 As New MySqlCommand(Str1.Replace("\", "\\"), CN)
            CMD1.ExecuteNonQuery()
        Catch ex As Exception
            Console.WriteLine(Dir & vbCrLf & ex.Message)
            WriteError("MySQLError", Str1 & vbCrLf & ex.Message)
        End Try
    End Sub

    Function CheckDirInDb(Dir As String) As (Integer, String)
        Dim CmdString As String = $"SELECT i, REPLACE(CONCAT(`Folder`, '{Spliter}'),'{Root}','') as URL FROM vbnet.Files where CONCAT(`Folder`)='{Dir.Replace("'", "\'")}' and `FileName` is null;"
        'Console.WriteLine(CmdString)
        Dim Ret1 As String = ""
        Dim Ind1 As Integer = 0
        Dim CMD As MySqlCommand
        Dim RDR As MySqlDataReader
        Try
            CMD = New MySqlCommand(CmdString.Replace("\", "\\"), CN)
            RDR = CMD.ExecuteReader()
            Dim Count As Integer = 0
            While RDR.Read
                Count += 1
                Ret1 = RDR("URL")
                Ind1 = RDR("i")
                If Count > 1 Then
                    Console.WriteLine("Warning: " & RDR("i") & " : " & RDR("URL"))
                    WriteError($"MySQLError Duplicate Dir {RDR("i")}", Dir)
                End If
            End While
            RDR.Close()
        Catch ex As Exception
            Console.WriteLine(Dir & vbCrLf & ex.Message)
            WriteError("MySQLError", CmdString & vbCrLf & ex.Message)
        End Try
        Return (Ind1, Ret1.TrimStart(Spliter))
    End Function

    Function CheckFileInDb(RawFileName As String) As (Integer, String)
        Dim CmdString As String = $"SELECT i, REPLACE(CONCAT(`Folder`, '{Spliter}', `FileName`),'{Root}','') as URL FROM vbnet.Files where CONCAT(`Folder`, '{Spliter}', `FileName`)='{RawFileName.Replace("'", "\'")}';"
        'Console.WriteLine(CmdString)
        Dim Ind1 As Integer = 0
        Dim Count As Integer = 0
        Dim Ret1 As String = ""
        Dim CMD As MySqlCommand
        Dim RDR As MySqlDataReader
        Try
            CMD = New MySqlCommand(CmdString.Replace("\", "\\"), CN)
            RDR = CMD.ExecuteReader()
            While RDR.Read
                Count += 1
                Ret1 = RDR("URL")
                Ind1 = RDR("i")
                If Count > 1 Then
                    Console.WriteLine("Warning: " & RDR("i") & " : " & RDR("URL"))
                    WriteError($"MySQLError Duplicate {RDR("i")}", RawFileName)
                End If
            End While
            RDR.Close()
        Catch ex As Exception
            Console.WriteLine(RawFileName & vbCrLf & ex.Message)
            WriteError("MySQLError", CmdString & vbCrLf & ex.Message)
        End Try
        Return (Ind1, Ret1.TrimStart(Spliter))
    End Function

    Function CheckLowerFileInDb(RawFileName As String) As (Integer, String)
        Dim CmdString As String = $"SELECT i, REPLACE(CONCAT(`Folder`, '{Spliter}', `FileName`),'{Root}','') as URL FROM vbnet.Files where LOWER(CONCAT(`Folder`, '{Spliter}', `FileName`))=LOWER('{RawFileName.Replace("'", "\'")}');"
        'Console.WriteLine(CmdString)
        Dim CMD As MySqlCommand
        Dim RDR As MySqlDataReader
        Dim Ret1 As String = ""
        Dim Ind1 As Integer = 0
        Dim Count As Integer = 0
        Try
            CMD = New MySqlCommand(CmdString, CN)
            RDR = CMD.ExecuteReader()
            While RDR.Read
                Count += 1
                Ret1 = RDR("URL")
                Ind1 = RDR("i")
                If Count > 1 Then
                    Console.WriteLine("Warning: " & RDR("i") & " : " & RDR("URL"))
                    WriteError($"MySQLError Duplicate {RDR("i")}", RawFileName)
                End If
            End While
            RDR.Close()
        Catch ex As Exception
            Console.WriteLine(RawFileName & vbCrLf & ex.Message)
            WriteError("MySQLError", CmdString & vbCrLf & ex.Message)
        End Try
        Return (Ind1, Ret1.TrimStart(Spliter))
    End Function

    Sub WriteLog(FileName As String)
        Dim CmdString As String = $"INSERT INTO `vbnet`.`WriteLog`(`CrDate`,`FileName`)VALUES(STR_TO_DATE('{Now.ToString("MM/dd/yyyy hh:mm:ss")}','%m/%d/%Y %H:%i:%s'),'{FileName}');"
        'Debug.Print(CmdString.Replace("\", "\\"))
        Try
            Dim CMD As New MySqlCommand(CmdString.Replace("\", "\\"), CN)
            CMD.ExecuteNonQuery()
        Catch ex As Exception
            Console.WriteLine(FileName & vbCrLf & ex.Message)
            WriteError("MySQLError", CmdString & vbCrLf & ex.Message)
        End Try
    End Sub

    Sub WriteError(Name As String, Message As String)
        Dim CmdString As String = $"INSERT INTO `vbnet`.`FileError`(`CrDate`,`Name`,`Message`)VALUES(STR_TO_DATE('{Now.ToString("MM/dd/yyyy hh:mm:ss")}','%m/%d/%Y %H:%i:%s'),'{Name}','{Message}');"
        'Debug.Print(CmdString.Replace("\", "\\"))
        Try
            Dim CMD As New MySqlCommand(CmdString.Replace("\", "\\"), CN)
            CMD.ExecuteNonQuery()
        Catch ex As Exception
            Console.WriteLine(Name & Message & ex.Message)
        End Try
    End Sub



End Module

Partial Module Program

    Public ConnectionString As String   '"server=127.0.0.1;user id=vbnet;port=33306;password=XXXXXXXXXXXXXXXXXXXX;persistsecurityinfo=True;database=vbnet"
    Public Root As String               '"/var/www/vb-net.com/html" or "J:\vb-net\Job57"
    Public Spliter As String            '"\" or "/"

    Public CN As MySqlConnection

    Sub Main(args As String())

        Try
            Dim JsonString As String = IO.File.ReadAllText(IO.Path.Combine(IO.Directory.GetParent(AppContext.BaseDirectory).FullName, "appsettings.json"))
            Dim JsonObj As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.Linq.JObject.Parse(JsonString)
            ConnectionString = JsonObj.Item("ConnectionString")
            Root = JsonObj.Item("Root").ToString.Replace("//", "/").Replace("\\", "\")
            Spliter = JsonObj.Item("Spliter").ToString.Replace("//", "/").Replace("\\", "\")
            CN = New MySqlConnection(ConnectionString)
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try

        'common environment OK, init variable to section 1
        NewExt.Add("")
        AllExt.Add("")

        'and go ahead
        Console.WriteLine("Select mode:")
        Console.WriteLine("(1) - Check nonLower extension")
        Console.WriteLine("(2) - Test DB connection.")
        Console.WriteLine("(3) - Rename Files")
        Console.WriteLine("(4) - Rename Directory Segments (start more than one times)")
        Console.WriteLine("(5) - Add all files to DB")
        Console.WriteLine("(6) - Check Accordance FileSystem and DB with LowerCase")
        Console.WriteLine("(7) - Set Up Correct Link")
        Console.WriteLine("(8) - Set Up Correct Link, Start from selected file, for ex /EPPlus-Office-Open-XML/Index.htm")
TryAgain:
        Dim Mode As String = Console.ReadLine()
        If IsNumeric(Mode) Then
            Select Case CInt(Mode)
                Case 1 : CheckNonlowerExtension()
                Case 2 : TestDbConnction()
                Case 3 : RenamePathSegment()
                Case 4 : RenameDirSegment()
                Case 5 : AddAllFilesToDB()
                Case 6 : CheckAccordance()
                Case 7 : IsCheckOnly = False : CheckAccordance()
                Case 8
                    Console.WriteLine("Set start file:")
                    StartPage = Console.ReadLine()
                    IsCheckOnly = False
                    CheckAccordance()
                Case Else
                    Console.WriteLine("Try again:")
                    GoTo TryAgain
            End Select
        Else
            GoTo TryAgain
        End If
    End Sub

End Module

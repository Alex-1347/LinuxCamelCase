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
        Console.WriteLine("(4) - Rename Directory Segments (nood to start more than one times)")
        Console.WriteLine("(5) - Add all files to DB")
        Console.WriteLine("(6) - Add new files to DB")
        Console.WriteLine("(7) - Check Accordance FileSystem and DB with LowerCase")
        Console.WriteLine("(8) - Set Up Correct Link")
        Console.WriteLine("(9) - Set Up Correct Link, Start from selected file, for ex /EPPlus-Office-Open-XML/Index.htm")
        Console.WriteLine("(10)- Repair links, get correct links from DB")
        Console.WriteLine("(11)- Process one page")
        Console.WriteLine("(12)- Special repair")
        Console.WriteLine("(13)- Get list of main wrong link")
        Console.WriteLine("(14)- Get repair Insert commands")
        Console.WriteLine("(15)- LinkChecker")
        Console.WriteLine("(16)- Search String")
        Console.WriteLine("(17)- Replace String")
        Console.WriteLine("(18)- Rename index to Index")
TryAgain:
        Dim Mode As String = Console.ReadLine()
        If IsNumeric(Mode) Then
            Select Case CInt(Mode)
                Case 1 : CheckNonlowerExtension()
                Case 2 : TestDbConnction()
                Case 3
                    Console.WriteLine("Are you sure (Y/N):")
                    If Console.ReadLine() = "Y" Then
                        RenamePathSegment()
                    End If

                Case 4
                    Console.WriteLine("Are you sure (Y/N):")
                    If Console.ReadLine() = "Y" Then
                        RenameDirSegment()
                    End If
                Case 5
                    Console.WriteLine("Are you sure (Y/N):")
                    If Console.ReadLine() = "Y" Then
                        AddAllFilesToDB()
                    End If
                Case 6 : AddOnlyNewFiles()
                Case 7 : CheckAccordance()
                Case 8
                    Console.WriteLine("Are you sure (Y/N):")
                    If Console.ReadLine() = "Y" Then
                        IsCheckOnly = False : CheckAccordance()
                    End If
                Case 9
                    Console.WriteLine("Set start file:")
                    StartPage = Console.ReadLine()
                    IsCheckOnly = False
                    CheckAccordance()
                Case 10
RepairNext:
                    RepairLinksForDB()
                Case 11
ProcessNext:
                    Console.WriteLine("Get page:")
                    ProcessOnePage(Console.ReadLine())
                    GoTo ProcessNext
                Case 12
                    Console.WriteLine("Get filter mask (like '/VS2010_NET4_TrainingKit/%js'):")
                    SpecialRepair(Console.ReadLine())
                Case 13
                    Console.WriteLine("SELECT * FROM `vbnet`.`FileError`")
                    Console.WriteLine("where `Name`='Link not found in DB' and `LostFile` is NULL")
                    Console.WriteLine("and Message not like '/AspNet-DocAndSamples-2017%'")
                    Console.WriteLine("and Message not like '/Dotnet/%'")
                    Console.WriteLine("and Message not like '/Freebsd/%'")
                    Console.WriteLine("and Message not like '/VS2010_NET4_TrainingKit/%'")
                    Console.WriteLine("and Message not like '/Sql/%'")
                    Console.WriteLine("and Message not like '/Windows/%'")
                    Console.WriteLine("and Message not like '/Elastix/%'")
                    Console.WriteLine("and Message not like '/My_cisco%'")
                    Console.WriteLine("and Message not like '/FreelanceProjects/%'")
                    Console.WriteLine("and Message not like '/PriceEraser%'")
                    Console.WriteLine("and Message not like '/Raymond/%'")
                    Console.WriteLine("and Message not like '/Hosting/%'")
                    Console.WriteLine("and Message not like '/Flowers%'")
                    Console.WriteLine("and Message not like '/2/%'")
                    Console.WriteLine("and Message not like '/Asp2/%'")
                    Console.WriteLine("and Message not like '%/VB2015/%'")
                    Console.WriteLine("and Message not like '/Standard/%'")
                    Console.WriteLine("and Message not like '%.js'")
                    Console.WriteLine("and Message not like '%.css';")
                    Console.WriteLine("SELECT * FROM Links")
                    Console.WriteLine("where code=404 and NotNeed is null")
                    Console.WriteLine("and FromUrl not like 'http://vb-net.com/VS2010_NET4_TrainingKit/%'")
                    Console.WriteLine("and FromUrl not like 'http://vb-net.com/Windows/Framework2007/%'")
                    Console.WriteLine("and FromUrl not like 'http://vb-net.com/Standard/Rfc/%'")
                    Console.WriteLine("and FromUrl not like 'http://vb-net.com/AspNet-DocAndSamples-2017%'")
                    Console.WriteLine("and FromUrl not like 'http://vb-net.com/Dotnet/MSDN/%'")
                    Console.WriteLine("and FromUrl not like 'http://vb-net.com/VB2015/%'")
                    Console.WriteLine("and FromUrl not like 'http://vb-net.com/Flowers/%'")
                    Console.WriteLine("and FromUrl not like 'http://vb-net.com/Dotnet/Tour5/%'")
                    Console.WriteLine("and FromUrl not like 'http://vb-net.com/Sql/Access/Set.htm'")
                    Console.WriteLine("and FromUrl not like 'http://vb-net.com/Elastix/Index.htm'")
                    Console.WriteLine("and FromUrl not like 'http://vb-net.com/Dotnet/Relation/%'")
                    Console.WriteLine("and FromUrl not like 'http://vb-net.com/Dotnet/Diagrams/%'")
                    Console.WriteLine("and FromUrl not like 'http://vb-net.com/Hosting/Qmail/%'")
                    Console.WriteLine("and FromUrl not like 'http://vb-net.com/Sql/Sql/%'")
                    Console.WriteLine("and FromUrl not like 'http://vb-net.com/Dotnet/Vb/%'")
                    Console.WriteLine("and FromUrl not like 'http://vb-net.com/Software/%'")
                    Console.WriteLine("and FromUrl not like 'http://vb-net.com/Convert/Xslt/Oper/%'")
                    Console.WriteLine("and FromUrl not like 'http://vb-net.com/My_cisco/Cisco_doc.htm'")
                    Console.WriteLine("and FromUrl not like 'http://vb-net.com/AndroidBook/Index.htm'")
                    Console.WriteLine("and FromUrl not like 'http://vb-net.com/Freebsd/%'")
                    Console.WriteLine("and Url not like '%spam@google.com'")
                    Console.WriteLine("order by FromUrl;")
                Case 14
                    Console.WriteLine("Get index list (XX-YY) or one line XX:")
                    Dim Ids As String = Console.ReadLine()
                    If Ids.Contains("-") Then
                        Dim Arr1() As String = Ids.Split("-")
                        For Id As Integer = CInt(Arr1(0)) To CInt(Arr1(1))
                            Console.WriteLine(InsertCmd(Id))
                        Next
                    Else
                        Console.WriteLine(InsertCmd(Ids))
                    End If
                    Console.WriteLine("Press Y when you are ready to update:")
                    If Console.ReadLine() = "Y" Then
                        GoTo RepairNext
                    End If
                Case 15
                    CN.Open()
                    LinkCheck("/", "http://vb-net.com")
                Case 16
                    Console.WriteLine("Enter search string:")
                    SearchString(Console.ReadLine(), Nothing)
                Case 17
                    'Root = "/var/www/vb-net.com/html/AspNet-DocAndSamples-2017"
                    Console.WriteLine("Enter search string:")
                    Dim Str1 As String = Console.ReadLine()
                    Console.WriteLine("Enter replace string:")
                    Dim Str2 As String = Console.ReadLine()
                    SearchString(Str1, Str2)
                Case 18
                    Console.WriteLine("Enter directory:")
                    RenameFiles(Console.ReadLine())
                Case Else
                    Console.WriteLine("Try again:")
                    GoTo TryAgain
            End Select
        Else
            GoTo TryAgain
        End If
    End Sub

    Function InsertCmd(I As Integer) As String
        Return $"UPDATE `vbnet`.`FileError` SET `CorrectLink` = '{GetOneWrongLink(I)}' WHERE `i` = {I};"
    End Function

End Module

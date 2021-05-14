Partial Module Program
    Sub RepairLinksForDB()
        If CN.State <> Data.ConnectionState.Open Then CN.Open()
        GetCorrectLinks(AddressOf ProcessingLink)
        CN.Close()
    End Sub

    Function ProcessingLink(Page As String, WrongLink As String, CorrectLink As String) As Boolean
        Dim HTML As String
        Try
            HTML = IO.File.ReadAllText(Root & Page)
            HTML = HTML.Replace(WrongLink, CorrectLink)
            IO.File.WriteAllText(Root & Page, HTML)
        Catch ex As Exception
            Console.WriteLine(Page & " not found" & vbCrLf & ex.Message)
            WriteError("Page not found", Page & vbCrLf & ex.Message)
        End Try
        PageUpdated(Page)
        Return True
    End Function

End Module


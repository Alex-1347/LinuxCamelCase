Partial Module Program
    Sub ProcessOnePage(Page As String)
        If CN.State <> Data.ConnectionState.Open Then CN.Open()
        Dim HTML As String = ""
        Try
            HTML = IO.File.ReadAllText(Root & Page)
        Catch ex As Exception
            Console.WriteLine(Page & " : " & ex.Message)
        End Try
        If Not String.IsNullOrEmpty(HTML) Then
            ParseOneFile(Root & Page, HTML)
            IO.File.WriteAllText(Root & Page, HTML)
            PageUpdated(Page)
        End If
    End Sub

End Module


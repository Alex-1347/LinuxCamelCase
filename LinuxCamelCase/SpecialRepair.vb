Partial Module Program
    Sub SpecialRepair(Mask As String)
        If CN.State <> Data.ConnectionState.Open Then CN.Open()
        CheckFileForMaskInDb(Mask, AddressOf ProcessingLink1)
        CN.Close()
    End Sub

    Function ProcessingLink1(Page As String, WrongLink As String, CorrectLink As String) As Boolean
        Dim HTML As String
        Try
            Dim FileName As String = Root & Page
            Dim File As String = IO.Path.GetFileName(FileName)
            Dim Dir As String = IO.Path.GetDirectoryName(FileName)
            Dim Ext As String = IO.Path.GetExtension(FileName)
            Dim CurDepth As List(Of String) = GetPageDepth(Dir)
            Dim NewDeph As New List(Of String)
            Dim RestLink As String = ""
            If WrongLink.StartsWith("../../../../") And CurDepth.Count > 4 Then
                For i As Integer = 0 To CurDepth.Count - 4
                    NewDeph.Add(CurDepth(i))
                Next
                RestLink = WrongLink.Replace("../../../..", "")
            ElseIf WrongLink.StartsWith("../../../") And CurDepth.Count > 3 Then
                For i As Integer = 0 To CurDepth.Count - 3
                    NewDeph.Add(CurDepth(i))
                Next
                RestLink = WrongLink.Replace("../../..", "")
            ElseIf WrongLink.StartsWith("../../") And CurDepth.Count > 2 Then
                For i As Integer = 0 To CurDepth.Count - 2
                    NewDeph.Add(CurDepth(i))
                Next
                RestLink = WrongLink.Replace("../..", "")
            ElseIf WrongLink.StartsWith("../") And CurDepth.Count > 1 Then
                For i As Integer = 0 To CurDepth.Count - 1
                    NewDeph.Add(CurDepth(i))
                Next
                RestLink = WrongLink.Replace("..", "")
            Else
                Return False
            End If
            Dim NewPath As String = String.Join("/", NewDeph) & RestLink

            Dim Result As (Integer, String) = CheckLowerFileInDb(Root & Spliter & NewPath)
            Console.WriteLine(Result.Item1 & " : " & NewPath)
            If Result.Item1 > 0 Then
                HTML = IO.File.ReadAllText(Root & Page)
                HTML = HTML.Replace(WrongLink, Result.Item2)
                IO.File.WriteAllText(Root & Page, HTML)
                PageUpdated(Page)
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Console.WriteLine(Page & " not found" & vbCrLf & ex.Message)
            WriteError("Page not found", Page & vbCrLf & ex.Message)
        End Try
    End Function

End Module


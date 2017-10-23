Imports System.ComponentModel
Imports System.Reflection

Module myApp

    Public Function initMembers(ByVal obj As Object) As Object
        Dim locObj As Object = obj
        Try
            Dim locType As Type = locObj.GetType
            Dim locMembers() As MemberInfo = locType.GetMembers()
            For Each locMember As MemberInfo In locMembers
                If locMember.MemberType = MemberTypes.Property Then
                    Dim locPropertyInfo As PropertyInfo = CType(locMember, PropertyInfo)
                    Dim locAttributeCollection As AttributeCollection = TypeDescriptor.GetProperties(locObj)(locMember.Name).Attributes
                    Dim locDefaultAttribute As DefaultValueAttribute = CType(locAttributeCollection(GetType(DefaultValueAttribute)), DefaultValueAttribute)
                    If locDefaultAttribute IsNot Nothing And locPropertyInfo.CanWrite = True Then
                        locPropertyInfo.SetValue(locObj, locDefaultAttribute.Value, Nothing)
                    End If
                End If
            Next
            Return locObj
        Catch ex As Exception
            Beep() : Debug.WriteLine(ex.Message) : Stop
            Return obj
        End Try
    End Function

End Module

Imports WebRaider.SharedLibrary

''' <summary>
''' Plugin interface
''' </summary>
Public Interface IRaider

    ''' <summary>
    ''' Gets or sets the name of the Plugin.
    ''' </summary>
    ''' <value>The name.</value>
    ReadOnly Property Name() As String

    Sub Setup()

    ''' <summary>
    ''' Prepares the attack.
    ''' </summary>
    ''' <param name="link">The link to attack.</param>
    ''' <param name="param">The attack parameter.</param>
    ''' <param name="attackPattern">The current attack pattern.</param>
    ''' <returns>
    ''' True if the plugin needs to attack False otherwise.
    ''' </returns>
    Function PrepareAttack(ByVal link As Link, ByVal param As Parameter, ByVal attackPattern As AttackPattern) As Boolean

    ReadOnly Property Attacks() As List(Of AttackPattern)

    Property Enabled() As Boolean
End Interface

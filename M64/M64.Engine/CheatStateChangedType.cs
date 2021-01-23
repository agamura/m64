#region Header
//+ <source name="CheatStateChangedType.cs" language="C#" begin="14-Nov-2012">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2012">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

namespace M64.Engine
{
    /// <summary>
    /// Specifies how the state of a <see cref="Cheat"/> has changed.
    /// </summary>
    public enum CheatStateChangedType
    {
        /// <summary>
        /// The <see cref="Cheat"/> has been activated.
        /// </summary>
        Activated = 1,

        /// <summary>
        /// The <see cref="Cheat"/> has been deactivated.
        /// </summary>
        Deactivated = 2
    }
}

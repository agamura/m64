#region Header
//+ <source name="CheatStateChangedType.cs" language="C#" begin="14-Nov-2021">
//+ <author href="mailto:j3d@tiletoons.com">J3d</author>
//+ <copyright year="2021">
//+ <holder web="https://tiletoons.com">Tiletoons!</holder>
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

#region Header
//+ <source name="ListChangedType.cs" language="C#" begin="21-Nov-2020">
//+ <author href="mailto:j3d@tiletoons.com">J3d</author>
//+ <copyright year="2020">
//+ <holder web="https://tiletoons.com">Tiletoons!</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

namespace M64.Engine
{
    /// <summary>
    /// Specifies how an <see cref="ObservableList&lt;T&gt;"/> has changed.
    /// </summary>
    public enum ListChangedType
    {
        /// <summary>
        /// The <see cref="ObservableList&lt;T&gt;"/> has been cleared.
        /// </summary>
        Cleared = 1,

        /// <summary>
        /// An item has been added to the <see cref="ObservableList&lt;T&gt;"/>.
        /// </summary>
        ItemAdded = 2,

        /// <summary>
        /// An item has been removed from the <see cref="ObservableList&lt;T&gt;"/>.
        /// </summary>
        ItemRemoved = 3,

        /// <summary>
        /// An item in the <see cref="ObservableList&lt;T&gt;"/> has changed.
        /// </summary>
        ItemChanged = 4
    }
}

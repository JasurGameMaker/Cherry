// Copyright (c) 2015 - 2023 Doozy Entertainment. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

//.........................
//.....Generated Class.....
//.........................
//.......Do not edit.......
//.........................

using System.Collections.Generic;
// ReSharper disable All
namespace Doozy.Runtime.Reactor
{
    public partial class Progressor
    {
        public static IEnumerable<Progressor> GetProgressors(ProgressorId.Loaders id) => GetProgressors(nameof(ProgressorId.Loaders), id.ToString());
    }
}

namespace Doozy.Runtime.Reactor
{
    public partial class ProgressorId
    {
        public enum Loaders
        {
            MainLoader
        }    
    }
}

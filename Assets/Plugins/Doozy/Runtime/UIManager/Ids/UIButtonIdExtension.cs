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
namespace Doozy.Runtime.UIManager.Components
{
    public partial class UIButton
    {
        public static IEnumerable<UIButton> GetButtons(UIButtonId.GDPR id) => GetButtons(nameof(UIButtonId.GDPR), id.ToString());
        public static bool SelectButton(UIButtonId.GDPR id) => SelectButton(nameof(UIButtonId.GDPR), id.ToString());

        public static IEnumerable<UIButton> GetButtons(UIButtonId.Wayfarer id) => GetButtons(nameof(UIButtonId.Wayfarer), id.ToString());
        public static bool SelectButton(UIButtonId.Wayfarer id) => SelectButton(nameof(UIButtonId.Wayfarer), id.ToString());
    }
}

namespace Doozy.Runtime.UIManager
{
    public partial class UIButtonId
    {
        public enum GDPR
        {
            Accept,
            Decline
        }

        public enum Wayfarer
        {
            Profile,
            Settings,
            Shop
        }    
    }
}
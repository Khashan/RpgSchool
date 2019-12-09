using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Anderson.CustomWindows
{
    public interface IWindowLogic
    {
        /// <summary>
        /// Show the window to the screen
        /// </summary>
        void Open();

        /// <summary>
        /// Close the selected window
        /// </summary>
        void Close();

        /// <summary>
        /// Show the window in front of everything
        ///</summary>
        void Focus();

        
        bool IsOpen();
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace Lucy
{
    /// <summary>
    /// TokenEntity is cross linked to make it faster to look ahead and behind.
    /// </summary>
    public class TokenEntity : LucyEntity
    {
        public TokenEntity Next { get; set; }

        public TokenEntity Previous { get; set; }
    }
}

﻿namespace CarritoAPI.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Dni { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsVip { get; set; }
    }
}

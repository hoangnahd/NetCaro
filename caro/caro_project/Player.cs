using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace caro_project
{
    internal class Player
    {
        public string Name { get; set; }
        public string Mark { get; set; }
        public string auth { get; set; }
        public bool isActive { get; set; }
        public bool startCoolDown { get; set; } = false;
        public Dictionary<int, List<int>> row2col { get; set; } = new Dictionary<int, List<int>>();
        public Dictionary<int, List<int>> col2row { get; set; } = new Dictionary<int, List<int>>();
        public bool isWin { get; set; } = false;
        public Player(string _name, string _mark, bool _isActive, string _auth)
        {
            Name = _name;
            Mark = _mark;
            isActive = _isActive;
            auth = _auth;
        }
    }
}

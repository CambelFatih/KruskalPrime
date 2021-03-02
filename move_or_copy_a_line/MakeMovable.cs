using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace move_or_copy_a_line
{
    class MakeMovable
    {
        private Form form;
        private bool _dragging = false;
        private Point _start_point = new Point(0, 0);

        public MakeMovable(Form form)
        {
            this.form = form;
        }

        public void SetMovable(params Control[] controls)
        {
            foreach (Control control in controls)
            {
                control.MouseDown += new MouseEventHandler(_MouseDown);
                control.MouseUp += new MouseEventHandler(_MouseUp);
                control.MouseMove += new MouseEventHandler(_MouseMove);
            }
        }

        private void _MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _dragging = true;
                _start_point = new Point(e.X, e.Y);
            }

        }

        private void _MouseUp(object sender, MouseEventArgs e)
        {
            _dragging = false;
        }

        private void _MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                Point p = form.PointToScreen(e.Location);
                form.Location = new Point(p.X - this._start_point.X, p.Y - this._start_point.Y);
            }
        }
    }
}
